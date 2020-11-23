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
    
    public partial class ACM_IDPWellbeing
    {
        public int Wellbeing_Id { get; set; }
        public int IndividualDevelopmentPlan_Id { get; set; }
        public string FamilyAndHomeSituation { get; set; }
        public int IsBasicHearingAndEyesigtTests { get; set; }
        public string BasicHearingAndEyesightTestsComment { get; set; }
        public int IsRoadToHealthCard { get; set; }
        public string RoadToHealthComment { get; set; }
        public int IsSignsOfAbuse { get; set; }
        public string SignsOfAbuseComments { get; set; }
        public int IsChronicIllness { get; set; }
        public string ChronicIllnessComment { get; set; }
        public int IsAwareOfHiv { get; set; }
        public string AwareOfHivComment { get; set; }
        public int IsAwareOfChronic { get; set; }
        public string AwareOfChronicComment { get; set; }
        public int IsAcuteIllness { get; set; }
        public string AcuteIllnessComment { get; set; }
        public int IsAcuteResponded { get; set; }
        public string AcuteRespondedComment { get; set; }
        public int IsDisability { get; set; }
        public string DisabilityComment { get; set; }
        public int IsAssistiveDevices { get; set; }
        public string AssistiveDevicesComment { get; set; }
        public string GeneralHealth { get; set; }
        public string ChronicAndAcuteIllness { get; set; }
        public string PhysicalDevelopment { get; set; }
        public string Disability { get; set; }
        public string Nutrition { get; set; }
        public string Clothing { get; set; }
        public string EmotionalDevelopment { get; set; }
        public string StrengthsAndresources { get; set; }
        public string NeedsConcerns { get; set; }
        public string ChangesWanted { get; set; }
        public string ActionsToEffectChange { get; set; }
    
        public virtual ACM_IndividualDevelopmentPlan ACM_IndividualDevelopmentPlan { get; set; }
    }
}