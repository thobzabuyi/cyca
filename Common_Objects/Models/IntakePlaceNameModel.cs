using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class IntakePlaceNameModel
    {
        private SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        public List<Country> GetCountries()
        {
            return (from c in dbContext.Countries
                    select c).ToList();
        }

        #region dropdowns

        #endregion
        #region Provinces
            public List<Province> GetProvinces()
            {
                return (from p in dbContext.Provinces
                        select p).ToList();

            }

            public Province GetProvince(int Id)
            {
                return (from p in dbContext.Provinces
                        where p.Province_Id == Id
                        select p).FirstOrDefault();

            }

            public Province EditProvince(int Id, string Province, string Abbreviation)
            {
                var ProvinceObj = (from p in dbContext.Provinces
                                   where p.Province_Id == Id
                                   select p).FirstOrDefault();
                ProvinceObj.Description = Province;
                ProvinceObj.Abbreviation = Abbreviation;
                dbContext.SaveChanges();
                return ProvinceObj;
            }

            public Province CreateProv(string Province, string Abbr)
            {
                var ProvinceObj = (from p in dbContext.Provinces
                                   where p.Description.Contains(Province)
                                   select p).FirstOrDefault();
                if (ProvinceObj == null)
                {
                    var newProvince = new Province();
                    newProvince.Description = Province;
                    newProvince.Abbreviation = Abbr;
                    dbContext.Provinces.Add(newProvince);
                    dbContext.SaveChanges();
                    return newProvince;
                }
                else
                {
                    return null;
                }

            }

            public void DeleteProv(int Id)
            {
                var DelObj = (from a in dbContext.Provinces
                              where a.Province_Id == Id
                              select a).FirstOrDefault();
                dbContext.Provinces.Remove(DelObj);
                dbContext.SaveChanges();
            }

        #endregion

        #region District

            public List<District> GetDistricts(string SearchProvince, string SearchDistrict)
            {

                var Result = (from a in dbContext.Districts
                                  orderby a.Description
                                  select a).ToList();

                if ((SearchProvince != null&& SearchProvince!="") || SearchDistrict != null)
                    {
                          int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                
                        Result = (from a in Result
                                  where a.Description.Contains(SearchDistrict)
                                  where a.Province_Id== SearchProvince_1
                                  orderby a.Description
                                  select a).ToList();
                }
                if (SearchProvince == null && SearchDistrict != null)
                {
                    Result = (from a in Result
                              where a.Description.Contains(SearchDistrict)
                              orderby a.Description
                              select a).ToList();
                }

                return Result;
            }
            //public List<Area> GetAreas()
            //{
            //    return (from a in dbContext.Areas
            //            select a).ToList();
            //}

            public District CreateDistrict(District NewEntry)
            {
                var DistrictObj = (from p in dbContext.Districts
                                   where p.Description.Contains(NewEntry.Description)
                                   select p).FirstOrDefault();
                if (DistrictObj == null)
                {
                    var newDistrict = new District();
                    newDistrict.Description = NewEntry.Description;
                    newDistrict.Province_Id = NewEntry.Province.Province_Id;
                    dbContext.Districts.Add(newDistrict);
                    dbContext.SaveChanges();

                    int DistrictId = (from s in dbContext.Districts
                                  where s.Description == NewEntry.Description
                                  select s.District_Id).FirstOrDefault();
                    //var NewArea = new Area();
                    //NewArea.Description = NewEntry.Description;
                    //NewArea.District_Id = DistrictId;
                    //dbContext.Areas.Add(NewArea);
                    //dbContext.SaveChanges();

                return newDistrict;
                }
                else
                {
                    return null;
                }
            }

            public District GetDistrict(int Id)
            {
                return (from p in dbContext.Districts
                        where p.District_Id == Id
                        select p).FirstOrDefault();

            }

            public District EditDistrict(District UpdatedObj)
            {
                var DistrictObj = (from p in dbContext.Districts
                                   where p.District_Id == UpdatedObj.District_Id
                                   select p).FirstOrDefault();
                DistrictObj.Description = UpdatedObj.Description;
                DistrictObj.Province_Id = UpdatedObj.Province_Id;
                dbContext.SaveChanges();
                //var DistrictObj2 = (from p in dbContext.Areas
                //                   where p.District_Id == UpdatedObj.District_Id
                //                   select p).FirstOrDefault();
                //DistrictObj2.Description = UpdatedObj.Description;
                //dbContext.SaveChanges();
            return DistrictObj;
            }

            public void DeleteDistrict(int Id)
            {
                var DelObj = (from a in dbContext.Districts
                              where a.District_Id == Id
                              select a).FirstOrDefault();
                dbContext.Districts.Remove(DelObj);

                //var DelObj2 = (from z in dbContext.Areas
                //           where z.District_Id == Id
                //           select z).FirstOrDefault();
                //dbContext.Areas.Remove(DelObj2);
                dbContext.SaveChanges();
            }

        #endregion

        #region Municipality

        public List<Local_Municipality> GetMunicipalities()
        {
            return (from m in dbContext.Local_Municipalities
                    select m).ToList();
        }

        public List<Local_Municipality> GetLocal_Municipalities()
                {
                    return (from m in dbContext.Local_Municipalities
                            select m).ToList();
                }
            public List<Local_Municipality> GetMunicipalities(string SearchProvince, string SearchDistrict, string SearchMunicipality)
            {

                var Result = (from a in dbContext.Local_Municipalities
                              orderby a.Description
                              select a).ToList();

                if ((SearchProvince != null && SearchProvince != "") || (SearchMunicipality != null&& SearchMunicipality != ""))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);

                    Result = (from a in dbContext.Local_Municipalities
                              //join b in dbContext.Areas on a.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on a.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchMunicipality)
                              where c.Description.Contains(SearchDistrict)
                              where c.Province_Id== SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                if (SearchProvince == null && SearchMunicipality != null)
                {
                    Result = (from a in dbContext.Local_Municipalities
                              where a.Description.Contains(SearchMunicipality)
                              orderby a.Description
                              select a).ToList();
                }

                return Result;
            }

            public Local_Municipality CreateMunicipality(Local_Municipality NewEntry)
            {
                var MunicipalityObj = (from p in dbContext.Local_Municipalities
                                       where p.Description.Contains(NewEntry.Description)
                                   select p).FirstOrDefault();
                if (MunicipalityObj == null)
                {
                    //var newMunicipality = new Local_Municipalities();
                    //newMunicipality.Description = NewEntry.Description;
                    //newMunicipality.Area_Id = (from a in dbContext.Areas
                    //                          where a.District_Id== NewEntry.Area.District.District_Id
                    //                          select a.Area_Id).FirstOrDefault();
                    //dbContext.Local_Municipalities.Add(newMunicipality);
                    //dbContext.SaveChanges();
                    int MunicipalityId = (from s in dbContext.Local_Municipalities
                                          where s.Description == NewEntry.Description
                                      select s.Local_Municipality_Id).FirstOrDefault();
                    var NewLocalMunicipality = new Local_Municipality();

                    NewLocalMunicipality.Description = NewEntry.Description;
                    NewLocalMunicipality.Local_Municipality_Id = MunicipalityId;
                    dbContext.Local_Municipalities.Add(NewLocalMunicipality);
                    dbContext.SaveChanges();
                return NewLocalMunicipality;
                }
                else
                {
                    return null;
                }
            }

            public Local_Municipality GetMunicipality(int? Id)
            {
                return (from p in dbContext.Local_Municipalities
                        where p.Local_Municipality_Id == Id
                        select p).FirstOrDefault();

            }

            public Local_Municipality EditMunicipality(Local_Municipality UpdatedObj)
            {
                var MunicipalityObj = (from p in dbContext.Local_Municipalities
                                   where p.Local_Municipality_Id == UpdatedObj.Local_Municipality_Id
                                   select p).FirstOrDefault();
                MunicipalityObj.Description = UpdatedObj.Description;
                //MunicipalityObj.Area_Id = UpdatedObj.Area_Id;
                dbContext.SaveChanges();
                var MunicipalityObj2 = (from p in dbContext.Local_Municipalities
                                where p.Local_Municipality_Id == UpdatedObj.Local_Municipality_Id
                                select p).FirstOrDefault();
                MunicipalityObj2.Description = UpdatedObj.Description;
                dbContext.SaveChanges();
            return MunicipalityObj;
            }

            public void DeleteMunicipality(int Id)
            {
                var DelObj = (from a in dbContext.Local_Municipalities
                              where a.Local_Municipality_Id == Id
                              select a).FirstOrDefault();
                dbContext.Local_Municipalities.Remove(DelObj);

                var DelObj2 = (from z in dbContext.Local_Municipalities
                               where z.Local_Municipality_Id == Id
                               select z).FirstOrDefault();
                dbContext.Local_Municipalities.Remove(DelObj2);
                dbContext.SaveChanges();
            }

        #endregion

        #region Town

            public List<Town> GetTowns()
                {
                    return (from t in dbContext.Towns
                            select t).ToList();
                }

            public List<Town> GetTowns(string SearchProvince, string SearchDistrict, string SearchMunicipality, string SearchTown)
            {

                var Result = (from a in dbContext.Towns
                              orderby a.Description
                              select a).ToList();
                #region One
                if (( SearchProvince != "")
                    && ( SearchDistrict != "")
                    && (SearchMunicipality != "")
                    && ( SearchTown != "")
                    &&
                    (SearchProvince != null)
                    && (SearchDistrict != null)
                    && (SearchMunicipality != null)
                    && (SearchTown != null))
                    {
                        int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                        int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                        int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                        Result = (from a in dbContext.Towns
                                  join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                                  //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                                  //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                                  join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                                  where a.Description.Contains(SearchTown)
                                  where d.Local_Municipality_Id== SearchMunicipality_1
                                  where c.District_Id== SearchDistrict_1
                                  where c.Province_Id == SearchProvince_1
                                  orderby a.Description
                                  select a).ToList();
                    }
                #endregion

                #region Two
                    if ((SearchProvince != "")
                    && (SearchDistrict != "")
                    && (SearchMunicipality != "")
                    && (SearchTown == ""))
                    {
                        int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                        int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                        int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                        Result = (from a in dbContext.Towns
                                  join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                                  //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                                  //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                                  join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                                  //where a.Description.Contains(SearchTown)
                                  where d.Local_Municipality_Id == SearchMunicipality_1
                                  where c.District_Id == SearchDistrict_1
                                  where c.Province_Id == SearchProvince_1
                                  orderby a.Description
                                  select a).ToList();
                    }
                #endregion

                #region Three
                if ((SearchProvince != "")
                    && (SearchDistrict != "")
                    && (SearchMunicipality == "")
                    && (SearchTown == ""))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region Four
                    if ((SearchProvince != "")
                    && (SearchDistrict == "")
                    && (SearchMunicipality == "")
                    && (SearchTown == ""))
                    {
                        int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                        //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                        //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                        Result = (from a in dbContext.Towns
                                  join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                                  //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                                  //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                                  join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                                  //where a.Description.Contains(SearchTown)
                                  //where e.Municipality_Id == SearchMunicipality_1
                                  //where c.District_Id == SearchDistrict_1
                                  where c.Province_Id == SearchProvince_1
                                  orderby a.Description
                                  select a).ToList();
                    }
                #endregion

                #region Five
                if ((SearchProvince == "")
                    && (SearchDistrict != "")
                    && (SearchMunicipality != "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region Six
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchMunicipality != "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region Seven
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchMunicipality == "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region Eight
                if ((SearchProvince == "")
                    && (SearchDistrict != "")
                    && (SearchMunicipality == "")
                    && (SearchTown == ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region Nine
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchMunicipality != "")
                    && (SearchTown == ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion

                #region ten
                if ((SearchProvince == null)
                    && (SearchDistrict == null)
                    && (SearchMunicipality == null)
                    && (SearchTown == null))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.Towns
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
                #endregion



                if (SearchProvince == null && SearchMunicipality != null)
                {
                    Result = (from a in dbContext.Towns
                              where a.Description.Contains(SearchMunicipality)
                              orderby a.Description
                              select a).ToList();
                }

                return Result;
            }

            public Town CreateTown(Town NewEntry)
            {
                var TownObj = (from p in dbContext.Towns
                               where p.Description.Contains(NewEntry.Description)
                                       select p).FirstOrDefault();
                if (TownObj == null)
                {
                    var newTown = new Town();
                    newTown.Description = NewEntry.Description;
                    newTown.Local_Municipality_Id = (from a in dbContext.Local_Municipalities
                                                    where a.Local_Municipality_Id== NewEntry.Local_Municipality.Local_Municipality_Id
                                                    select a.Local_Municipality_Id).FirstOrDefault();
                    dbContext.Towns.Add(newTown);
                    dbContext.SaveChanges();
                    return newTown;
                }
                else
                {
                    return null;
                }
            }

            public Town GetTown(int? Id)
            {
                return (from p in dbContext.Towns
                        where p.Town_Id == Id
                        select p).FirstOrDefault();

            }

            public Town EditTown(Town UpdatedObj)
            {
                var TownObj = (from p in dbContext.Towns
                               where p.Town_Id == UpdatedObj.Town_Id
                                       select p).FirstOrDefault();
                TownObj.Description = UpdatedObj.Description;
                TownObj.Local_Municipality_Id = (from a in dbContext.Local_Municipalities
                                                 where a.Local_Municipality_Id == UpdatedObj.Local_Municipality.Local_Municipality_Id
                                                 select a.Local_Municipality_Id).FirstOrDefault();

                dbContext.SaveChanges();
                return TownObj;
            }


            public void DeleteTown(int Id)
            {
                var DelObj = (from a in dbContext.Towns
                              where a.Town_Id == Id
                              select a).FirstOrDefault();
                dbContext.Towns.Remove(DelObj);
                dbContext.SaveChanges();
            }
        #endregion

        #region Ward
            public List<NISIS_Ward> GetWards()
            {
                return (from t in dbContext.NISIS_Wards
                        select t).ToList();
            }

            public List<NISIS_Ward> GetWards(string SearchProvince, string SearchDistrict, string SearchLocal_Municipality, string SearchTown)
        {

            var Result = (from a in dbContext.NISIS_Wards
                          orderby a.Description
                          select a).ToList();
            #region One
                if ((SearchProvince != "")
                    && (SearchDistrict != "")
                    && (SearchLocal_Municipality != "")
                    && (SearchTown != "")
                    &&
                    (SearchProvince != null)
                    && (SearchDistrict != null)
                    && (SearchLocal_Municipality != null)
                    && (SearchTown != null))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchLocal_Municipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Two
                if ((SearchProvince != "")
                && (SearchDistrict != "")
                && (SearchLocal_Municipality != "")
                && (SearchTown == ""))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchLocal_Municipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Three
                if ((SearchProvince != "")
                    && (SearchDistrict != "")
                    && (SearchLocal_Municipality == "")
                    && (SearchTown == ""))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Four
                if ((SearchProvince != "")
                && (SearchDistrict == "")
                && (SearchLocal_Municipality == "")
                && (SearchTown == ""))
                {
                    int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Five
                if ((SearchProvince == "")
                    && (SearchDistrict != "")
                    && (SearchLocal_Municipality != "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchLocal_Municipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Six
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchLocal_Municipality != "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchLocal_Municipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Seven
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchLocal_Municipality == "")
                    && (SearchTown != ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Eight
                if ((SearchProvince == "")
                    && (SearchDistrict != "")
                    && (SearchLocal_Municipality == "")
                    && (SearchTown == ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region Nine
                if ((SearchProvince == "")
                    && (SearchDistrict == "")
                    && (SearchLocal_Municipality != "")
                    && (SearchTown == ""))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    int? SearchMunicipality_1 = Convert.ToInt32(SearchLocal_Municipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              where d.Local_Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion

            #region ten
                if ((SearchProvince == null)
                    && (SearchDistrict == null)
                    && (SearchLocal_Municipality == null)
                    && (SearchTown == null))
                {
                    //int? SearchProvince_1 = Convert.ToInt32(SearchProvince);
                    //int? SearchDistrict_1 = Convert.ToInt32(SearchDistrict);
                    //int? SearchMunicipality_1 = Convert.ToInt32(SearchMunicipality);

                    Result = (from a in dbContext.NISIS_Wards
                              join d in dbContext.Local_Municipalities on a.Local_Municipality_Id equals d.Local_Municipality_Id
                              //join e in dbContext.Municipalities on d.Municipality_Id equals e.Municipality_Id
                              //join b in dbContext.Areas on e.Area_Id equals b.Area_Id
                              join c in dbContext.Districts on d.District_Municipality_Id equals c.District_Id
                              //where a.Description.Contains(SearchTown)
                              //where e.Municipality_Id == SearchMunicipality_1
                              //where c.District_Id == SearchDistrict_1
                              //where c.Province_Id == SearchProvince_1
                              orderby a.Description
                              select a).ToList();
                }
            #endregion



            if (SearchProvince == null && SearchLocal_Municipality != null)
            {
                Result = (from a in dbContext.NISIS_Wards
                          where a.Description.Contains(SearchLocal_Municipality)
                          orderby a.Description
                          select a).ToList();
            }

            return Result;
        }

            public NISIS_Ward CreateWard(NISIS_Ward NewEntry, string loginName)
            {
               
                var WardObj = (from p in dbContext.NISIS_Wards
                               where p.Description.Contains(NewEntry.Description)
                               select p).FirstOrDefault();
                if (WardObj == null)
                {
                    var newWard = new NISIS_Ward();
                    string NewWardNumber = (Convert.ToString(NewEntry.Description).Replace("Ward ", ""));
                    string NewWardCode =  NewEntry.Description.Replace("Ward ", "00");
                    newWard.Description = NewEntry.Description;
                    newWard.Local_Municipality_Id = (from a in dbContext.Local_Municipalities
                                                     where a.Local_Municipality_Id == NewEntry.Local_Municipality.Local_Municipality_Id
                                                     select a.Local_Municipality_Id).FirstOrDefault();
                    newWard.Ward_Code = NewWardCode;
                    newWard.Ward_Number = NewWardNumber;
                    newWard.Date_Created = DateTime.Now;
                    newWard.Created_By = loginName;
                    newWard.Is_Active = true;
                    newWard.Is_Deleted = false;
                    dbContext.NISIS_Wards.Add(newWard);
                    dbContext.SaveChanges();
                    return newWard;
                }
                else
                {
                    return null;
                }
            }

            public NISIS_Ward GetWard(int? Id)
            {
                return (from p in dbContext.NISIS_Wards
                        where p.NISIS_Ward_Id == Id
                        select p).FirstOrDefault();

            }

            public NISIS_Ward EditWard(NISIS_Ward UpdatedObj, string loginName)
            {
                var WardObj = (from p in dbContext.NISIS_Wards
                               where p.NISIS_Ward_Id == UpdatedObj.NISIS_Ward_Id
                               select p).FirstOrDefault();
                    string NewWardNumber = (Convert.ToString(UpdatedObj.Description).Replace("Ward ", ""));
                    string NewWardCode = UpdatedObj.Description.Replace("Ward ", "00");
                    WardObj.Description = UpdatedObj.Description;
                                WardObj.Local_Municipality_Id = (from a in dbContext.Local_Municipalities
                                                         where a.Local_Municipality_Id == UpdatedObj.Local_Municipality.Local_Municipality_Id
                                                                 select a.Local_Municipality_Id).FirstOrDefault();
                    WardObj.Ward_Code = NewWardCode;
                    WardObj.Ward_Number = NewWardNumber;
                    WardObj.Date_Last_Modified = DateTime.Now;
                    WardObj.Last_Modified_By = loginName;
                    dbContext.SaveChanges();
                return WardObj;
            }


            public void DeleteWard(int Id)
            {
                var DelObj = (from a in dbContext.NISIS_Wards
                              where a.NISIS_Ward_Id == Id
                              select a).FirstOrDefault();
                dbContext.NISIS_Wards.Remove(DelObj);
                dbContext.SaveChanges();
            }


        #endregion



    }
}
