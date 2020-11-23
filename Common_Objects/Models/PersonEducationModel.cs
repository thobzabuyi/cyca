using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class PersonEducationModel
    {
        public Person_Education GetSpecificPersonEducation(int personEducationId)
        {
            Person_Education personEducation;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEducationItems = (from p in dbContext.Person_Education_Entities
                                            where p.Person_Education_Id.Equals(personEducationId)
                                            select p).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                personEducation = (from p in personEducationItems
                                   select p).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return personEducation;
        }

        public List<Person_Education> GetListOfPersonEducationItems(bool showInActive, bool showDeleted)
        {
            List<Person_Education> listOfPersonEducationItems;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEducationItems = (from p in dbContext.Person_Education_Entities
                               where p.Is_Active || p.Is_Active.Equals(!showInActive)
                               where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                               select p).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfPersonEducationItems = (from p in personEducationItems
                                              select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersonEducationItems;
        }

        public List<Person_Education> GetListOfPersonEducationItemsForPerson(int personId, bool showInActive, bool showDeleted)
        {
            List<Person_Education> listOfPersonEducationItems;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personEducationItems = (from p in dbContext.Person_Education_Entities
                                            where p.Person_Id.Equals(personId)
                                            where p.Is_Active || p.Is_Active.Equals(!showInActive)
                                            where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                                            select p).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfPersonEducationItems = (from p in personEducationItems
                                              select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersonEducationItems;
        }

        public Person_Education CreatePersonEducation(int personId, int schoolId, int? gradeCompletedId, string yearCompleted, DateTime? dateLastAttended, string additionalInformation,  DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            Person_Education newPersonEducation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var personEducation = new Person_Education()
                {
                    Person_Id = personId,
                    School_Id = schoolId,
                    Grade_Completed_Id = gradeCompletedId,
                    Year_Completed = yearCompleted,
                    Date_Last_Attended = dateLastAttended,
                    Additional_Information = additionalInformation,
                    Date_Created = dateCreated,
                    Created_By = createdBy,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted
                };

                try
                {
                    newPersonEducation = dbContext.Person_Education_Entities.Add(personEducation);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return newPersonEducation;
        }

        public Person_Education EditPersonEducation(int personEducationId, int schoolId, int? gradeCompletedId, string yearCompleted, DateTime? dateLastAttended, string additionalInformation, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Person_Education editPersonEducation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editPersonEducation = (from p in dbContext.Person_Education_Entities
                                           where p.Person_Education_Id.Equals(personEducationId)
                                           select p).FirstOrDefault();

                    if (editPersonEducation == null) return null;

                    editPersonEducation.School_Id = schoolId;
                    editPersonEducation.Grade_Completed_Id = gradeCompletedId;
                    editPersonEducation.Year_Completed = yearCompleted;
                    editPersonEducation.Date_Last_Attended = dateLastAttended;
                    editPersonEducation.Additional_Information = additionalInformation;
                    editPersonEducation.Date_Last_Modified = dateLastModified;
                    editPersonEducation.Modified_By = modifiedBy;
                    editPersonEducation.Is_Active = isActive;
                    editPersonEducation.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editPersonEducation;
        }
    }
}
