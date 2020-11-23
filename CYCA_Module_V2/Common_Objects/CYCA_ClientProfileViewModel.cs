using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCA_DynamicDataBaseModel
    {
        public List<CYCA_DynamicDataModel> dynamicDataModels { get; set; }
        public int ChildId { get; set; }
    }
    public class CYCA_DynamicDataModel
    {
        public int Id { get; set; }
        public string DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; set; }

        public string Venue { get; set; }
        public string CreatedFor { get; set; }
        public int CreatedForId { get; set; }
        public string File { get; set; }
        public List<FileModel> Data { get; set; }
        public int FormTypeId { get; set; }

    }
    
    public class CYCA_ClientProfileViewModel
    {
    }
    public class CYCA_ClientBiometricViewModel
    {
        [Display(Name = "Enrollment Verified")]
        public bool IsVerified { get; set; }
        [Display(Name = "Enrolled")]
        public bool HasBiometric { get; set; }
        [Display(Name = "PIVA Verified")]
        public bool IsPivaVerified { get; set; }
        public string UniqueIdentifier { get; set; }
    }
    public class CYCA_AdmissionsHistoryViewModel
    {
        public int Id { get; set; }
    }
    public abstract class ResponseMessage
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public Network Network { get; set; }
        public Configuration Configuration { get; set; }
    }
    public class FPCaptureRs : ResponseMessage
    {
        public int FingerprintSet { get; set; }
        public string FingerprintSetName
        {
            get { return FingerprintSet.ToString(); }

        }
        public int Codec { get; set; }
        public string CodecName
        {
            get { return Codec.ToString(); }

        }
        public List<Finger> Fingers { get; set; }
        public string RequestType { get; set; }
        public int PersonId { get; set; }
        public Guid UuId { get; set; }
    }
    public class Network
    {
        public string MAC { get; set; }
        public string IP { get; set; }
        public string Mask { get; set; }
        public string GatewayIP { get; set; }

    }
    public class Configuration
    {
        [JsonProperty(PropertyName = "saps.FacReg")]
        public FacReg FacReg { get; set; }
        [JsonProperty(PropertyName = "saps.FacSvc")]
        public FacSvc FacSvc { get; set; }
    }
    public class FacReg
    {
        public string Cluster { get; set; }
        public string ClusterCode { get; set; }
        public string Division { get; set; }
        public string DivisionCode { get; set; }
        public string Component { get; set; }
        public string ComponentCode { get; set; }
        public string SubComponent { get; set; }
        public string SubComponentCode { get; set; }
    }
    public class FacSvc
    {
        public string Name { get; set; }
        public string WebserviceUrl { get; set; }
    }
    public class Finger
    {
        public int Sequence { get; set; }
        public int Code { get; set; }
        public string Name
        {
            get { return Code.ToString(); }

        }
        public int? Codec { get; set; }
        public string CodecName
        {
            get { return Codec != null ? Codec.ToString() : null; }
        }
        public byte[] Print { get; set; }
    }
    public enum CYCATabType
    {
        Biometric,
        CarePlan,
        Inventory,
        Admissions,
        ReportableIncidents,
        MedicalAssessments,
        History,
        BodySearch,
        ExtraMuralActivity,
        GangAndTattoos,
        RenderTattoos,
        //RenderGangAndTattoos,
        DischargeHistory
    }
    public enum CYCAReportTabType
    {
        admission,
        bedspace,
        illigalitems,
        gang,
        pregnantchildren,
        dischargedchidren,
        reportableincident
    }
    public enum CYCARoleType
    {
        FacilityManager,
        TeamLeader,
        CareWorker,
        ProvincialCoordinator,
        Other
    }

 
}