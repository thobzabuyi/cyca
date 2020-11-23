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
    
    public partial class apl_Cyca_Admission_DocumentType
    {
        public apl_Cyca_Admission_DocumentType()
        {
            this.CYCA_Admissions_AdmissionDocument = new HashSet<CYCA_Admissions_AdmissionDocument>();
            this.CYCA_BodilySearch_Document = new HashSet<CYCA_BodilySearch_Document>();
            this.CYCA_IllegalItems_Document = new HashSet<CYCA_IllegalItems_Document>();
            this.CYCA_Admissions_Document = new HashSet<CYCA_Admissions_Document>();
            this.CYCA_Admissions_IllegalItemsFound = new HashSet<CYCA_Admissions_IllegalItemsFound>();
            this.CYCA_BodilySearch = new HashSet<CYCA_BodilySearch>();
            this.CYCA_GangAndTatooDocument = new HashSet<CYCA_GangAndTatooDocument>();
        }
    
        public int DocType_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<CYCA_Admissions_AdmissionDocument> CYCA_Admissions_AdmissionDocument { get; set; }
        public virtual ICollection<CYCA_BodilySearch_Document> CYCA_BodilySearch_Document { get; set; }
        public virtual ICollection<CYCA_IllegalItems_Document> CYCA_IllegalItems_Document { get; set; }
        public virtual ICollection<CYCA_Admissions_Document> CYCA_Admissions_Document { get; set; }
        public virtual ICollection<CYCA_Admissions_IllegalItemsFound> CYCA_Admissions_IllegalItemsFound { get; set; }
        public virtual ICollection<CYCA_BodilySearch> CYCA_BodilySearch { get; set; }
        public virtual ICollection<CYCA_GangAndTatooDocument> CYCA_GangAndTatooDocument { get; set; }
    }
}
