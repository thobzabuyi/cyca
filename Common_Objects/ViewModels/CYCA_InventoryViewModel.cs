using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
     public class CYCA_InventoryViewModel
    {
        public int Inventory_Id { get; set; }
        public int Client_Id { get; set; }
        public int Facility_Id { get; set; }
        public int Admission_Id { get; set; }
        public int Inventory_Type_Id { get; set; }
        public string Item_Type { get; set; }
        public string Item_Color { get; set; }
        public int Item_Quantity { get; set; }
        public string Item_Description { get; set; }
        public DateTime Date_Handed_In { get; set; }
        public int Item_Handed_By { get; set; }
        public int Item_Handed_To { get; set; }
        public int Return_Status_Id { get; set; }
        public DateTime Date_Returned { get; set; }
        public int Quantity_Returned { get; set; }
        public int Returned_By { get; set; }
        public string Reason_Not_Returned { get; set; }
        public DateTime Date_Created { get; set; }
        public int Created_By { get; set; }
        public DateTime Date_Last_Modified { get; set; }
        public int Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
    }
}
