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
    
    public partial class CYCA_BodilySearch
    {
        public CYCA_BodilySearch()
        {
            this.CYCA_BodilySearch_Document = new HashSet<CYCA_BodilySearch_Document>();
        }
    
        public int Bodily_Search_Id { get; set; }
        public Nullable<System.DateTime> Bodily_Search_Date { get; set; }
        public Nullable<System.DateTime> Bodily_Search_Time { get; set; }
        public string Description { get; set; }
        public Nullable<int> Search_Reason_Id { get; set; }
        public Nullable<int> Conducted_By { get; set; }
        public Nullable<int> Witnessed_By { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        public Nullable<int> Document_Type_Id { get; set; }
        public Nullable<int> Person_Id { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public string OtherDocTypeDescription { get; set; }
        public string OtherSeacrhReasonDescription { get; set; }
    
        public virtual apl_Cyca_Admission_DocumentType apl_Cyca_Admission_DocumentType { get; set; }
        public virtual apl_Cyca_Bodily_Search_Reasons apl_Cyca_Bodily_Search_Reasons { get; set; }
        public virtual User apl_User { get; set; }
        public virtual User apl_User1 { get; set; }
        public virtual CYCA_Admissions_AdmissionDetails CYCA_Admissions_AdmissionDetails { get; set; }
        public virtual ICollection<CYCA_BodilySearch_Document> CYCA_BodilySearch_Document { get; set; }
        public virtual Person int_Person { get; set; }
    }
}