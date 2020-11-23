using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class CorrespondenceModel
    {
        public CPR_Correspondence GetSpecificCorrespondence(int correspondenceId)
        {
            CPR_Correspondence correspondence;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var correspondenceList = (from x in dbContext.CPR_Correspondence
                                          where x.CPR_Correnspondence_Id.Equals(correspondenceId)
                                          select x).ToList();

                correspondence = (from i in correspondenceList
                                  select i).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return correspondence;
        }

        public List<CPR_Correspondence> GetListOfCorrespondence()
        {
            List<CPR_Correspondence> correspondence;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var correspondenceList = (from x in dbContext.CPR_Correspondence
                                          select x).ToList(); ;

                correspondence = (from x in correspondenceList
                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return correspondence;
        }

        public List<CPR_Correspondence> GetListOfCorrespondence(int sentToPersonId)
        {
            List<CPR_Correspondence> correspondence;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var correspondenceList = (from x in dbContext.CPR_Correspondence
                                          where x.Sent_To_Person_Id.Equals(sentToPersonId)
                                          select x).ToList();

                correspondence = (from x in correspondenceList
                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return correspondence;
        }

        public List<CPR_Correspondence> GetListOfCorrespondence(int sentToPersonId, int sentByUserId)
        {
            List<CPR_Correspondence> correspondence;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var correspondenceList = (from x in dbContext.CPR_Correspondence
                                          where x.Sent_To_Person_Id.Equals(sentToPersonId)
                                          where x.Sent_By_User_Id.Equals(sentByUserId)
                                          select x).ToList();

                correspondence = (from x in correspondenceList
                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return correspondence;
        }

        public CPR_Correspondence CreateCorrespondence(int correspondenceLetterId, string letterSent, DateTime dateSent, int sentByUserId, int sentToPersonId)
        {
            CPR_Correspondence newCorrespondence;

            var dbContext = new SDIIS_DatabaseEntities();

            var correspondence = new CPR_Correspondence()
            {
                CPR_Correspondence_Letter_Id = correspondenceLetterId,
                Letter_Sent = letterSent,
                Date_Sent = dateSent,
                Sent_By_User_Id = sentByUserId,
                Sent_To_Person_Id = sentToPersonId
            };

            try
            {
                newCorrespondence = dbContext.CPR_Correspondence.Add(correspondence);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return newCorrespondence;
        }
    }
}
