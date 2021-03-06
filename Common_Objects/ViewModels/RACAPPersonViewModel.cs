﻿using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common_Objects.ViewModels
{
   public class RACAPPersonViewModel
    {
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        public int Person_Id { get; set; }
        //Amended by Herman
        public int? Intake_Assessment_Id { get; set; }
        //End Amended by Herman
        [Required]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }
        [Display(Name = "First Name")]
        public string First_Name_ParentTwo { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [Display(Name = "Last Name")]
        public string Last_Name_ParentTwo { get; set; }
        [Display(Name = "Known As")]
        public string Known_As { get; set; }
        [Display(Name = "Known As")]
        public string Known_As_ParentTwo { get; set; }
        [Display(Name = "Identity Type")]
        public int? Identification_Type_Id { get; set; }
        [Display(Name = "Identity Type")]
        public int? Identification_Type_Id_ParentTwo { get; set; }
        public string selectedIdType { get; set; }
        [Display(Name = "Identity Number")]
        public string selectedIdType_ParentTwo { get; set; }
        [Display(Name = "Identity Number")]
        public string Identification_Number { get; set; }
        [Display(Name = "Identity Number")]
        public string Identification_Number_ParentTwo { get; set; }
        public bool Is_Piva_Validated { get; set; }
        public bool Is_Piva_Validated_ParentTwo { get; set; }
        public string Piva_Transaction_Id { get; set; }
        public string Piva_Transaction_Id_ParentTwo { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date_Of_Birth { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy", ApplyFormatInEditMode = true)]


        public DateTime? Date_Of_Birth_ParentTwo { get; set; }
        public int? Age { get; set; }
        //Addition by Herman
        [Display(Name = "Age From")]
        public int? AgeFrom { get; set; }
        [Display(Name = "Age To")]
        public int? AgeTo { get; set; }
        //end addition by Herman
        public int? Age_ParentTwo { get; set; }
        [Display(Name = "Estimated Age")]
        public bool Is_Estimated_Age { get; set; }
        [Display(Name = "Estimated Age")]
        public bool Is_Estimated_Age_ParentTwo { get; set; }
        [Display(Name = "Language")]
        public int? Language_Id { get; set; }
        [Display(Name = "Language")]
        public int? Language_Id_ParentTwo { get; set; }
        public string selectedLanguage { get; set; }
        public string selectedLanguage_ParentTwo { get; set; }

        [Display(Name = "Gender")]
        public int? Gender_Id { get; set; }
        [Display(Name = "Gender")]
        public int? Gender_Id_ParentTwo { get; set; }
        public string selectedGender { get; set; }
        public string selectedGender_ParentTwo { get; set; }

        [Display(Name = "Marital Status")]
        public int? Marital_Status_Id { get; set; }
        [Display(Name = "Marital Status")]
        public int? Marital_Status_Id_ParentTwo { get; set; }
        public string selectedMaritalStatus { get; set; }
        public string selectedMaritalStatus_ParentTwo { get; set; }

        [Display(Name = "Preferred Contact Type")]
        public int? Preferred_Contact_Type_Id { get; set; }
        [Display(Name = "Preferred Contact Type")]
        public int? Preferred_Contact_Type_Id_ParentTwo { get; set; }
        public string selectedContact { get; set; }
        public string selectedContact_ParentTwo { get; set; }

        [Display(Name = "Religion")]
        public int? Religion_Id { get; set; }
        [Display(Name = "Religion")]
        public int? Religion_Id_ParentTwo { get; set; }
        public string selectedReligion { get; set; }
        public string selectedReligion_ParentTwo { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone_Number_ParentTwo { get; set; }
        [Display(Name = "Mobile Number")]
        public string Mobile_Phone_Number { get; set; }


        [Display(Name = "Mobile Number")]
        public string Mobile_Phone_Number_ParentTwo { get; set; }
        [Display(Name = ("Email Address"))]
        public string Email_Address { get; set; }
        [Display(Name = ("Email Address"))]
        public string Email_Address_ParentTwo { get; set; }
        [Display(Name = ("Population Group"))]
        public int? Population_Group_Id { get; set; }
        [Display(Name = ("Population Group Second Choice"))]
        public int? Population_Group_Id_Second_Choice { get; set; }
        [Display(Name = ("Population Group"))]
        public int? Population_Group_Id_ParentTwo { get; set; }
        public string selectedPopulation { get; set; }
        public string selectedPopulation_ParentTwo { get; set; }

        [Display(Name = "Nationality")]
        public int? Nationality_Id { get; set; }
        [Display(Name = "Nationality")]
        public int? Nationality_Id_ParentTwo { get; set; }
        public string selectNationality { get; set; }
        public string selectNationality_ParentTwo { get; set; }

        [Display(Name = "Disability Type")]
        public int? Disability_Type_Id { get; set; }
        public string selectedDisability { get; set; }
        [Display(Name = "Disability Type")]
        public int? Disability_Type_Id_ParentTwo { get; set; }
        public string selectedDisability_ParentTwo { get; set; }
        public DateTime? Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime? Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int? Special_Need_Id { get; set; }

        public string selectedSpecialNeed { get; set; }

        //public int? Special_Need { get; set; }
        public int? Skin_Colour_Id { get; set; }
        public string selectedSkin_Colour { get; set; }
        public int? Race_Id { get; set; }

        public int? Race_Id_Option2 { get; set; }
        public int? Eye_Colour_Id { get; set; }
        public string selectedEye_Colour { get; set; }
        public int? Ethnic_Cultural_Id { get; set; }
        public string selectedEthnic_Cultural { get; set; }
        public int? Body_Structure_Id { get; set; }
        public string selecetedBody_Structure { get;set;}

        public virtual ICollection<IdentificationTypeLookupRACAP> Identification_Type { get; set; }
        public virtual ICollection<LanguageTypeLookupRACAP> Language_Type { get; set; }
        public virtual ICollection<GenderTypeLookupRACAP> Gender_Type { get; set; }
        public virtual ICollection<MaritalTypeLookupRACAP> Marital_Type { get; set; }
        public virtual ICollection<ContactTypeLookupRACAP> Contact_Type { get; set; }
        public virtual ICollection<ReligionTypeLookupRACAP> Religion_Type { get; set; }
        public virtual ICollection<Population_GroupTypeLookupRACAP> Population_Group { get; set; }
        public virtual ICollection<Special_NeedTypeLookupRACAP> Special_NeedType { get; set; }
        public virtual ICollection<RaceTypeLookupRACAP> Race_Type { get; set; }
        public virtual ICollection<Skin_ColourTypeLookupRACAP> Skin_Colour_Type { get; set; }
        public virtual ICollection<Eye_ColourTypeLookupRACAP> Eye_Colour_Type { get; set; }
        public virtual ICollection<Ethnic_CulturalTypeLookupRACAP> Ethnic_Cultural_Type { get; set; }
        public virtual ICollection<Body_StructureTypeLookupRACAP> Body_Structure_Type { get; set; }
        public virtual ICollection<NationalityTypeLookupRACAP> Nationality_Group { get; set; }
        public virtual ICollection<DisabilityTypeLookupRACAP> Disability_Group { get; set; }

        public virtual ICollection<Province> Province_Type { get; set; }

    }

    /// <summary>
    /// Identification Type dropdown list
    ///// </summary>
    public class IdentificationTypeRACAP
    {
        public int? selectedIdType { get; set; }
        public IEnumerable<IdentificationTypeLookupRACAP> Identification_Type_List { get; set; }
    }
    public class IdentificationTypeLookupRACAP
    {
        public int Identification_Type_Id { get; set; }
        public string Description { get; set; }

    }


    public class LanguageTypeRACAP
    {
        public int? selectedLanguage { get; set; }
        public IEnumerable<LanguageTypeLookupRACAP> Language_Type_List { get; set; }
    }

    public class LanguageTypeLookupRACAP
    {
        public int Language_Id { get; set; }
        public string Description { get; set; }

    }

    public class GenderTypeRACAP
    {
        public int? selectedGender { get; set; }
        public IEnumerable<GenderTypeLookupRACAP> Gender_Type_List { get; set; }
    }

    public class GenderTypeLookupRACAP
    {
        public int Gender_Id { get; set; }
        public string Description { get; set; }

    }

    public class MaritalTypeRACAP
    {
        public int? selectedMaritalStatus { get; set; }
        public IEnumerable<MaritalTypeLookupRACAP> Gender_Type_List { get; set; }
    }

    public class MaritalTypeLookupRACAP
    {
        public int Marital_Status_Id { get; set; }
        public string Description { get; set; }

    }


    public class ContactTypeRACAP
    {
        public int? selectedContact { get; set; }
        public IEnumerable<ContactTypeLookupRACAP> Contact_Type_List { get; set; }
    }

    public class ContactTypeLookupRACAP
    {
        public int Preferred_Contact_Type_Id { get; set; }
        public string Description { get; set; }

    }


    public class ReligionTypeRACAP
    {
        public int? selectedReligion { get; set; }
        public IEnumerable<ReligionTypeLookupRACAP> Religion_Type_List { get; set; }
    }

    public class ReligionTypeLookupRACAP
    {
        public int Religion_Id { get; set; }
        public string Description { get; set; }

    }

    public class Population_GroupTypeRACAP
    {
        public int? selectedPopulation { get; set; }
        public IEnumerable<Population_GroupTypeLookupRACAP> Population_Type_List { get; set; }
    }

    public class Population_GroupTypeLookupRACAP
    {
        public int Population_Group_Id { get; set; }
        public string Description { get; set; }

    }

    public class Special_NeedTypeLookupRACAP
    {
        public int Special_Need_Id { get; set; }
        public string Description { get; set; }

    }


    public class RaceTypeRACAP
    {
        public int? selectedGender { get; set; }
        public IEnumerable<RaceTypeLookupRACAP> Race_Type_List { get; set; }
    }

    public class RaceTypeLookupRACAP
    {
        public int Race_Id { get; set; }
        public string Description { get; set; }

    }

    public class Skin_ColourTypeRACAP
    {
        public int? selectedSkin_Colour { get; set; }
        public IEnumerable<Skin_ColourTypeLookupRACAP> Skin_Colour_Type_List { get; set; }
    }
    public class Skin_ColourTypeLookupRACAP
    {
        public int Skin_Colour_Id { get; set; }
        public string Description { get; set; }

    }

    public class Body_StructureTypeRACAP
    {
        public int? selectedBody_Structure { get; set; }
        public IEnumerable<Body_StructureTypeLookupRACAP> Body_Structure_Type_List { get; set; }
    }
    public class Body_StructureTypeLookupRACAP
    {
        public int Body_Structure_Id { get; set; }
        public string Description { get; set; }

    }

    public class Ethnic_CulturalTypeRACAP
    {
        public int? selectedEthnic_Cultural { get; set; }
        public IEnumerable<Ethnic_CulturalTypeLookupRACAP> Ethnic_Cultural_Type_List { get; set; }
    }
    public class Eye_ColourTypeLookupRACAP
    {
        public int Eye_Colour_Id { get; set; }
        public string Description { get; set; }

    }

    public class Eye_ColourTypeRACAP
    {
        public int? selectedEye_Colour { get; set; }
        public IEnumerable<Eye_ColourTypeLookupRACAP> Eye_Colour_Type_List { get; set; }
    }




    public class Ethnic_CulturalTypeLookupRACAP
    {
        public int Ethnic_Cultural_Id { get; set; }
        public string Description { get; set; }

    }

 

    public class NationalityTypeRACAP
    {
        public int? selectNationality { get; set; }
        public IEnumerable<NationalityTypeLookupRACAP> Nationality_Type_List { get; set; }
    }

    public class NationalityTypeLookupRACAP
    {
        public int Nationality_Id { get; set; }
        public string Description { get; set; }

    }

    public class DisabilityTypeRACAP
    {
        public int? selectedDisability { get; set; }
        public IEnumerable<DisabilityTypeLookupRACAP> Disability_Type_List { get; set; }
    }

    public class DisabilityTypeLookupRACAP
    {
        public int Disability_Type_Id { get; set; }
        public string Description { get; set; }

    }

 





}
