using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common_Objects.Models;
using System.ComponentModel.DataAnnotations;

namespace Common_Objects.ViewModels
{
   public class CPREnquiriesByEmployerViewModel
    {
       public CPR_EnquiriesByEmployer EnquiriesByEmployer { get; set; }
       public CPR_EnquiriesEmployerDetails EmployerDetails { get; set; }
       public CPR_EquiriesPersonDetails PersonDetails { get; set; }

        public Address PhysicalAddress_1 { get; set; }

        public List<CPR_EquiriesPersonDetails> EnquiredPersons { get; set; }

       public int CurrentUserId { get; set; }
       public string CurrentUserSurname { get; set; }
       public string CurrentUserName { get; set; }
       public string CurrentUserDepartment { get; set; }
       public string CurrentUserTelephone { get; set; }
       public string CurrentUserFax { get; set; }
       public string CurrentUserEmail { get; set; }
       public string CurrentUserMobile { get; set; }

        public int? SelectedProvinceId_1 { get; set; }
        public int? SelectedDistrictId_1 { get; set; }
        public int? Selected_Local_Municipality_Id_1 { get; set; }
        public int? SelectedTownId_1 { get; set; }

        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }

        [Display(Name = "Municipality")]
        public int Selected_Local_Municipality_Id { get; set; }
        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Local_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Magistrate")]
        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        [Display(Name = "Province")]
        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }
    }
}
