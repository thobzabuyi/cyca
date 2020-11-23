using Common_Objects.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;


namespace Common_Objects.ViewModels
{
   public class ACMSearchViewModel
    {
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public string Search_ID_Number { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }
        public int ActuarialRisk_Id { get; set; }
        public int CaseWorklist_Id { get; set; }
        public int ProcessList_Id { get; set; }
        public int CourtReport_Id { get; set; }
        public int IndividualDevelopmentPlan_Id { get; set; }
        //public DateTime DateOfAssessmentMeeting { get; set; }
        public string Duration_Of_Involvement { get; set; }

        public int Person_Attended_Meeting_Status_Id { get; set; }
        public SelectList MeetingAttendenceStatus_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var meetingattendencestatuslist = (from a in _db.ACM_YesNoOption
                                                   select a).ToList();

                var ListOfMeetingAttendenceStatuses = (from m in meetingattendencestatuslist
                                                       select new SelectListItem()
                                                       {
                                                           Text = m.Description,
                                                           Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                           Selected = m.YesNoOption_Id.Equals(Person_Attended_Meeting_Status_Id)
                                                       }).ToList();

                var selectList = new SelectList(ListOfMeetingAttendenceStatuses, "Value", "Text", Person_Attended_Meeting_Status_Id);
                return selectList;
            }
        }

        [Required]
        public int Person_Job_Position_Id { get; set; }
        public SelectList Position_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfJobPositions = (from a in _db.Job_Positions
                                          select a).ToList();

                var jobPositionList = (from o in listOfJobPositions
                                       select new SelectListItem()
                                       {
                                           Text = o.Description,
                                           Value = o.Job_Position_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = o.Job_Position_Id.Equals(Person_Job_Position_Id)
                                       }).ToList();

                var selectList = new SelectList(jobPositionList, "Value", "Text", Person_Job_Position_Id);
                return selectList;
            }
        }

        [Required]
        public int Person_Organisation_Id { get; set; }
        public SelectList Organisation_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfOrganisations = (from a in _db.Organizations
                                           select a).ToList();


                var organisationList = (from o in listOfOrganisations
                                        select new SelectListItem()
                                        {
                                            Text = o.Description,
                                            Value = o.Organization_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = o.Organization_Id.Equals(Person_Organisation_Id)
                                        }).ToList();

                var selectList = new SelectList(organisationList, "Value", "Text", Person_Organisation_Id);

                return selectList;
            }
        }

        [Required]
        public int SelectedRelationship_Id { get; set; }
        [Display(Name = "Relationship")]
        public SelectList Relationship_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfRelationships = _db.Relationship_Types.Where(x => x.Description.Length > 0).ToList();

                var employees = (from e in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = e.Description,
                                     Value = e.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = e.Relationship_Type_Id.Equals(SelectedRelationship_Id)
                                 }).ToList();

                var selectList = new SelectList(employees, "Value", "Text", SelectedRelationship_Id);

                return selectList;
            }
        }
    }
}
