using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common_Objects.Models
{
    public class UserMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The First Name field is required")]
        [StringLength(150, ErrorMessage = "The First Name field cannot be more than 150 characters in length")]
        [Display(Name = "First Name", Description = "The First Name for this specific User")]
        [DataType(DataType.Text)]
        public string First_Name;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Last Name field is required")]
        [StringLength(150, ErrorMessage = "The Last Name field cannot be more than 150 characters in length")]
        [Display(Name = "Last name", Description = "The Last Name for this specific User")]
        [DataType(DataType.Text)]
        public string Last_Name;

        [StringLength(5, ErrorMessage = "The Initials field cannot be more than 5 characters in length")]
        [Display(Name = "Initials", Description = "The Initials for this specific User")]
        [DataType(DataType.Text)]
        public string Initials;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Email Address field is required")]
        [StringLength(150, ErrorMessage = "The Email Address field cannot be more than 150 characters in length")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address", Description = "The Email Address for this specific User")]
        [DataType(DataType.EmailAddress)]
        public string Email_Address;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified menu item is deleted")]
        public bool Is_Deleted;

        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_Created;
    }

    public class EmployeeMetadata
    {
        [StringLength(10, ErrorMessage = "The Persal Number field cannot be more than 10 characters in length")]
        [Display(Name = "Persal Number", Description = "The Persal Number for this specific Employee")]
        [DataType(DataType.Text)]
        public string Persal_Number;

        [StringLength(15, ErrorMessage = "The Phone Number field cannot be more than 15 characters in length")]
        [Display(Name = "Phone Number", Description = "The Phone Number for this specific Employee")]
        [DataType(DataType.Text)]
        public string Phone_Number;

        [StringLength(15, ErrorMessage = "The Mobile Phone Number field cannot be more than 15 characters in length")]
        [Display(Name = "Mobile Phone Number", Description = "The Mobile Phone Number for this specific Employee")]
        [DataType(DataType.Text)]
        public string Mobile_Phone_Number;

        [StringLength(15, ErrorMessage = "The ID Number field cannot be more than 15 characters in length")]
        [Display(Name = "ID Number", Description = "The ID Number for this specific Employee")]
        [DataType(DataType.Text)]
        public string ID_Number;

        [Display(Name = "Shift Worker?", Description = "Indicates if the specified Employee is a shift worker")]
        public bool Is_Shift_Worker;

        [Display(Name = "Casual Worker?", Description = "Indicates if the specified Employee is a casual worker")]
        public bool Is_Casual_Worker;

        [Display(Name = "Date Created", Description = "A timestamp of when the record was created on the system")]
        public bool Date_Created;

        [Display(Name = "Created By", Description = "Indicates the username that created the record")]
        public bool Created_By;

        [Display(Name = "Last Modified", Description = "A timestamp of when the record was last modified")]
        public bool Date_Last_Modified;

        [Display(Name = "Modified By", Description = "Indicates the username that last modified the record")]
        public bool Modified_By;

        [Display(Name = "Is Active?", Description = "Indicates if the specified Employee is currently active on the system")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified Employee is deleted from the system")]
        public bool Is_Deleted;
    }

    public class RoleMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Description field is required")]
        [StringLength(150, ErrorMessage = "The Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The Description for this specific Role")]
        [DataType(DataType.Text)]
        public string Description;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified menu item is deleted")]
        public bool Is_Deleted;

        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_Created;
    }

    public class RoleDelegationMetadata
    {
        [Required(ErrorMessage = "The Delegated From field is required")]
        public int From_User_Id;

        [Required(ErrorMessage = "The Delegated To field is required")]
        public int To_User_Id;

        [Required(ErrorMessage = "The Description field is required")]
        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_From;

        [Required(ErrorMessage = "The Description field is required")]
        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_To;
    }

    public class GroupMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Group Description field is required")]
        [StringLength(150, ErrorMessage = "The Group Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The Name of the Group")]
        [DataType(DataType.Text)]
        public string Description;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;
    }

    public class MenuMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Description field is required")]
        [StringLength(150, ErrorMessage = "The Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The description for this specific menu")]
        [DataType(DataType.Text)]
        public string Description;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified menu item is deleted")]
        public bool Is_Deleted;

        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_Created;
    }

    public class MenuItemMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Menu Text field is required")]
        [StringLength(150, ErrorMessage = "The Menu Text field cannot be more than 150 characters in length")]
        [Display(Name = "Menu Text", Description = "The display text for the menu item")]
        [DataType(DataType.Text)]
        public string Menu_Text;

        [StringLength(2000, ErrorMessage = "The Menu Tooltip field cannot be more than 2000 characters in length")]
        [Display(Name = "Menu Tooltip", Description = "The tooltip text for the menu item")]
        [DataType(DataType.Text)]
        public string Menu_Tooltip;

        [Required(ErrorMessage = "Please select a valid Menu structure where this menu item will display")]
        public int Menu_Id;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified menu item is deleted")]
        public bool Is_Deleted;

        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_Created;
    }

    public class ModuleMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Module Description field is required")]
        [StringLength(150, ErrorMessage = "The Module Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The description text for the Module")]
        [DataType(DataType.Text)]
        public string Description;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Base URL field is required")]
        [StringLength(250, ErrorMessage = "The Base URL field cannot be more than 250 characters in length")]
        [Display(Name = "Base URL", Description = "The base url for the Module")]
        [DataType(DataType.Text)]
        public string Base_URL;

        [Display(Name = "Is Active?", Description = "Indicates if the specified menu item is visible")]
        public bool Is_Active;

        [Display(Name = "Is Deleted?", Description = "Indicates if the specified menu item is deleted")]
        public bool Is_Deleted;

        [Display(Name = "Date Created", Description = "The date the record was created")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date_Created;
    }

    public class ModuleControllerMetadata
    {
        [Required(ErrorMessage = "Please select a valid Module this item should be a child of")]
        public int Module_Id;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Module Controller Name field is required")]
        [StringLength(150, ErrorMessage = "The Module Description field cannot be more than 150 characters in length")]
        [Display(Name = "Controller Name", Description = "The Name of the Controller")]
        [DataType(DataType.Text)]
        public string Module_Controller_Name;
    }

    public class ModuleActionMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Module Action Name field is required")]
        [StringLength(150, ErrorMessage = "The Module Action Name field cannot be more than 150 characters in length")]
        [Display(Name = "Action Name", Description = "The Name of the Controller")]
        [DataType(DataType.Text)]
        public string Module_Action_Name;
    }

    public class OrganizationMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Description field is required")]
        [StringLength(150, ErrorMessage = "The Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The Description of the Organization")]
        [DataType(DataType.Text)]
        public string Description;

        [Display(Name = "Telephone Number")]
        public string Telephone_Number;

        [Display(Name = "Fax Number")]
        public string Fax_Number;

        [Display(Name = "Email Address")]
        public string Email_Address;
    }

    public class ReceptionRegisterMetadata
    {
        [Display(Name = "Reason for Visit")]
        public string Reason_For_Visit;

        [Display(Name = "Visit Date")]
        public string Visit_Date;
    }

    public class PersonMetadata
    {
        [Display(Name = "First Name")]
        public string First_Name;

        [Display(Name = "Last Name")]
        public string Last_Name;

        [Display(Name = "Known As")]
        public string Known_As;

        [Display(Name = "ID Number")]
        public string Identification_Number;

        [Display(Name = "Date of Birth")]
        public string Date_Of_Birth;

        [Display(Name = "Estimated")]
        public bool Is_Estimated_Age;

        [Display(Name = "Phone Number")]
        public string Phone_Number;

        [Display(Name = "Mobile Phone Number")]
        public string Mobile_Phone_Number;

        [Display(Name = "Email Address")]
        public string Email_Address;
    }

    public class AddressMetadata
    {
        [Display(Name = "Address Line 1")]
        public string Address_Line_1;

        [Display(Name = "Address Line 2")]
        public string Address_Line_2;

        [Display(Name = "Postal Code")]
        public string Postal_Code;
    }

    public class TownMetadata
    {
        [Display(Name = "Description")]
        public string Description;
    }

    public class PersonEducationMetadata
    {
        [Display(Name = "Year Completed")]
        public string Year_Completed;

        [Display(Name = "Date Last Attended")]
        public string Date_Last_Attended;

        [Display(Name = "Additional Information")]
        public string Additional_Information;
    }

    public class PersonEmploymentMetadata
    {
        [Display(Name = "NameOfEmployer")]
        public string NameOfEmployer;
        [Display(Name = "Occupation")]
        public string Occupation;
    }

    public class SchoolMetadata
    {
        [Display(Name = "Contact Person")]
        public string Contact_Person;

        [Display(Name = "Telephone Number")]
        public string Telephone_Number;

        [Display(Name = "Cellphone Number")]
        public string Cellphone_Number;

        [Display(Name = "Fax Number")]
        public string Fax_Number;

        [Display(Name = "Email Address")]
        public string Email_Address;
    }

    public class IntakeAssessmentMetadata
    {
        [Display(Name = "Date Reported")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string Assessment_Date;

        [Display(Name = "Presenting Problem")]
        public string Presenting_Problem;

        [Display(Name = "Preliminary Assessment")]
        public string Preliminary_Assessment;

        [Display(Name = "Priority Intervention?")]
        public bool Is_Priority_Intervention;

        [Display(Name = "Referred for Assessment?")]
        public bool Is_Referred_For_Assessment;

        [Display(Name = "Referred to Other Service Provider?")]
        public bool Is_Referred_To_Other_Service_Provider;

        [Display(Name = "Supervisor Comments")]
        public string Supervisor_Comments;

        [Display(Name = "Referral Focus Areas")]
        public List<Referral_Focus_Area> Referral_Focus_Areas;

        [Display(Name = "Brief Case Background")]
        public string Case_Background;

        [Display(Name = "Social Worker Comments")]
        public string Social_Worker_Comments;

        [Display(Name = "Case Allocation Comments")]
        public string Case_Allocation_Comments;

        [Display(Name = "Date Allocated")]
        public string Date_Allocated;

        [Display(Name = "Date Due")]
        public string Date_Due;

    }

    #region CPR

    public class UnsuitabilityMetadata
    {
        [Display(Name = "City / Town")]
        public int SelectedTown_Id;
    }

    public class IncidentMetadata
    {
        [Display(Name = "Incident Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Incident_Date;

        [Display(Name = "Notification Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Notification_Date;

        [Display(Name = "Abuse Confirmed?")]
        public bool Is_Abuse_Confirmed;

        [Display(Name = "Abuse Confirmed Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Abuse_Confirmed_Date;

        [Display(Name = "Abuse Circumstances")]
        public string Abuse_Circumstances;

        [Display(Name = "Other")]
        public string Case_Closure_Reason_Other;

        [Display(Name = "Motivation")]
        public string Case_Closure_Motivation;
    }

    public class MedicalDetailMetadata
    {
        [Display(Name = "Form 9 Completed?")]
        public bool Is_Form9_Completed;

        [Display(Name = "J 88 Completed?")]
        public bool Is_J88_Completed;

        [Display(Name = "Practitioner Name")]
        public string Practitioner_Name;

        [Display(Name = "Practitioner Contact Number")]
        public string Practitioner_Contact_Number;

        [Display(Name = "Treatment Given By")]
        public string Treatment_Given_By;

        [Display(Name = "Treatment Place")]
        public string Treatment_Place;

        [Display(Name = "Date Treatment Given")]
        public DateTime? Treatment_Date;

        [Display(Name = "Treatment Details")]
        public string Treatment_Details;

        [Display(Name = "Was There Medical Follow-up?")]
        public bool Is_Medical_Followup;
    }

    public class AllegedOffenderMetadata
    {
        [Display(Name = "Known Offender?")]
        public bool Is_Known_Offender;

        [Display(Name = "Drivers Licence")]
        public string Drivers_License;
    }

    public class CPRSection153Metadata
    {
        [Display(Name = "Notice Granted?")]
        public bool Is_Notice_Granted;

        [Display(Name = "Date Notice Granted")]
        public DateTime? Date_Notice_Granted;

        [Display(Name = "Notice Issued?")]
        public bool Is_Notice_Issued;

        [Display(Name = "Date Notice Issued")]
        public DateTime? Date_Notice_Issued;
    }

    public class CPRConvictionMetadata
    {
        [Display(Name = "J14 Reference")]
        public string J14_Reference_Number;

        [Display(Name = "SAPS Case Number")]
        public string SAPS_Case_Number;

        [Display(Name = "Conviction")]
        public string Conviction;

        [Display(Name = "Nature of Charge")]
        public string Nature_Of_Charge;

        [Display(Name = "Sentence")]
        public string Sentence;

        [Display(Name = "Date Sentenced")]
        public DateTime? Date_Sentenced;

        [Display(Name = "Account of Charge")]
        public string Account_Of_Charge_And_Conviction;
    }

    public class CPRInformantMetadata
    { }

    public class CPRFirstReporterMetadata
    { }

    public class CPRSAPSDetailMetadata
    {
        [Display(Name = "Reported to SAPS?")]
        public bool Is_Reported_To_SAPS;

        [Display(Name = "Date Reported")]
        public DateTime? Date_Case_Reported;

        [Display(Name = "Case Number")]
        public string Case_Number;

        [Display(Name = "Police Intervention?")]
        public bool Is_Police_Intervention;

        [Display(Name = "Comments")]
        public string Comments;
    }

    public class CPRChildrensCourtDetailMetadata
    {
        [Display(Name = "Form 36 Issued?")]
        public bool Is_Form4_Issued;

        [Display(Name = "Date Form 36 Issued")]
        public DateTime? Date_Form4_Issued;

        [Display(Name = "Court Order Date")]
        public DateTime? Date_Court_Order_Issued;
    }

    public class CPRIncidentMonitoringMetadata
    {
        [Display(Name = "Is Active?")]
        public bool Is_Normal_Monitoring_Active;

        [Display(Name = "Is Active?")]
        public bool Is_Form36_Monitoring_Active;

        [Display(Name = "Is Active?")]
        public bool Is_Childrens_Court_Monitoring_Active;

        [Display(Name = "Is Active?")]
        public bool Is_Criminal_Court_Monitoring_Active;

        [Display(Name = "Switch Off?")]
        public bool Is_Normal_Monitoring_Switched_Off;

        [Display(Name = "Switch Off?")]
        public bool Is_Form36_Monitoring_Switched_Off;

        [Display(Name = "Switch Off?")]
        public bool Is_Childrens_Court_Monitoring_Switched_Off;

        [Display(Name = "Switch Off?")]
        public bool Is_Criminal_Court_Monitoring_Switched_Off;

        [Display(Name = "21 Days")]
        public DateTime? Normal_21Days_Date;

        [Display(Name = "30 Days")]
        public DateTime? Normal_30Days_Date;

        [Display(Name = "60 Days")]
        public DateTime? Normal_60Days_Date;

        [Display(Name = "48 Hours")]
        public DateTime? Form36_48Hours_Date;

        [Display(Name = "Weekly")]
        public DateTime? Form36_Weekly_Date;

        [Display(Name = "Monthly")]
        public DateTime? Form36_Monthly_Date;

        [Display(Name = "3 Months")]
        public DateTime? Childrens_Court_3Months_Date;

        [Display(Name = "6 Months")]
        public DateTime? Childrens_Court_6Months_Date;

        [Display(Name = "1 Year")]
        public DateTime? Criminal_Court_1Year_Date;

        [Display(Name = "2 Years")]
        public DateTime? Criminal_Court_2Year_Date;
    }

    public class CaseNoteMetadata
    {
        [Display(Name = "Date")]
        public DateTime? Date_Note_Taken;

        [Display(Name = "Case Note")]
        public string Case_Note_Text;
    }

    public class ActionTakenMetadata
    {
        [Display(Name = "Date")]
        public DateTime? Date_Action_Taken_Noted;

        [Display(Name = "Action Taken")]
        public string Action_Taken_Description;

        [Display(Name = "Way Forward")]
        public string Way_Forward_Description;
    }

    #endregion

    #region NISIS

    public class NisisSiteMetadata
    {
        [Display(Name = "Site Name")]
        public string Site_Name;

        [Display(Name = "Alternative Name")]
        public string Site_Alternative_Name;

        [Display(Name = "GPS Coordinates (Lat)")]
        public string GPS_Coordinates_Lat;

        [Display(Name = "GPS Coordinates (Long)")]
        public string GPS_Coordinates_Long;

        [Display(Name = "Responsible Organization")]
        public string Responsible_Organization;

        [Display(Name = "Prioritization Group")]
        public string Prioritization_Group;

        [Display(Name = "Target Start Date")]
        public string Target_Start_Date;

        [Display(Name = "Target End Date")]
        public string Target_End_Date;

        [Display(Name = "Primary Contact")]
        public string Primary_Contact;

        [Display(Name = "Estimated Population")]
        public string Estimated_Population;

        [Display(Name = "Source of Population Estimate")]
        public string Source_of_Population_Estimate;

        [Display(Name = "Budget Committed")]
        public string Budget_Committed;
    }

    public class NisisSiteEAMetadata
    {
        [Display(Name = "EA Code")]
        public string EA_Code;

        [Display(Name = "Community Name")]
        public string Community_Name;
    }

    public class NisisSiteEASegmentMetadata
    {
        [Display(Name = "Boundary Description")]
        public string Boundary_Description;

        [Display(Name = "Listing Starting Point")]
        public string Listing_Start_Point_Description;

        [Display(Name = "Listing Route")]
        public string Listing_Route;
    }

    public class NisisListingMetadata
    {
        [Display(Name = "Household Head First Name")]
        public string Household_Head_First_Name;

        [Display(Name = "Household Head Last Name")]
        public string Household_Head_Last_Name;

        [Display(Name = "Household Head Middle Name")]
        public string Household_Head_Middle_Name;

        [Display(Name = "Street Address")]
        public string Street_Address;

        [Display(Name = "House Other Number")]
        public string House_Other_Number;

        [Display(Name = "Private Dwelling Number")]
        public string Dwelling_Unit_Number;

        [Display(Name = "Structure Description")]
        public string Structure_Description;

        [Display(Name = "Queries or Comments")]
        public string Queries_or_Comments;
    }

    public class NisisProfilingInstanceMetadata
    {
        [Display(Name = "Date Profiled")]
        public string Profiling_Date;

        [Display(Name = "Dwelling Unit Number")]
        public string Dwelling_Unit_Number;

        [Display(Name = "Household Number")]
        public string Household_Number;

        [Display(Name = "Male:")]
        public string Household_Number_Of_Males;

        [Display(Name = "Female:")]
        public string Household_Number_Of_Females;

        [Display(Name = "Dwelling Unit Address")]
        public string Dwelling_Unit_Address;

        [Display(Name = "Dwelling Unit Description")]
        public string Dwelling_Unit_Description;
    }

    public class NisisCommunityProfilingInstanceMetadata
    {
        [Display(Name = "Date Profiled")]
        public string Profiling_Date;
    }

    public class NisisProfilingInstanceReferralMetadata
    {
        [Display(Name = "Supporting Comments")]
        public string Supporting_Comments;

        [Display(Name = "SLA Delivery Date")]
        public DateTime? SLA_Delivery_Date;

        [Display(Name = "Delivery Date")]
        public DateTime? Delivery_Date;

        [Display(Name = "Expected Delivery Date")]
        public DateTime? Expected_Delivery_Date;

        [Display(Name = "Referral Reason")]
        public string Referral_Reason;
    }

    public class ProfilingToolMetadata
    {
        [Display(Name = "Introduction Date")]
        public DateTime? Introduction_Date;

        [Display(Name = "Is Deprecated?")]
        public bool IsDeprecated;
    }

    public class QuestionnaireSectionMetadata
    {
        [Display(Name = "Section Name")]
        public string Section_Name;

        [Display(Name = "Section Header")]
        public string Section_Header;

        [Display(Name = "Is Per Person?")]
        public bool Is_Per_Person;
    }

    public class QuestionnaireQuestionMetadata
    {
        [Display(Name = "Question Prompt")]
        public string Question_Text;

        [Display(Name = "Is Required?")]
        public bool Is_Required;
    }

    public class QuestionnaireQuestionOptionMetadata
    {
        [Display(Name = "Option Text")]
        public string Option_Text;
    }

    public class QuestionnaireQuestionColumnMetadata
    {
        [Display(Name = "Column Heading Text")]
        public string Column_Heading;

        [Display(Name = "Display Input per Option?")]
        public bool Is_Per_Option;
    }

    #endregion

    #region

    public class VEPServicesTypeMetadata
    {
        public int ServiceTypeId;

        [Display(Name = "Service Offered")]
        public int CaseId;

        [Display(Name = "Service Notes")]
        public string ServiceNotes;

    }

    public class ServiceTypeMetadata
    {
        [Display(Name = "Service Notes")]
        public string Service_Notes;

    }

    public class VEPReferalMetadata
    {
        //public int Referalsid;

        public int CaseId;

        public int FromDepartment;

        public int ToDepartment;

        [Display(Name = "Referal Notes")]
        public string Notes;

    }

    public class ReferalTypeMetadata
    {
        public int Referalsid;

        public int CaseId;

        public int FromDepartment;

        public int ToDepartment;

        [Display(Name = "Referal Notes")]
        public string Notes;

    }

    public class VEPSettlementMetadata
    {
        [Display(Name = "Settlement Type")]
        public string Settlement;


    }

    public class VEPSexualOrientationMetaData
    {
        [Display(Name = "Sexual Orientation")]
        public string SexualOrientation;
    }

    #endregion
}