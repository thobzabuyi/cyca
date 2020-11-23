using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    /// <summary>
    /// PCM Person ViewModel
    /// </summary>
    public class PCMPersonViewModel 
    {
        public int Person_Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string First_Name { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [Display(Name = "Know As")]
        public string Known_As { get; set; }
        [Display(Name = "Identity Type")]
        public int? Identification_Type_Id { get; set; }
        public string selectedIdType { get; set; }
        [Display(Name = "Identity Number")]
        public string Identification_Number { get; set; }
        public bool Is_Piva_Validated { get; set; }
        public string Piva_Transaction_Id { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime? Date_Of_Birth { get; set; }
        public int? Age { get; set; }
        [Display(Name = "Estimated Age")]
        public bool Is_Estimated_Age { get; set; }
        [Display(Name ="Language")]
        public int? Language_Id { get; set; }
        public string selectedLanguage { get; set; }
        [Display(Name ="Gender")]
        public int? Gender_Id { get; set; }
        public string selectedGender { get; set; }
        [Display(Name ="Marital Status")]
        public int? Marital_Status_Id { get; set; }
        public string selectedMaritalStatus { get; set; }
        [Display(Name = "Preferred Contact Type")]
        public int? Preferred_Contact_Type_Id { get; set; }
        public string selectedContact { get; set; }
        [Display(Name = "Religion")]
        public int? Religion_Id { get; set; }
        public string selectedReligion { get; set; }
        [Display(Name ="Phone Number")]
        public string Phone_Number { get; set; }
        [Display(Name ="Mobile Number")]
        public string Mobile_Phone_Number { get; set; }
        [Display(Name=("Email Address"))]
        public string Email_Address { get; set; }
        [Display(Name = ("Population Group"))]
        public int? Population_Group_Id { get; set; }
        public string selectedPopulation { get; set; }
        [Display(Name = "Nationality")]
        public int? Nationality_Id { get; set; }
        public string selectNationality { get; set; }
        [Display(Name = "Disability Type")]
        public int? Disability_Type_Id { get; set; }
        public string selectedDisability { get; set; }
        public DateTime? Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime? Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public virtual ICollection<IdentificationTypeLookup> Identification_Type { get; set; }
        public virtual ICollection<LanguageTypeLookup> Language_Type { get; set; }
        public virtual ICollection<GenderTypeLookup> Gender_Type { get; set; }
        public virtual ICollection<MaritalTypeLookup> Marital_Type { get; set; }
        public virtual ICollection<ContactTypeLookup> Contact_Type { get; set; }
        public virtual ICollection<ReligionTypeLookup> Religion_Type { get; set; }
        public virtual ICollection<Population_GroupTypeLookup> Population_Group { get; set; }
        public virtual ICollection<NationalityTypeLookup> Nationality_Group { get; set; }
        public virtual ICollection<DisabilityTypeLookup> Disability_Group { get; set; }

        public int IntakeAssPar { get; set; }
    }


    /// <summary>
    /// Identification Type dropdown list
    /// </summary>
    public class IdentificationType
    {
        public int? selectedIdType { get; set; }
        public IEnumerable<IdentificationTypeLookup> Identification_Type_List { get; set; }
    }

    public class IdentificationTypeLookup
    {
        public int Identification_Type_Id { get; set; }
        public string Description { get; set; }

    }
    /// <summary>
    /// Language dropdown list
    /// </summary>
    public class LanguageType
    {
        public int? selectedLanguage { get; set; }
        public IEnumerable<LanguageTypeLookup> Language_Type_List { get; set; }
    }

    public class LanguageTypeLookup
    {
        public int Language_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// Gender dropdown list
    /// </summary>
    public class GenderType
    {
        public int? selectedGender { get; set; }
        public IEnumerable<GenderTypeLookup> Gender_Type_List { get; set; }
    }

    public class GenderTypeLookup
    {
        public int Gender_Id { get; set; }
        public string Description { get; set; }

    }
    /// <summary>
    /// marital status dropdown list
    /// </summary>
    public class MaritalType
    {
        public int? selectedMaritalStatus { get; set; }
        public IEnumerable<MaritalTypeLookup> Gender_Type_List { get; set; }
    }

    public class MaritalTypeLookup
    {
        public int Marital_Status_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// contact type dropdown list
    /// </summary>
    public class ContactType
    {
        public int? selectedContact { get; set; }
        public IEnumerable<ContactTypeLookup> Contact_Type_List { get; set; }
    }

    public class ContactTypeLookup
    {
        public int Preferred_Contact_Type_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// religion type dropdown list
    /// </summary>
    public class ReligionType
    {
        public int? selectedReligion { get; set; }
        public IEnumerable<ReligionTypeLookup> Religion_Type_List { get; set; }
    }

    public class ReligionTypeLookup
    {
        public int Religion_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// Population_Group type dropdown list
    /// </summary>
    public class Population_GroupType
    {
        public int? selectedPopulation { get; set; }
        public IEnumerable<Population_GroupTypeLookup> Population_Type_List { get; set; }
    }

    public class Population_GroupTypeLookup
    {
        public int Population_Group_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// Nationality type dropdown list
    /// </summary>
    public class NationalityType
    {
        public int? selectNationality { get; set; }
        public IEnumerable<NationalityTypeLookup> Nationality_Type_List { get; set; }
    }

    public class NationalityTypeLookup
    {
        public int Nationality_Id { get; set; }
        public string Description { get; set; }

    }

    /// <summary>
    /// Disability type dropdown list
    /// </summary>
    public class DisabilityType
    {
        public int? selectedDisability { get; set; }
        public IEnumerable<DisabilityTypeLookup> Disability_Type_List { get; set; }
    }

    public class DisabilityTypeLookup
    {
        public int Disability_Type_Id { get; set; }
        public string Description { get; set; }

    }
}
