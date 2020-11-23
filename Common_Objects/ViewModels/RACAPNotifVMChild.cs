using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPNotifVMChild
    {
        public int NotificationId { get; set; }
        public int PerSonId { get; set; }
        [Display(Name ="Added/Updated")]
        public int AddedUpdated { get; set; }
        public string From { get; set; }
        [Display(Name = "Child Ref No:")]
        public string ChildRefNo { get; set; }
        [Display(Name = "Child ID No:")]

        public string ChildId { get; set; }
        [Display(Name = "Child Name:")]

        public string ChildName { get; set; }
        [Display(Name = "Child Surname:")]
        public string ChildSurname { get; set; }

    }
}
