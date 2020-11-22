﻿using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Response.User;
using JS.Base.WS.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using JS.Base.WS.API.DTO.SP_Parameter;
using System.Data.SqlClient;
using JS.Utilities;
using JS.Base.WS.API.Global;

namespace JS.Base.WS.API.Services
{
    public class UserService
    {
        private MyDBcontext db = new MyDBcontext();
        private long currentUserId = CurrentUser.GetId();

        public List<UserStatus> GetUserStatuses()
        {
            var result = db.UserStatus.Where(y => y.IsActive == true & y.ShowToCustomer == true).Select(x => new UserStatus
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Description = x.Description,
                Colour = x.Colour,
            }).ToList();

            return result;
        }

        public UserDetails GetUserDetails(long userId)
        {
            var result = new UserDetails();

            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (user.PersonId != null)
            {
                result = db.Users.Where(x => x.Id == userId).Select(x => new UserDetails
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Name = x.Name,
                    Surname = x.Surname,
                    EmailAddress = x.EmailAddress,
                    PhoneNumber = x.PhoneNumber,
                    Type = x.UserType.Description,
                    Image = x.Image,
                    LastLoginTime = x.LastLoginTime,
                    LastLoginTimeEnd = x.LastLoginTimeEnd,
                    IsOnline = x.IsOnline,
                    Role = db.UserRoles.Where(p => p.UserId == x.Id).Select(i => new RoleDto
                    {
                        Description = i.Role.Description,
                        Parent = i.Role.Parent,
                    }).FirstOrDefault(),
                    Person = db.People.Where(y => y.Id == x.PersonId).Select(y => new Person
                    {
                        FirstName = y.FirstName,
                        SecondName = y.SecondName,
                        Surname = y.Surname,
                        secondSurname = y.secondSurname,
                        FullName = y.FullName,
                        BirthDate = y.BirthDate,
                        Gender = y.Gender.Description,
                        PersonType = db.UserRoles.Where(m => m.UserId == x.Id).Select(m => m.Role.PersonType.Description).FirstOrDefault(),
                        DocumentNumber = y.DocumentNumber == null ? string.Empty : y.DocumentNumber,
                        DocumentDescription = y.DocumentType.Description == null ? string.Empty : y.DocumentType.Description,
                        Locators = db.Locators.Where(z => z.PersonId == y.Id).Select(z => new Locators
                        {
                            Id = z.Id,
                            Description = z.Description,
                            Type = z.LocatorType.Description,
                            IsMain = z.IsMain,
                        }).OrderByDescending(m => m.Id).ToList(),

                    }).FirstOrDefault()

                }).FirstOrDefault();
            }
            else
            {
                result = new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    EmailAddress = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.UserType.Description,
                    Image = user.Image,
                    LastLoginTime = user.LastLoginTime,
                    LastLoginTimeEnd = user.LastLoginTimeEnd,
                    IsOnline = user.IsOnline,
                    Role = db.UserRoles.Where(p => p.UserId == user.Id).Select(i => new RoleDto
                    {
                        Description = i.Role.Description,
                        Parent = i.Role.Parent,
                    }).FirstOrDefault(),
                    Person = new Person
                    {
                        FirstName = string.Empty,
                        SecondName = string.Empty,
                        Surname = string.Empty,
                        secondSurname = string.Empty,
                        FullName = string.Empty,
                        BirthDate = string.Empty,
                        Gender = string.Empty,
                        DocumentNumber = string.Empty,
                        DocumentDescription = string.Empty,
                        Locators = new List<Locators>()
                        {
                            new Locators
                            {
                               Description = string.Empty,
                               Type = string.Empty,
                               IsMain = false
                            }
                        }
                    },

                };
            }

            return result;
        }

        public bool UpdateUserLogInOut(bool isLogIn, string userName, long userId)
        {
            bool result = true;

            if (isLogIn)
            {
                var user = db.Users.Where(x => x.UserName == userName || x.EmailAddress == userName).FirstOrDefault();
                user.LastLoginTime = DateTime.Now;
                user.IsOnline = true;

                db.SaveChanges();
            }
            else
            {
                var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    user.LastLoginTimeEnd = DateTime.Now;
                    user.IsOnline = false;
                }

                db.SaveChanges();
            }

            return result;
        }

        public ValidateUserName ValidateUserName(string userName)
        {
            try
            {
                var response = ExecuteSP.ExecuteStoredProcedure<ValidateUserName>("SP_ValidateUserName", Constants.ConnectionStrings.JSBase,
                    new SqlParameter[]{
                    new SqlParameter("@UserName", userName),
                    },
                    reader =>
                    {
                        return new ValidateUserName
                        {
                            UserNameExist = (bool)reader["UserNameExist"],                           
                        };
                    });

                return response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el nombre de usuario" + ex.Message);
            }
        }
    }
}