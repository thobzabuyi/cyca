using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;
using Common_Objects.ViewModels;

namespace Common_Objects.ViewModels
{
   public class CPROnlineNotificationsListViewModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<CPR_OnlineNotification__ChildDetails> ChildDetailsPublicSubmit { get; set; }
        public List<CPR_OnlineNotification__ChildDetails> ChildDetailsMandatorySybmit { get; set; }
        public List<CPR_OnlineNotification_RequestFeedback> RequestForFeedback { get; set; }
        public List<CPR_OnlineNotification__ChildDetails> GetChildDetailsMandatorySybmit()
        {
            return (from m in db.CPR_OnlineNotification__ChildDetails
                    where m.MandatoryReporter_Id != null
                    select m).ToList();
        }
        public List<CPR_OnlineNotification__ChildDetails> GetChildDetailsPublicSubmit()
        {
            return (from m in db.CPR_OnlineNotification__ChildDetails
                    where m.Reporter_Id != null
                    select m).ToList();
        }
        public List<CPR_OnlineNotification_RequestFeedback> GetRequestForFeedback()
        {
            return (from m in db.CPR_OnlineNotification_RequestFeedback
                    where m.isCompleted == false
                    select m).ToList();
        }
        public void AddCPR_OnlineNotification__ChildDetails(CPR_OnlineNotification__ChildDetails child)
        {
            db.CPR_OnlineNotification__ChildDetails.Add(child);
        }
        public void SaveInfo()
        {
            db.SaveChanges();
        }
        public void AddCPR_OnlineNotification_AlledgedOffender(CPR_OnlineNotification_AlledgedOffender offender)
        {
            db.CPR_OnlineNotification_AlledgedOffender.Add(offender);
        }
        public void AddCPR_OnlineNotification_FirstReporter(CPR_OnlineNotification_FirstReporter firstreporter)
        {
            db.CPR_OnlineNotification_FirstReporter.Add(firstreporter);
        }
        public void AddCPR_OnlineNotifications_Incedent(CPR_OnlineNotifications_Incedent incident)
        {
            db.CPR_OnlineNotifications_Incedent.Add(incident);
        }
        public void AddCPR_OnlineNotification_MandatoryReporter(CPR_OnlineNotification_MandatoryReporter reporter)
        {
            db.CPR_OnlineNotification_MandatoryReporter.Add(reporter);
        }
        public IQueryable<CPR_OnlineNotification__ChildDetails> GetCPR_OnlineNotification__ChildDetails()
        {
            return db.CPR_OnlineNotification__ChildDetails.Where(x => x.ReferenceNumber != null);
        }
        public void AddCPR_OnlineNotifications_Reporter(CPR_OnlineNotifications_Reporter Reporter)
        {
            db.CPR_OnlineNotifications_Reporter.Add(Reporter);
        }
        public void AddCPR_OnlineNotification_RequestFeedback(CPR_OnlineNotification_RequestFeedback newFeedback)
        {
            db.CPR_OnlineNotification_RequestFeedback.Add(newFeedback);
        }
    }
}
