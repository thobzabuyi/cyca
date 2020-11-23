using Common_Objects.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class NisisSiteListViewModel
    {
        [Display(Name = "Site Name")]
        public string Search_Site_Name { get; set; }

        public int Search_Province_Id { get; set; }
        public int Search_Municipality_Id { get; set; }
        public int Search_Local_Municipality_Id { get; set; }
        public int Search_Ward_Id { get; set; }
        public int Search_Profiling_Tool_Id { get; set; }

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
                                     Selected = x.Province_Id.Equals(Search_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Search_Province_Id);

                return selectList;
            }
        }

        [Display(Name = "District Municipality")]
        public SelectList Municipality_List
        {
            get
            {
                var municipalityModel = new DistrictModel();
                var listOfMunicipalities = municipalityModel.GetListOfDistricts(Search_Province_Id);

                var municipalities = (from x in listOfMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Search_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Search_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Search_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Search_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Search_Local_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Ward")]
        public SelectList Ward_List
        {
            get
            {
                var nisisWardModel = new NisisWardModel();
                var listOfNisisWards = nisisWardModel.GetListOfNisisWards(false, false, Search_Local_Municipality_Id);

                var nisisWards = (from x in listOfNisisWards
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Ward_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Ward_Id.Equals(Search_Ward_Id)
                                  }).ToList();

                var selectList = new SelectList(nisisWards, "Value", "Text", Search_Ward_Id);

                return selectList;
            }
        }

        public List<NISIS_Site> Nisis_Sites;
    }
}
