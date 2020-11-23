using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using CYCA_Module_V2.Common_Objects;

namespace CYCA_Module_V2.Controllers
{
    public class AppointmentsController : Controller
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        private readonly EmployeeModel employee = new EmployeeModel();
        public PartialViewResult GetAppointments(int Id)
        {
            var p = db.Persons.Where(pp => pp.Person_Id == Id).Single();
            CYCA_AppointmentsViewModel VM = new CYCA_AppointmentsViewModel();
            return PartialView("~/Views/Appointments/_Index.cshtml", VM);
        }
        public PartialViewResult GetTeamAppointments()
        {
            CYCA_AppointmentsViewModel VM = new CYCA_AppointmentsViewModel();
            return PartialView("~/Views/Appointments/_TeamAppointments.cshtml", VM);
        }
        public JsonResult GetScheduledEvents(string start, string end)
        {
            int personId = Convert.ToInt32(Session["Pers_Id"]);
            DateTime Start = DateTime.Parse(start);
            DateTime End = DateTime.Parse(end);
            List<ScheduleEvent> EventItems = new List<ScheduleEvent>();
            IEnumerable<CYCA_Appointment> selectedEvents = null;
            // IEnumerable<CYCA_Appointment> repeatEvents = null;

            var person = db.Persons.Where(p => p.Person_Id == personId).Single();
            var admisison = (from a in db.CYCA_Admissions_AdmissionDetails
                             join c in db.Clients on a.Client_Id equals c.Client_Id
                             join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                             where c.Person_Id == person.Person_Id && a.Is_Active == true
                             select new CYCAAdmissionsViewModel
                             {
                                 AddmissionId = a.Admission_Id,
                                 FacilityName = f.FacilityName
                             }).FirstOrDefault();
            if(admisison!=null)
            {
                selectedEvents = db.CYCA_Appointment.Where(s => (s.AppointmentStartDate >= Start & s.AppointmentEndDate <= End & s.AdmissionID == admisison.AddmissionId));
            }

            //repeatEvents = testHarness.ScheduleEvents.Where(s => (s.repeat != null));
            foreach (var scheduleEvent in selectedEvents)
            {
                var category = db.apl_Cyca_Appointment_Category.Where(ac => ac.AppointmentCategoryId == scheduleEvent.AppointmentCatergoryId).Single();
                var type = db.apl_Cyca_Appointment_Type.Where(at => at.AppointmentTypeId == scheduleEvent.AppointmentTypeID).Single();
                ScheduleEvent itm = new ScheduleEvent();
                itm.id = scheduleEvent.ChildAppointmentID;
                itm.admissionId = scheduleEvent.AdmissionID;
                itm.title = category.Description + "-" + type.Description;
                itm.start = scheduleEvent.AppointmentStartDate.ToString("s");
                itm.end = scheduleEvent.AppointmentEndDate.ToString("s");
                itm.duration = scheduleEvent.Duration.ToString();
                //itm.notes = scheduleEvent.notes;
                //itm.statusId = scheduleEvent.statusId;
                //itm.statusString = scheduleEvent.statusString;
                itm.allDay = false;
                itm.appointmentCategoryId = scheduleEvent.AppointmentCatergoryId;
                itm.appointmentTypeId = scheduleEvent.AppointmentTypeID;
                itm.childName = person.First_Name + " " + person.Last_Name;
                //  itm.repeat = scheduleEvent.repeat;
                // itm.color = scheduleEvent.statusString;
                EventItems.Add(itm);
            }
            return Json(EventItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTeamScheduledEvents(string start, string end)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            DateTime Start = DateTime.Parse(start);
            DateTime End = DateTime.Parse(end);
            List<ScheduleEvent> EventItems = new List<ScheduleEvent>();
            IEnumerable<CYCA_Appointment> selectedEvents = null;
            //All children
            var children =
               (from p in db.Persons
                join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                from imleft in pim.DefaultIfEmpty()
                join cc in db.Clients on p.Person_Id equals cc.Person_Id
                join a in db.CYCA_Admissions_AdmissionDetails on new { client = cc.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                join ca in db.CYCA_Child_Allocation on a.Admission_Id equals ca.Admission_Id
                join u in db.Users on ca.User_Id equals u.User_Id
                where u.User_Id == userId
                select new CYCAChildAllocationViewModel
                {
                    GangMembership = ggg.Description,
                    ImgUrl = imleft.Image_Filename ?? "" ?? "/images/unknown.png",
                    Child_Allocation_Id = ca.Child_Allocation_Id,
                    Person_Id = p.Person_Id,
                    Child_First_Name = p.First_Name,
                    Child_Last_First_Name = p.Last_Name,
                    Child_ID_No = p.Identification_Number,
                    LoggedInUserName = u.First_Name + " " + u.Last_Name,
                    Child_Name = p.First_Name + " " + p.Last_Name,
                    Date_Transferred = ca.Date_Allocated
                }).ToList();
            foreach (CYCAChildAllocationViewModel m in children)
            {
                //Get all admissions for all kids
                var admisison = (from a in db.CYCA_Admissions_AdmissionDetails
                                 join c in db.Clients on a.Client_Id equals c.Client_Id
                                 join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                                 where c.Person_Id == m.Person_Id && a.Is_Active == true
                                 select new CYCAAdmissionsViewModel
                                 {
                                     AddmissionId = a.Admission_Id,
                                     FacilityName = f.FacilityName
                                 }).FirstOrDefault();
                if (admisison != null)
                {
                    selectedEvents = db.CYCA_Appointment.Where(s => (s.AppointmentStartDate >= Start & s.AppointmentEndDate <= End & s.AdmissionID == admisison.AddmissionId));
                }

                //repeatEvents = testHarness.ScheduleEvents.Where(s => (s.repeat != null));
                foreach (var scheduleEvent in selectedEvents)
                {
                    var category = db.apl_Cyca_Appointment_Category.Where(ac => ac.AppointmentCategoryId == scheduleEvent.AppointmentCatergoryId).Single();
                    var type = db.apl_Cyca_Appointment_Type.Where(at => at.AppointmentTypeId == scheduleEvent.AppointmentTypeID).Single();
                    ScheduleEvent itm = new ScheduleEvent();
                    itm.id = scheduleEvent.ChildAppointmentID;
                    itm.admissionId = scheduleEvent.AdmissionID;
                    itm.title = m.Child_Name + ":" + category.Description + "-" + type.Description;
                    itm.start = scheduleEvent.AppointmentStartDate.ToString("s");
                    itm.end = scheduleEvent.AppointmentEndDate.ToString("s");
                    //itm.duration = scheduleEvent.duration.ToString();
                    //itm.notes = scheduleEvent.notes;
                    //itm.statusId = scheduleEvent.statusId;
                    //itm.statusString = scheduleEvent.statusString;
                    itm.allDay = false;
                    itm.appointmentCategoryId = scheduleEvent.AppointmentCatergoryId;
                    itm.appointmentTypeId = scheduleEvent.AppointmentTypeID;
                    itm.childName = m.Child_Name;
                    //  itm.repeat = scheduleEvent.repeat;
                    // itm.color = scheduleEvent.statusString;
                    EventItems.Add(itm);
                }
            }
            return Json(EventItems, JsonRequestBehavior.AllowGet);
        }
        public void PushEvent(ScheduleEvent Event)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var persId = Convert.ToInt32(Session["Pers_Id"]); 
            //Get Active Admission
            var admisison = (from a in db.CYCA_Admissions_AdmissionDetails
                             join c in db.Clients on a.Client_Id equals c.Client_Id
                             join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                             where c.Person_Id == persId && a.Is_Active == true
                             select new CYCAAdmissionsViewModel
                             {
                                 AddmissionId = a.Admission_Id,
                                 FacilityName = f.FacilityName
                             }).FirstOrDefault();

            var schEvent = db.CYCA_Appointment.Where(a => a.ChildAppointmentID == Event.id).FirstOrDefault();
            bool LNewRecord = false;
            if (schEvent == null) // was unassigned, now being assigned
            {
                schEvent = new CYCA_Appointment();
                LNewRecord = true;

            }
            schEvent.AdmissionID = admisison.AddmissionId;
            schEvent.AppointmentCatergoryId = Event.appointmentCategoryId;
            schEvent.AppointmentTypeID = Event.appointmentTypeId;
            schEvent.AppointmentStartDate = DateTime.Parse(Event.start);
            schEvent.CreatedBy = userId;
            schEvent.DateCaptured = DateTime.Now;
        
            if (Event.end != null)
            {
                schEvent.AppointmentEndDate = DateTime.Parse(Event.end);
            }
            else
            {
                schEvent.AppointmentEndDate = DateTime.Parse(Event.start).AddMinutes(30);
            }
            TimeSpan span = schEvent.AppointmentEndDate - schEvent.AppointmentStartDate;
            schEvent.Duration = span.TotalMinutes.ToString();
             schEvent.DurationMinutes = (int)span.TotalMinutes;
            if (LNewRecord)
            {
                db.CYCA_Appointment.Add(schEvent);
            }
            db.SaveChanges();
            //schEvent.notes = Event.notes;
            //schEvent.repeat = Event.repeat;
            //schEvent.repeatRule = Event.repeatRule;
            //schEvent.statusId = Event.statusId;
            //schEvent.statusString = Event.statusString;
            //schEvent.title = Event.title;
            //schEvent.equipmentID = Event.equipmentId;
            //schEvent.repeat = Event.repeat;
        }

        public void DeleteAppointment(int Id)
        {
            var app = db.CYCA_Appointment.Where(ap => ap.ChildAppointmentID == Id).Single();
            db.CYCA_Appointment.Remove(app);
            db.SaveChanges();
        }
        public JsonResult GetAppointment(int Id)
        {
            var app = db.CYCA_Appointment.Where(ap => ap.ChildAppointmentID == Id).ToList();
            var appointment = (from c in app
                               where c.ChildAppointmentID == Id
                               select new ScheduleEvent()
                               {
                                   // title = c.ti
                                   duration = c.Duration,
                                   appointmentCategoryId = c.AppointmentCatergoryId,
                                   appointmentTypeId = c.AppointmentTypeID,
                                   id = c.ChildAppointmentID,
                                   start = c.AppointmentStartDate.ToString("dd MMM yyyy hh:mm tt"),
                               }).Single();

            return Json(appointment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSetupInfo()
        {
            SchedulerSetup Data = new SchedulerSetup();
            var categories = (from a in db.apl_Cyca_Appointment_Category
                     select new AppointmentList()
                     {
                         Id = a.AppointmentCategoryId,
                         Description = a.Description
                     }).ToList();
            Data.AppointmentCategories.AddRange(categories);
            var types = (from a in db.apl_Cyca_Appointment_Type
                              select new AppointmentList()
                              {
                                  Id = a.AppointmentTypeId,
                                  Description = a.Description,
                                  CatId = a.AppointmentCategoryId
                              }).ToList();
            Data.AppointmentTypes.AddRange(types);


            //Data.AppointmentTypes.AddRange(db.apl_Cyca_Appointment_Type);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}