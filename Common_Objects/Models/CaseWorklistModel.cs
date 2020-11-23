using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace Common_Objects.Models
{
    public class CaseWorklistModel
    {
        public ACMCaseWorkList loadCaseWorklist(int ChildAssessment_Id, int User_Id)
        {
            var _db = new SDIIS_DatabaseEntities();

            var CaseWorkListId = _db.ACM_ChildAssessment.Where(x => x.ChildAssessment_Id == ChildAssessment_Id).Select(y => y.CaseWorklist_Id).FirstOrDefault();
            var Case = new ACMCaseWorkList();

            var currentUser = new User();

            #region Logged in User Details
            var userModel = new UserModel();
            currentUser = userModel.GetSpecificUser(User_Id);
            #endregion

            var model = _db.ACM_CaseWorkList.Where(x => x.CaseWoklist_Id == CaseWorkListId).FirstOrDefault();
            var socialWorker = _db.Social_Workers.Where(x => x.User_Id == currentUser.User_Id);

            Case.ACMCase = model;
            Case.Investigation = _db.ACM_Investigation.Where(x => x.CaseWorklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault() ?? new ACM_Investigation();
            Case.Investigation.ClientList = _db.Persons.Where(x => x.Person_Id == model.int_Intake_Assessment.Client.Person.Person_Id).ToList();
            //Case.Investigation.ClientList = _db.Persons.Where(x => x.Person_Id == model.int_Intake_Assessment.Client.).ToList();

            Case.ChildAssessmentInfo = _db.ACM_ChildAssessment.Where(x => x.CaseWorklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault();
            if (Case.ChildAssessmentInfo == null)
            {
                Case.ChildAssessmentInfo = new ACM_ChildAssessment();
                Case.ChildAssessmentInfo.CaseWorklist_Id = CaseWorkListId;
            }
            Case.ChildAssessmentInfo.SelectedRelationship_Type_Id = Convert.ToInt32(Case.ChildAssessmentInfo.RelationshipType_It);
            Case.ChildAssessmentInfo.SelectedEngagementTypeList_Id = Convert.ToInt32(Case.ChildAssessmentInfo.TypeOfEngagement_Id);

            Case.ChildrenCourtReportInfo = _db.ACM_ChildrensCourtReport.Where(x => x.ChildAssesment_Id == Case.ChildAssessmentInfo.ChildAssessment_Id).FirstOrDefault();

            if (Case.ChildrenCourtReportInfo != null)
            {
                //Case.ChildrenCourtReportInfo.InformationSources = _db.ACM_InformationSources.Where(x => x.ChildrensCourtReport_Id == Case.ChildrenCourtReportInfo.ChildrensCourtReport_Id).ToList();
                if (Case.ChildrenCourtReportInfo.InformationSources == null)
                {
                    //Case.ChildrenCourtReportInfo.InformationSources = new List<ACM_InformationSources>();
                }
            }
            else
            {
                Case.ChildrenCourtReportInfo = new ACM_ChildrensCourtReport();
                //Case.ChildrenCourtReportInfo.InformationSources = new List<ACM_InformationSources>();
                Case.ChildrenCourtReportInfo.ReportByUser_Id = currentUser.User_Id;
                Case.ChildrenCourtReportInfo.apl_User = _db.Users.Where(x => x.User_Id == currentUser.User_Id).FirstOrDefault();
            }

            Case.SocialHistoryInfo = _db.ACM_SocialHistory.Where(x => x.ChildAssessment_Id == Case.ChildAssessmentInfo.ChildAssessment_Id).FirstOrDefault();

            Case.MedicalFactorsInfo = _db.ACM_MedicalFactors.Where(x => x.ChildAssessment_Id == Case.ChildAssessmentInfo.ChildAssessment_Id).FirstOrDefault();
            if (Case.MedicalFactorsInfo == null)
            {
                Case.MedicalFactorsInfo = new ACM_MedicalFactors();
                Case.MedicalFactorsInfo.ChildAssessment_Id = Case.ChildAssessmentInfo.ChildAssessment_Id;
            }

            Case.InterventionPlanningInfo = _db.ACM_InterventionPlanning.Where(x => x.ChildAssesment_Id == Case.ChildAssessmentInfo.ChildAssessment_Id).FirstOrDefault();
            if (Case.InterventionPlanningInfo == null)
            {
                Case.InterventionPlanningInfo = new ACM_InterventionPlanning();
                Case.InterventionPlanningInfo.ChildAssesment_Id = Case.ChildAssessmentInfo.ChildAssessment_Id;
            }

            Case.Investigation.ProcessNotesList = _db.ACM_InterviewProcessNote.Where(x => x.CaseWorklist_Id == Case.ACMCase.CaseWoklist_Id).ToList();

            Case.SouthAfricanAssessmentToolInfo = _db.ACM_SouthAfricanSafetyAssessmentTool.Where(x => x.Caseworklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault();
            if (Case.SouthAfricanAssessmentToolInfo == null)
            {
                Case.SouthAfricanAssessmentToolInfo = new ACM_SouthAfricanSafetyAssessmentTool();
                Case.SouthAfricanAssessmentToolInfo.Caseworklist_Id = Case.ACMCase.CaseWoklist_Id;
                Case.SouthAfricanAssessmentToolInfo.Client_Id = Case.ACMCase.int_Intake_Assessment.Client.Client_Id;
                //Case.SouthAfricanAssessmentToolInfo.CaregiversAndGuardians = new List<ACM_ParentCaregiversAssessed>();
            }
            else
            {
                //Case.SouthAfricanAssessmentToolInfo.CaregiversAndGuardians = _db.ACM_ParentCaregiversAssessed.Where(x => x.Sasat_Id == Case.SouthAfricanAssessmentToolInfo.Id).ToList();
            }
            Case.SourcesOfInformationPerson = new ACMSearchViewModel();
            Case.SourcesOfInformationPerson.Person_List = new List<Person>();
            Case.MaltreatmentInfo = _db.ACM_TypeOfReportedSuspectedMaltreatment.Where(x => x.Sasat_Id == Case.SouthAfricanAssessmentToolInfo.Id).FirstOrDefault();
            if (Case.MaltreatmentInfo == null)
            {
                Case.MaltreatmentInfo = new ACM_TypeOfReportedSuspectedMaltreatment();
                Case.MaltreatmentInfo.Sasat_Id = Case.SouthAfricanAssessmentToolInfo.Id;
            }
            //Case.ActuarialRiskInfo = _db.ACM_ActuarialRiskAssessment.Where(x => x.Caseworklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault();
            if (Case.ActuarialRiskInfo == null)
            {
                Case.ActuarialRiskInfo = new ACM_ActuarialRiskAssessment();
                Case.ActuarialRiskInfo.SourcesOfInformationList = new List<ACM_AraSourcesOfInformation>();
            }
            else
            {
                //Case.ActuarialRiskInfo.SourcesOfInformationList = _db.ACM_AraSourcesOfInformation.Where(x => x.ActuarialRisk_Id == Case.ActuarialRiskInfo.Id).ToList();
                if (Case.ActuarialRiskInfo.SourcesOfInformationList == null)
                {
                    Case.ActuarialRiskInfo.SourcesOfInformationList = new List<ACM_AraSourcesOfInformation>();
                }
            }

            Case.ChildVulnerabilityInfo = _db.ACM_ChildVulnerability.Where(x => x.Sasat_Id == Case.SouthAfricanAssessmentToolInfo.Id).FirstOrDefault();
            if (Case.ChildVulnerabilityInfo == null)
            {
                Case.ChildVulnerabilityInfo = new ACM_ChildVulnerability();
                Case.ChildVulnerabilityInfo.Sasat_Id = Case.SouthAfricanAssessmentToolInfo.Id;
            }

            Case.SafetyThreatsInfo = _db.ACM_SafetyThreats.Where(x => x.Sasat_Id == Case.SouthAfricanAssessmentToolInfo.Id).FirstOrDefault();
            if (Case.SafetyThreatsInfo == null)
            {
                Case.SafetyThreatsInfo = new ACM_SafetyThreats();
                Case.SafetyThreatsInfo.Sasat_Id = Case.SouthAfricanAssessmentToolInfo.Id;
            }

            //Added by Nkanyiso
            Case.ACMIndividualDevelopmentPlan = _db.ACM_IndividualDevelopmentPlan.Where(x => x.CaseWorklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault();
            if (Case.ACMIndividualDevelopmentPlan == null)
            {
                Case.ACMIndividualDevelopmentPlan = new ACM_IndividualDevelopmentPlan();
                Case.ACMIndividualDevelopmentPlan.CaseWorklist_Id = CaseWorkListId;
            }

            //Added by Nkanyiso
            Case.ACMIDPChildrenDetails = _db.ACM_IDPChildrenDetails.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPChildrenDetails == null)
            {
                Case.ACMIDPChildrenDetails = new ACM_IDPChildrenDetails();
                Case.ACMIDPChildrenDetails.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }

            //Added  by Nkanyiso
            Case.ACMIDPParentsDetails = _db.ACM_IDPParentsDetails.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPParentsDetails == null)
            {
                Case.ACMIDPParentsDetails = new ACM_IDPParentsDetails();
                Case.ACMIDPParentsDetails.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }
            Case.ACMIDPParentsDetails.ACMIDPParentsDetails_List = _db.ACM_IDPParentsDetails.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();

            //Added by Nkanyiso
            Case.ACMIDPWellbeing = _db.ACM_IDPWellbeing.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPWellbeing == null)
            {
                Case.ACMIDPWellbeing = new ACM_IDPWellbeing();
                Case.ACMIDPWellbeing.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }

            //Added by Nkanyiso
            Case.ACMIDPDevelopmentalAreaBeloning = _db.ACM_IDPDevelopmentalAreaBeloning.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPDevelopmentalAreaBeloning == null)
            {
                Case.ACMIDPDevelopmentalAreaBeloning = new ACM_IDPDevelopmentalAreaBeloning();
                Case.ACMIDPDevelopmentalAreaBeloning.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }


            //Added  by Nkanyiso
            Case.ACMIDPDevelopmentalAreaMastery = _db.ACM_IDPDevelopmentalAreaMastery.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPDevelopmentalAreaMastery == null)
            {
                Case.ACMIDPDevelopmentalAreaMastery = new ACM_IDPDevelopmentalAreaMastery();
                Case.ACMIDPDevelopmentalAreaMastery.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }

            //Added  by Nkanyiso
            Case.ACMIDPDevelopmentalAreaIndependence = _db.ACM_IDPDevelopmentalAreaIndependence.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPDevelopmentalAreaIndependence == null)
            {
                Case.ACMIDPDevelopmentalAreaIndependence = new ACM_IDPDevelopmentalAreaIndependence();
                Case.ACMIDPDevelopmentalAreaIndependence.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }

            //Added  by Nkanyiso
            Case.ACMIDPDevelopmentalAreaGenerosity = _db.ACM_IDPDevelopmentalAreaGenerosity.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).FirstOrDefault();
            if (Case.ACMIDPDevelopmentalAreaGenerosity == null)
            {
                Case.ACMIDPDevelopmentalAreaGenerosity = new ACM_IDPDevelopmentalAreaGenerosity();
                Case.ACMIDPDevelopmentalAreaGenerosity.IndividualDevelopmentPlan_Id = Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id;
            }
            //Added by Nkanyiso

            Case.ACMCarePlan = _db.ACM_CarePlan.Where(x => x.CaseWorklist_Id == Case.ACMCase.CaseWoklist_Id).FirstOrDefault();
            if (Case.ACMCarePlan == null)
            {
                Case.ACMCarePlan = new ACM_CarePlan();
                Case.ACMCarePlan.CaseWorklist_Id = CaseWorkListId;
            }
            //  Case.Investigation.ProcessNotesList = _db.ACM_InterviewProcessNote.Where(x => x.Investigation_Id == Case.Investigation.Investigation_Id).ToList();
            Case.ACMIndividualDevelopmentPlan.IDPWellBeingList = _db.ACM_IDPWellbeing.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();
            Case.ACMIndividualDevelopmentPlan.IDPBelongList = _db.ACM_IDPDevelopmentalAreaBeloning.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();
            Case.ACMIndividualDevelopmentPlan.IDPMasteryList = _db.ACM_IDPDevelopmentalAreaMastery.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();
            Case.ACMIndividualDevelopmentPlan.IDPIndependenceList = _db.ACM_IDPDevelopmentalAreaIndependence.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();
            Case.ACMIndividualDevelopmentPlan.IDPGenerosityList = _db.ACM_IDPDevelopmentalAreaGenerosity.Where(x => x.IndividualDevelopmentPlan_Id == Case.ACMIndividualDevelopmentPlan.IndividualDevelopmentPlan_Id).ToList();

            Case.InterviewedPerson = new ACMInterviewedPersonSearchViewModel();
            Case.InterviewedPerson.Person_List = new List<Person>();

            //Get Client Famile Structure, Perants, Siblings ect.
            //We need to build some lists for the Childrens Court Report.
            var test = Case.ACMCase.int_Intake_Assessment.Client.Client_Biological_Parents;
            var test2 = Case.ACMCase.int_Intake_Assessment.Client.Client_Adoptive_Parents;
            var test3 = Case.ACMCase.int_Intake_Assessment.Client.Client_CareGivers;

            var aditionalCareGivers = new List<Person>();

            foreach (var adopt in test2)
            {
                aditionalCareGivers.Add(adopt.Person);
            }
            foreach (var care in test3)
            {
                aditionalCareGivers.Add(care.Person);
            }

            Case.AlternateCaregivers = aditionalCareGivers;

            Case.Siblings = new List<Person>();
            Case.OtherLivingWith = new List<Person>();


            //People who were consulted. People who were consulted or contributed to this assessment (this should include the child, his/her caregivers and other significant adults in his/her life)
            //var ClientFamilyMembers = Case.ACMCase.int_Intake_Assessment.Client.Client_Family_Members;
            var AdoptiveParents = Case.ACMCase.int_Intake_Assessment.Client.Client_Adoptive_Parents;
            var CareGivers = Case.ACMCase.int_Intake_Assessment.Client.Client_CareGivers;

            //Incomplete... to add client, and other significant adults in his/her life
            var PeopleWhoWereConsultedList = new List<Person>();

            foreach (var adopt in AdoptiveParents)
            {
                PeopleWhoWereConsultedList.Add(adopt.Person);
            }

            foreach (var adopt in AdoptiveParents)
            {
                PeopleWhoWereConsultedList.Add(adopt.Person);
            }
            foreach (var care in CareGivers)
            {
                PeopleWhoWereConsultedList.Add(care.Person);
            }

            Case.PeopleWhoWereConsultedList = PeopleWhoWereConsultedList;

            return Case;
        }

    }
}
