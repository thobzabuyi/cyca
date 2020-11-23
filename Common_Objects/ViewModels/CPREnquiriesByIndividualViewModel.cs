using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;
using System.Web.Mvc;
using System.Globalization;

namespace Common_Objects.ViewModels
{
    public class CPREnquiriesByIndividualViewModel
    {
        public CPR_EnquiriesByIndividual EnquiriesByIndividual { get; set; }
        //public CPR_EnquiriesEmployerDetails EmployerDetails { get; set; }
        public CPR_EquiriesPersonDetails PersonDetails { get; set; }

        public int CurrentUserId { get; set; }
        public string CurrentUserSurname { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentUserDepartment { get; set; }
        public string CurrentUserTelephone { get; set; }
        public string CurrentUserFax { get; set; }
        public string CurrentUserEmail { get; set; }
        public string CurrentUserMobile { get; set; }

        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }
    }
}
