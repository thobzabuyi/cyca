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
    
    public partial class ACM_SafetyInterventions
    {
        public int Id { get; set; }
        public int Sasat_Id { get; set; }
        public string DirectService { get; set; }
        public string UseOfExtendedFamily { get; set; }
        public string CommunityUse { get; set; }
        public string ParentCaregiver { get; set; }
        public string AlledgedPerpetrator { get; set; }
        public string NonOffendingParentCargiver { get; set; }
        public string LegalInterventionPlanned { get; set; }
        public string Other { get; set; }
        public string OtherDescription { get; set; }
        public string VoluntaryAggrees { get; set; }
        public string ProtectiveCustody { get; set; }
    
        public virtual ACM_SouthAfricanSafetyAssessmentTool ACM_SouthAfricanSafetyAssessmentTool { get; set; }
    }
}
