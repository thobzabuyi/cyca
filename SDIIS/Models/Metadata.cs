using System;
using System.ComponentModel.DataAnnotations;

namespace SDIIS.Models
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

    public class GroupMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Group Description field is required")]
        [StringLength(150, ErrorMessage = "The Group Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The Name of the Group")]
        [DataType(DataType.Text)]
        public string Description;
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
}