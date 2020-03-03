﻿using JS.Base.WS.API.Base;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Common;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Authorization;
using JS.Base.WS.API.Models.PersonProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JS.Base.WS.API.Controllers
{
    [RoutePrefix("api/common")]
    [Authorize]
    public class CommonController : ApiController
    {
        MyDBcontext db = new MyDBcontext();
        long currenntUserId = CurrentUser.GetId();

        [HttpGet]
        [Route("GetGenders")]
        public List<GenderDto> GetGenders()
        {
            List<GenderDto> genders = db.Genders.Where(x => x.IsActive == true).Select(y => new GenderDto
            {
                Id = y.Id,
                Description = y.Description,
                ShortName = y.ShortName
            }).ToList();

            return genders;
        }

        [HttpGet]
        [Route("GetLocatorsTypes")]
        public IHttpActionResult GetLocatorsTypes()
        {
            var result = db.LocatorTypes.Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
            }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetInfoCurrentUser")]
        public IHttpActionResult GetInfoCurrentUser()
        {
            var result = db.Users.Where(x => x.Id == currenntUserId).Select(x => new
            {
                UserName = x.UserName,
                Password = x.Password,
                Name = x.Name,
                SurName = x.Surname,
                EmailAddress = x.EmailAddress,
                Image = x.Image
            }).FirstOrDefault();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetInfoCurrentPerson")]
        public IHttpActionResult GetInfoCurrentPerson()
        {
            var currentUser = db.Users.Where(x => x.Id == currenntUserId).FirstOrDefault();

            if (currentUser?.PersonId > 0)
            {
                var result = db.People.Where(x => x.Id == currentUser.PersonId).Select(x => new
                {
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    SurName = x.Surname,
                    SecondSurname = x.secondSurname,
                    BirthDate = x.BirthDate,
                    FullName = x.FullName,
                    GenderId = x.GenderId,
                }).FirstOrDefault();

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetInfoCurrentLocators")]
        public IHttpActionResult GetInfoCurrentLocators()
        {
            var currentUser = db.Users.Where(x => x.Id == currenntUserId).FirstOrDefault();

            if (currentUser?.PersonId > 0)
            {
                var result = db.Locators.Where(x => x.PersonId == currentUser.PersonId).Select(x => new
                {
                    LocatorTypeId = x.LocatorTypeId,
                    LocatorTypeDescription = x.LocatorType.Description,
                    Description = x.Description,
                    IsMain = x.IsMain
                }).ToList();

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("UpdateInfoCurrentUser")]
        public IHttpActionResult UpdateInfoCurrentUser(InfoCurrentUser request)
        {
            Response response = new Response();

            try
            {
                var currentUser = db.Users.Where(x => x.Id == currenntUserId).FirstOrDefault();

                currentUser.UserName = request.UserName;
                currentUser.Password = request.Password;
                currentUser.Name = request.Name;
                currentUser.Surname = request.Password;
                currentUser.EmailAddress = request.EmailAddress;
                currentUser.LastModificationTime = DateTime.Now;
                currentUser.LastModifierUserId = currenntUserId;
                db.SaveChanges();

                response.Message = "Registro actualizado con exito";
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw(ex);
            }

            response.Code = "008";
            response.Message = "Petición no procesada";
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateInfoCurrentPerson")]
        public IHttpActionResult UpdateInfoCurrentPerson(InfoCurrentPerson request)
        {
            Response response = new Response();

            try
            {
                var currentUser = db.Users.Where(x => x.Id == currenntUserId).FirstOrDefault();

                if (currentUser?.PersonId > 0)
                {
                    var currentPerson = db.People.Where(x => x.Id == currentUser.PersonId).FirstOrDefault();

                    currentPerson.FirstName = request.FirstName;
                    currentPerson.secondSurname = request.SecondName;
                    currentPerson.Surname = request.SurName;
                    currentPerson.secondSurname = request.SecondSurname;
                    currentPerson.FullName = request.FirstName + " " + request.SecondName + " " + request.SurName + " " + request.SecondSurname;
                    currentPerson.BirthDate = request.BirthDate;
                    currentPerson.GenderId = request.GenderId;
                    currentPerson.LastModificationTime = DateTime.Now;
                    currentPerson.LastModifierUserId = currenntUserId;

                    db.SaveChanges();

                    response.Message = "Registro actualizado con exito";
                    return Ok(response);
                }
                else
                {
                    Person person = new Person();

                    person.FirstName = request.FirstName;
                    person.SecondName = request.SecondName;
                    person.Surname = request.SurName;
                    person.secondSurname = request.SecondSurname;
                    person.FullName = request.FirstName + " " + request.SecondName + " " + request.SurName + " " + request.SecondSurname;
                    person.BirthDate = request.BirthDate;
                    person.GenderId = request.GenderId;
                    person.CreationTime = DateTime.Now;
                    person.CreatorUserId = currenntUserId;
                    person.IsActive = true;
                    person.IsDeleted = false;

                    db.People.Add(person);
                    db.SaveChanges();

                    //Update currentUser
                    var newPerson = db.People.OrderByDescending(x => x.Id).FirstOrDefault();

                    currentUser.PersonId = newPerson.Id;
                    db.SaveChanges();

                    response.Message = "Registro actualizado con exito";
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            response.Code = "008";
            response.Message = "Petición no procesada";
            return Ok(response);
        }



        #region Models

        public class InfoCurrentUser
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string SurName { get; set; }
            public string EmailAddress { get; set; }
        }

        public class InfoCurrentPerson
        {
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string SurName { get; set; }
            public string SecondSurname { get; set; }
            public string BirthDate { get; set; }
            public string FullName { get; set; }
            public int GenderId { get; set; }
        }

        #endregion

    }
}
