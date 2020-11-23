using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using CYCA_Module_V2.Common_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CYCA_Module_V2.Controllers
{
    public class ClientController : Controller
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        ChildrenModel children = new ChildrenModel();
        private readonly UserModel userModel = new UserModel();
        // GET: Client
        public ActionResult Index(int id)
        {

            //CYCA_GangAndTattoosViewModel gangAndTattoosViewModel = new CYCA_GangAndTattoosViewModel();
            //gangAndTattoosViewModel.cYCA_DynamicDataBaseModel = GetCYCA_DynamicDataBaseModel();
            //gangAndTattoosViewModel.IntakeDataViewModels = GetIntakeDataViewModels();
            var currentUser = new User();
            string facilityname = "";

            if (Session["CurrentUser"] != null)
            {
                var loggedInUser = (User)Session["CurrentUser"];

                var userModel = new UserModel();
                currentUser = userModel.GetSpecificUser(loggedInUser.User_Id);

                facilityname = userModel.GetFacilityByUserId(loggedInUser.User_Id);
            }
            ViewBag.FacilityName = facilityname;


            var intakeDataViewModel = new IntakeDataViewModel();



            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(id);
            Session["Pers_Id"] = personToEdit.Person_Id;
            //Get Active Admission
            var admisison = (from a in db.CYCA_Admissions_AdmissionDetails
                             join c in db.Clients on a.Client_Id equals c.Client_Id
                             join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                             where c.Person_Id == personToEdit.Person_Id && a.Is_Active == true
                             select new CYCAAdmissionsViewModel
                             {
                                 AddmissionId = a.Admission_Id,
                                 FacilityName = f.FacilityName
                             }).FirstOrDefault();
            //var readmisison = (from a in db.CYCA_ReAdmissionDetails
            //                 join c in db.Clients on a.Client_Id equals c.Client_Id
            //                 join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
            //                 where c.Person_Id == personToEdit.Person_Id && a.Is_Active == true
            //                 select new CYCAAdmissionsViewModel
            //                 {
            //                     ReAddmissionId = a.ReAdmission_Id,
            //                     FacilityName = f.FacilityName
            //                 }).FirstOrDefault();
            if (admisison!=null)
            {
                if (admisison.FacilityName.ToLower() == facilityname.ToLower())
                {
                    intakeDataViewModel.SameFacility = true;
                    intakeDataViewModel.CanDischarge = true;

                }
                else
                {
                    intakeDataViewModel.SameFacility = false;
                    intakeDataViewModel.CanDischarge = false;

                }
                intakeDataViewModel.AdmittedAt = admisison.FacilityName;
                intakeDataViewModel.CanAdmit = false;
            }
            //else if (readmisison != null)
            //{
            //    intakeDataViewModel.AdmittedAt = readmisison.FacilityName;
            //    intakeDataViewModel.CanDischarge = true;
            //    intakeDataViewModel.CanAdmit = false;
            //}
            else
            {
                intakeDataViewModel.CanAdmit = true;
                intakeDataViewModel.CanDischarge = false;

            }
            var receptionRegister = personToEdit.Reception_Registers.Any() ? personToEdit.Reception_Registers.First() : new Reception_Register();

            var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

            if (physicalAddress.Town != null)
            {
                physicalAddress.Selected_Local_Municipality_Id = physicalAddress.Town.Local_Municipality.Local_Municipality_Id;
                physicalAddress.Selected_Municipality_Id = physicalAddress.Town.Local_Municipality.District_Municipality_Id;
                physicalAddress.Selected_Province_Id = physicalAddress.Town.Local_Municipality.District.Province_Id;
            }

            if (postalAddress.Town != null)
            {
                postalAddress.Selected_Local_Municipality_Id = postalAddress.Town.Local_Municipality.Local_Municipality_Id;
                postalAddress.Selected_Municipality_Id = postalAddress.Town.Local_Municipality.District_Municipality_Id;
                postalAddress.Selected_Province_Id = postalAddress.Town.Local_Municipality.District.Province_Id;
            }

            intakeDataViewModel.Person = personToEdit;
            intakeDataViewModel.ReceptionRegister = receptionRegister;
            intakeDataViewModel.PhysicalAddress = physicalAddress;
            intakeDataViewModel.PostalAddress = postalAddress;
            


            #region disability multiselct Get
            var intakeClientViewModel = new IntakeClientViewModel();

            var disabilityType = new DisabilityModel();
            intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();


            if (intakeDataViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(personToEdit.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();
                intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs = new int[intakeClientViewModel.SelectedDisabilityType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilityType)
                {
                    intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }

            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilityType.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Disability_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilityType = AvailableDisabilityType;

            intakeClientViewModel.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            #region Sub disability type

            var disabilitySubType = new DisabilitySubTypeModel();
            intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();


            if (intakeDataViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(intakeDataViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();
                intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs = new int[intakeClientViewModel.SelectedDisabilitySubType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilitySubType)
                {
                    intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs[index] = item.DisabilityType_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilitySubType = new List<SelectListItem>();


            foreach (var item in disabilitySubType.GetListOfDisabilitiesType())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs.Contains(item.DisabilityType_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilitySubType.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.DisabilityType_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilitySubType = AvailableDisabilitySubType;

            intakeClientViewModel.PostedDisabilitySubType.ListOfDisabilitySubTypeIDs = new SelectList(AvailableDisabilitySubType, "Value", "Text");
            #endregion

            var img = db.Person_Images.Where(ps => ps.Person_Id == id).SingleOrDefault();
            if (img != null)
            {
                intakeDataViewModel.ImgUrl = img.Image_Filename;
            }
            //else
            //{
            //    intakeDataViewModel.ImgUrl = string.Empty;
            //}


            return View(intakeDataViewModel);
        }

        public PartialViewResult ShowClientProfile(string title)
        {
            CYCA_ClientProfileViewModel viewModel = new CYCA_ClientProfileViewModel();
            return PartialView("~/Views/Client/_Profile.cshtml", viewModel);
        }
        public int GetFacilityIdByUserID(int UserId)
        {           
            return (from f in db.apl_Cyca_Facility
                    join e in db.Employees on f.Facility_Id equals e.Facility_Id
                    join u in db.Users on e.User_Id equals u.User_Id
                    where u.User_Id == UserId
                    select f.Facility_Id).SingleOrDefault();

        }
        public PartialViewResult ShowTabs(int id, CYCATabType type,int formType)
        {
            object viewModel = null;
            var dynamicModel = new CYCADynamicFormModel();
            if (formType > 0)
            {
                using (SDIIS_DatabaseEntities _context = new SDIIS_DatabaseEntities())
                {
                    //int clientid = (from c in _context.Clients
                    //                join p in _context.Persons on c.Person_Id equals p.Person_Id
                    //                where c.Person_Id == id
                    //                select c.Client_Id).SingleOrDefault();
                    int clientid = dynamicModel.GetClientIdByPersonId(id);

                    var data1 = ((from f in _context.CYCA_Dynamic_Form
                                 join fd in _context.CYCA_Dynamic_Form_Data on f.Dynamic_Form_Id equals fd.Dynamic_Form_Id
                                 join u in _context.Users on fd.User_Id equals u.User_Id
                                 join c in _context.Clients on fd.Client_Id equals c.Client_Id
                                 join p in _context.Persons on c.Person_Id equals p.Person_Id
                                 //join p in _context.Persons on fd.Client_Id equals p.Person_Id
                                 join fc in _context.apl_Cyca_Facility on fd.Venue_Id equals fc.Facility_Id
                                 //where p.Person_Id == id && f.Dynamic_Form_Type_Id == formType
                                 where fd.Client_Id == clientid && f.Dynamic_Form_Type_Id == formType

                                 select new 
                                {
                                    Id = fd.Dynamic_Form_Data_Id,
                                    CreatedBy = u.First_Name + " " + u.Last_Name,
                                    CreatedById = u.User_Id,
                                    CreatedFor = p.First_Name + " " + p.Last_Name,
                                    DateCreated = fd.CreatedDate.ToString(),
                                    CreatedForId = p.Person_Id,
                                    Venue = fc.FacilityName,
                                    File = fd.Data,
                                    FormTypeId = f.Dynamic_Form_Type_Id
                                 })).ToList();
                    //Gangs, then add tattoos
                    if(formType==5)
                    {
                        var data2 = ((from f in _context.CYCA_Dynamic_Form
                                      join fd in _context.CYCA_Dynamic_Form_Data on f.Dynamic_Form_Id equals fd.Dynamic_Form_Id
                                      join u in _context.Users on fd.User_Id equals u.User_Id
                                      join c in _context.Clients on fd.Client_Id equals c.Client_Id
                                      join p in _context.Persons on c.Person_Id equals p.Person_Id
                                      //join p in _context.Persons on fd.Client_Id equals p.Person_Id
                                      join fc in _context.apl_Cyca_Facility on fd.Venue_Id equals fc.Facility_Id
                                      where fd.Client_Id == clientid && f.Dynamic_Form_Type_Id == 6

                                      select new
                                      {
                                          Id = fd.Dynamic_Form_Data_Id,
                                          CreatedBy = u.First_Name + " " + u.Last_Name,
                                          CreatedById = u.User_Id,
                                          CreatedFor = p.First_Name + " " + p.Last_Name,
                                          DateCreated = fd.CreatedDate.ToString(),
                                          CreatedForId = p.Person_Id,
                                          Venue = fc.FacilityName,
                                          File = fd.Data,
                                          FormTypeId = f.Dynamic_Form_Type_Id
                                      })).ToList();
                        data1.AddRange(data2);
                    }
                    var data = new List<CYCA_DynamicDataModel>();
                    foreach (var i in data1)
                    {
                        var files = ConvertStringToModel(i.File);
                   
                        data.Add(new CYCA_DynamicDataModel()
                        {
                            Id = i.Id,
                            CreatedBy = i.CreatedBy,
                            CreatedById = i.CreatedById,
                            CreatedFor = i.CreatedFor,
                            DateCreated = i.DateCreated,
                            CreatedForId = i.CreatedForId,
                            Venue = i.Venue,
                            Data = files,
                            FormTypeId = i.FormTypeId
                        });
                    }

                   // var data = dynamicModel.GetFileData(id, formType);
     
                    CYCA_DynamicDataBaseModel finalModel = new CYCA_DynamicDataBaseModel();
                    finalModel.dynamicDataModels = new List<CYCA_DynamicDataModel>();
                    finalModel.dynamicDataModels.AddRange(data);
                    finalModel.ChildId = id;
                    viewModel = finalModel;
                };
            }

            switch (type)
            {
                case CYCATabType.Biometric:
                    CYCA_ClientProfileModel model = new CYCA_ClientProfileModel();
                    viewModel = model.GetBiometricViewModel(id);
                    return PartialView("~/Views/Client/_Biometric.cshtml", viewModel);
                case CYCATabType.CarePlan:
                    return PartialView("~/Views/Client/_CarePlan.cshtml", viewModel);
                case CYCATabType.Inventory:
                    var intake = new IntakeDataViewModel();
                    //intake.Person_Id = id;
                    Session["Pers_Id"] = id;
                    viewModel = intake;
                    List<apl_Cyca_Inventory_Type> inventory_type_list = db.apl_Cyca_Inventory_Type.ToList();
                    ViewBag.InventoryTypeList = new SelectList(inventory_type_list, "Inventory_Type_Id", "Description");

                    List<apl_Cyca_Inventory_Return_Status> inventory_return_status_list = db.apl_Cyca_Inventory_Return_Status.ToList();
                    ViewBag.ReturnStatusList = new SelectList(inventory_return_status_list, "Return_Status_Id", "Description");

                    var userList = db.Roles
                             .Where(r => r.Description.Contains("CYCA"))
                             .SelectMany(x => x.Users)
                             .Distinct()
                             .ToList();
                    ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");
                    List<apl_Cyca_Venue> venue_list = db.apl_Cyca_Venue.ToList();
                    ViewBag.VenueList = new SelectList(venue_list, "Venue_Id", "VenueName");
                    return PartialView("~/Views/Client/_PersonalInventory.cshtml", viewModel);
                case CYCATabType.Admissions:
                    AdmitController ac = new AdmitController();
                    return ac.Admission(id);
                case CYCATabType.BodySearch:
                    //AdmitController bs = new AdmitController();
                    //return bs.BodySearchList(id);
                    var bodysearch = GetBodySearchAndIllegalItem(dynamicModel.GetClientIdByPersonId(id));                   
                    return PartialView("~/Views/Client/_BodySearchAndIllegalItemHistory.cshtml", bodysearch);
                case CYCATabType.ReportableIncidents:
                    return PartialView("~/Views/Client/_ReportableIncidents.cshtml", viewModel);
                case CYCATabType.MedicalAssessments:
                    return PartialView("~/Views/Client/_MedicalAssessment.cshtml",viewModel);
                //case CYCATabType.RenderGangAndTattoos:
                //    return PartialView("~/Views/Client/_RenderGangAndTattoos.cshtml", viewModel);
                case CYCATabType.GangAndTattoos:
                    var info = GetGangsAndTatoos(dynamicModel.GetClientIdByPersonId(id));
                    return PartialView("~/Views/Client/_GangAndTattoos.cshtml", info);
                case CYCATabType.RenderTattoos:
                    return PartialView("~/Views/Client/_RenderTattoos.cshtml", viewModel);
                case CYCATabType.DischargeHistory:
                    var history = GetDischarge(dynamicModel.GetClientIdByPersonId(id));
                    return PartialView("~/Views/Client/_DischargeHistory.cshtml", history);
                case CYCATabType.History:
                    AdmitController c = new AdmitController();
                    return c.Admission(id);
                case CYCATabType.ExtraMuralActivity:                   
                    var extramural = new CYCAAdmissionExtraMuralActivityViewModel();
                    //intake.Person_Id = id;
                    Session["Pers_Id"] = id;                 
                    viewModel = extramural;
                    
                    List<apl_Cyca_Physical_Build> physical_build = db.apl_Cyca_Physical_Build.ToList();
                    ViewBag.PhysicalBuildList = new SelectList(physical_build, "Physical_Build_Id", "Description");

                    List<Eye_Color> eye_color = db.Eye_Colors.ToList();
                    ViewBag.EyeColorList = new SelectList(eye_color, "Eye_Color_Id", "Description");

                    List<Hair_Color> hair_color = db.Hair_Colors.ToList();
                    ViewBag.HairColorList = new SelectList(hair_color, "Hair_Color_Id", "Description");

                    List<apl_Cyca_Child_Hobbies> child_hobbies = db.apl_Cyca_Child_Hobbies.ToList();
                    ViewBag.ChildHobbyList = new SelectList(child_hobbies, "Hobby_Id", "Description");

                    List<apl_Cyca_Sport_Activity> sport_activities = db.apl_Cyca_Sport_Activity.ToList();
                    ViewBag.SportActivityList = new SelectList(sport_activities, "Activity_Id", "Description");
                    
                    return PartialView("~/Views/Admit/_ExtraMuralActivity.cshtml", viewModel);
                default:
                    return null;
            }
        }

       
        //public PartialViewResult RenderGangAndTattoos()
        //{

        //    CYCA_ClientProfileViewModel viewModel = new CYCA_ClientProfileViewModel();
        //    IntakeDataViewModel intakeDataViewModel = new IntakeDataViewModel();
        //    //intakeDataViewModel = db.CYCA_Dynamic_Form.Where(x => x.CYCA_Dynamic_Form_Type == CYCA_Dynamic_Form).Include(x => x.cYCA_DynamicDataBaseModel).FirstOrDefault();
        //    CYCA_DynamicDataBaseModel cYCA_DynamicDataBaseModel = new CYCA_DynamicDataBaseModel();
        //    return PartialView("~/Views/Client/_GangAndTattoos.cshtml", viewModel);
        //}

        public IntakeDataViewModel IntakeDataView()
        {
            IntakeDataViewModel intakeDataViewModel = new IntakeDataViewModel();

            return intakeDataViewModel;
        }

      /*  public JsonResult getDischarges(CYCADischargeViewModel vm)
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
            CYCA_Admissions_Discharge dis = new CYCA_Admissions_Discharge()
            {
                KeepBedSpace = vm.KeepBedSpace,
                Comments = vm.Comments,
                AdmissionId = vm.AdmissionId,
                DischargeDate = Convert.ToDateTime(vm.DischargeDate),
                DischargeReasonId = vm.DischargeReasonId ?? 0,
                PersonReceivingDesignationId = vm.UserReceivedDesignationId ?? 0,
                OgranizationId = vm.UserReceivedOrganisationId ?? 0,
                PersonReceivingName = vm.UserReceivedName,
                //ReturnDate = vm.KeepBedSpace?Convert.ToDateTime(vm.ExpectedReturnDate):null,
                UserHandedOverId = vm.UserHandedOverId ?? 0,
                UserId = userId
            };
            if (dis.KeepBedSpace)
            {
                dis.ReturnDate = Convert.ToDateTime(vm.ExpectedReturnDate);
            }

            var admission = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Admission_Id == vm.AdmissionId).Single();
            admission.Is_Active = false;

            db.CYCA_Admissions_Discharge.Add(dis);


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
        } */

        public CYCA_DynamicDataBaseModel DynamicDataBaseModel()
        {
            List<CYCA_DynamicDataBaseModel> cYCA_Dynamic = new List<CYCA_DynamicDataBaseModel>();

            return DynamicDataBaseModel();
        }
        //public PartialViewResult RenderTattoos()
        //{
        //    CYCA_ClientProfileViewModel viewModel = new CYCA_ClientProfileViewModel();

        //    return PartialView("~/Views/Client/RenderTattoos.cshtml", viewModel);

        //}

        public PartialViewResult VerifyBiometric(FPCaptureRs request)
        {
            string x = request.CodecName;
            return null;
        }
        public List<FileModel> ConvertStringToModel(string info)
        {

            var files = new List<FileModel>();
            if (info != null)
            {
                var checkFiles = JObject.Parse(info)["files"];
                if (checkFiles != null)
                {
                    files = checkFiles.ToObject<FileModel[]>().ToList();
                }
                
            }


            return files;
        }

        public GangAndTatoosViewModel GetGangsAndTatoos(int clientID)
        {
            //var currentAdmissionId = db.CYCA_Admissions_AdmissionDetails.Where(m => m.Client_Id == clientID && m.Is_Active == true).SingleOrDefault();
            //var details = new GangAndTatoosViewModel();
            //details.clientID = clientID;
            //details.gangs = GetGangmemberships(clientID);
            //details.tatoos = GetTatoos(clientID);
            //details.admissionID = currentAdmissionId.Admission_Id;
            //return details;


            var gangHistory = GetGangmemberships(clientID);
            var tattooHistory = GetTatoos(clientID);

          

            var records = new GangAndTatoosViewModel
            {
                clientID = clientID,
                gangs = gangHistory,
                tatoos = tattooHistory
            };

            ViewBag.ClientId = clientID;
            return records;
        }
        public List<GangViewModel> GetGangmemberships(int clientID) {
            var adIds = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Client_Id == clientID).Select(ad =>ad.Admission_Id).ToList();
          //  return db.CYCA_Admissions_GangMembership.Where(g => adIds.Contains(Convert.ToInt32(g.Admission_Id))).ToList();
            var models = db.CYCA_Admissions_GangMembership.Where(g => adIds.Contains(g.Admission_Id ?? default(int))).ToList();
            var viewModels = new List<GangViewModel>();

            foreach (var m in models)
            {
                var files = GetGangFiles(m.Gang_Membership_Id);
                var membershipName = GetMembershipName(Convert.ToInt32(m.Gang_Membership_Type_Id));
               viewModels.Add( new GangViewModel
                {
                    Gang_Membership_Id = m.Gang_Membership_Id,
                    Gang_Membership_Type_Id = m.Gang_Membership_Type_Id,
                    Is_Member = m.Is_Member,
                    Created_By = m.Created_By,
                    Date_Captured = m.Date_Captured,
                    Membership_Rank = m.Membership_Rank,
                    ReAdmission_Id = m.ReAdmission_Id,
                    clientID = clientID,
                    Date_Created = m.Date_Created,
                    Admission_Id = m.Admission_Id,
                    Is_Active = m.Is_Active,
                    Is_Deleted = m.Is_Deleted,
                    Membership_Type = membershipName,
                    Gang_Membership_Additional_Info = m.OtherGangMemberDescription,
                    Document_Type_Id = m.Document_Type_Id ?? default(int),
                    Additional_Info = m.OtherDocTypeDescription,
            
                    documents = files
                }
                );
    }

            return viewModels;
        }
        public string GetMembershipName(int membershipID) {
            return db.apl_Cyca_Gang_Membership_Type.Where(m => m.Gang_Membership_Type_Id == membershipID).Select(m => m.Description).FirstOrDefault();
        }
        public List<CYCA_GangAndTatooDocument> GetGangFiles(int membershipID)
        {
            return db.CYCA_GangAndTatooDocument.Where(d => d.Gang_Membership_Id == membershipID).ToList();
        }
        public List<CYCA_GangAndTatooDocument> GetTatoosFiles(int tatooID)
        {
            return db.CYCA_GangAndTatooDocument.Where(d => d.Tatoo_Id == tatooID).ToList();
        }
        public List<TatooVewModel> GetTatoos(int clientID) {
            var adIds = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Client_Id == clientID).Select(ad => ad.Admission_Id).ToList();
            var models = db.CYCA_Tatoos.Where(g => adIds.Contains(g.Admission_Id ?? default(int))).ToList();
            var viewModels = new List<TatooVewModel>();

            foreach (var m in models)
            {
                var files = GetTatoosFiles(m.Tatoo_Id);
                viewModels.Add(new TatooVewModel
                {
                    Tatoo_Id = m.Tatoo_Id,
                    Tatoo_Position = m.Tatoo_Position,
                    Tatoo_Size = m.Tatoo_Size,
                    Tatoo_Visible = m.Tatoo_Visible,
                    Tatoo_Description = m.Tatoo_Description,
                    Created_By = m.Created_By,
                    clientID = clientID,
                    Date_Captured = m.Date_Captured,
                    Document_Type_Id = m.Document_Type_Id ?? default(int),
                    Additional_Info = m.OtherDocTypeDescription,
                    ReAdmission_Id = m.ReAdmission_Id,
                    Date_Created = m.Date_Created,
                    Admission_Id = m.Admission_Id,
                    Is_Active = m.Is_Active,
                    Is_Deleted = m.Is_Deleted,
                    documents = files

                });
             }

            return viewModels;
        }
        public PartialViewResult EditGang(int id, int clientId, string display)
        {
            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");
            var gang = GetGangmemberships(clientId).Where(g => g.Gang_Membership_Id == id).FirstOrDefault();
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            gang.RequestType = display;
            return PartialView("~/Views/Client/AddGang.cshtml", gang);
        }
        public PartialViewResult EditTatoo(int id, int clientId, string display)
        {

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            var tatoo = GetTatoos(clientId).Where(g => g.Tatoo_Id == id).FirstOrDefault();
            tatoo.RequestType = display;

            return PartialView("~/Views/Client/AddTatoo.cshtml", tatoo);
        }
        public PartialViewResult AddNewGang(string display, int admissionID, int clientId)
        {
            List<apl_Cyca_Gang_Membership_Type> gangMemberships = db.apl_Cyca_Gang_Membership_Type.ToList();
            ViewBag.GangMembershipTypeList = new SelectList(gangMemberships, "Gang_Membership_Type_Id", "Description");
            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            var gang = new GangViewModel();

            gang.RequestType = display;
            gang.Admission_Id = admissionID;
            gang.clientID = clientId;
            return PartialView("~/Views/Client/AddGang.cshtml", gang);
        }
        public PartialViewResult AddNewTatoo(string display, int admissionID, int clientId)
        {

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");
            var tatoo = new TatooVewModel();
            tatoo.RequestType = display;
            tatoo.Admission_Id = admissionID;
            tatoo.clientID = clientId;
            
            return PartialView("~/Views/Client/AddTatoo.cshtml", tatoo);
        }
        public JsonResult SaveTatoo(TatooVewModel vm)
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

            var currentAdmissionId = db.CYCA_Admissions_AdmissionDetails.Where(m => m.Client_Id == vm.clientID && m.Is_Active == true).SingleOrDefault();
            vm.Admission_Id = currentAdmissionId.Admission_Id;

            var tatoo = new CYCA_Tatoos()
            {
                Admission_Id = vm.Admission_Id,
                Tatoo_Description = vm.Tatoo_Description,
                Tatoo_Position = vm.Tatoo_Position,
                Tatoo_Size = vm.Tatoo_Size,
                Tatoo_Visible = vm.Tatoo_Visible,
                Date_Captured = DateTime.Now,
                Date_Created = DateTime.Now,
                Created_By = GetUserName(),
                Document_Type_Id = vm.Document_Type_Id,
                OtherDocTypeDescription = vm.Additional_Info,
                Is_Active = true,
                Is_Deleted = false

            };




            db.CYCA_Tatoos.Add(tatoo);
            db.SaveChanges();
            var id = tatoo.Tatoo_Id;
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_GangAndTatooDocument doc = new CYCA_GangAndTatooDocument()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = tatoo.Admission_Id,
                        Tatoo_Id = id,

                        Document_Name = file.FileName,
                        DocAppExt = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = GetUserName(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_GangAndTatooDocument.Add(doc);
                    db.SaveChanges();
                }

            }
            db.SaveChanges();
            var result = tatoo.Tatoo_Id;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveGang(GangViewModel vm)
        {
            if (vm.clientID == 0)
            {
                var clientid = db.CYCA_Admissions_AdmissionDetails.Where(m => m.Admission_Id == vm.Admission_Id && m.Is_Active == true).SingleOrDefault();
                vm.clientID = clientid.Client_Id;
            }
       
            var currentAdmissionId = db.CYCA_Admissions_AdmissionDetails.Where(m => m.Client_Id == vm.clientID && m.Is_Active == true).SingleOrDefault();
            vm.Admission_Id = currentAdmissionId.Admission_Id;

            var gang = new CYCA_Admissions_GangMembership()
            {
                Admission_Id = vm.Admission_Id,
                Gang_Membership_Type_Id = vm.Gang_Membership_Type_Id,
                Is_Member = vm.Is_Member,
                Membership_Rank = vm.Membership_Rank,
                Date_Captured = DateTime.Now,
                Date_Created = DateTime.Now,
                Created_By = GetUserName(),
                Document_Type_Id = vm.Document_Type_Id,
                OtherDocTypeDescription = vm.Additional_Info,
                OtherGangMemberDescription = vm.Gang_Membership_Additional_Info,
                Is_Active = true,
                Is_Deleted = false

            };



            var gangs = db.CYCA_Admissions_GangMembership.Where(m => m.Admission_Id == vm.Admission_Id && m.Is_Active).ToList();
            foreach (var g in gangs)
            {
                g.Is_Active = false;
            }

            db.CYCA_Admissions_GangMembership.Add(gang);
            db.SaveChanges();
            var id = gang.Gang_Membership_Id;
            string uname = Request["uploadername"];
            HttpFileCollectionBase filesp = Request.Files;

            if (filesp != null && filesp.Count > 0)
            {
                for (int i = 0; i < filesp.Count; i++)
                {
                    HttpPostedFileBase file = filesp[i];

                    CYCA_GangAndTatooDocument doc = new CYCA_GangAndTatooDocument()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = gang.Admission_Id,
                        Gang_Membership_Id = id,

                        Document_Name = file.FileName,
                        DocAppExt = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = GetUserName(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }

                    db.CYCA_GangAndTatooDocument.Add(doc);

                    db.SaveChanges();
                }

            }

            var edit = db.CYCA_Admissions_AdmissionDetails.Find(vm.Admission_Id);
            var rec = db.CYCA_Admissions_AdmissionDetails.Find(vm.Admission_Id);

            int gangmemId = (from g in db.CYCA_Admissions_GangMembership
                             where g.Admission_Id == vm.Admission_Id && g.Is_Active
                             select g.Gang_Membership_Id).SingleOrDefault();

            CYCA_Admissions_GangMembership editgang = db.CYCA_Admissions_GangMembership.Find(gangmemId);
            editgang.Gang_Membership_Type_Id = vm.Gang_Membership_Type_Id;


            if (rec != null)
            {
                db.Entry(edit).CurrentValues.SetValues(rec);
                db.SaveChanges();
            }

            db.SaveChanges();
            var result = gang.Gang_Membership_Id;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult UpdateTatoo(TatooVewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var document = db.CYCA_Tatoos.Where(d => d.Tatoo_Id == vm.Tatoo_Id).SingleOrDefault();

            var record = db.CYCA_Tatoos.Where(d => d.Tatoo_Id == vm.Tatoo_Id).SingleOrDefault();

            record.Tatoo_Description = vm.Tatoo_Description;
            record.Tatoo_Position = vm.Tatoo_Position;
            record.Tatoo_Size = vm.Tatoo_Size;
            record.Document_Type_Id = vm.Document_Type_Id;
            record.OtherDocTypeDescription = vm.Additional_Info;
            record.Tatoo_Visible = vm.Tatoo_Visible;
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


                    CYCA_GangAndTatooDocument doc = new CYCA_GangAndTatooDocument()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,
                        Tatoo_Id = record.Tatoo_Id,
                        AdmissionDocTypeID = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        DocAppExt = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = GetUserName(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_GangAndTatooDocument.Add(doc);
                    db.SaveChanges();

                }

            }
            var info = GetGangsAndTatoos(vm.clientID);
            return PartialView("~/Views/Client/_GangAndTattoos.cshtml", info);

        }
        public bool DeleteGangAndTatooDocumentFilesByIDs(int[] documentIDs)
        {
            var state = true;
            if (documentIDs != null)
            {
                if (db.CYCA_GangAndTatooDocument.Any(d => documentIDs.Contains(d.Document_Id)))
                {
                    var files = db.CYCA_GangAndTatooDocument.Where(d => documentIDs.Contains(d.Document_Id));
                    db.CYCA_GangAndTatooDocument.RemoveRange(files);
                    db.SaveChanges();
                    //check if files are deleted

                    if (db.CYCA_GangAndTatooDocument.Contains(files.FirstOrDefault()))
                    {
                        state = true;
                    }
                }
            }
            return state;
        }
        public PartialViewResult UpdateGang(GangViewModel vm)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var document = db.CYCA_Admissions_GangMembership.Where(d => d.Gang_Membership_Id == vm.Gang_Membership_Id).SingleOrDefault();

            var record = db.CYCA_Admissions_GangMembership.Where(d => d.Gang_Membership_Id == vm.Gang_Membership_Id).SingleOrDefault();

            record.Document_Type_Id = vm.Document_Type_Id;
            record.Gang_Membership_Type_Id = vm.Gang_Membership_Type_Id;
            record.Membership_Rank = vm.Membership_Rank;
            record.Is_Member = vm.Is_Member;
            record.OtherDocTypeDescription = vm.Additional_Info;
            record.OtherGangMemberDescription = vm.Gang_Membership_Additional_Info;

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
                    CYCA_GangAndTatooDocument doc = new CYCA_GangAndTatooDocument()
                    {
                        DocumentType = file.ContentType,
                        Admission_Id = record.Admission_Id,
                        Gang_Membership_Id = record.Gang_Membership_Id,
                        AdmissionDocTypeID = vm.Document_Type_Id,
                        Document_Name = file.FileName,
                        DocAppExt = Path.GetExtension(file.FileName),
                        DateSaved = DateTime.Now,
                        Date_Created = DateTime.Now,
                        Created_By = GetUserName(),
                        Is_Active = true,
                        Is_Deleted = false
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        doc.DataDocument = reader.ReadBytes(file.ContentLength);
                    }
                    db.CYCA_GangAndTatooDocument.Add(doc);
                    db.SaveChanges();

                }

            }
            var info = GetGangsAndTatoos(vm.clientID);
            return PartialView("~/Views/Client/_GangAndTattoos.cshtml", info);
        }
        public string GetUserName()
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
            return (from a in db.Users
                    where a.User_Id == userId
                    select a.First_Name + " " + a.Last_Name).FirstOrDefault();
        }


        #region INVENTORY REGISTRATION
        public ActionResult PersonalInventory()
        {
            List<apl_Cyca_Inventory_Type> inventory_Types = db.apl_Cyca_Inventory_Type.ToList();
            ViewBag.InventoryTypeList = new SelectList(inventory_Types, "Inventory_Type_Id", "Description");

            List<apl_Cyca_Inventory_Return_Status> return_status = db.apl_Cyca_Inventory_Return_Status.ToList();
            ViewBag.ReturnStatusList = new SelectList(return_status, "Return_Status_Id", "Description");
            
            var userList = db.Roles
                             .Where(r => r.Description.Contains("CYCA"))
                             .SelectMany(x => x.Users)
                             .Distinct()
                             .ToList();
            ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");
          
            return View();
        }      
        public ActionResult GetPersonalInventoryList()
        {
            var intakeDataViewModel = new IntakeDataViewModel();
            int Id  = Convert.ToInt32(Session["Pers_Id"]);
            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(Id);
            //Session["Pers_Id"] = personToEdit.Person_Id;
           // int Id = Convert.ToInt32(Session["Pers_Id"]);
            CYCA_InventoryModel model = new CYCA_InventoryModel();
            IntakeDataViewModel VM = new IntakeDataViewModel();

            List<IntakeDataViewModel> lis = new List<IntakeDataViewModel>();
            lis.AddRange(model.GetListOfPersonalInventory(Id).Select(x => new IntakeDataViewModel
            {
                Inventory_Id = x.Inventory_Id,
                selectedInventoryType = x.selectedInventoryType,
                Item_Color = x.Item_Color,
                Item_Type = x.Item_Type,
                Item_Quantity = x.Item_Quantity,
                Item_Description = x.Item_Description,
                Return_Status_Id = x.Return_Status_Id,
                selectedReturnStatus = x.selectedReturnStatus,
                Date_Handed_In = x.Date_Handed_In,
                Date_Returned = x.Date_Returned,
                Reason_Not_Returned = x.Reason_Not_Returned,
                Quantity_Returned = x.Quantity_Returned
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFacilityInventoryList()
        {
            var intakeDataViewModel = new IntakeDataViewModel();
            int Id = Convert.ToInt32(Session["Pers_Id"]);
            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(Id);
            //Session["Pers_Id"] = personToEdit.Person_Id;
            // int Id = Convert.ToInt32(Session["Pers_Id"]);
            CYCA_InventoryModel model = new CYCA_InventoryModel();
            IntakeDataViewModel VM = new IntakeDataViewModel();

            List<IntakeDataViewModel> lis = new List<IntakeDataViewModel>();
            lis.AddRange(model.GetListOfFacilityInventory(Id).Select(x => new IntakeDataViewModel
            {
                Inventory_Id = x.Inventory_Id,
                selectedInventoryType = x.selectedInventoryType,
                Item_Color = x.Item_Color,
                Item_Type = x.Item_Type,
                Item_Quantity = x.Item_Quantity,
                Item_Description = x.Item_Description,
                Return_Status_Id = x.Return_Status_Id,
                selectedReturnStatus = x.selectedReturnStatus,
                Date_Handed_In = x.Date_Handed_In,
                Date_Returned = x.Date_Returned,
                Reason_Not_Returned = x.Reason_Not_Returned
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddNewInventory(IntakeDataViewModel vm)
        {
            int id = Convert.ToInt32(vm.Person_Id);
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var result = false;


            var intakeDataViewModel = new IntakeDataViewModel();
            //var ChildId = Convert.ToInt32(Session["Person_Id"]);
            //var personModel = new PersonModel();
            //var personToEdit = personModel.GetSpecificPerson(ChildId);
            //Session["Pers_Id"] = personToEdit.Person_Id;
            int Id = Convert.ToInt32(Session["Pers_Id"]);

            CYCA_InventoryModel model = new CYCA_InventoryModel();
            IntakeDataViewModel VM = new IntakeDataViewModel();

            //try
            //{
            if (Id > 0)
            {
                model.AddInventory(vm, Id, userId);

                result = true;
            }
            //}
            //catch (Exception ex)
            //{
            //  throw ex;
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }
      
        //Populate inventory form
        public JsonResult GetInventoryByInventoryId(int InventoryId)
        {
            CYCA_InventoryModel model = new CYCA_InventoryModel();
            IntakeDataViewModel vm = new IntakeDataViewModel();

            vm = model.GetInventoryByInventoryId(InventoryId);

            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }    
        public JsonResult EditInventory(IntakeDataViewModel vm)
        {
            int id = Convert.ToInt32(vm.Inventory_Id);

            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;  
            }
            var result = false;
            CYCA_InventoryModel Model = new CYCA_InventoryModel();

            try
            {
                if (vm.Inventory_Id > 0)
                {

                    Model.UpdateInventory(vm, id, userId);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PersonalInventories(int id)
        {
            IntakeDataViewModel returnModel = new IntakeDataViewModel();
            returnModel.IntakeDataViewModels = new List<IntakeDataViewModel>();
            returnModel.PersonId = id;
            var ListP = (from pf in db.CYCA_PersonalAndFacilityInventory
                         join p in db.Persons on pf.Person_Id equals p.Person_Id
                         join it in db.apl_Cyca_Inventory_Type on pf.Inventory_Type_Id equals it.Inventory_Type_Id
                         join r in db.apl_Cyca_Inventory_Return_Status on pf.Return_Status_Id equals r.Return_Status_Id
                         where p.Person_Id == id && pf.Inventory_Type_Id == 1
                         select new IntakeDataViewModel
                         {

                             Inventory_Id = pf.Inventory_Id,
                             selectedInventoryType = it.Description,
                             Item_Color = pf.Item_Color,
                             Item_Quantity = pf.Item_Quantity.ToString(),
                             Item_Description = pf.Item_Description,
                             Return_Status_Id = r.Return_Status_Id,
                             selectedReturnStatus = r.Description,
                             Date_Handed_In = pf.Date_Handed_In,
                             Date_Returned = pf.Date_Returned,
                             Reason_Not_Returned = pf.Reason_Not_Returned
                         });
                
            return PartialView("~/Views/Admit/_PersonalInventory.cshtml", returnModel);
        }         
        public JsonResult ReturnInventory(IntakeDataViewModel vm)
        {
            int id = Convert.ToInt32(vm.Inventory_Id);

            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];           
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;         
            }
            var result = false;
            CYCA_InventoryModel Model = new CYCA_InventoryModel();

            try
            {
                if (vm.Inventory_Id > 0)
                {

                    Model.ReturnInventory(vm, id, userId);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Discharge
        public ClientDischargeHistoryModel GetDischarge(int clientid)
        {
            var dischargeHistory = GetDishcargeHistoryByClienIDint(clientid);


            var dischargeViewList = new List<CYCADischargeViewModel>();

            var records = new ClientDischargeHistoryModel
            {
                CLientID = clientid,
                ClientDischargeHistory = dischargeHistory
            };
            return records;
        }
        public PartialViewResult EditDischarge(int id, int clientId, string display)
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

            var p = db.Persons.Where(pp => pp.Person_Id == clientId).Single();

            List<StaffDisplay> staffViewList = (from s in staffList
                                                select new StaffDisplay()
                                                {
                                                    Id = s.Id,
                                                    ShortName = s.ShortName + " (" + String.Join(" ", s.roles) + ")",
                                                }).ToList();
            ViewBag.StaffViewList = new SelectList(staffViewList, "Id", "ShortName");

            List<apl_Cyca_Discharge_ReceivingPerson_Designation_Type> receivingPersonDesignationList = db.apl_Cyca_Discharge_ReceivingPerson_Designation_Type.ToList();
            ViewBag.ReceivingPersonDesignationList = new SelectList(receivingPersonDesignationList, "DesignationTypeId", "Description");

            List<Organization> organizations = db.Organizations.Where(o => o.Is_Active == true && o.Is_Deleted == false).ToList();
            ViewBag.OrganizationList = new SelectList(organizations, "Organization_Id", "Description");

            List<apl_Cyca_Admission_Discharge_Reason> discharge_Reasons = db.apl_Cyca_Admission_Discharge_Reason.ToList();
            ViewBag.DischargeReasonList = new SelectList(discharge_Reasons, "DischargeReasonId", "Description");

            List<apl_Cyca_Admission_DocumentType> cyca_documents = db.apl_Cyca_Admission_DocumentType.ToList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");

            var file = this.GetDischarge(clientId);
            var discharge = file.ClientDischargeHistory.Where(c => c.DischargeId == id).SingleOrDefault();
            discharge.RequestType = display;
            return PartialView("~/Views/Admit/_Discharge.cshtml", discharge);
        }
        public List<CYCADischargeViewModel> GetDishcargeHistoryByClienIDint(int clientId)
        {


            var data = (from hist in db.CYCA_Admissions_Discharge

                        join add in db.CYCA_Admissions_AdmissionDetails on hist.AdmissionId equals add.Admission_Id
                        join child in db.Clients on add.Client_Id equals child.Client_Id
                        join person in db.Persons on child.Person_Id equals person.Person_Id

                        join userOver in db.Employees on hist.UserHandedOverId equals userOver.User_Id
                        join empOver in db.Users on hist.UserHandedOverId equals empOver.User_Id

                        join rOrg in db.Organizations on hist.OgranizationId equals rOrg.Organization_Id
                        join reason in db.apl_Cyca_Admission_Discharge_Reason on hist.DischargeReasonId equals reason.DischargeReasonId

                        where child.Client_Id == clientId
                        select new CYCADischargeViewModel
                        {
                            AdmissionId = add.Admission_Id,
                            DischargeId = hist.DischargeId,
                            ChildFullName = person.First_Name + " " + person.Last_Name,
                            PersonId = person.Person_Id,
                            UserHandedOverId = hist.UserHandedOverId,
                            selectedUserHandedOver = empOver.User_Name,
                            UserHandedOverDesignationId = userOver.Job_Position_Id,
                            selectedUserHandedOverDesignation = "",
                            UserReceivedDesignationId = hist.PersonReceivingDesignationId,
                            selectedUserReceivedDesignation = "",
                            UserReceivedName = hist.PersonReceivingName,
                            DischargeReasonId = hist.DischargeReasonId,
                            selectedDischargeReason = reason.Description,
                            DischargeDate = hist.DischargeDate.ToString(),
                            KeepBedSpace = hist.KeepBedSpace,
                            ExpectedReturnDate = hist.ReturnDate.ToString(),
                            Comments = hist.Comments,
                            UserReceivedOrganisationId = hist.OgranizationId,
                            OtherOrgComment = hist.OtherOrgComment,
                            selectedUserReceivedOrganisation = rOrg.Description,
                            DocType_Id = hist.DocType_Id
                            
                        }).ToList();

            foreach (var d in data)
            {
                var historyFiles = new List<CYCA_Admissions_Document>();
                d.Files = children.GetFilesByDischargeID(d.DischargeId);          
                if (d.UserReceivedDesignationId != null)
                {
                    d.selectedUserReceivedDesignation = db.Job_Positions.Where(r => r.Job_Position_Id == d.UserReceivedDesignationId).Select(t => t.Definition).FirstOrDefault();
                }
                if (d.UserHandedOverDesignationId != null)
                {
                    d.selectedUserReceivedDesignation = db.Job_Positions.Where(r => r.Job_Position_Id == d.UserHandedOverDesignationId).Select(t => t.Definition).FirstOrDefault();
                }
            }
            return data;
        }


        //Body Search on the Tab
        public CYCAAdmissionBodilySearchPartiallViewModel GetBodySearchAndIllegalItem(int clientid)
        {
            var bodysearchHistory = GetBodySearchHistoryClienIDint(clientid);
            var illegalitemHistory = GetIllegalItemHistoryClienIDint(clientid);

            var illegalitemViewList = new List<CycaAdmissionIllegalItemsViewModel>();
            var bodysearchViewList = new List<CYCAAdmissionBodySearchViewModel>();

            var records = new CYCAAdmissionBodilySearchPartiallViewModel
            {
                CLientID = clientid,
                CYCAAdmissionBodySearchViewModels = bodysearchHistory,
                CycaAdmissionIllegalItemsViewModels = illegalitemHistory
            };
            return records;
        }
        public CYCAAdmissionBodilySearchPartiallViewModel GetBodySearch(int clientid)
        {
            var bodysearchHistory = GetBodySearchHistoryClienIDint(clientid);           
            var bodysearchViewList = new List<CYCAAdmissionBodySearchViewModel>();

            var records = new CYCAAdmissionBodilySearchPartiallViewModel
            {
                CLientID = clientid,
                CYCAAdmissionBodySearchViewModels = bodysearchHistory,              
            };
            return records;
        }
        public List<CYCAAdmissionBodySearchViewModel> GetBodySearchHistoryClienIDint(int clientId)
        {
            var docL = (from d in db.CYCA_BodilySearch_Document
                        select new LiteFiles
                        {
                            Document_Name = d.Document_Name,
                            Document_Id = d.Document_Id,
                            Admission_Id = d.Admission_Id,
                            Bodily_Search_Id = d.Bodily_Search_Id

                        }).ToList();

            var ListP = (from bs in db.CYCA_BodilySearch
                         join a in db.CYCA_Admissions_AdmissionDetails on bs.Admission_Id equals a.Admission_Id
                         join c in db.Clients on a.Client_Id equals c.Client_Id
                         join bsr in db.apl_Cyca_Bodily_Search_Reasons on bs.Search_Reason_Id equals bsr.Search_Reason_Id
                         //join dt in db.apl_Cyca_Admission_DocumentType on bs.Document_Type_Id equals dt.DocType_Id
                         join p in db.Persons on bs.Person_Id equals p.Person_Id
                         join u in db.Users on bs.Conducted_By equals u.User_Id
                         //where p.Person_Id == Id
                         where c.Client_Id == clientId
                         select new CYCAAdmissionBodySearchViewModel
                         {
                             BodySearchDate = bs.Bodily_Search_Date.ToString(),
                             BodySearchId = bs.Bodily_Search_Id,
                             Description = bs.Description,
                             Admission_Id = a.Admission_Id,
                             WitnessedBy = p.First_Name + " " + p.Last_Name,
                             //WitnessedBy = db.Users.Find(bs.Witnessed_By).First_Name,
                             Person_Id = p.Person_Id,
                             ConductedBy = u.First_Name + " " + u.Last_Name,
                             ReasonForSearch = bsr.Description,
                             saveBodySearchId = bs.Bodily_Search_Id,
                             SearchReasonId = bs.Search_Reason_Id,
                             WitnessedById = bs.Witnessed_By,
                             ConductedById = bs.Conducted_By,
                             Document_Type_Id = bs.Document_Type_Id,
                             OtherReasonForSearch = bs.OtherSeacrhReasonDescription,
                             Additional_Info = bs.OtherDocTypeDescription
                             

                         }).ToList();
      
            foreach (var d in ListP)
            {
                //var bodysearchFiles = new List<CYCA_BodilySearch_Document>();
                //d.Files = children.GetFilesByBodySearchID(d.BodySearchId);
                d.liteFiles = docL.Where(x => x.Bodily_Search_Id == d.BodySearchId).ToList();
            }
            return ListP;
        }
        public PartialViewResult EditViewBodySearch(int id, int clientId, string display)
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

            //Get Venue Staff
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

            var file = this.GetBodySearch(clientId);
            var bodysearch = file.CYCAAdmissionBodySearchViewModels.Where(c => c.BodySearchId == id).SingleOrDefault();
            bodysearch.RequestType = display;
            return PartialView("~/Views/Admit/_BodySearchAddEdit.cshtml", bodysearch);
        }
        public PartialViewResult AddBodySearch(int clientId, string display)
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
            var person = (from c in db.Clients
                     join p in db.Persons on c.Person_Id equals p.Person_Id
                     where c.Client_Id == clientId
                     select p.Person_Id).FirstOrDefault();

            //Get Venue Staff
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

            //var file = this.GetBodySearch(clientId);
            //var bodysearch = file.CYCAAdmissionBodySearchViewModels.Where(c => c.BodySearchId == id).SingleOrDefault();
            
            CYCAAdmissionBodySearchViewModel vm = new CYCAAdmissionBodySearchViewModel();
            vm.Person_Id = person;
            vm.RequestType = display;
            vm.CLientID = clientId;

            return PartialView("~/Views/Admit/_BodySearchAddEdit.cshtml", vm);
        }

        //Illegal Items on the Tab
        public CYCAAdmissionBodilySearchPartiallViewModel GetIllegalItem(int clientid)
        {
            var illegalitemHistory = GetIllegalItemHistoryClienIDint(clientid);


            var illegalitemViewList = new List<CycaAdmissionIllegalItemsViewModel>();

            var records = new CYCAAdmissionBodilySearchPartiallViewModel
            {
                CLientID = clientid,
                CycaAdmissionIllegalItemsViewModels = illegalitemHistory
            };
            return records;
        }
        public List<CycaAdmissionIllegalItemsViewModel> GetIllegalItemHistoryClienIDint(int clientId)
        {
            var docL = (from d in db.CYCA_IllegalItems_Document
                        select new LiteFiles
                        {
                            Document_Name = d.Document_Name,
                            Document_Id = d.Document_Id,
                            Admission_Id = d.Admission_Id,
                            Item_Found_Id = d.Item_Found_Id

                        }).ToList();

            var IllegalItemList = (from ii in db.CYCA_Admissions_IllegalItemsFound
                                   join a in db.CYCA_Admissions_AdmissionDetails on ii.Admission_Id equals a.Admission_Id
                                   join c in db.Clients on a.Client_Id equals c.Client_Id
                                   join u in db.Users on ii.Handed_By equals u.User_Id
                                   join p in db.Persons on ii.Person_Id equals p.Person_Id
                                   //join u in db.Users on bs.Conducted_By equals u.User_Id
                                   //where p.Person_Id == Id
                                   where c.Client_Id == clientId
                                   select new CycaAdmissionIllegalItemsViewModel
                                   {
                                       Item_Found_Id = ii.Item_Found_Id,
                                       Description = ii.Description,
                                       Quantity = ii.Quantity,
                                       selectedHandedBy = u.First_Name + " " + u.Last_Name,
                                       IllegalItemDate = ii.Date_Captured.ToString(),
                                       Handed_To = ii.Handed_By,
                                       Handed_By = ii.Captured_By,
                                       DocType_Id = ii.Document_Type_Id,
                                       Additional_Info = ii.OtherDocTypeDescription
                                       
                                       
                                   }).ToList();

            foreach (var d in IllegalItemList)
            {
                //var illegalitemFiles = new List<CYCA_IllegalItems_Document>();
                //d.Files = children.GetFilesByIllegalItemID(d.Item_Found_Id);
                d.liteFiles = docL.Where(x => x.Item_Found_Id == d.Item_Found_Id).ToList();
            }
            return IllegalItemList;
        }
        public PartialViewResult AddIllegalItem(int clientId, string display)
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
            var person = (from c in db.Clients
                          join p in db.Persons on c.Person_Id equals p.Person_Id
                          where c.Client_Id == clientId
                          select p.Person_Id).FirstOrDefault();

            //Get Venue Staff
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

            //var file = this.GetBodySearch(clientId);
            //var bodysearch = file.CYCAAdmissionBodySearchViewModels.Where(c => c.BodySearchId == id).SingleOrDefault();

            CycaAdmissionIllegalItemsViewModel vm = new CycaAdmissionIllegalItemsViewModel();
            vm.Person_Id = person;
            vm.RequestType = display;
            vm.CLientID = clientId;

            return PartialView("~/Views/Admit/_IllegalItemAddEdit.cshtml", vm);
        }
        public PartialViewResult EditViewIllegalItem(int id, int clientId, string display)
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

            //Get Venue Staff
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

            var file = this.GetIllegalItem(clientId);
            var illegalitem = file.CycaAdmissionIllegalItemsViewModels.Where(c => c.Item_Found_Id == id).SingleOrDefault();
            illegalitem.RequestType = display;
            return PartialView("~/Views/Admit/_IllegalItemAddEdit.cshtml", illegalitem);
        }


        #endregion
    }
}