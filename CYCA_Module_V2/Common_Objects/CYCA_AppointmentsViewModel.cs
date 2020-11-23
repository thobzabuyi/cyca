using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common_Objects.Models;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCA_AppointmentsViewModel
    {
        public int ChildId { get; set; }
    }
    public class ScheduleEvent
    {
        public int id { get; set; }
        public string title { get; set; }
        public int appointmentCategoryId { get; set; }
        public int appointmentTypeId { get; set; }
        public int admissionId { get; set; }
        public int clientId { get; set; }
        public string childName { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string duration { get; set; }
        public int durationMinutes { get; set; }
        public string statusId { get; set; }
        public string statusString { get; set; }
        public string color { get; set; }
        public bool allDay { get; set; }
        public string repeatRule { get; set; }
        public string repeat { get; set; }
    }

    public class SchedulerSetup
    {
        public string ResourceView { get; set; }
        public List<AppointmentList> AppointmentTypes { get; set; }
        public List<AppointmentList> AppointmentCategories { get; set; }

        public SchedulerSetup()
        {
            AppointmentTypes = new List<AppointmentList>();
            AppointmentCategories = new List<AppointmentList>();
        }
    }
    public class AppointmentList
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CatId { get; set; }
    }

}