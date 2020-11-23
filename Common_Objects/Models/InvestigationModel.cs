using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class InvestigationModel
    {
        public ACM_Investigation CreateACMInvestigation(int? pCaseWorklist_Id, DateTime? pDateOfInterview, Int32 pInterviewType_Id, string pPurposeOfInterview, string pProcess, string pEvaluation, string pPlanOfAction, int? pSocialWorker_Id)
        {
            ACM_Investigation newACMInvestigation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var acminvestigation = new ACM_Investigation()
                {
                    CaseWorklist_Id = pCaseWorklist_Id
                    //,
                    //DateOfInterview = pDateOfInterview,
                    //InterviewType_Id = pInterviewType_Id,
                    //PurposeOfInterview = pPurposeOfInterview,
                    //Process = pProcess,
                    //Evaluation = pEvaluation,
                    //PlanOfAction = pPlanOfAction,
                    //SocialWorker_Id = pSocialWorker_Id
                };

                try
                {
                    newACMInvestigation = dbContext.ACM_Investigation.Add(acminvestigation);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return null;
                }
                return newACMInvestigation;
            }
        }



        public ACM_Investigation EditInvestigation(int pInvestigationId, int? pCaseWorklist_Id, DateTime? pDateOfInterview, Int32 pInterviewType_Id, string pPurposeOfInterview, string pProcess, string pEvaluation, string pPlanOfAction, int? pSocialWorker_Id)
        {
            ACM_Investigation editInvestgation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editInvestgation = (from inv in dbContext.ACM_Investigation
                                        where inv.Investigation_Id.Equals(pInvestigationId)
                                        select inv).FirstOrDefault();

                    if (editInvestgation == null) return null;
                    // string pProcess, string pEvaluation, string pPlanOfAction, int? pSocialWorker_Id)
                    editInvestgation.CaseWorklist_Id = pCaseWorklist_Id;
                    //editInvestgation.DateOfInterview = pDateOfInterview;
                    //editInvestgation.PurposeOfInterview = pPurposeOfInterview;
                    //editInvestgation.Process = pProcess;
                    //editInvestgation.Evaluation = pEvaluation;
                    //editInvestgation.PlanOfAction = pPlanOfAction;
                    //editInvestgation.SocialWorker_Id = pSocialWorker_Id;


                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editInvestgation;
        }


        public ACM_Investigation GetSpecificInvestigation(int investigationId)
        {
            ACM_Investigation aCMInvestigation;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var aCMInvstigations = (from Inv in dbContext.ACM_Investigation
                                        where Inv.Investigation_Id.Equals(investigationId)
                                        select Inv).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                aCMInvestigation = (from Inv in aCMInvstigations
                                    select Inv).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return aCMInvestigation;
        }


        public List<ACM_Investigation> GetListOfInvestigations()
        {
            List<ACM_Investigation> listOfInvestigations;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var investigations = (from e in dbContext.ACM_Investigation
                                      select e).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfInvestigations = (from e in investigations
                                        select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfInvestigations;

        }
    }

}
