using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Common_Objects.Models;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class ACMCaseWorkList
    {
        public bool isSupervisor { get; set; }
        public ACM_CaseWorkList ACMCase { get; set; }
        public ACM_ChildAssessment ChildAssessmentInfo { get; set; }
        public ACM_ChildrensCourtReport ChildrenCourtReportInfo { get; set; }
        public ACM_ChildrenConcerned ChildrensCourtChildConcerned { get; set; }
        public ACM_SpecialCurcumstances ChildrensCourtSpecialCircumstances { get; set; }
        public ACM_ViewsOfChildConcerned ChildrensCourtViewsOfChild { get; set; }
        public ACM_EvaluationRecommendationConclusion ChildrensCourtEvaluation { get; set; }
        public ACM_RecommendedAssistChildFamily ChildrensCourtAssistFamily { get; set; }
        public ACM_RecommendedAssistChild ChildrensCourtAssistChild { get; set; }


        public ACM_FosterCareBiologicalParentsOrGuardians ACMFosterCareBiologicalParentsOrGuardians { get; set; }
        public ACM_FosterParent ACMFosterParent { get; set; }
        public ACM_ClusterFosterCareScheme ACMClusterFosterCareScheme { get; set; }
        public ACM_DesignatedCPOOrSocialWorker ACMDesignatedCPOOrSocialWorker { get; set; }
        public Client_Foster_Parent ClientFosterParent { get; set; }
        public ACM_SocialHistory SocialHistoryInfo { get; set; }
        public ACM_MedicalFactors MedicalFactorsInfo { get; set; }
        public ACM_InterventionPlanning InterventionPlanningInfo { get; set; }
        public ACM_Investigation Investigation { get; set; }
        public ACM_InterviewProcessNote ProcessNote { get; set; }
        public ACMInterviewedPersonSearchViewModel InterviewedPerson { get; set; }
        public ACMSearchViewModel AssessedPersonSearch { get; set; }
        public ACM_FactorsResultingToInvestigation ACMFactorsResultingToInvestigation { get; set; }
        public ACM_MeasuresToAssistFamily ACMMeasuresToAssistFamily { get; set; }
        public ACM_PrivateFamilyArrangements ACMPrivateFamilyArrangements { get; set; }
        public ACM_WrittenRequest ACMWrittenRequest { get; set; }
        public ACM_PermanencyPlan ACMPermanencyPlan { get; set; }
        public ACM_Evaluation ACMEvaluation { get; set; }
        public ACM_Recommendation ACMRecommendation { get; set; }
        public ACM_FamilyProfile FamilyProfile { get; set; }
        public ACM_IdentifyingChildDetails ChildDetails { get; set; }
        public Social_Worker apl_Social_Worker { get; set; }
        public ACM_ACSSCareGiverDeclaration ACMACSSCareGiverDeclaration { get; set; }
        public ACM_ACSSSocialWorkerDeclaration ACMACSSSocialWorkerDeclaration { get; set; }
        public ACM_SouthAfricanSafetyAssessmentTool SouthAfricanAssessmentToolInfo { get; set; }
        public ACM_ActuarialRiskAssessment RiskAssessmentInfo { get; set; }
        public ACM_ActuarialRiskAssessment ActuarialRiskInfo { get; set; }
        public ACM_TypeOfReportedSuspectedMaltreatment MaltreatmentInfo { get; set; }
        public ACM_ChildVulnerability ChildVulnerabilityInfo { get; set; }
        public ACM_SafetyThreats SafetyThreatsInfo { get; set; }
        public ACM_ProtectiveCapacities ProtectiveCapacitiesInfo { get; set; }
        public ACM_SafetyInterventions SafetyInterventions { get; set; }
        public ACM_SafetyDecision SafetyDecisions { get; set; }
        public ACM_IDPChildrenDetails ACMIDPChildrenDetails { get; set; }
        public ACM_IDPParentsDetails ACMIDPParentsDetails { get; set; }
        public ACM_IDPWellbeing ACMIDPWellbeing { get; set; }
        public ACM_CarePlan ACMCarePlan { get; set; }

        public ACM_IndividualDevelopmentPlan ACMIndividualDevelopmentPlan { get; set; }
        public ACM_IDPDevelopmentalAreaBeloning ACMIDPDevelopmentalAreaBeloning { get; set; }
        public ACM_IDPDevelopmentalAreaMastery ACMIDPDevelopmentalAreaMastery { get; set; }
        public ACM_IDPDevelopmentalAreaIndependence ACMIDPDevelopmentalAreaIndependence { get; set; }
        public ACM_IDPDevelopmentalAreaGenerosity ACMIDPDevelopmentalAreaGenerosity { get; set; }

        public ACM_MedicalParticulars ACMMedicalParticulars { get; set; }
        public ACM_ChildDetailsRemovalAlreadyInAlternativeCare ACMChildDetailsRemovalAlreadyInAlternativeCare { get; set; }
        public ACM_DischargeFromAlternativeCare ACMDischargeFromAlternativeCare { get; set; }

        public ACM_LeaveOfAbsence ACMLeaveOfAbsence { get; set; }

        public ACM_AproovalToRemainInTheCareOfCYCC ACMAproovalToRemainInTheCareOfCYCC { get; set; }
        public ACM_ApplicationForMinisterConsent ACMApplicationForMinisterConsent { get; set; }
        public ACM_PlacementSupervisionAndAfterCare ACMPlacementSupervisionAndAfterCare { get; set; }
        public ACM_CourtOutcome ACMCourtOutcome { get; set; }
        public ACM_PlanOfAction ACMPlanOfAction { get; set; }
        public ACM_InterventionEvaluationReport ACMInterventionEvaluationReport { get; set; }

        public ACM_ConsentToSurgicalOperationByAChild ACMConsentToSurgicalOperationByAChild { get; set; }
        public ACM_ParentsGuardiansAssentingToSurgicalOperation ACMParentsGuardiansAssentingToSurgicalOperation { get; set; }
        public ACM_ConsentToSurgicalOperationOfAChildByAParentAgedBellowEighteenYears ACMConsentToSurgicalOperationOfAChildByAParentAgedBellowEighteenYears { get; set; }
        public ACM_ParentAgedBelowEighteenYearsGivingConsent ACMParentAgedBelowEighteenYearsGivingConsent { get; set; }

        public List<Person> Client_Person_List { get; set; }
        public List<Person> AlternateCaregivers { get; set; }
        public List<Person> Siblings { get; set; }
        public List<Person> OtherLivingWith { get; set; }
        public List<Person> PeopleWhoWereConsultedList { get; set; }
        public string SocialWorkerAddress { get; set; }
        public string SocialWorkerQualifications { get; set; }


        public YesNoOptionsEnumType YesNoOptionsEnumType { get; set; }
        public SchoolGradeEnumType SchoolGradeEnumType { get; set; }

        public SchoolTypeEnumType SchoolTypeEnumType { get; set; }

        public ACMSearchViewModel SourcesOfInformationPerson { get; set; }

        public School aplSchool { get; set; }
        public School_Type aplSchoolType { get; set; }
        public Grade aplGrade { get; set; }

        public ACM_Attachment ACMAttachment { get; set; }
        public ACM_FosterCarePlan ACMFosterCarePlan { get; set; }
        public ACM_FosterCareViewsOfChildren ACMFosterCareViewsOfChildren { get; set; }

        //Form 36: Authority for Removal of Child to Temporary Safe Care
        public ACM_AuthorityToRemove Form36AuthorityForRemoval { get; set; }
        public ACM_ReasonForRemoval Form36ReasonForRemoval { get; set; }
        public ACM_AcknowledgementOfReceipt Form36AcknowledgementOfReceipt { get; set; }
        public ACM_CopiesOfAuthority Form36CopiesOfAuthority { get; set; }
        public ACM_OfficialConductingTheRemoval Form36OfficialConductingTheRemoval { get; set; }

        public ACM_SummaryOfDevelopmentArea ACMSummaryOfDevelopmentArea { get; set; }
        public ACM_CarePlanReviewCaregiverDetails ACMCarePlanReviewCaregiverDetails { get; set; }
        public ACM_CarePlanReviewPersonSeen ACMCarePlanReviewPersonSeen { get; set; }

        public Client_Biological_Parent ClientBiologicalParent { get; set; }

        public int SelectedEvaluation_Id { get; set; }
        public SelectList Evaluation_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var evaluationList = (from a in _db.apl_EvaluationConclusions
                                            select a).ToList();

                var evaluation = (from m in evaluationList
                                          select new SelectListItem()
                                          {
                                              Text = m.Description,
                                              Value = m.Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.Id.Equals(SelectedEvaluation_Id)
                                          }).ToList();

                var selectList = new SelectList(evaluation, "Value", "Text", SelectedEvaluation_Id);
                return selectList;
            }
        }



    }
}
