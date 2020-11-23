using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class RACAPSocialWorkerViewModel
    {
        public int SocialWorkerId { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int IntakeId { get; set; }
        public string SocialWorkerName { get; set; }
        public string SocialWorkerSurname { get; set; }
        public string AccreditationRef { get; set; }
        public DateTime? AccreditationDate { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Province { get; set; }


    }
}
