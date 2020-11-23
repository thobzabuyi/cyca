using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CPRQuicklistViewModel
    {
        public bool is_Filtered { get; set; }
        public int? pageNumber { get; set; }

        public List<CaseType> Case_Type_List 
        {
            get
            {
                var caseTypes = new List<CaseType>();
                caseTypes.Add(new CaseType() { Id = 1, Description = "Closed" });
                caseTypes.Add(new CaseType() { Id = 2, Description = "Registered" });
                caseTypes.Add(new CaseType() { Id = 3, Description = "Unmonitored" });

                return caseTypes;
            }
        }
        public int? Selected_Case_Type_Id { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }

        public int? Selected_Province_Id { get; set; }
        public int? Selected_District_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        [Display(Name = "District")]
        public SelectList District_List
        {
            get
            {
                var districtModel = new DistrictModel();
                var listOfDistricts = districtModel.GetListOfDistricts(Selected_Province_Id ?? -1);

                var districtList = (from d in listOfDistricts
                                    select new SelectListItem()
                                    {
                                        Text = d.Description,
                                        Value = d.District_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = d.District_Id.Equals(Selected_District_Id)
                                    }).ToList();

                var selectList = new SelectList(districtList, "Value", "Text", Selected_District_Id);

                return selectList;
            }
        }

    }

    public class CaseType
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
