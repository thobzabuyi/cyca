//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common_Objects.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACM_ProtectiveCapacities
    {
        public ACM_ProtectiveCapacities()
        {
            this.ACM_ProtectiveCapacitiesCareGivers = new HashSet<ACM_ProtectiveCapacitiesCareGivers>();
        }
    
        public int Id { get; set; }
        public int Sasat_Id { get; set; }
        public string ChildParticipateInSafetyInterventions { get; set; }
        public string CaregiverWillingAndAble { get; set; }
        public Nullable<int> CaregiverWillingAndAble_Name { get; set; }
        public string CaregiverPhysicalAndEmotionalCapacity { get; set; }
        public Nullable<int> CaregiverPhysicalAndEmotionalCapacity_Name { get; set; }
        public string WillingToReconize { get; set; }
        public Nullable<int> WillingToReconize_Name { get; set; }
        public string AbilityToAccessAndUse { get; set; }
        public Nullable<int> AbilityToAccessAndUse_Name { get; set; }
        public string SupportiveReltionships { get; set; }
        public Nullable<int> SupportiveRelaitionships_Name { get; set; }
        public string AcceptTheInvolvement { get; set; }
        public Nullable<int> AcceptTheInvolvement_Name { get; set; }
        public string HealthyRelationship { get; set; }
        public Nullable<int> HealthyRelationship_Name { get; set; }
        public string AwareOfAndCommited { get; set; }
        public Nullable<int> AwareOfAndCommited_Name { get; set; }
        public string EffectiveProblemsolving { get; set; }
        public Nullable<int> EffectiveProblemSolving_Name { get; set; }
        public string Other { get; set; }
        public string Other_Description { get; set; }
    
        public virtual ICollection<ACM_ProtectiveCapacitiesCareGivers> ACM_ProtectiveCapacitiesCareGivers { get; set; }
        public virtual ACM_SouthAfricanSafetyAssessmentTool ACM_SouthAfricanSafetyAssessmentTool { get; set; }
    }
}