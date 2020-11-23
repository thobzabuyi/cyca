using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMDiversionViewModel
    {
        public int Diversion_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public int Source_Referral { get; set; }
        public Nullable<int> Province_Id { get; set; }
        //public virtual ICollection<ProvinceTypeLookup> Province_Type { get; set; }
        public Nullable<int> Service_Provider_Category { get; set; }
        public Nullable<int> Services_Provider_Id { get; set; }
        public Nullable<int> Programme_Id { get; set; }
        public Nullable<int> Modules { get; set; }
        public int? No_Modules { get; set; }
        public int? Sessions { get; set; }
        public Nullable<int> No_Session { get; set; }
        //public string Session_StartDate { get; set; }
        //public Nullable<System.DateTime> Session_EndDate { get; set; }
        public Nullable<int> Levels { get; set; }
        public string Court_Order { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }

        //PCM_D_SEssion_Outcome
        public int DSession_Id { get; set; }
        //public Nullable<int> Intake_Assessment_Id { get; set; }
        public string Current_Module_Attended { get; set; }
        public string Session_Attend { get; set; }
        public string Session_Date { get; set; }
        public string Name_of_the_Facilitator { get; set; }
        public string Name_of_Co_Facilitator { get; set; }
        public string Process_Notes { get; set; }
        public string Next_Session_Date { get; set; }
        public string Compliance { get; set; }


        //PCM_D_Programme

        //public int Programme_Id { get; set; }
        //public Nullable<int> Services_Provider_Id { get; set; }
        public string Programme_name { get; set; }
        public string Programme_Status { get; set; }
        public Nullable<System.DateTime> Programme_Expiry_Date { get; set; }

        //PCM_D_Service_Provider
        public string Services_Provider_Name { get; set; }
        public virtual ICollection<ServicesProviderNameTypeLookup> ServicesProviderName_Type { get; set; }
        public string Services_Provider_Status { get; set; }
        

        //PCM_D_Diversion_Outcome
        public int Diversion_Outotcome_Id { get; set; }
        //public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<System.DateTime> Court_Date { get; set; }
        public string Remand { get; set; }
        public string Reason_Remand { get; set; }
        public Nullable<System.DateTime> Next_Court_Date { get; set; }
        public string Court_Outcome { get; set; }
        public string Case_Status { get; set; }

        public virtual ICollection<DescriptionTypeLookup> Description_Type { get; set; }
        public string Description { get; set; }


        public int S_P_Id { get; set; }
        public int? P_Id { get; set; }

        public int? Modules_Id { get; set; }
        public string Module_Name { get; set; }

        public int M_Id { get; set; }
        public string Module { get; set; }
        public int? No_Module { get; set; }
        public int? No_Sessions { get; set; }
        public DateTime? Session_StartDate { get; set; }
        public DateTime? Session_EndDate { get; set; }

        public virtual ICollection<SourceReferralLookupPCM> SourceReferral_List { get; set; }
        public int? Source_Referral_Id { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "District")]
        public int District_Id { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Local Municipality")]
        public int? Local_Municipality_Id { get; set; }
        public int? selectLocalMunicipality { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Organisational Type")]
        public int? Organization_Id_Type { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Diversion Programmes")]
        public int Div_Program_Id { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Organization")]
        public int Organization_Id { get; set; }
        public virtual ICollection<OrganizationLookupPcm> PCMOrganisation_List { get; set; }
        public virtual ICollection<OrganisationTypeLookupPCM> OrganisationType_List { get; set; }
        public IEnumerable<LocalMunicipalityLookupAdopt> LocalMunicipality_List { get; set; }

        public virtual ICollection<DiversionProgrammesLookupPcm> DiversionProgrammes_List { get; set; }
        public virtual ICollection<ProvinceLookupPCM> Province_List { get; set; }
        public virtual ICollection<DistrictLookupPCM> District_List { get; set; }
        public string DesrciptionDivesionPrograme { get; set; }
        public string DesrciptionCourttype { get; set; }
        public DateTime? courtEpxdate { get; set; }
        public string RefSourcedesrciption { get; set; }
        public DateTime? CourtDate { get; set; }
        public int? levelDiversion { get; set; }
        public DateTime? Court_OrderDiv { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Order")]
        public int ? Recomendation_Order_Id { get; set; }
        public string orders { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Programme Level")]
        public int ? Programme_Level_Id { get; set; }
        public virtual ICollection<ProgrammeLevelLookupPCM> ProgrammeLevel_List { get; set; }
        public virtual ICollection<RecomendationOrderLookupPcm> RecomendationOrder_List { get; set; }

        public int? Court_Type_Id { get; set; }

        public int? Childrens_Court_Outcome_Id { get; set; }

        public int? Formal_Courtcome_Id { get; set; }

        public int? PCM_Preliminary_Id { get; set; }
        public virtual ICollection<ProgrammeLookupPCM> Programme_List { get; set; }
        public int AddEditValue { get; set; }

        public virtual ICollection<ProgrammeAgeGroupLookupPCM> ProgrammeAgeGroup_List { get; set; }
        public int Programme_AgeGroup_Id { get; set; }
    }

    public class DescriptionTypeLookup
    {
        public int Province_Id { get; set; }
        public string Description { get; set; }
    }


    public class DescriptionType
    {
        public int? selectedDescription { get; set; }
        public IEnumerable<DescriptionTypeLookup> Description_Type_List { get; set; }
    }


    public class ServicesProviderNameTypeLookup
    {
        public int Services_Provider_Id { get; set; }
        public string Services_Provider_Name { get; set; }
    }


    public class ServicesProviderNameType
    {
        public int? selectedServicesProviderName { get; set; }
        public IEnumerable<ServicesProviderNameTypeLookup> ServicesProviderName_Type_List { get; set; }
    }

   

    public class SourceReferralPCM
    {
        public int? SourceReferralDetails { get; set; }
        public IEnumerable<SourceReferralLookupPCM> SourceReferral_List { get; set; }
    }
    public class SourceReferralLookupPCM
    {
        public int Source_Referral_Id { get; set; }
        public string Description { get; set; }



    }

    public class ProgrammeLevelPCM
    {
        public int? ProgrammeLevelDetails { get; set; }
        public IEnumerable<ProgrammeLevelLookupPCM> ProgrammeLevel_List { get; set; }
    }
    public class ProgrammeLevelLookupPCM
    {
        public int Programme_Level_Id { get; set; }
        public int? Description { get; set; }



    }

    public class ProgrammePCM
    {
        public int? ProgrammeDetails { get; set; }
        public IEnumerable<ProgrammeLookupPCM> Programme_List { get; set; }
    }
    public class ProgrammeLookupPCM
    {
        public int Div_Program_Id { get; set; }
        public string Programme_Name { get; set; }



    }

    public class ProgrammeAgeGroupPCM
    {
        public int? ProgrammeAgeGroupDetails { get; set; }
        public IEnumerable<ProgrammeAgeGroupLookupPCM> ProgrammeAgeGroup_List { get; set; }
    }
    public class ProgrammeAgeGroupLookupPCM
    {
        public int Programme_AgeGroup_Id { get; set; }
        public string Description { get; set; }



    }


}
