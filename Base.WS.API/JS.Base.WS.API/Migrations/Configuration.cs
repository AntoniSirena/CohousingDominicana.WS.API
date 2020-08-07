namespace JS.Base.WS.API.Migrations
{
    using JS.Base.WS.API.Models;
    using JS.Base.WS.API.Models.Authorization;
    using JS.Base.WS.API.Models.Domain;
    using JS.Base.WS.API.Models.Domain.AccompanyingInstrument;
    using JS.Base.WS.API.Models.PersonProfile;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JS.Base.WS.API.DBContext.MyDBcontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JS.Base.WS.API.DBContext.MyDBcontext context)
        {
            //Locator Types
            context.LocatorTypes.AddOrUpdate(
              p => p.Description,
              new LocatorType { Code = "01", Description = "Direccion" },
              new LocatorType { Code = "02", Description = "Telefono Resid" },
              new LocatorType { Code = "03", Description = "Cellular" },
              new LocatorType { Code = "04", Description = "Correo" },
              new LocatorType { Code = "05", Description = "Persona" }
              );

            //System user
            //context.Users.AddOrUpdate(
            //  p => p.UserName,
            //  new User { UserName = "system", Password = "system*123", Name = "System", Surname = "System", PersonId = 6, EmailAddress = "system@hotmail.com", StatusId = 1, CreationTime = DateTime.Now, CreatorUserId = 1, IsActive = true, IsDeleted = false },
            //  new User { UserName = "admin", Password = "admin*123", Name = "Admin", Surname = "Admin", PersonId = 6, EmailAddress = "admin@hotmail.com", StatusId = 1, CreationTime = DateTime.Now, CreatorUserId = 1, IsActive = true, IsDeleted = false }
            //);

            context.UserStatus.AddOrUpdate(
                x => x.ShortName,
                new UserStatus { ShortName = "Active", Description = "Activo", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "rgb(52, 212, 146)" },
                new UserStatus { ShortName = "Inactive", Description = "Inactivo", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "rgb(241, 81, 81)" },
                new UserStatus { ShortName = "PendingToActive", Description = "Pendiente de activar", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "rgb(243, 183, 71)" }
                );

            context.Genders.AddOrUpdate(
                x => x.Description,
                new Gender { ShortName = "M", Description = "Maculino", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new Gender { ShortName = "F", Description = "Femenino", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now }
                );

            //Document Types
            context.DocumentTypes.AddOrUpdate(
                x => x.Description,
                new DocumentType { ShortName = "C�dula", Description = "C�dula", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new DocumentType { ShortName = "Pasaporte", Description = "Pasaporte", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new DocumentType { ShortName = "RNC", Description = "RNC", IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now }
                );

            //RequestStatuses
            context.RequestStatus.AddOrUpdate(
                x => x.ShortName,
                new RequestStatus { ShortName = "InProcess", Name = "En proceso", Colour = "rgb(52, 212, 146)", AllowEdit = true, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new RequestStatus { ShortName = "Completed", Name = "Completado", Colour = "rgb(59, 159, 199)", AllowEdit = false, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new RequestStatus { ShortName = "PendingToApprove", Name = "Pendiente de aprobar", Colour = "rgb(52, 135, 243)", AllowEdit = false, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new RequestStatus { ShortName = "Approved", Name = "Aprobado", Colour = "rgb(157, 68, 230)", AllowEdit = false, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new RequestStatus { ShortName = "Cancelad", Name = "Cancelado", Colour = "rgb(243, 41, 91)", AllowEdit = false, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now },
                new RequestStatus { ShortName = "InObservation", Name = "En observaci�n", Colour = "rgb(111, 219, 205)", AllowEdit = false, IsActive = true, CreatorUserId = 1, CreationTime = DateTime.Now }

                );

            //Variables
            context.Variables.AddOrUpdate(
                x => x.ShortName,
                new Variable { ShortName = "A", Description = "A", Title = "Planificaci�n" },
                new Variable { ShortName = "B", Description = "B", Title = "Dominio de contenidos" },
                new Variable { ShortName = "C", Description = "C", Title = "Utilizaci�n de Estrategias y Actividades" },
                new Variable { ShortName = "D", Description = "D", Title = "Utilizaci�n de Recursos Pedag�gicos" },
                new Variable { ShortName = "E", Description = "E", Title = "Procesos Evaluativos" },
                new Variable { ShortName = "F", Description = "F", Title = "Clima del Aula" },
                new Variable { ShortName = "G", Description = "G", Title = "Reflexi�n de la Pr�ctica" },
                new Variable { ShortName = "H", Description = "H", Title = "Relaciones con los Padres, las Madres, los Tutores y la Comunidad" }
                );

            //VariableDetails

            int aId = context.Variables.Where(x => x.ShortName == "A").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "A1", Description = "Planificaci�n de acuerdo al enfoque por competencias sugerido por el Ministerio de Educaci�n", VariableID = aId },
                new VariableDetail { Number = "A2", Description = "Concordancia entre lo planificado y lo realizado en las clases", VariableID = aId },
                new VariableDetail { Number = "A3", Description = "Planificaci�n atendiendo a la diversidad del aula", VariableID = aId }
                );

            int bId = context.Variables.Where(x => x.ShortName == "B").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "B1", Description = "Dominio de los contenidos del �rea durante el desarrollo de la clase", VariableID = bId },
                new VariableDetail { Number = "B2", Description = "Claridad de ideas cuando se comunica de forma oral o escrita", VariableID = bId },
                new VariableDetail { Number = "B3", Description = "Relaci�n de los contenidos del �rea observada con los de las otras �reas del conocimiento", VariableID = bId },
                new VariableDetail { Number = "B4", Description = "Atenci�n al uso pertinente por parte de los ni�os del vocabulario del �rea que corresponde al grado.", VariableID = bId },
                new VariableDetail { Number = "B5", Description = "Desarrollo de los procesos de comprensi�n y producci�n de la lectura y escritura a trav�s de los textos propuestos en el curr�culo del grado", VariableID = bId },
                new VariableDetail { Number = "B6", Description = "Uso del enfoque funcional, comunicativo y textual en el desarrollo de la clase.", VariableID = bId },
                new VariableDetail { Number = "B7", Description = "Uso del lenguaje matem�tico asociado al tema de la clase, si corresponde", VariableID = bId }
                );

            int cId = context.Variables.Where(x => x.ShortName == "C").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "C1", Description = "Realizaci�n de actividades relacionadas con los intereses y las necesidades de los ni�os", VariableID = cId },
                new VariableDetail { Number = "C2", Description = "Aprovechamiento de los saberes previos de los ni�os", VariableID = cId },
                new VariableDetail { Number = "C3", Description = "Asignaciones significativas", VariableID = cId },
                new VariableDetail { Number = "C4", Description = "Uso de metodolog�a centrada en el estudiante y procesos de aprendizaje constructivistas en la clase para el desarrollo de competencias del �rea", VariableID = cId },
                new VariableDetail { Number = "C5", Description = "Participaci�n y discusi�n de ideas y apertura a las opiniones de los ni�os", VariableID = cId },
                new VariableDetail { Number = "C6", Description = "Involucramiento de los ni�os en el trabajo escolar", VariableID = cId },
                new VariableDetail { Number = "C7", Description = "Construcci�n del conocimiento matem�tico a partir de la manipulaci�n de objetos concretos", VariableID = cId },
                new VariableDetail { Number = "C8", Description = "objetos concretos, la experimentaci�n, la indagaci�n, discusi�n de ideas y manejo de informaci�n, cuando aplica.", VariableID = cId },
                new VariableDetail { Number = "C9", Description = "Utilizaci�n de espacios para actividades cient�ficas al aire libre, cuando aplica", VariableID = cId }
                );

            int dId = context.Variables.Where(x => x.ShortName == "D").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "D1", Description = "Aula letrada con producciones de los ni�os y/o del docente atendiendo a los contenidos curriculares trabajados", VariableID = dId },
                new VariableDetail { Number = "D2", Description = "Recursos pedag�gicos pertinentes al �rea que se imparte", VariableID = dId },
                new VariableDetail { Number = "D3", Description = "Uso de Tecnolog�as de Informaci�n y Comunicaci�n (TIC)", VariableID = dId },
                new VariableDetail { Number = "D4", Description = "Biblioteca de uso funcional", VariableID = dId },
                new VariableDetail { Number = "D5", Description = "Equipamiento y uso del laboratorio de Ciencias para actividades experimentales de Ciencias de la Naturaleza, si aplica", VariableID = dId }
                );

            int eId = context.Variables.Where(x => x.ShortName == "E").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "E1", Description = "Utilizaci�n de instrumentos pertinentes para recabar informaci�n sobre el desempe�o de los estudiantes", VariableID = eId },
                new VariableDetail { Number = "E2", Description = "An�lisis de las producciones de los estudiantes", VariableID = eId }
                );

            int fId = context.Variables.Where(x => x.ShortName == "F").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "F1", Description = "Disciplina funcional de trabajo", VariableID = fId },
                new VariableDetail { Number = "F2", Description = "Interacci�n y actitudes en el aula", VariableID = fId },
                new VariableDetail { Number = "F3", Description = " Organizaci�n del mobiliario del aula", VariableID = fId },
                new VariableDetail { Number = "F4", Description = "Manejo del tiempo pedag�gico durante el desarrollo de la clase", VariableID = fId }
                );

            int gId = context.Variables.Where(x => x.ShortName == "G").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "G1", Description = "Uso de los resultados de las observaciones para el an�lisis de su propia pr�ctica", VariableID = gId },
                new VariableDetail { Number = "G2", Description = "Actitud hacia el acompa�amiento", VariableID = gId },
                new VariableDetail { Number = "G3", Description = "Nivel de iniciativa para participar en grupos de estudios y/o proyectos de investigaci�n - acci�n", VariableID = gId }
                );

            int hId = context.Variables.Where(x => x.ShortName == "H").Select(x => x.Id).FirstOrDefault();
            context.VariableDetails.AddOrUpdate(
                x => x.Number,
                new VariableDetail { Number = "H1", Description = "Involucramiento a los padres, las madres o los tutores en el desarrollo de actividades de las clases", VariableID = hId },
                new VariableDetail { Number = "H2", Description = "Comunicaci�n con los padres, las madres o los tutores acerca del progreso de sus ni�os", VariableID = hId },
                new VariableDetail { Number = "H3", Description = "Elaboraci�n de proyectos de aula que potencien la interacci�n de los ni�os con la comunidad", VariableID = hId }
                );

            context.CommentsRevisedDocumentsDefs.AddOrUpdate(
               x => x.ShortName,
               new CommentsRevisedDocumentsDef { ShortName = "A", Description = "Portafolio Docente" },
               new CommentsRevisedDocumentsDef { ShortName = "B", Description = "Diario Reflexivo del docente" },
               new CommentsRevisedDocumentsDef { ShortName = "C", Description = "Diario Reflexivo del docente" },
               new CommentsRevisedDocumentsDef { ShortName = "D", Description = "Evidencias de investigaci�nacci�n" },
               new CommentsRevisedDocumentsDef { ShortName = "E", Description = "Libros de consulta usados (nombre y editora)" },
               new CommentsRevisedDocumentsDef { ShortName = "F", Description = "Otros elementos" }
               );
        }
    }
}
