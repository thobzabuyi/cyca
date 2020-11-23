using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPNotifVMParent
    {
        public int NotificationId { get; set; }
        public int PerSonId { get; set; }

        [Display(Name ="Added/Updated")]
        public int AddedUpdated { get; set; }
        public string From { get; set; }
        [Display(Name = "Parent Ref No:")]
        public string ParentRefNo { get; set; }
        [Display(Name = "Parent ID No:")]

        public string ParentId { get; set; }
        [Display(Name = "Parent Name:")]

        public string ParentName { get; set; }
        [Display(Name = "Parent Surname:")]

        public string ParentSurname { get; set; }
    }
}
