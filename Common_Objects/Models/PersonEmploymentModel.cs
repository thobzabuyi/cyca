using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class PersonEmploymentModel
    {
        public Person_Employment GetSpecificPersonEmployment(int personEmploymentId)
        {
            Person_Employment personEmployment;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEmploymentItems = (from p in dbContext.Person_Employment_Entities
                                             where p.Person_Employment_Id.Equals(personEmploymentId)
                                             select p).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                personEmployment = (from p in personEmploymentItems
                                    select p).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return personEmployment;
        }

        public List<Person_Employment> GetListOfPersonEmploymentItems(bool showInActive, bool showDeleted)
        {
            List<Person_Employment> listOfPersonEmploymentItems;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEmploymentItems = (from p in dbContext.Person_Employment_Entities
                                             where p.Is_Active || p.Is_Active.Equals(!showInActive)
                                             where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                                             select p).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfPersonEmploymentItems = (from p in personEmploymentItems
                                               select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersonEmploymentItems;
        }

        public List<Person_Employment> GetListOfPersonEmploymentItemsForPerson(int personId, bool showInActive, bool showDeleted)
        {
            List<Person_Employment> listOfPersonEmploymentItems;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEmploymentItems = (from p in dbContext.Person_Employment_Entities
                                             where p.Person_Id.Equals(personId)
                                             where p.Is_Active || p.Is_Active.Equals(!showInActive)
                                             where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                                             select p).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfPersonEmploymentItems = (from p in personEmploymentItems
                                               select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersonEmploymentItems;
        }

        public Person_Employment CreatePersonEmployment(int personId, int? natureOfEmploymentId, 
            string occupation, int? incomeRangeId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted, string nameOfEmployer)
        {
            Person_Employment newPersonEmployment;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var personEmployment = new Person_Employment()
                {
                    Person_Id = personId,                    
                    Nature_Of_Employment_Id = natureOfEmploymentId,
                    Occupation = occupation,
                    Income_Range_Id = incomeRangeId,
                    Date_Created = dateCreated,
                    Created_By = createdBy,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted,
                    NameOfEmployer = nameOfEmployer
                };

                try
                {
                    newPersonEmployment = dbContext.Person_Employment_Entities.Add(personEmployment);
                    dbContext.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            return newPersonEmployment;
        }

        public Person_Employment EditPersonEmployment(int personEmploymentId,  int? natureOfEmploymentId, string occupation, int? incomeRangeId, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted ,string NameOfEmployer)
        {
            Person_Employment editPersonEmployment;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editPersonEmployment = (from p in dbContext.Person_Employment_Entities
                                            where p.Person_Employment_Id.Equals(personEmploymentId)
                                            select p).FirstOrDefault();

                    if (editPersonEmployment == null) return null;

                    editPersonEmployment.NameOfEmployer = NameOfEmployer;
                    editPersonEmployment.Nature_Of_Employment_Id = natureOfEmploymentId;
                    editPersonEmployment.Occupation = occupation;
                    editPersonEmployment.Income_Range_Id = incomeRangeId;
                    editPersonEmployment.Date_Last_Modified = dateLastModified;
                    editPersonEmployment.Modified_By = modifiedBy;
                    editPersonEmployment.Is_Active = isActive;
                    editPersonEmployment.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editPersonEmployment;
        }

    }
}
