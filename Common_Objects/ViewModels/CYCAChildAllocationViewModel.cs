using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CYCAChildAllocationViewModel
    {
        public string Child_Status { get; set; }
        public string ImgUrl { get; set; }
        public string GangMembership { get; set; }
        public int Child_Allocation_Id { get; set; }
        public int? Person_Id { get; set; }
        public string Child_First_Name { get; set; }
        public string Child_Last_First_Name { get; set; }
        public string Child_ID_No { get; set; }
        public string Child_Name { get; set; }
        public string OtherGangDescription { get; set; }

        public int? User_Id { get; set; }
        public string LoggedInUserName { get; set; }

        //Users Allocated To Venue
        public int Team_Allocation_Id { get; set; }
        public string Start_Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Date { get; set; }
        public string End_Time { get; set; }
        public string Shift { get; set; }

        //Movement 
        public int Movement_Id { get; set; }
        public string CareWorker_Name { get; set; }
        public int? Venue_Id { get; set; }
        public string Venue_Name { get; set; }
        public int? Moved_By { get; set; }
        public string Moved_To_Name { get; set; }
        public string Date_Moved { get; set; }
        public string Time_Moved { get; set; }
        public DateTime? Checked_In_Date { get; set; }

        //Transfer
        public int? Transfer_Id { get; set;}
        public DateTime? Date_Transferred { get; set; }
        public DateTime? Time_Transferred { get; set; }
        public int Transferred_By { get; set; }
        public int Transferred_To { get; set; }
        public string Area { get; set; }
        public int? Transfer_Status_Id { get; set; }
        [Display(Name ="Decline Reason")]
        [Required(ErrorMessage ="This field is required")]
        public string Decline_Reason { get; set; }
        public string selectedTransferStatus { get; set; }
        public string transferredbyName { get; set; }

    }
    public class CYCAChildMovementTransfer
    {
        public int Person_Id { get; set; }
        public int Venue_Id { get; set; }
        public int TransferredTo_Id { get; set; }
        public int AssignedTo_Id { get; set; }
        public int Child_Allocation_Id { get; set; }
        public int Admission_Id { get; set; }
        public string Role { get; set; }

    }
}
