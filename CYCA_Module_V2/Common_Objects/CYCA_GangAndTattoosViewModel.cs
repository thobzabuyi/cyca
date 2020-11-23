//using Common_Objects.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Common_Objects.ViewModels;

//namespace CYCA_Module_V2.Common_Objects
//{
//    public class CYCA_GangAndTattoosViewModel
//    {
       
//        public List<CYCA_DynamicDataModel> dynamicDataModels { get; set; }
//        public int ChildId { get; set; }
//        public List<CYCA_DynamicDataBaseModel> cYCA_DynamicDataBaseModel { get; set; }

//        public List<IntakeDataViewModel> IntakeDataViewModels { get; set; }
//        public int PersonId { get; set; }

//        public string ImgUrl { get; set; }
//        public bool CanAdmit { get; set; }
//        public bool CanDischarge { get; set; }
//        public string AdmittedAt { get; set; }
//        public Reception_Register ReceptionRegister { get; set; }
//        public Person Person { get; set; }
//        public Address PhysicalAddress { get; set; }
//        public Address PostalAddress { get; set; }

//        //PERSONAL/FACILITY INVENTORY 
//        public int Inventory_Id { get; set; }
//        public int Person_Id { get; set; }
//        public int Facility_Id { get; set; }
//        public int Admission_Id { get; set; }

//        [Required(ErrorMessage = "This field is Required")]
//        public int? Inventory_Type_Id { get; set; }

//        [Display(Name = "Inventory Type")]
//        public string selectedInventoryType { get; set; }

//        [Display(Name = "Item Type")]
//        [StringLength(30, ErrorMessage = "Text entered is too long")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Item_Type { get; set; }

//        [Display(Name = "Item Color")]
//        [StringLength(15, ErrorMessage = "Text entered is too long")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Item_Color { get; set; }

//        [Display(Name = "Quantity")]
//        [RegularExpression(@"^\d+$", ErrorMessage = "Please type number")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Item_Quantity { get; set; }

//        [Display(Name = "Item Description")]
//        public string Item_Description { get; set; }

//        [Display(Name = "Date Handed in/Date Issued")]
//        [Required(ErrorMessage = "This field is Required")]
//        public DateTime Date_Handed_In { get; set; }

//        [Display(Name = "Date handed In")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Date_Handed_Inn { get; set; }

//        public int Item_Handed_By { get; set; }
//        public int? Item_Handed_To { get; set; }

//        [Display(Name = "Item(s) Received/Issued by")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string selectedUser { get; set; }

//        [Required(ErrorMessage = "This field is Required")]
//        public int? Return_Status_Id { get; set; }

//        [Display(Name = "Return Status")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string selectedReturnStatus { get; set; }

//        [Display(Name = "Date Returned")]
//        [Required(ErrorMessage = "This field is Required")]
//        public DateTime? Date_Returned { get; set; }

//        [Display(Name = "Date Returned")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Date_Returnedd { get; set; }

//        [Display(Name = "Quantity Returned")]
//        [RegularExpression(@"^\d+$", ErrorMessage = "Please type number")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Quantity_Returned { get; set; }

//        [Display(Name = "Returned By")]
//        [Required(ErrorMessage = "This field is Required")]
//        public int? Returned_By { get; set; }

//        [Display(Name = "Returned/ Accepted By")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string selectedReturnedBy { get; set; }

//        [Display(Name = "Reason (if not returned)")]
//        [Required(ErrorMessage = "This field is Required")]
//        public string Reason_Not_Returned { get; set; }

//        public DateTime Date_Created { get; set; }
//        public int Created_By { get; set; }
//        public DateTime Date_Last_Modified { get; set; }
//        public int Modified_By { get; set; }
//        public bool Is_Active { get; set; }
//        public bool Is_Deleted { get; set; }

//    }
//}