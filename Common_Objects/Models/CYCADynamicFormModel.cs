using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class CYCADynamicFormModel
    {
        private readonly SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();
        public CYCADynamicFormViewModel GetDynamicForm(int dynamicFormType)
        {
            CYCADynamicFormViewModel model;
            try
            {
                var results = (from df in dbContext.CYCA_Dynamic_Form
                               where df.Dynamic_Form_Type_Id.Equals(dynamicFormType) && df.Is_Active.Equals(true)
                               select new CYCADynamicFormViewModel
                               {
                                   Definition = df.Definition,
                                   DynamicFormId = df.Dynamic_Form_Id,
                                   DynamicFormTypeId = df.Dynamic_Form_Type_Id,
                                   SummaryDefinition = df.SummaryDefinition,
                                   IsActive = df.Is_Active,
                                   Version = df.Version
                               }).SingleOrDefault();

                model = results;
            }
            catch (Exception)
            {
                return null;
            }
            return model;
        }
        public CYCA_Dynamic_Form_Data GetDynamicFormData(int dynamicFormDataId)
        {
            CYCA_Dynamic_Form_Data model;
            try
            {
                var results = (from df in dbContext.CYCA_Dynamic_Form_Data
                               where df.Dynamic_Form_Data_Id.Equals(dynamicFormDataId)
                               select df).SingleOrDefault();

                model = results;
            }
            catch (Exception)
            {
                return null;
            }
            return model;
        }
        public List<CYCA_Dynamic_Form_Data> GetDynamicFormDatasForUser(int dynamicFormType, int userId)
        {
            List<CYCA_Dynamic_Form_Data> model = new List<CYCA_Dynamic_Form_Data>(); ;
            try
            {
                var results = (from df in dbContext.CYCA_Dynamic_Form_Data
                               where df.CYCA_Dynamic_Form.Dynamic_Form_Type_Id.Equals(dynamicFormType) &&
                                     df.User_Id.Equals(userId)
                               select df).ToList();

                model.AddRange(results);
            }
            catch (Exception)
            {
                return null;
            }
            return model;
        }
        public int GetFacilityIdByUserID(int UserId)
        {

            return (from p in dbContext.Employees
                    join c in dbContext.Users on p.User_Id equals c.User_Id
                    // join i in db.Organizations on p.Organization_Id equals i.Organization_Id
                    join f in dbContext.apl_Cyca_Facility on p.Facility_Id equals f.Facility_Id
                    where c.User_Id == (UserId)
                    select f.Facility_Id).SingleOrDefault();
        }

        public int GetProvinceIdByFacilityId(int UserId)
        {

            return (from f in dbContext.apl_Cyca_Facility
                    join p in dbContext.Provinces on f.Province_Id equals p.Province_Id                  
                    where f.Facility_Id == GetFacilityIdByUserID(UserId)
                    select p.Province_Id).SingleOrDefault();
        }

        public int GetClientIdByPersonId(int PersonId)
        {

            return (from c in dbContext.Clients
                    join p in dbContext.Persons on c.Person_Id equals p.Person_Id
                    where c.Person_Id == PersonId
                    select c.Client_Id).SingleOrDefault();
        }

        //public List<DynamicDropDown> GetDynamicDropDown(int Type, int Type2)
        //{
        //    var returnList = new List<DynamicDropDown>();
        //    switch (Type)
        //    {
        //        case 1:
        //            returnList = (from g in dbContext.apl_Cyca_Gang_Membership_Type
        //                          select new DynamicDropDown()
        //                          {
        //                              Id = g.Gang_Membership_Type_Id,
        //                              Desc = g.Description
        //                          }).ToList();
        //            break;
        //    }

        //    return returnList;
        //}

        public List<DynamicDropDown> GetDynamicDropDown(int Type, int UserId)
        {
            var returnList = new List<DynamicDropDown>();
            var staffViewList = new List<DynamicDropDown>();
            int facilityId = GetFacilityIdByUserID(UserId);
            switch (Type)
            {
                case 1:
                    //returnList = (from g in dbContext.Users
                    //              join e in dbContext.Employees on g.User_Id equals e.User_Id
                    //              join f in dbContext.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id

                    //              where e.Facility_Id == facilityId
                    //              select new DynamicDropDown()
                    //              {
                    //                  Id = g.User_Id,
                    //                  Desc = g.First_Name + " " + g.Last_Name + " " + g.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToList(),

                    //              }).ToList();


                    List<DynamicDropDown> staffList = (from g in dbContext.Users
                                                       join e in dbContext.Employees on g.User_Id equals e.User_Id
                                                       join f in dbContext.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id

                                                       where e.Facility_Id == facilityId
                                                       select new DynamicDropDown()
                                                       {
                                                           Id = g.User_Id,
                                                           roles = g.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToList(),
                                                           Desc = g.First_Name + " " + g.Last_Name,
                                                           //roles = g.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToList()
                                                       }).ToList();
                    foreach (var s in staffList)
                    {
                        if (s.roles.Count > 0)
                        {
                            staffViewList.Add(new DynamicDropDown()
                            {
                                Id = s.Id,
                                Desc = s.Desc + " (" + String.Join(" ", s.roles) + ")",
                            });
                        }
                    }

                    //staffViewList = (from s in staffList
                    //                                    select new DynamicDropDown()
                    //                                    {
                    //                                        Id = s.Id,
                    //                                        Desc = s.Desc + " (" + String.Join(" ", s.roles) + ")",
                    //                                    }).ToList();

                    break;
            }

            //return returnList;
            return staffViewList;
        }



        public List<DynamicDropDownFOrKidsInTheCenter> GetDropDownFOrKidsInTheCenter(int Type, int UserId)
        {
            var returnList2 = new List<DynamicDropDownFOrKidsInTheCenter>();
            int facilityId = GetFacilityIdByUserID(UserId);
            switch (Type)
            {
                case 2:
                    returnList2 = (from g in dbContext.Persons
                                   join c in dbContext.Clients on g.Person_Id equals c.Person_Id
                                   join a in dbContext.CYCA_Admissions_AdmissionDetails on c.Client_Id equals a.Client_Id
                                   join f in dbContext.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                                   where a.Facility_Id == facilityId && a.Is_Active == true
                                   select new DynamicDropDownFOrKidsInTheCenter()
                                   {
                                       Id = g.Person_Id,
                                       Desc = g.First_Name + " " + g.Last_Name
                                   }).Distinct().ToList();
                    break;
            }

            return returnList2;
        }

        public List<DropDownFOrVenuesInTheCenter> GetDropDownFOrVenuesInTheCenter(int Type, int UserId)
        {
            var returnList2 = new List<DropDownFOrVenuesInTheCenter>();
            int facilityId = GetFacilityIdByUserID(UserId);
            switch (Type)
            {
                case 3:
                    returnList2 = (from g in dbContext.apl_Cyca_Venue

                                   where g.Facility_Id == facilityId
                                   select new DropDownFOrVenuesInTheCenter()
                                   {
                                       Id = g.Venue_Id,
                                       Desc = g.VenueName
                                   }).ToList();


                    break;
            }

            return returnList2;
        }

        public List<DropDownFOrRolesInTheCenter> GetDropDownFOrRolesInTheCenter(int Type, int UserId)
        {
            var returnList2 = new List<DropDownFOrRolesInTheCenter>();
            //int facilityId = GetFacilityIdByUserID(UserId);
            switch (Type)
            {
                case 4:
                    returnList2 = (from g in dbContext.Roles

                                   where g.Role_Id >= 32 && g.Role_Id <= 41
                                   //where g.Role_Id == 33 || g.Role_Id == 34 || g.Role_Id == 35
                                   //|| g.Role_Id == 36 || g.Role_Id ==37 || g.Role_Id == 38
                                   //|| g.Role_Id == 39 || g.Role_Id == 40 || g.Role_Id == 41
                                   select new DropDownFOrRolesInTheCenter()
                                   {
                                       Id = g.Role_Id,
                                       Desc = g.Description
                                   }).ToList();


                    break;
            }

            return returnList2;
        }


        public List<CYCA_Dynamic_Form_Data> GetDynamicFormDatasForClient(int dynamicFormType, int clientId)
        {
            List<CYCA_Dynamic_Form_Data> model = new List<CYCA_Dynamic_Form_Data>(); ;
            try
            {
                var results = (from df in dbContext.CYCA_Dynamic_Form_Data
                               where df.CYCA_Dynamic_Form.Dynamic_Form_Type_Id.Equals(dynamicFormType) &&
                                     df.Client_Id.Equals(clientId)
                               select df).ToList();

                model.AddRange(results);
            }
            catch (Exception)
            {
                return null;
            }
            return model;
        }
        public CYCA_Dynamic_Form_Data AddOrUpdateDynamicFormDatas(CYCA_Dynamic_Form_Data data)
        {

            CYCA_Dynamic_Form_Data record = dbContext.CYCA_Dynamic_Form_Data.Where(d => d.Dynamic_Form_Data_Id == data.Dynamic_Form_Data_Id).SingleOrDefault();
            if (record == null)
            {
                record = new CYCA_Dynamic_Form_Data()
                {
                    Client_Id = data.Client_Id,
                    CreatedDate = data.CreatedDate,
                    Data = data.Data,
                    Dynamic_Form_Id = data.Dynamic_Form_Id,
                    User_Id = data.User_Id,
                    Venue_Id = data.Venue_Id
                };
                dbContext.CYCA_Dynamic_Form_Data.Add(record);

                //dbContext.SaveChanges();
                data.Dynamic_Form_Data_Id = record.Dynamic_Form_Data_Id;
            }
            else
            {
                record.Data = data.Data;
            }
            dbContext.SaveChanges();

            return data;

        }

        /*   public void GetFileData(int id, int formType)
           {
             var f=  (from f in dbContext.CYCA_Dynamic_Form
                join fd in dbContext.CYCA_Dynamic_Form_Data on f.Dynamic_Form_Id equals fd.Dynamic_Form_Id
                join u in dbContext.Users on fd.User_Id equals u.User_Id
                join p in dbContext.Persons on fd.Client_Id equals p.Person_Id
                join fc in dbContext.apl_Cyca_Facility on fd.Venue_Id equals fc.Facility_Id
                where p.Person_Id == id && f.Dynamic_Form_Type_Id == formType

                select new CYCA_DynamicDataModel()
                {
                    Id = fd.Dynamic_Form_Data_Id,
                    CreatedBy = u.First_Name + " " + u.Last_Name,
                    CreatedById = u.User_Id,
                    CreatedFor = p.First_Name + " " + p.Last_Name,
                    DateCreated = fd.CreatedDate.ToString(),
                    CreatedForId = p.Person_Id,
                    Venue = fc.FacilityName,
                    File = fd.
                }).ToList();
           }*/
        public Person GetChild(int Id)
        {
            var returnModel = dbContext.Persons.Where(p => p.Person_Id.Equals(Id)).Single();
            return returnModel;
        }
    }
    public class DynamicDropDown
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public List<string> roles { get; set; }
    }

    public class DynamicDropDownFOrKidsInTheCenter
    {
        public int Id { get; set; }
        public string Desc { get; set; }
    }

    public class DropDownFOrVenuesInTheCenter
    {
        public int Id { get; set; }
        public string Desc { get; set; }
    }

    public class DropDownFOrRolesInTheCenter
    {
        public int Id { get; set; }
        public string Desc { get; set; }
    }
}
