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
    
    public partial class ACM_IDPDevelopmentalAreaBeloning
    {
        public int Belonging_Id { get; set; }
        public int IndividualDevelopmentPlan_Id { get; set; }
        public int IsBirthCertificate { get; set; }
        public string BirthCertificateComment { get; set; }
        public int IsId { get; set; }
        public string IdComment { get; set; }
        public string IdentityPersonalAndFamily { get; set; }
        public string IdentityCultural { get; set; }
        public string IdentitySexual { get; set; }
        public string IdentityReligious { get; set; }
        public string Safety { get; set; }
        public string CaringRelationshipCaregiver { get; set; }
        public string PositiveCommunication { get; set; }
        public string PositiveRelationship { get; set; }
        public string BoundriesForDailyLiving { get; set; }
        public string SenseOfPlaceInWorld { get; set; }
        public string StrenghtAndResources { get; set; }
        public string NeedsConcerns { get; set; }
        public string ChangesWanted { get; set; }
        public string ActionsToEffectChange { get; set; }
    
        public virtual ACM_IndividualDevelopmentPlan ACM_IndividualDevelopmentPlan { get; set; }
    }
}
