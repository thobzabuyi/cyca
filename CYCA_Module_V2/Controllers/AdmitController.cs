 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web.Helpers;
using CYCA_Module_V2.Common_Objects;
using System.Data.Entity.Migrations;
using Microsoft.Ajax.Utilities;

namespace CYCA_Module_V2.Controllers
{
    public class AdmitController : Controller
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        ChildrenModel children = new ChildrenModel();


        public PartialViewResult Admission(int Id)
        {
            ViewBag.ChildId = Id;
            List<apl_Admission_Type> admission_Types = db.apl_Admission_Type.ToList();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");
            CYCAAdmissionPartialViewModel returnModel = new CYCAAdmissionPartialViewModel();

            if (System.Web.HttpContext.Current.Session["CurrentUser"] != null)
            {
                var loggedInUser = (User)System.Web.HttpContext.Current.Session["CurrentUser"];

                var userModel = new UserModel();

                returnModel.LoggedInUserFacility = userModel.GetFacilityByUserId(loggedInUser.User_Id);
            }


            returnModel.CYCAAdmissionViewModels = new List<CYCAAdmissionViewModel>();
            var docL = (from d in db.CYCA_Admissions_Document
                        select new LiteFiles
                        {
                            Document_Name =  d.Document_Name,
                            Document_Id =  d.Document_Id,
                            Admission_Id =  d.Admission_Id,
                            ReAdmission_Id =  d.ReAdmission_Id,
                            DischargeId =  d.DischargeId
                        }).ToList();
            //var RdocL = db.CYCA_ReAdmissions_Document.ToList();
            returnModel.PersonId = Id;

            var client = db.Clients.Where(c => c.Person_Id == Id).Single();
            bool Activeadmission = db.CYCA_Admissions_AdmissionDetails.Any(a => a.Client_Id == client.Client_Id && a.Is_Active == true);
            //bool ActiveReadmission = db.CYCA_ReAdmissionDetails.Any(a => a.Client_Id == client.Client_Id && a.Is_Active == true);
            int latestAdmission = 0;
            var lAdmission = (from a in db.CYCA_Admissions_AdmissionDetails
                                  //join c in db.Clients on a.Client_Id equals c.Client_Id
                              where a.Client_Id == client.Client_Id
                              orderby a.Admission_Id descending
                              select a).ToList();
            if(lAdmission.Count>0)
            {
                latestAdmission = lAdmission.Select(a => a.Admission_Id).Max();
            }    
       
            var ListP = (
               (from a in db.CYCA_Admissions_AdmissionDetails
                join at in db.apl_Admission_Type on a.Admission_Type_Id equals at.Admission_Type_Id
                //join gm in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gm.Admission_Id where gm.Is_Active
                //join gmd in db.apl_Cyca_Gang_Membership_Type on gm.Gang_Membership_Type_Id equals gmd.Gang_Membership_Type_Id
                join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                join v in db.apl_Cyca_Venue on a.Venue_Id equals v.Venue_Id
                //join re in db.CYCA_Admissions_ReAdmissionDetails on a.Admission_Id equals re.Admission_Id
                join c in db.Clients on a.Client_Id equals c.Client_Id
                where c.Person_Id == Id 
                select new CYCAAdmissionViewModel
                {

                    Admission_Id = a.Admission_Id,
                    ActiveAdmission = Activeadmission,
                    LatestAdmission = latestAdmission,
                    //ActiveReAdmission = ActiveReadmission,
                    //SelectedGangMemberType = gmd.Description,
                    CaseStartDate = a.Case_Start_Date,
                    CaseEndDate = a.Case_End_Date.ToString(),
                    AdmissionDate = a.Date_Captured.ToString(),
                    selectedAdmissionType = at.Description,
                    FacilityName = f.FacilityName,
                    ReAdmissionCount = a.CYCA_Admissions_ReAdmissionDetails.Count(),
                    Is_Active = a.Is_Active,
                    selectedVenue = v.VenueName,
                    DateCreated = a.Date_Created.ToString(),
                    Person_Id = Id
                }).ToList());
            foreach (var item in ListP)
            {
                //item.files = docL.Where(x => x.Admission_Id == item.Admission_Id && x.ReAdmission_Id == null && x.DischargeId == null).ToList();
                item.liteFiles = docL.Where(x => x.Admission_Id == item.Admission_Id && x.ReAdmission_Id == null && x.DischargeId == null).ToList();
            }
            try
            {
                var data = (
               (from re in db.CYCA_ReAdmissionDetails
                    //join a in db.CYCA_Admissions_AdmissionDetails on re.Admission_Id equals a.Admission_Id
                join at in db.apl_Admission_Type on re.Admission_Type_Id equals at.Admission_Type_Id
                //join gm in db.CYCA_Admissions_GangMembership on re.ReAdmission_Id equals gm.ReAdmission_Id
                //join gmd in db.apl_Cyca_Gang_Membership_Type on gm.Gang_Membership_Type_Id equals gmd.Gang_Membership_Type_Id
                join f in db.apl_Cyca_Facility on re.Facility_Id equals f.Facility_Id
                join v in db.apl_Cyca_Venue on re.Venue_Id equals v.Venue_Id
                join c in db.Clients on re.Client_Id equals c.Client_Id
                where c.Person_Id == Id
                select new CYCAAdmissionViewModel
                {
                    Re_Admission_Id = re.ReAdmission_Id,
                    Admission_Id = re.Admission_Id,
                    CaseStartDate = re.Case_Start_Date,
                    CaseEndDate = re.Case_End_Date.ToString(),
                    AdmissionDate = re.Date_Captured.ToString(),
                    selectedAdmissionType = at.Description,
                    //SelectedGangMemberType = gmd.Description,
                    FacilityName = f.FacilityName,
                    selectedVenue = v.VenueName,
                    DateCreated = re.Date_Created.ToString(),
                    Is_Active = re.Is_Active,
                    ActiveAdmission = Activeadmission,
                    //ActiveReAdmission = ActiveReadmission

                }).ToList());
                foreach (var item in data)
                {
                    //item.files = docL.Where(x => x.ReAdmission_Id == item.Re_Admission_Id).ToList();
                    item.liteFiles = docL.Where(x => x.ReAdmission_Id == item.Re_Admission_Id).ToList();
                }
                ViewBag.ListP2 = data;
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }


            returnModel.CYCAAdmissionViewModels.AddRange(ListP);
            return PartialView("~/Views/Admit/_Admissions.cshtml", returnModel);
        }   
        public PartialViewResult AdmitNewPerson(int Id)
        {
     
            var p = db.Persons.Where(pp => pp.Person_Id == Id).Single();
            //Biometrics
            var afis = db.int_DSD_Afis.Where(af => af.Person_Id.Equals(p.Person_Id)).SingleOrDefault();

            List<apl_Admission_Type> admission_Types = db.apl_Admission_Type.ToList();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            //initialise view model
           var VM = new CYCAAdmissionViewModel();
            VM.Person_Id = Id;
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            if (afis != null)
            {
                VM.HasBiometric = true;
                VM.IsPivaVerified = p.Is_Piva_Validated;
                VM.IsVerified = afis.Is_Verified;
            }
            else
            {
                VM.HasBiometric = false;
                VM.IsPivaVerified = false;
                VM.IsVerified = false;
            }
            return PartialView("~/Views/Admit/_AdmitNew.cshtml", VM);
        }        
        public PartialViewResult AdmitNewBodilySearch(int Id)
        {
                     
            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);     
            var userList = db.Roles
                                .Where(r => r.Description.Contains("CYCA"))
                                .SelectMany(x => x.Users)
                                .Distinct()
                                .ToList();
            ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Bodily_Search_Reasons> search_Reasons = db.apl_Cyca_Bodily_Search_Reasons.ToList();
            ViewBag.SearchReasonList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            ViewBag.admitid = Id;
            return PartialView("~/Views/Admit/AdmitNewBodySearch.cshtml", VM);
        }
        public PartialViewResult AdmitNewIllegalItems(int Id)
        {
            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);

            //ViewBag.UserList = new SelectList(users, "[User_Id]", "First_Name");
            ViewBag.Userlist = from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               join f in db.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id
                               where f.Facility_Id == facility
                               select new
                               {
                                   userId = u.User_Id,
                                   Full_Name = u.First_Name + " " + u.Last_Name
                               };
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            var VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            return PartialView("~/Views/Admit/_AdmitIllegalItemNew.cshtml", VM);
        }
        public PartialViewResult AdmitNewGangMembership(int Id)
        {
            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            return PartialView("~/Views/Admit/_AdmitGangMembershipNew.cshtml", VM);
        }
        public PartialViewResult AdmitNewUploadDocument(int Id)
        {
            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            // var facility = GetFacilityIdByUserID(userId);
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            return PartialView("~/Views/Admit/_AdmitUploadDocumentNew.cshtml", VM);
        }
    

        public PartialViewResult ReAdmitPerson(int Id)
        {

            var p = db.Persons.Where(pp => pp.Person_Id == Id).Single();
            //Biometrics
            var afis = db.int_DSD_Afis.Where(af => af.Person_Id.Equals(p.Person_Id)).SingleOrDefault();

            List<apl_Admission_Type> admission_Types = db.apl_Admission_Type.ToList();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            //initialise view model
            var VM = new CYCAAdmissionViewModel();
            VM.Person_Id = Id;
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            if (afis != null)
            {
                VM.HasBiometric = true;
                VM.IsPivaVerified = p.Is_Piva_Validated;
                VM.IsVerified = afis.Is_Verified;
            }
            else
            {
                VM.HasBiometric = false;
                VM.IsPivaVerified = false;
                VM.IsVerified = false;
            }
            return PartialView("~/Views/Admit/_ReAdmitNew.cshtml", VM);
        }
        public PartialViewResult ReAdmitNewBodilySearch(int Id)
        {

            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);
            var userList = db.Roles
                                .Where(r => r.Description.Contains("CYCA"))
                                .SelectMany(x => x.Users)
                                .Distinct()
                                .ToList();
            ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Bodily_Search_Reasons> search_Reasons = db.apl_Cyca_Bodily_Search_Reasons.ToList();
            ViewBag.SearchReasonList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            ViewBag.admitid = Id;
            return PartialView("~/Views/Admit/_ReAdmitBodySearchNew.cshtml", VM);
        }
        public PartialViewResult ReAdmitNewIllegalItems(int Id)
        {
            var p = (from a in db.CYCA_Admissions_AdmissionDetails
                     join c in db.Clients on a.Client_Id equals c.Client_Id
                     join pp in db.Persons on c.Person_Id equals pp.Person_Id
                     where a.Admission_Id == Id
                     select pp).Single();
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);

            //ViewBag.UserList = new SelectList(users, "[User_Id]", "First_Name");
            ViewBag.Userlist = from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               join f in db.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id
                               where f.Facility_Id == facility
                               select new
                               {
                                   userId = u.User_Id,
                                   Full_Name = u.First_Name + " " + u.Last_Name
                               };
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            CYCAdmissionModel Model = new CYCAdmissionModel();

            //initialise view model
            var VM = new CYCAAdmissionViewModel();
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            VM.Admission_Id = Id;
            return PartialView("~/Views/Admit/_ReAdmitIllegalItemNew.cshtml", VM);
        }
       

        public PartialViewResult Discharge(int Id)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityId = GetFacilityIdByUserID(userId);
            //var venueId = GetUsersByVenueId(venueId);

            //Get Venue Staff
            List<StaffDisplay> staffList = (from u in db.Users
                                            join e in db.Employees on u.User_Id equals e.User_Id
                                            where e.Facility_Id == facilityId 
                                            select new StaffDisplay()
                                            {
                                                Id = u.User_Id,
                                                ShortName = u.First_Name + " " + u.Last_Name,
                                                roles = u.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToList()
                                            }).ToList();

            var p = db.Persons.Where(pp => pp.Person_Id == Id).Single();
            var staffViewList = new List<StaffDisplay>();

            foreach (var s in staffList)
            {
                if (s.roles.Count > 0)
                {
                    staffViewList.Add(new StaffDisplay()
                    {
                        Id = s.Id,
                        ShortName = s.ShortName + " (" + String.Join(" ", s.roles) + ")",
                    });
                }
            }

            //List<StaffDisplay> staffViewList = (from s in staffList                                               
            //                                    select new StaffDisplay()
            //                                    {
            //                                        Id = s.Id,
            //                                        ShortName = s.ShortName + " (" + String.Join(" ", s.roles) + ")",
            //                                    }).ToList();
         


            ViewBag.StaffViewList = new SelectList(staffViewList, "Id", "ShortName");

          
            ViewBag.StaffList = new SelectList(staffList, "Id", "ShortName", "roles");

            List<apl_Cyca_Discharge_ReceivingPerson_Designation_Type> receivingPersonDesignationList = db.apl_Cyca_Discharge_ReceivingPerson_Designation_Type.ToList();
            ViewBag.ReceivingPersonDesignationList = new SelectList(receivingPersonDesignationList, "DesignationTypeId", "Description");

            List<Organization> organizations = db.Organizations.Where(o => o.Is_Active == true && o.Is_Deleted == false).ToList();
            if(organizations.Exists(o=>o.Organisation_Type_Id==6))
            {
                ViewBag.OrganizationList = new SelectList(organizations.Where(o=>o.Organisation_Type_Id==6).ToList(), "Organization_Id", "Description");
            }
            else
            {
                ViewBag.OrganizationList = new SelectList(organizations, "Organization_Id", "Description");
            }



            List<apl_Cyca_Admission_Discharge_Reason> discharge_Reasons = db.apl_Cyca_Admission_Discharge_Reason.ToList();
            ViewBag.DischargeReasonList = new SelectList(discharge_Reasons, "DischargeReasonId", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");
            //Setup ViewModel
            CYCADischargeViewModel vm = new CYCADischargeViewModel();
            vm.PersonId = Id;
            vm.AdmissionId = GetActiveAdmissionID(vm.PersonId, facilityId);
            vm.ChildFullName = p.First_Name + " " + p.Last_Name;
            vm.RequestType = "Discharge";
            return PartialView("~/Views/Admit/_Discharge.cshtml", vm);

        }
        public JsonResult AddDischarge(CYCADischargeViewModel vm)
        {
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var client = db.Clients.Where(c => c.Person_Id == vm.PersonId).Single();
            var facility = GetFacilityIdByUserID(userId);
            //if (vm.AdmissionId < 1)
            //{
            //    var Admissionid = db.CYCA_ReAdmissionDetails.Where(a => a.Client_Id == client.Client_Id && a.Is_Active == true).Single();
            //    vm.AdmissionId = Admissionid.Admission_Id;

            //    var readmission = db.CYCA_ReAdmissionDetails.Where(a => a.Admission_Id == vm.AdmissionId && a.Is_Active == true).Single();
            //    readmission.Is_Active = false;
            //}
            //else
            //{
                
            //}

            var admission = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Admission_Id == vm.AdmissionId).Single();
            admission.Is_Active = false;

            CYCA_Admissions_Discharge dis = new CYCA_Admissions_Discharge()
            {
                KeepBedSpace = vm.KeepBedSpace,
                Comments = vm.Comments,
                AdmissionId = vm.AdmissionId,
                DischargeDate = Convert.ToDateTime(vm.DischargeDate),
                DischargeReasonId = vm.DischargeReasonId ?? 0,

                PersonReceivingDesignationId = vm.UserReceivedDesignationId ?? 0,
                OgranizationId = vm.UserReceivedOrganisationId ?? 0,
                OtherOrgComment = vm.OtherOrgComment,
                PersonReceivingName = vm.UserReceivedName,
                //ReturnDate = vm.KeepBedSpace?Convert.ToDateTime(vm.ExpectedReturnDate):null,
                UserHandedOverId = vm.UserHandedOverId ?? 0,
                UserId = userId,
                DocType_Id = vm.DocType_Id   
            };
            if (dis.KeepBedSpace)
            {
                dis.ReturnDate = Convert.ToDateTime(vm.ExpectedReturnDate);
            }
                       
            db.CYCA_Admissions_Discharge.Add(dis);
            db.SaveChanges();
            var id = dis.DischargeId;
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = dis.AdmissionId,
                        Document_Type_Id = vm.DocType_Id,
                        DischargeId = id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();
                }
             
            }
            db.SaveChanges();
            var result = dis.DischargeId;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult UpdateDischarge(CYCADischargeViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;
          
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var document = db.CYCA_Admissions_Discharge.Where(d => d.DischargeId == vm.DischargeId).SingleOrDefault();

            var record = db.CYCA_Admissions_Discharge.Where(d => d.DischargeId == vm.DischargeId).SingleOrDefault();
           
            record.KeepBedSpace = vm.KeepBedSpace;
            record.Comments = vm.Comments;
        
            record.DischargeReasonId = vm.DischargeReasonId ?? 0;
            record.PersonReceivingDesignationId = vm.UserReceivedDesignationId ?? 0;
            record.OtherOrgComment = vm.OtherOrgComment;
            record.OgranizationId = vm.UserReceivedOrganisationId ?? 0;
            record.PersonReceivingName = vm.UserReceivedName;
            record.DocType_Id = vm.DocType_Id;
            record.UserHandedOverId = vm.UserHandedOverId ?? 0;
            record.UserId = userId;
            if (vm.ExpectedReturnDate != null)
            {
                record.ReturnDate = Convert.ToDateTime(vm.ExpectedReturnDate);
            }
            else { 
            }
            if (record != null)
            {
                db.Entry(document).CurrentValues.SetValues(record);
                db.SaveChanges();
            }
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                  
                    HttpPostedFileBase file = filesp[i];
                  //var exists = db.CYCA_Admissions_Document.Where(d => d.Document_Id == filesp[i].dischargeId)
                 
                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.AdmissionId,
                        DischargeId = record.DischargeId,
                        Document_Type_Id = vm.DocType_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };
                      
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();
                    
                }

            }
            ClientController client = new ClientController();
            var dynamicModel = new CYCADynamicFormModel();
          var history = client.GetDischarge(dynamicModel.GetClientIdByPersonId(vm.PersonId));
            return PartialView("~/Views/Client/_DischargeHistory.cshtml", history);
        }
        public bool DeleteDocumentFilesByIDs(int[] documentIDs)
        { 
            var state = true;
            if (documentIDs != null)
            {
                if (db.CYCA_Admissions_Document.Any(d => documentIDs.Contains(d.Document_Id)))
                {
                    var files = db.CYCA_Admissions_Document.Where(d => documentIDs.Contains(d.Document_Id));
                    db.CYCA_Admissions_Document.RemoveRange(files);
                    db.SaveChanges();
                    //check if files are deleted

                    if (db.CYCA_Admissions_Document.Contains(files.FirstOrDefault()))
                    {
                        state = true;
                    }

                }
                else if (db.CYCA_BodilySearch_Document.Any(d => documentIDs.Contains(d.Document_Id)))
                {
                    var files = db.CYCA_BodilySearch_Document.Where(d => documentIDs.Contains(d.Document_Id));
                    db.CYCA_BodilySearch_Document.RemoveRange(files);
                    db.SaveChanges();
                    //check if files are deleted

                    if (db.CYCA_BodilySearch_Document.Contains(files.FirstOrDefault()))
                    {
                        state = true;
                    }
                }
                else if (db.CYCA_IllegalItems_Document.Any(d => documentIDs.Contains(d.Document_Id)))
                {
                    var files = db.CYCA_IllegalItems_Document.Where(d => documentIDs.Contains(d.Document_Id));
                    db.CYCA_IllegalItems_Document.RemoveRange(files);
                    db.SaveChanges();
                    //check if files are deleted

                    if (db.CYCA_IllegalItems_Document.Contains(files.FirstOrDefault()))
                    {
                        state = true;
                    }
                }
             
            }
            return state;
        }

        public FileContentResult ViewDocument(int id)
        {
            CYCA_Admissions_Document doc = new CYCA_Admissions_Document();
            doc = db.CYCA_Admissions_Document.Where(a => a.Document_Id == id).SingleOrDefault();
            Response.AppendHeader("content-disposition", "inline; filename=file.pdf"); //this will open in a new tab.. remove if you want to open in the same tab.
            return File(doc.DataDocument, "application/pdf");
        }
        public ActionResult DownloadFile(int id)
        {
            CYCA_Admissions_Document doc = new CYCA_Admissions_Document();


            try
            {
                // Loading dile info.  
                doc = db.CYCA_Admissions_Document.Where(a => a.Document_Id == id).SingleOrDefault();

                // Info.  
                return this.GetFile(doc.DataDocument, doc.Document_Ext);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View(doc);
        }
        public ActionResult DownloadGangFile(int id)
        {
            CYCA_GangAndTatooDocument doc = new CYCA_GangAndTatooDocument();


            try
            {
                // Loading dile info.  
                doc = db.CYCA_GangAndTatooDocument.Where(a => a.Document_Id == id).SingleOrDefault();

                // Info.  
                return this.GetFile(doc.DataDocument, doc.DocAppExt);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View(doc);
        }
        private FileResult GetFile(byte[] fileContent, string fileContentType)
        {
            // Initialization.  
            FileResult file = null;

            try
            {
                // Get file.  
                // byte[] byteContent = Convert.FromBase64String(fileContentType);
                file = this.File(fileContent, fileContentType);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                // Info.  
                return null;
            }

            // info.  
            return file;
        }
        public ActionResult DownloadSearchFile(int id)
        {
            CYCA_BodilySearch_Document doc = new CYCA_BodilySearch_Document();


            try
            {
                // Loading dile info.  
                doc = db.CYCA_BodilySearch_Document.Where(a => a.Document_Id == id).SingleOrDefault();

                // Info.  
                return this.GetSearchFile(doc.DataDocument, doc.Document_Ext);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View(doc);
        }
        private FileResult GetSearchFile(byte[] fileContent, string fileContentType)
        {
            // Initialization.  
            FileResult file = null;

            try
            {
                // Get file.  
                // byte[] byteContent = Convert.FromBase64String(fileContentType);
                file = this.File(fileContent, fileContentType);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                // Info.  
                return null;
            }

            // info.  
            return file;
        }
        public ActionResult DownloadIllegalItemFile(int id)
        {
            CYCA_IllegalItems_Document doc = new CYCA_IllegalItems_Document();


            try
            {
                // Loading dile info.  
                doc = db.CYCA_IllegalItems_Document.Where(a => a.Document_Id == id).SingleOrDefault();

                // Info.  
                return this.GetSearchFile(doc.DataDocument, doc.Document_Ext);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View(doc);
        }
        private FileResult GetIllegalItemFile(byte[] fileContent, string fileContentType)
        {
            // Initialization.  
            FileResult file = null;

            try
            {
                // Get file.  
                // byte[] byteContent = Convert.FromBase64String(fileContentType);
                file = this.File(fileContent, fileContentType);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                // Info.  
                return null;
            }

            // info.  
            return file;
        }


        public PartialViewResult BodySearchRecords(int Id)
        {
            try
            {

                List<CYCAAdmissionBodySearchViewModel> returnModel = new List<CYCAAdmissionBodySearchViewModel>();
                var docL = db.CYCA_BodilySearch_Document.ToList();
                var L = db.CYCA_BodilySearch.Where(x => x.Person_Id == Id).ToList();
                var ListP = (
                   (from a in db.CYCA_BodilySearch

                    where a.Person_Id == Id
                    select new CYCAAdmissionBodySearchViewModel
                    {
                        BodySearchDate = a.Bodily_Search_Date.ToString(),
                        BodySearchId = a.Bodily_Search_Id,
                        Description = a.Description,
                        // AdmissionId = abs.Admission_Id,
                        WitnessedById = a.Witnessed_By,
                        // PersonId = c.Person_Id,
                        ConductedById = a.Conducted_By,
                        SearchReasonId = a.Search_Reason_Id
                    }).ToList());
                foreach (var item in ListP)
                {
                    item.Files = docL.Where(x => x.Bodily_Search_Id == item.BodySearchId).ToList();
                }
                returnModel.AddRange(ListP);
                return PartialView("~/Views/Admit/_BodySearchList.cshtml", returnModel);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return PartialView("~/Views/Admit/_BodySearchList.cshtml");
            }
        }
        public PartialViewResult CreateBodilySearch(int? Id)
        {
            var p = db.Persons.Where(pp => pp.Person_Id == Id).Single();
            CYCAAdmissionBodySearchViewModel VM = new CYCAAdmissionBodySearchViewModel();


            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facility = GetFacilityIdByUserID(userId);

            List<apl_Cyca_Bodily_Search_Reasons> search_Reasons = db.apl_Cyca_Bodily_Search_Reasons.ToList();
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");
            ViewBag.SearchReasonList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");
            ViewBag.Userlist = from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               join f in db.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id
                               where f.Facility_Id == facility
                               select new
                               {
                                   userId = u.User_Id,
                                   Full_Name = u.First_Name + " " + u.Last_Name
                               };

            //var data = (from s in db.CYCA_Admissions_BodilySearch
            //            join docs in db.CYCA_BodilySearch_Document on s.Bodily_Search_Id equals docs.Bodily_Search_Id


            //initialise view model
            VM.ChildFullName = p.First_Name + " " + p.Last_Name;
            VM.Person_Id = p.Person_Id;
            return PartialView("~/Views/Admit/_NewBodilySearch.cshtml", VM);


        }
        
        public JsonResult AddNewAdmission(CYCAAdmissionViewModel vm)
        {
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var client = db.Clients.Where(c => c.Person_Id == vm.Person_Id).Single();
            var facility = GetFacilityIdByUserID(userId);
            CYCA_Admissions_AdmissionDetails newob = new CYCA_Admissions_AdmissionDetails
            {
                Client_Id = client.Client_Id,
                Case_Start_Date = DateTime.Now,
                Case_Start_Time = DateTime.Now.ToString("HH:mm"),
                Case_End_Date = Convert.ToDateTime(vm.CaseEndDate),
                Case_End_Time = vm.CaseEndTime,
                Facility_Id = facility,
                Admission_Type_Id = vm.Admission_Type_Id,
                Comments_And_Observation = vm.CommentsAndObservation,
                Venue_Id = vm.VenueId,
                Document_Type_Id = vm.Document_Type_Id,
                OtherDocTypeDescription = vm.Additional_Info,
                Admission_Date = DateTime.Now,
                Date_Captured = DateTime.Now,
                Captured_By = userId,
                Date_Created = DateTime.Now,
                Created_By = currentUser.fullname,
                Date_Last_Modified = vm.Date_Last_Modified,
                Modified_By = vm.Modified_By,
                Is_Active = true,
                Is_Deleted = false
            };
            db.CYCA_Admissions_AdmissionDetails.Add(newob);
            //Add Gang Details

            // db.CYCA_Admissions_GangMembership.Add(gm);

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;
            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = newob.Admission_Id,
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
            var result = newob.Admission_Id;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddReAdmission(CYCAAdmissionViewModel vm)
        {
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            //var p = db.CYCA_Admissions_AdmissionDetails.Where(aa => aa.Admission_Id == vm.Admission_Id).Single();
           
            var client = db.Clients.Where(c => c.Person_Id == vm.Person_Id).Single();
            var facility = GetFacilityIdByUserID(userId);
            CYCA_ReAdmissionDetails newob = new CYCA_ReAdmissionDetails
            {               
                Admission_Id = vm.Admission_Id,
                Client_Id = client.Client_Id,
                Case_Start_Date = DateTime.Now,
                Case_Start_Time = DateTime.Now.ToString("HH:mm"),
                Case_End_Date = Convert.ToDateTime(vm.CaseEndDate),
                Case_End_Time = vm.CaseEndTime,
                Facility_Id = facility,
                Admission_Type_Id = vm.Admission_Type_Id,
                Comments_And_Observation = vm.CommentsAndObservation,
                Venue_Id = vm.VenueId,
                Document_Type_Id = vm.Document_Type_Id,
                //OtherGangMemberDescription = vm.GangMembership_Additional_Info,
                OtherDocTypeDescription = vm.Additional_Info,
                Admission_Date = DateTime.Now,
                Date_Captured = DateTime.Now,
                Captured_By = userId,
                Date_Created = DateTime.Now,
                Created_By = currentUser.fullname,                
                Is_Active = true,
                Is_Deleted = false
            };
            db.CYCA_ReAdmissionDetails.Add(newob);

            //var admission = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Client_Id == client.Client_Id && a.Is_Active == true).Single();
            //admission.Is_Active = false;

            
            CYCA_Admissions_AdmissionDetails edit = db.CYCA_Admissions_AdmissionDetails.Find(vm.Admission_Id);
            edit.Is_Active = true;
            edit.Admission_Date = DateTime.Now;
            edit.Date_Last_Modified = DateTime.Now;
            edit.Modified_By = userId.ToString();



            //int gangmemId = (from g in db.CYCA_Admissions_GangMembership
            //                 where g.Admission_Id == vm.Admission_Id && g.ReAdmission_Id == null
            //                 select g.Gang_Membership_Id).SingleOrDefault();

            //CYCA_Admissions_GangMembership editgang = db.CYCA_Admissions_GangMembership.Find(gangmemId);
            //editgang.Gang_Membership_Type_Id = vm.Gang_Member_Type_Id;

            //Add Gang Details
            //CYCA_Admissions_GangMembership gm = new CYCA_Admissions_GangMembership()
            //{
            //    Admission_Id = vm.Admission_Id,
            //    ReAdmission_Id = newob.ReAdmission_Id,
            //    Gang_Membership_Type_Id = vm.Gang_Member_Type_Id,
            //    Date_Captured = DateTime.Now,
            //    Date_Created = DateTime.Now,
            //    Created_By = currentUser.fullname,
            //    Is_Active = true,
            //    Is_Deleted = false
            //};
            //newob.CYCA_Admissions_GangMembership.Add(gm);
            //db.CYCA_Admissions_GangMembership.Add(gm);

            string uname = Request["uploadername"];
            
            HttpFileCollectionBase filesp = Request.Files;
            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = vm.Admission_Id,
                        ReAdmission_Id = newob.ReAdmission_Id,
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();
                }
            }

            db.SaveChanges();
            var result = newob.ReAdmission_Id;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddBodilySearch(CYCAAdmissionViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            CYCAdmissionModel model = new CYCAdmissionModel();
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
          
            int results = 0;
            try
            {
                CYCA_Admissions_BodilySearch newob = new CYCA_Admissions_BodilySearch();
                newob.Admission_Id = vm.Admission_Id;
                newob.Bodily_Search_Date = DateTime.Now;
                newob.Bodily_Search_Time = DateTime.Now;
                newob.Description = vm.Description;
                newob.Search_Reason_Id = vm.Search_Reason_Id;
                newob.Conducted_By = vm.Conducted_By;
                newob.Witnessed_By = vm.Witnessed_By;
                newob.Date_Created = DateTime.Now;
                newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                    join u in db.Users on a.Captured_By equals u.User_Id
                                    select u.First_Name + " " + u.Last_Name).FirstOrDefault();
                newob.Is_Active = true;
                newob.Is_Deleted = false;
                db.CYCA_Admissions_BodilySearch.Add(newob);
                db.SaveChanges();
                results = newob.Bodily_Search_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGangMembership(CYCAAdmissionViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            int results = 0;
            try
            {
                var gm = db.apl_Cyca_Gang_Membership_Type.Where(m => m.Gang_Membership_Type_Id == vm.Gang_Member_Type_Id).Single();

                CYCA_Admissions_GangMembership newob = new CYCA_Admissions_GangMembership();
                newob.Admission_Id = vm.Admission_Id;
                newob.Gang_Membership_Type_Id = vm.Gang_Member_Type_Id;
                if (gm.Description == "No Gang Membership")
                {
                    newob.Is_Member = false;
                }
                else
                {
                    newob.Is_Member = true;
                }
                newob.Date_Created = DateTime.Now;
                newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                    join u in db.Users on a.Captured_By equals u.User_Id
                                    select u.First_Name + " " + u.Last_Name).FirstOrDefault();
                newob.Is_Active = true;
                newob.Is_Deleted = false;
                db.CYCA_Admissions_GangMembership.Add(newob);
                db.SaveChanges();
                results = newob.Gang_Membership_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddIllegalItem(CYCAAdmissionViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            CYCAdmissionModel model = new CYCAdmissionModel();
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();
            int results = 0;
            try
            {
                CYCA_Admissions_IllegalItemsFound newob = new CYCA_Admissions_IllegalItemsFound();
                newob.Admission_Id = vm.Admission_Id;
                newob.Description = vm.Item_Description;
                newob.Quantity = vm.Quantity;
                newob.Handed_By = vm.Handed_By;
                newob.Date_Captured = DateTime.Now;
                newob.Date_Created = DateTime.Now;
                newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                    join u in db.Users on a.Captured_By equals u.User_Id
                                    select u.First_Name + " " + u.Last_Name).FirstOrDefault();
                newob.Is_Active = true;
                newob.Is_Deleted = false;
                db.CYCA_Admissions_IllegalItemsFound.Add(newob);
                db.SaveChanges();
                results = newob.Item_Found_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddUploadDocument(CYCAAdmissionViewModel vm)
        {

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = vm.Admission_Id,
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();
                }
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Enroll(EnrollModel vm)
        {
            var g = new Guid(vm.Id);
            int_DSD_Afis afis = new int_DSD_Afis()
            {
                Person_Id = vm.PersonId,
                Is_Verified = true,
                Uid = g,
                CreatedTimestamp = DateTime.Now
            };
            db.int_DSD_Afis.Add(afis);
            db.SaveChanges();
            return null;
        }
        public JsonResult DisEnroll(EnrollModel vm)
        {
            var g = new Guid(vm.Id);
            var rec = db.int_DSD_Afis.Where(d => d.Uid == g).SingleOrDefault();
            if (rec != null)
            {
                db.int_DSD_Afis.Remove(rec);
                db.SaveChanges();
            }
            return null;
        }
        public int GetFacilityIdByUserID(int UserId)
        {
            //return (from p in db.Employees
            //        join c in db.Users on p.User_Id equals c.User_Id
            //        join f in db.apl_Cyca_Facility on p.Organization_Id equals f.Organization_Id
            //        where c.User_Id == (UserId)
            //        select f.Facility_Id).SingleOrDefault();
            return (from f in db.apl_Cyca_Facility
                    join e in db.Employees on f.Facility_Id equals e.Facility_Id
                    join u in db.Users on e.User_Id equals u.User_Id
                    where u.User_Id == UserId
                    select f.Facility_Id).SingleOrDefault();

        }

        public int GetUsersByVenueId(int VenueId)
        {
            return (from p in db.Employees
                    join c in db.apl_Cyca_Venue on p.User_Id equals c.Venue_Id
                    join f in db.apl_Cyca_Facility on p.Organization_Id equals f.Organization_Id
                    where c.Venue_Id == (VenueId)
                    select f.Facility_Id).SingleOrDefault();
            //return (from f in db.apl_Cyca_Facility
            //        join e in db.Employees on f.Facility_Id equals e.Facility_Id
            //        join u in db.Users on e.User_Id equals u.User_Id
            //        where u.User_Id == UserId
            //        select f.Facility_Id).SingleOrDefault();

        }

        public int GetActiveAdmissionID(int personId, int facilityId)
        {
            return (from p in db.Persons
                    join c in db.Clients on p.Person_Id equals c.Person_Id
                    join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityId, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                    where p.Person_Id == personId
                    select a.Admission_Id).FirstOrDefault();
        }

        public PartialViewResult BodySearchList(int Id)
        {
            //Session["Pers_Id"] = id;
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            //string loginName = User.Identity.Name;
            //Session["LoginName"] = loginName;

            

            try
            {
                var userList = db.Roles
                                .Where(r => r.Description.Contains("CYCA"))
                                .SelectMany(x => x.Users)
                                .Distinct()
                                .ToList();
                ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");

                List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
                ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

                List<apl_Cyca_Bodily_Search_Reasons> search_Reasons = db.apl_Cyca_Bodily_Search_Reasons.ToList();
                ViewBag.SearchReasonList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");

                List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
                ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");
                
               

                //List<CYCAAdmissionBodySearchViewModel> returnModel = new List<CYCAAdmissionBodySearchViewModel>();
                CYCAAdmissionBodilySearchPartiallViewModel returnModel = new CYCAAdmissionBodilySearchPartiallViewModel();
                returnModel.CYCAAdmissionBodySearchViewModels = new List<CYCAAdmissionBodySearchViewModel>();
                returnModel.CycaAdmissionIllegalItemsViewModels = new List<CycaAdmissionIllegalItemsViewModel>();
                var docL = db.CYCA_BodilySearch_Document.ToList();
                var illegalDocL = db.CYCA_IllegalItems_Document.ToList();


                var use = ((from u in db.Users
                            join e in db.Employees on u.User_Id equals e.User_Id
                            join f in db.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id
                            select new
                            {
                                userId = u.User_Id,
                                Full_Name = u.First_Name + " " + u.Last_Name
                            }).ToList());
                //int PersonId = (from re in db.CYCA_ReAdmissionDetails
                //                join c in db.Clients on re.Client_Id equals c.Client_Id
                //                join p in db.Persons on c.Person_Id equals p.Person_Id
                //                where re.ReAdmission_Id == Id
                //                select p.Person_Id).FirstOrDefault();

                ViewBag.use = use;
                //Id = PersonId;
                ViewBag.childId = Id;





                var ListP = (from bs in db.CYCA_BodilySearch
                             join a in db.CYCA_Admissions_AdmissionDetails on bs.Admission_Id equals a.Admission_Id
                             join bsr in db.apl_Cyca_Bodily_Search_Reasons on bs.Search_Reason_Id equals bsr.Search_Reason_Id
                             join p in db.Persons on bs.Person_Id equals p.Person_Id
                             //join u in db.Users on bs.Conducted_By equals u.User_Id
                             where p.Person_Id == Id
                             select new CYCAAdmissionBodySearchViewModel
                             {
                                 BodySearchDate = bs.Bodily_Search_Date.ToString(),
                                 BodySearchId = bs.Bodily_Search_Id,
                                 Description = bs.Description,
                                 Admission_Id = a.Admission_Id,
                                 WitnessedBy = p.First_Name + " " + p.Last_Name,
                                 //WitnessedBy = db.Users.Find(bs.Witnessed_By).First_Name,                                 
                                 Person_Id = Id,
                                 ConductedBy = p.First_Name + " " + p.Last_Name,
                                 ReasonForSearch = bsr.Description,
                                 saveBodySearchId = bs.Bodily_Search_Id

            }).ToList();
                //int bodyid = Convert.ToInt32(ListP.Select(m => m.saveBodySearchId));
                //ViewBag.bodysearchId = bodyid;
                foreach (var item in ListP) 
                {
                    
                    //TempData["Message"] = item.BodySearchId;
                    item.Files = docL.Where(x => x.Bodily_Search_Id == item.BodySearchId).ToList();

                    int bodysearchid = Convert.ToInt32(item.Files.Where(x => x.Bodily_Search_Id == item.BodySearchId).Select(x => x.Bodily_Search_Id).FirstOrDefault());
                    ViewBag.bodysearchId = bodysearchid;

                }
                TempData["Message"] = ViewBag.bodysearchId;
                //var bodyId = (from bcd in db.CYCA_BodilySearch_Document
                //              join bc in db.CYCA_BodilySearch on bcd.Bodily_Search_Id equals bc.Bodily_Search_Id
                //              where bcd.Bodily_Search_Id == Convert.ToInt32(ListP.Select(x => x.BodySearchId).FirstOrDefault())
                //              select bcd.Bodily_Search_Id).ToList();
                //foreach (var x in bodyId)
                //{
                //    ViewBag.bodysearchId = x;
                //}


                var IllegalItemList = (from ii in db.CYCA_Admissions_IllegalItemsFound
                                       join a in db.CYCA_Admissions_AdmissionDetails on ii.Admission_Id equals a.Admission_Id
                                       join u in db.Users on ii.Handed_By equals u.User_Id
                                       join p in db.Persons on ii.Person_Id equals p.Person_Id
                                       //join u in db.Users on bs.Conducted_By equals u.User_Id
                                       where p.Person_Id == Id
                                       select new CycaAdmissionIllegalItemsViewModel
                                       {
                                           Item_Found_Id = ii.Item_Found_Id,
                                           Description = ii.Description,
                                           Quantity = ii.Quantity,
                                           selectedHandedBy = u.First_Name + " " + u.Last_Name,
                                           IllegalItemDate = ii.Date_Captured.ToString()
                                       }).ToList();
                foreach (var item in IllegalItemList)
                {
                    item.Files = illegalDocL.Where(x => x.Item_Found_Id == item.Item_Found_Id).ToList();
                }
                //returnModel.AddRange(ListP);
                returnModel.CYCAAdmissionBodySearchViewModels.AddRange(ListP);
                returnModel.CycaAdmissionIllegalItemsViewModels.AddRange(IllegalItemList);
                return PartialView("~/Views/Admit/_BodySearchList.cshtml", returnModel);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Admit/_BodySearchList.cshtml");
            }
        }

        public JsonResult NewCLientImage(CYCA_BodilySearch vm)
        {


            // var currentUser = (User)Session["CurrentUser"];
            //var userId = -1;

            //if (currentUser != null)
            //{
            //    userId = currentUser.User_Id;
            //}
            HttpFileCollectionBase filesp = Request.Files;
            try
            {


                if (filesp != null && filesp.Count > 0)
                {
                    for (int i = 0; i < filesp.Count; i++)
                    {
                        HttpPostedFileBase file = filesp[i];
                        apl_User_Image doc = new apl_User_Image()
                        {
                    
                            User_Id = 32,
                            Image_Filename = file.FileName,                     
                            Date_Created = DateTime.Now,
                            Image_Type_Id = 1,
                            Image_Content_Type = "img",

                            Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                          join u in db.Users on a.Captured_By equals u.User_Id
                                          select u.First_Name + " " + u.Last_Name).FirstOrDefault()
                         
                        };

                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            doc.Image_Content = reader.ReadBytes(file.ContentLength);
                        }
                        db.apl_User_Image.Add(doc);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NewBodilySearch(CYCA_BodilySearch vm)
        {
            int adminId = 0;
            int personId = 0;
            if (vm.Admission_Id == null)
            {
                var admissionId = (from a in db.CYCA_Admissions_AdmissionDetails
                                   //join re in db.CYCA_ReAdmissionDetails on a.Admission_Id equals re.Admission_Id
                                   join c in db.Clients on a.Client_Id equals c.Client_Id
                                   join pp in db.Persons on c.Person_Id equals pp.Person_Id
                                   where pp.Person_Id == vm.Person_Id & a.Is_Active == true
                                   select a.Admission_Id).Single();
                adminId = admissionId;
                personId = Convert.ToInt32(vm.Person_Id);
            }
            else if (vm.Person_Id == null)
            {
                //int PersonId = (from re in db.CYCA_ReAdmissionDetails
                //                join c in db.Clients on re.Client_Id equals c.Client_Id
                //                join p in db.Persons on c.Person_Id equals p.Person_Id
                //                where re.ReAdmission_Id == Id
                //                select p.Person_Id).FirstOrDefault();
                var p = (from a in db.CYCA_Admissions_AdmissionDetails
                         
                         join c in db.Clients on a.Client_Id equals c.Client_Id
                         join pp in db.Persons on c.Person_Id equals pp.Person_Id
                         where a.Admission_Id == vm.Admission_Id
                         select pp).Single();
                personId = p.Person_Id;
                adminId = Convert.ToInt32(vm.Admission_Id);
            }
                                             
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;
            CYCA_BodilySearch body = new CYCA_BodilySearch()
            {

                Admission_Id = adminId,
                Conducted_By = vm.Conducted_By,
                Description = vm.Description,
                Witnessed_By = vm.Witnessed_By,
                Search_Reason_Id = vm.Search_Reason_Id,
                Date_Created = DateTime.Now,
                Person_Id = personId,
                Document_Type_Id = vm.Document_Type_Id,
                OtherDocTypeDescription = vm.OtherDocTypeDescription,
                OtherSeacrhReasonDescription = vm.OtherSeacrhReasonDescription,
                Bodily_Search_Date = DateTime.Now,
                Bodily_Search_Time = DateTime.Now,
                Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                              join u in db.Users on a.Captured_By equals u.User_Id
                              select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                Is_Active = true,
                Is_Deleted = false

            };
            db.CYCA_BodilySearch.Add(body);
            db.SaveChanges();

            // var currentUser = (User)Session["CurrentUser"];
            //var userId = -1;

            //if (currentUser != null)
            //{
            //    userId = currentUser.User_Id;
            //}
            try
            {

                var currentUser = (User)Session["CurrentUser"];
                var userId = -1;

                if (currentUser != null)
                {
                    userId = currentUser.User_Id;
                }
                if (filesp != null && filesp.Count > 0)
                {
                    for (int i = 0; i < filesp.Count; i++)
                    {
                        HttpPostedFileBase file = filesp[i];
                        CYCA_BodilySearch_Document doc = new CYCA_BodilySearch_Document()
                        {
                            Bodily_Search_Id = body.Bodily_Search_Id,
                            DocumentType = file.ContentType,
                            Document_Type_Id = vm.Document_Type_Id,
                            Document_Name = file.FileName,
                            Document_Ext = Path.GetExtension(file.FileName),
                            DateSaved = DateTime.Now,
                            Date_Created = DateTime.Now,
                            Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                          join u in db.Users on a.Captured_By equals u.User_Id
                                          select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                            Is_Active = true,
                            Is_Deleted = false
                        };

                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            doc.DataDocument = reader.ReadBytes(file.ContentLength);
                        }
                        db.CYCA_BodilySearch_Document.Add(doc);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NewIllegalItem(CycaAdmissionIllegalItemsViewModel vm)
        {
            int adminId = 0;
            int personId = 0;
            if (vm.Admission_Id == null)
            {
                var admissionId = (from a in db.CYCA_Admissions_AdmissionDetails
                                   //join re in db.CYCA_ReAdmissionDetails on a.Admission_Id equals re.Admission_Id
                                   join c in db.Clients on a.Client_Id equals c.Client_Id
                                   join pp in db.Persons on c.Person_Id equals pp.Person_Id
                                   where pp.Person_Id == vm.Person_Id & a.Is_Active == true
                                   select a.Admission_Id).Single();
                adminId = admissionId;
                personId = Convert.ToInt32(vm.Person_Id);
            }
            else if (vm.Person_Id == null)
            {
                var p = (from a in db.CYCA_Admissions_AdmissionDetails
                         
                         join c in db.Clients on a.Client_Id equals c.Client_Id
                         join pp in db.Persons on c.Person_Id equals pp.Person_Id
                         where a.Admission_Id == vm.Admission_Id 
                         select pp).Single();
                personId = p.Person_Id;
                adminId = Convert.ToInt32(vm.Admission_Id);
            }

            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;
            CYCA_Admissions_IllegalItemsFound body = new CYCA_Admissions_IllegalItemsFound()
            {

                Admission_Id = adminId,                               
                Description = vm.Description,
                Quantity = vm.Quantity,
                Handed_By = vm.Handed_By,
                Person_Id = personId,
                Document_Type_Id = vm.DocType_Id,
                OtherDocTypeDescription = vm.Additional_Info,
                Date_Captured = DateTime.Now,
                Captured_By = userId,               
                Date_Created = DateTime.Now,                                
                Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                              join u in db.Users on a.Captured_By equals u.User_Id
                              select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                Is_Active = true,
                Is_Deleted = false

            };
            db.CYCA_Admissions_IllegalItemsFound.Add(body);
            db.SaveChanges();

            // var currentUser = (User)Session["CurrentUser"];
            //var userId = -1;

            //if (currentUser != null)
            //{
            //    userId = currentUser.User_Id;
            //}
            try
            {

               
                if (filesp != null && filesp.Count > 0)
                {
                    for (int i = 0; i < filesp.Count; i++)
                    {
                        HttpPostedFileBase file = filesp[i];
                        CYCA_IllegalItems_Document doc = new CYCA_IllegalItems_Document()
                        {
                            Item_Found_Id = body.Item_Found_Id,
                            DocumentType = file.ContentType,
                            Document_Type_Id = vm.DocType_Id,
                            Document_Name = file.FileName,
                            Document_Ext = Path.GetExtension(file.FileName),
                            DateSaved = DateTime.Now,
                            Date_Created = DateTime.Now,
                            Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                          join u in db.Users on a.Captured_By equals u.User_Id
                                          select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                            Is_Active = true,
                            Is_Deleted = false
                        };

                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            doc.DataDocument = reader.ReadBytes(file.ContentLength);
                        }
                        db.CYCA_IllegalItems_Document.Add(doc);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NewBodilySearchdoc(CYCAAdmissionBodySearchViewModel vm)
        {

            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }



            CYCA_Admissions_BodilySearch body = new CYCA_Admissions_BodilySearch()
            {
                Conducted_By = vm.ConductedById,
                Description = vm.Description,
                Witnessed_By = vm.WitnessedById,
                Search_Reason_Id = vm.SearchReasonId,
                Date_Created = DateTime.Now,
                Bodily_Search_Date = DateTime.Now,
                Bodily_Search_Time = DateTime.Now,
                Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                              join u in db.Users on a.Captured_By equals u.User_Id
                              select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                Is_Active = true,
                Is_Deleted = false

            };
            db.CYCA_Admissions_BodilySearch.Add(body);
            db.SaveChanges();



            return Json(1, JsonRequestBehavior.AllowGet);
        }
        
        public CYCAAdmissionBodySearchViewModel GetBodilySearchId(int Id)
        {
            CYCAAdmissionBodySearchViewModel vm = new CYCAAdmissionBodySearchViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_BodilySearch act = db.CYCA_BodilySearch.Find(Id);
                    if (act != null)
                    {
                        vm.BodySearchId = Id;
                        vm.ConductedBy = db.Users.Find(act.Conducted_By).First_Name + " " + db.Users.Find(act.Conducted_By).Last_Name;
                        vm.BodySearchDate = act.Bodily_Search_Date.ToString();
                        vm.WitnessedBy = db.Users.Find(act.Witnessed_By).First_Name + " " + db.Users.Find(act.Witnessed_By).Last_Name;
                        vm.Description = act.Description;
                        vm.ReasonForSearch = db.apl_Cyca_Bodily_Search_Reasons.Find(act.Search_Reason_Id).Description;

                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {


                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return vm;
        }      
        public JsonResult GetBodySearchByBodySearchId(int BodySearchId)
        {
            CYCAAdmissionsModel model = new CYCAAdmissionsModel();
            CYCAAdmissionBodilySearchPartiallViewModel vm = new CYCAAdmissionBodilySearchPartiallViewModel();

            vm = model.GetBodySearchByBodySearchId(BodySearchId);
            ViewBag.bodysearchId = BodySearchId;
            //ViewBag.bodysearchid = BodySearchId;
            TempData["Message"] = BodySearchId;

            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetIllegalItemByIllegalItemId(int Item_Found_Id)
        {
            
            CYCAAdmissionsModel model = new CYCAAdmissionsModel();
            CYCAAdmissionBodilySearchPartiallViewModel vm = new CYCAAdmissionBodilySearchPartiallViewModel();

            vm = model.GetIllegalItemByIllegalItemId(Item_Found_Id);

            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        public JsonResult EditBodySearch(CYCAAdmissionBodilySearchPartiallViewModel vm)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var result = false;

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            int BodySearchId = Convert.ToInt32(vm.BodySearchId);
            if (BodySearchId > 0)
            {
                CYCA_BodilySearch edit = db.CYCA_BodilySearch.Find(BodySearchId);
                edit.Search_Reason_Id = vm.Search_Reason_Id;
                edit.Conducted_By = vm.Conducted_By;
                edit.Witnessed_By = vm.Witnessed_By;
                edit.Document_Type_Id = vm.Document_Type_Id;
                edit.Description = vm.Description;               
                edit.Date_Last_Modified = DateTime.Now;
                edit.Modified_By = db.Users.Find(userId).First_Name + " " + db.Users.Find(userId).Last_Name;


                if (filesp != null && filesp.Count > 0)
                {
                    for (int i = 0; i < filesp.Count; i++)
                    {
                        HttpPostedFileBase file = filesp[i];
                        CYCA_BodilySearch_Document doc = db.CYCA_BodilySearch_Document.Find(BodySearchId);
                        {

                            doc.DocumentType = file.ContentType;
                            doc.Document_Type_Id = vm.Document_Type_Id;
                            doc.Document_Name = file.FileName;
                            doc.Document_Ext = Path.GetExtension(file.FileName);
                            doc.Modified_By = db.Users.Find(userId).First_Name + " " + db.Users.Find(userId).Last_Name;
                            doc.Date_Last_Modified = DateTime.Now;
                          
                        };

                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            doc.DataDocument = reader.ReadBytes(file.ContentLength);
                        }
                        db.CYCA_BodilySearch_Document.Add(doc);                       
                    }
                }

                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }      
        public PartialViewResult UpdateIllegalItem(CycaAdmissionIllegalItemsViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var document = db.CYCA_Admissions_IllegalItemsFound.Where(d => d.Item_Found_Id == vm.Item_Found_Id).SingleOrDefault();

            var record = db.CYCA_Admissions_IllegalItemsFound.Where(d => d.Item_Found_Id == vm.Item_Found_Id).SingleOrDefault();

            
            record.Description = vm.Description;
            record.Quantity = vm.Quantity;
            record.Handed_By = vm.Handed_By;
            record.Document_Type_Id = vm.DocType_Id;
            record.OtherDocTypeDescription = vm.Additional_Info;
            record.Modified_By = userId.ToString();
            record.Date_Last_Modified = DateTime.Now;
            

            if (record != null)
            {
                db.Entry(document).CurrentValues.SetValues(record);
                db.SaveChanges();
            }
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {

                    HttpPostedFileBase file = filesp[i];
                    //var exists = db.CYCA_Admissions_Document.Where(d => d.Document_Id == filesp[i].dischargeId)

                    CYCA_IllegalItems_Document doc = new CYCA_IllegalItems_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,
                        Item_Found_Id = record.Item_Found_Id,
                        Document_Type_Id = vm.DocType_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_IllegalItemsFound
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_IllegalItems_Document.Add(doc);
                    db.SaveChanges();

                }

            }
            ClientController client = new ClientController();
            var dynamicModel = new CYCADynamicFormModel();
            var history = client.GetBodySearchAndIllegalItem(dynamicModel.GetClientIdByPersonId(Convert.ToInt32(vm.Person_Id)));
            return PartialView("~/Views/Client/_BodySearchAndIllegalItemHistory.cshtml", history);
        }
        public PartialViewResult UpdateBodySearch(CYCAAdmissionBodySearchViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var document = db.CYCA_BodilySearch.Where(d => d.Bodily_Search_Id == vm.BodySearchId).SingleOrDefault();

            var record = db.CYCA_BodilySearch.Where(d => d.Bodily_Search_Id == vm.BodySearchId).SingleOrDefault();


            record.Description = vm.Description;
            record.Conducted_By = vm.ConductedById;
            record.Witnessed_By = vm.WitnessedById;
            record.Document_Type_Id = vm.Document_Type_Id;
            record.Search_Reason_Id = vm.SearchReasonId;
            record.OtherSeacrhReasonDescription = vm.OtherSeacrhReasonDescription;
            record.OtherDocTypeDescription = vm.OtherDocTypeDescription;
            record.Modified_By = userId.ToString();
            record.Date_Last_Modified = DateTime.Now;


            if (record != null)
            {
                db.Entry(document).CurrentValues.SetValues(record);
                db.SaveChanges();
            }
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {

                    HttpPostedFileBase file = filesp[i];
                    //var exists = db.CYCA_Admissions_Document.Where(d => d.Document_Id == filesp[i].dischargeId)

                    CYCA_BodilySearch_Document doc = new CYCA_BodilySearch_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,
                        Bodily_Search_Id = record.Bodily_Search_Id,
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_IllegalItemsFound
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_BodilySearch_Document.Add(doc);
                    db.SaveChanges();

                }

            }
            ClientController client = new ClientController();
            var dynamicModel = new CYCADynamicFormModel();
            var history = client.GetBodySearchAndIllegalItem(dynamicModel.GetClientIdByPersonId(Convert.ToInt32(vm.Person_Id)));
            return PartialView("~/Views/Client/_BodySearchAndIllegalItemHistory.cshtml", history);
        }
    
        public ActionResult GetExtraMuralList()
        {
            //var CYCAAdmissionExtraMuralActivityViewModel = new CYCAAdmissionExtraMuralActivityViewModel();
            int Id = Convert.ToInt32(Session["Pers_Id"]);
            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(Id);           
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            CYCA_ExtraMuralActivityModel model = new CYCA_ExtraMuralActivityModel();
            //CYCAAdmissionExtraMuralActivityViewModel VM = new CYCAAdmissionExtraMuralActivityViewModel();

            List<CYCAAdmissionExtraMuralActivityViewModel> lis = new List<CYCAAdmissionExtraMuralActivityViewModel>();
            lis.AddRange(model.GetExtraMuralActivityList(Id).Select(x => new CYCAAdmissionExtraMuralActivityViewModel
            {
                Extra_Mural_Activity_Id = x.Extra_Mural_Activity_Id,
                Admission_Id = x.Admission_Id,
                selectedPhysicalBuild = x.selectedPhysicalBuild,
                selectedEyeColor = x.selectedEyeColor,
                selectedHairColor = x.selectedHairColor,
                selectedHobby = x.selectedHobby,
                selectedSportActivity = x.selectedSportActivity,
                DateCreated = x.DateCreated,                
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddExtraMuralActivity(CYCAAdmissionExtraMuralActivityViewModel vm)
        {            
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];            
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;     
            }
            var result = false;
            var CYCAAdmissionExtraMuralActivityViewModel = new CYCAAdmissionExtraMuralActivityViewModel();            
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();        
            var admitId = (from a in db.CYCA_Admissions_AdmissionDetails                          
                           join c in db.Clients on a.Client_Id equals c.Client_Id
                           join pp in db.Persons on c.Person_Id equals pp.Person_Id
                           where a.Is_Active == true && pp.Person_Id == vm.Person_Id
                           select a.Admission_Id).Single();
            int Id = admitId;            
            if (Id > 0)               
            {
                CYCA_Admissions_ExtraMuralActivity newob = new CYCA_Admissions_ExtraMuralActivity();
                newob.Weight = vm.Weight;              
                newob.Hobby_Id = string.Join(",", vm.Hobby_Id);                
                newob.Activity_Id = string.Join(",", vm.Activity_Id);
                newob.Eye_Color_Id = vm.Eye_Color_Id;
                newob.Hair_Color_Id = vm.Hair_Color_Id;
                newob.Physical_Build_Id = vm.Physical_Build_Id;
                newob.Admission_Id = Id;
                newob.Date_Created = DateTime.Now;
                newob.Created_By = db.Users.Find(userId).First_Name + " " + db.Users.Find(userId).Last_Name;
                newob.Is_Active = true;
                newob.Is_Deleted = false;
                newob.Description = vm.Additional_Description;

                db.CYCA_Admissions_ExtraMuralActivity.Add(newob);
                db.SaveChanges();
                
                result = true;
            }         
            return Json(result, JsonRequestBehavior.AllowGet);
        }      
        public JsonResult GetActivitybyActivityId(int ActivityId)
        {
            CYCA_ExtraMuralActivityModel model = new CYCA_ExtraMuralActivityModel();
            CYCAAdmissionExtraMuralActivityViewModel vm = new CYCAAdmissionExtraMuralActivityViewModel();

            vm = model.GetActivitybyActivityId(ActivityId);

            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        public JsonResult EditExtraMuralActivity(CYCAAdmissionExtraMuralActivityViewModel vm)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var result = false;         

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();         
            int extramuralActivityId = Convert.ToInt32(vm.Extra_Mural_Activity_Id);           
            if (vm.Extra_Mural_Activity_Id > 0)
            {
                CYCA_Admissions_ExtraMuralActivity edit = db.CYCA_Admissions_ExtraMuralActivity.Find(extramuralActivityId);
                edit.Weight = vm.Weight;
                edit.Hobby_Id = string.Join(",", vm.Hobby_Id);
                edit.Activity_Id = string.Join(",", vm.Activity_Id);
                edit.Eye_Color_Id = vm.Eye_Color_Id;
                edit.Hair_Color_Id = vm.Hair_Color_Id;
                edit.Physical_Build_Id = vm.Physical_Build_Id;                                                             
                edit.Description = vm.Additional_Description;
                edit.Date_Last_Modified = DateTime.Now;
                edit.Modified_By = db.Users.Find(userId).First_Name + " " + db.Users.Find(userId).Last_Name;
               
                db.SaveChanges();               
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Populate Admission & Readmission table
        //public CYCAAdmissionPartialViewModel GetAdmissionAndReadmission(int PersonId)
        //{
        //    var readmissionHistory = GetReAdmissionByPersonId(PersonId);
        //    var readmissionViewList = new List<CYCAAdmissionViewModel>();

        //    var admissionHistory = GetAdmissionByPersonId(PersonId);
        //    var admissionViewList = new List<CYCAAdmissionViewModel>();
           
        //    var records = new CYCAAdmissionPartialViewModel
        //    {
        //        PersonId = PersonId,
        //        CYCAAdmissionViewModels = admissionHistory,
        //        readmissionHistory
        //        //CYCAAdmissionViewModels = readmissionHistory
        //    };
        //    return records;
        //}

        //ReAdmission 
        public CYCAAdmissionPartialViewModel GetReAdmission(int PersonId)
        {
            var readmissionHistory = GetReAdmissionByPersonId(PersonId);
            var readmissionViewList = new List<CYCAAdmissionViewModel>();

            var records = new CYCAAdmissionPartialViewModel
            {
                PersonId = PersonId,
                CYCAAdmissionViewModels = readmissionHistory,
            };
            return records;
        }
        public List<CYCAAdmissionViewModel> GetReAdmissionByPersonId(int PersonId)
        {
            var data = (from re in db.CYCA_ReAdmissionDetails
                        join c in db.Clients on re.Client_Id equals c.Client_Id
                        join p in db.Persons on c.Person_Id equals p.Person_Id
                        join at in db.apl_Admission_Type on re.Admission_Type_Id equals at.Admission_Type_Id
                        //join gm in db.CYCA_Admissions_GangMembership on re.ReAdmission_Id equals gm.ReAdmission_Id
                        //join gmd in db.apl_Cyca_Gang_Membership_Type on gm.Gang_Membership_Type_Id equals gmd.Gang_Membership_Type_Id
                        join f in db.apl_Cyca_Facility on re.Facility_Id equals f.Facility_Id
                        join v in db.apl_Cyca_Venue on re.Venue_Id equals v.Venue_Id   
                        //join docs in db.CYCA_Admissions_Document on re.ReAdmission_Id equals docs.ReAdmission_Id
                        where c.Person_Id == PersonId
                        select new CYCAAdmissionViewModel
                        {
                            Re_Admission_Id = re.ReAdmission_Id,
                            Admission_Id = re.Admission_Id,
                            Person_Id = PersonId,
                            childFullNames = p.First_Name + " " + p.Last_Name,
                            CaseStartDate = re.Case_Start_Date,
                            CaseEndDate = re.Case_End_Date.ToString(),                            
                            AdmissionDate = re.Date_Captured.ToString(),
                            selectedAdmissionType = at.Description,
                            //SelectedGangMemberType = gmd.Description,
                            FacilityName = f.FacilityName,
                            selectedVenue = v.VenueName,
                            DateCreated = re.Date_Created.ToString(),
                            Is_Active = re.Is_Active,
                            Admission_Type_Id = at.Admission_Type_Id,
                            //Gang_Member_Type_Id = gmd.Gang_Membership_Type_Id,
                            VenueId = v.Venue_Id,
                            Document_Type_Id = re.Document_Type_Id,
                            CommentsAndObservation = re.Comments_And_Observation,
                            Additional_Info = re.OtherDocTypeDescription,
                            //GangMembership_Additional_Info = re.OtherGangMemberDescription

                            //ActiveAdmission = Activeadmission,
                            //ActiveReAdmission = ActiveReadmission

                        }).ToList();
            foreach (var d in data)
            {
                var readmissionFiles = new List<CYCA_Admissions_Document>();
                d.files = children.GetFilesByReAdmissionID(d.Re_Admission_Id);
            }
            return data;
        }
        public PartialViewResult EditViewReadmission(int id, int PersonId, string display)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var p = db.Persons.Where(pp => pp.Person_Id == PersonId).Single();
            //Biometrics
            var afis = db.int_DSD_Afis.Where(af => af.Person_Id.Equals(p.Person_Id)).SingleOrDefault();

            List<apl_Admission_Type> admission_Types = db.apl_Admission_Type.ToList();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            //initialise view model
            var VM = new CYCAAdmissionViewModel();
            VM.Person_Id = PersonId;
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            if (afis != null)
            {
                VM.HasBiometric = true;
                VM.IsPivaVerified = p.Is_Piva_Validated;
                VM.IsVerified = afis.Is_Verified;
            }
            else
            {
                VM.HasBiometric = false;
                VM.IsPivaVerified = false;
                VM.IsVerified = false;
            }

            var file = this.GetReAdmission(PersonId);
            var readmit = file.CYCAAdmissionViewModels.Where(c => c.Re_Admission_Id == id).SingleOrDefault();
            readmit.RequestType = display;
            return PartialView("~/Views/Admit/_ReAdmitNew.cshtml", readmit);
        }
        public PartialViewResult UpdateReAdmission(CYCAAdmissionViewModel vm)
        {

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            //var document = db.CYCA_Admissions_Document.Where(d => d.ReAdmission_Id == vm.Re_Admission_Id);
            var document = db.CYCA_ReAdmissionDetails.Where(d => d.ReAdmission_Id == vm.Re_Admission_Id).SingleOrDefault();

            var record = db.CYCA_ReAdmissionDetails.Where(d => d.ReAdmission_Id == vm.Re_Admission_Id).SingleOrDefault();

            record.Venue_Id = vm.VenueId;
            record.Case_End_Date = Convert.ToDateTime(vm.CaseEndDate);
            record.Comments_And_Observation = vm.CommentsAndObservation;
            record.Admission_Type_Id = vm.Admission_Type_Id;
            record.Document_Type_Id = vm.Document_Type_Id;            
            record.OtherDocTypeDescription = vm.Additional_Info;
            //record.OtherGangMemberDescription = vm.GangMembership_Additional_Info;
            record.Modified_By = userId.ToString();
            record.Date_Last_Modified = DateTime.Now;

            //if (record != null)
            //{
            //    CYCA_Admissions_GangMembership edit = db.CYCA_Admissions_GangMembership.Where(j => j.ReAdmission_Id == record.ReAdmission_Id).FirstOrDefault();
            //    if (edit != null)
            //    {
            //        edit.Gang_Membership_Type_Id = vm.Gang_Member_Type_Id;
            //    }

            //    db.Entry(document).CurrentValues.SetValues(record);
            //    db.SaveChanges();
            //}

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];
                    //var exists = db.CYCA_Admissions_Document.Where(d => d.Document_Id == filesp[i].dischargeId)
                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,
                        ReAdmission_Id = record.ReAdmission_Id,
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_IllegalItemsFound
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();

                }

            }
            return Admission(vm.Person_Id);
        }



        //Admission
        public CYCAAdmissionPartialViewModel GetAdmission(int PersonId)
        {
            var admissionHistory = GetAdmissionByPersonId(PersonId);
            var admissionViewList = new List<CYCAAdmissionViewModel>();

            var records = new CYCAAdmissionPartialViewModel
            {
                PersonId = PersonId,
                CYCAAdmissionViewModels = admissionHistory,
            };
            return records;
        }
        public List<CYCAAdmissionViewModel> GetAdmissionByPersonId(int PersonId)
        {       
            var ListP = (from a in db.CYCA_Admissions_AdmissionDetails
                         join at in db.apl_Admission_Type on a.Admission_Type_Id equals at.Admission_Type_Id
                         //join gm in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gm.Admission_Id
                         //join gmd in db.apl_Cyca_Gang_Membership_Type on gm.Gang_Membership_Type_Id equals gmd.Gang_Membership_Type_Id
                         join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                         join v in db.apl_Cyca_Venue on a.Venue_Id equals v.Venue_Id
                         //join re in db.CYCA_Admissions_ReAdmissionDetails on a.Admission_Id equals re.Admission_Id
                         join c in db.Clients on a.Client_Id equals c.Client_Id
                         where c.Person_Id == PersonId 
                         select new CYCAAdmissionViewModel
                         {
                             Admission_Id = a.Admission_Id,                           
                             //SelectedGangMemberType = gmd.Description,
                             CaseStartDate = a.Case_Start_Date,
                             CaseEndDate = a.Case_End_Date.ToString(),
                             AdmissionDate = a.Date_Captured.ToString(),
                             selectedAdmissionType = at.Description,
                             FacilityName = f.FacilityName,
                             ReAdmissionCount = a.CYCA_Admissions_ReAdmissionDetails.Count(),
                             Is_Active = a.Is_Active,
                             selectedVenue = v.VenueName,
                             DateCreated = a.Date_Created.ToString(),
                             Person_Id = PersonId,
                             //Gang_Member_Type_Id = gmd.Gang_Membership_Type_Id,
                             Admission_Type_Id = at.Admission_Type_Id,
                             FacilityId = f.Facility_Id,
                             VenueId = v.Venue_Id,
                             CommentsAndObservation = a.Comments_And_Observation,
                             Document_Type_Id = a.Document_Type_Id,
                             GangMembership_Additional_Info = a.OtherGangMemberDescription,
                             Additional_Info = a.OtherDocTypeDescription
                             
                         }).ToList();

            foreach (var item in ListP)
            {
                var admissionFiles = new List<CYCA_Admissions_Document>();
                item.files = children.GetFilesByAdmissionID(item.Admission_Id);
            }
            return ListP;
        }
        public PartialViewResult EditViewAdmission(int id, int PersonId, string display)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var p = db.Persons.Where(pp => pp.Person_Id == PersonId).Single();
            //Biometrics
            var afis = db.int_DSD_Afis.Where(af => af.Person_Id.Equals(p.Person_Id)).SingleOrDefault();

            List<apl_Admission_Type> admission_Types = db.apl_Admission_Type.ToList();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            List<apl_Cyca_Venue> cyca_Venues = db.apl_Cyca_Venue.ToList();
            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            //initialise view model
            var VM = new CYCAAdmissionViewModel();
            VM.Person_Id = PersonId;
            VM.childFullNames = p.First_Name + " " + p.Last_Name;
            if (afis != null)
            {
                VM.HasBiometric = true;
                VM.IsPivaVerified = p.Is_Piva_Validated;
                VM.IsVerified = afis.Is_Verified;
            }
            else
            {
                VM.HasBiometric = false;
                VM.IsPivaVerified = false;
                VM.IsVerified = false;
            }

            var file = this.GetAdmission(PersonId);
            var readmit = file.CYCAAdmissionViewModels.Where(c => c.Admission_Id == id).SingleOrDefault();
            readmit.RequestType = display;
            return PartialView("~/Views/Admit/_AdmitNew.cshtml", readmit);
        }
        public PartialViewResult UpdateAdmission(CYCAAdmissionViewModel vm)
        {

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            //var document = db.CYCA_Admissions_Document.Where(d => d.ReAdmission_Id == vm.Re_Admission_Id);
            var document = db.CYCA_Admissions_AdmissionDetails.Where(d => d.Admission_Id == vm.Admission_Id).SingleOrDefault();

            var record = db.CYCA_Admissions_AdmissionDetails.Where(d => d.Admission_Id == vm.Admission_Id).SingleOrDefault();

            record.Venue_Id = vm.VenueId;
            record.Case_End_Date = Convert.ToDateTime(vm.CaseEndDate);
            record.Comments_And_Observation = vm.CommentsAndObservation;
            record.Admission_Type_Id = vm.Admission_Type_Id;
            record.Document_Type_Id = vm.Document_Type_Id;
            //record.OtherGangMemberDescription = vm.GangMembership_Additional_Info;
            record.OtherDocTypeDescription = vm.Additional_Info;
            record.Modified_By = userId.ToString();
            record.Date_Last_Modified = DateTime.Now;

            //if (record != null)
            //{
            //    CYCA_Admissions_GangMembership edit = db.CYCA_Admissions_GangMembership.Where(j => j.Admission_Id == record.Admission_Id).FirstOrDefault();
            //    if (edit != null)
            //    {
            //        edit.Gang_Membership_Type_Id = vm.Gang_Member_Type_Id;
            //    }

            //    db.Entry(document).CurrentValues.SetValues(record);
            //    db.SaveChanges();
            //}

            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];
                    //var exists = db.CYCA_Admissions_Document.Where(d => d.Document_Id == filesp[i].dischargeId)
                    CYCA_Admissions_Document doc = new CYCA_Admissions_Document()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,                      
                        Document_Type_Id = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        Document_Ext = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = (from a in db.CYCA_Admissions_IllegalItemsFound
                                      join u in db.Users on a.Captured_By equals u.User_Id
                                      select u.First_Name + " " + u.Last_Name).FirstOrDefault(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_Admissions_Document.Add(doc);
                    db.SaveChanges();

                }

            }
            return Admission(vm.Person_Id);
        }





    }
}