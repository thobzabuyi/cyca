using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPViewChildDetailsViewModel
    {
        //intake
        public int int_Person_Child_Id { get; set; }
        public int RACAP_CaseId { get; set; }
        public int int_Assesment_Id { get; set; }

        public int RACAP_WorkListId { get; set; }
        public int Record_Id { get; set; }
        public int Registration_Id { get; set; }
        public string RegistrationStatus { get; set; }
        public string RecordStatus { get; set; }
        public string RACAP_Ref_Number { get; set; }
        //        First Name
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Known_As { get; set; }
        public string Identity_Type { get; set; }
        public string Identity_Number { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public int? Age { get; set; }
        public int? Estimated_Age { get; set; }
        public int? Language { get; set; }
        public string Lamguage_S { get; set; }
        public int? Gender { get; set; }
        public string Gender_S { get; set; }
        public int? Population_Group { get; set; }
        public string Population_Group_S { get; set; }


        //    Features:

        public int? Skin_Color { get; set; }
        public string Skin_Color_S { get; set; }
        public List<int> Disability { get; set; }
        public int? Body_Structure { get; set; }
        public string Body_Structure_S { get; set; }
        public int? Ethnic_CulturalGroup { get; set; }
        public string Ethnic_CulturalGroup_S { get; set; }
        public List<int> Special_Needs { get; set; }
        public int? Eye_Color { get; set; }
        public string Eye_Color_S { get; set; }
        public int? Race { get; set; }
        public string Race_S { get; set; }

        //    Org:
        public int? Organisation_Responsible_For_Child { get; set; }
        public string Organisation_Responsible_For_Child_S { get; set; }
        public int? Social_Worker_Id { get; set; }
        public string Social_Worker { get; set; }
        public string Accreditation_Ref { get; set; }
        public DateTime? Accreditation_Date { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Province { get; set; }
        public int? OrganisationType { get; set; }
        public string OrganisationType_S { get; set; }



        //    Supporting Doc:
        public int? SupDocId { get; set; }
        public string SupDocDescription { get; set; }

    }
}
