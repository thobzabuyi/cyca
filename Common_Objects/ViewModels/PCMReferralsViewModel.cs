using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMReferralsViewModel
    {
        public int Referrals_Id { get; set; }
        public int? PCM_Case_Id { get; set; }
        public int? Client_Employee_Details_ID { get; set; }
        public string theClerk { get; set; }
        public string theParticular { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Date_Modified { get; set; }

        public int? Type_Referral_Id { get; set; }
        public string Type_Referral { get; set; }
        public virtual ICollection<ReferralTypeLookup> Referral_Type { get; set; }

        //PCM_Referral_To_Court
        public int Court_Referrals_Id { get; set; }
        public bool Child_Need_Care_Protection { get; set; }
        public bool Stay_At_Parents_Home { get; set; }
        public bool Comitted_Minor_Offence { get; set; }
        public Nullable<int> Employee_Id_Copy_To { get; set; }

        //counselling-Accredited programme- Social worker
        public int? Category_Referrals_Id { get; set; }
        public DateTime? Period_From { get; set; }
        public DateTime? Period_To { get; set; }
        public string Progress_Report { get; set; }

        //public int? Type_Referral_Id { get; set; }

        [Required]
        [Display(Name = "Refferal Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime? Refferal_Date { get; set; }
        

        public bool Referral_of_Child_To_Counselling { get; set; }
        public bool Referral_Child_To_Social_Worker { get; set; }
        public bool Accredited_Programme { get; set; }
       public int? Intake_Assessment_Id { get; set; }
    }

    public class ReferralType
    {
        public int? selectedReferral { get; set; }
        public IEnumerable<ReferralTypeLookup> Referral_Type_List { get; set; }
    }

    public class ReferralTypeLookup
    {
        public int? Type_Referral_Id { get; set; }
        public string Type_Referral { get; set; }
    }
}
