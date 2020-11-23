using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class CPROnlineNotificationRequestFeedbackViewModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public int Feedback_Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ReferenceNumber { get; set; }
        public string Subject { get; set; }
        public string MessageDetails { get; set; }
        public bool isCompleted { get; set; }
        public CPR_OnlineNotification_RequestFeedback GetrequestDetails(int Id)
        {
            return db.CPR_OnlineNotification_RequestFeedback.Where(x => x.Feedback_Id == Id).FirstOrDefault();
        }
        public void SaveInfo()
        {
            db.SaveChanges();

        }
    }
}
