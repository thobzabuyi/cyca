using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace Common_Objects.Models
{
    public class CYCAPersonModel
    {
        public Person GetSpecificPerson(int personId)
        {
            Person person;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var persons = (from p in dbContext.Persons
                               where p.Person_Id.Equals(personId)
                               select p).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                person = (from p in persons
                          select p).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return person;
        }

        #region CYCA SEARCH
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<Person> GetListOfPersonsPCM(bool showInActive, bool showDeleted)
        {
            List<Person> listOfPersons;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {

                //var persons = (from ass in db.Intake_Assessments
                //               join client in db.Clients on ass.Client_Id equals client.Client_Id
                //               join p in db.Persons on client.Person_Id equals p.Person_Id
                //               join subcat in db.Problem_Sub_Categories on ass.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id
                //               where (p.Is_Active || p.Is_Active.Equals(!showInActive))
                //              && (!p.Is_Deleted || p.Is_Deleted.Equals(showDeleted))
                //               //&&(subcat.Problem_Sub_Category_Id == 22)
                //               select p).ToList().Distinct();

                var persons = (from p in dbContext.Persons
                               join t in dbContext.Clients on p.Person_Id equals t.Person_Id
                               join q in dbContext.Intake_Assessments on t.Client_Id equals q.Client_Id
                               where p.Is_Active || p.Is_Active.Equals(!showInActive)
                               && !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)                              
                               select p).ToList().Distinct();



                listOfPersons = (from p in persons
                                 select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersons;
        }
        #endregion
    }
}
