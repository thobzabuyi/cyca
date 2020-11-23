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
    
    public partial class ACM_AuthorityToRemove
    {
        public ACM_AuthorityToRemove()
        {
            this.ACM_AcknowledgementOfReceipt = new HashSet<ACM_AcknowledgementOfReceipt>();
            this.ACM_CopiesOfAuthority = new HashSet<ACM_CopiesOfAuthority>();
            this.ACM_OfficialConductingTheRemoval = new HashSet<ACM_OfficialConductingTheRemoval>();
            this.ACM_ReasonForRemoval = new HashSet<ACM_ReasonForRemoval>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Safecare_Id { get; set; }
        public int ResponsiblePerson_Id { get; set; }
        public string SpecialNeeds { get; set; }
        public string MedicalConditions { get; set; }
        public string Behaviour { get; set; }
        public string Other { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int Caseworklist_Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ResidentialAddress { get; set; }
        public string WorkAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> RelationshipToChild { get; set; }
    
        public virtual ICollection<ACM_AcknowledgementOfReceipt> ACM_AcknowledgementOfReceipt { get; set; }
        public virtual ICollection<ACM_CopiesOfAuthority> ACM_CopiesOfAuthority { get; set; }
        public virtual ICollection<ACM_OfficialConductingTheRemoval> ACM_OfficialConductingTheRemoval { get; set; }
        public virtual ICollection<ACM_ReasonForRemoval> ACM_ReasonForRemoval { get; set; }
    }
}