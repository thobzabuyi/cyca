using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Common_Objects.Models
{
    public class PCMPreliminaryModel
    {
        //string cs = "data source=.;initial catalog=SDIIS_Database_Tes;Integrated Security=True";

        public List<PCMPreliminaryViewModel> gridList()
        {
            List<PCMPreliminaryViewModel> pvm = new List<PCMPreliminaryViewModel>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var pList = (from d in db.PCM_Preliminary_Details
                         join s in db.PCM_Preliminary_Status
                         on d.PCM_Preliminary_Status_Id equals s.Preliminary_Status_Id
                         join r in db.PCM_Preliminary_Recommendation
                         on d.PCM_Recommendation_Id equals r.PCM_Recommendation_Id
                         //where (pcmPre.Client_Id == 27890)
                         select new
                         {
                             d.PCM_Preliminary_Id,
                             d.PCM_Case_Id,
                             d.Client_Id,
                             d.Intake_Assessment_Id,
                             d.PCM_Preliminary_Date,
                             d.PCM_Outcome_Reason,
                             d.PCM_Offence_Id,
                             d.PCM_Recommendation_Id,
                             r.Recommendation,
                             s.Preliminary_Status_Id,
                             s.Preliminary_Status



                         }).ToList();

            foreach (var item in pList)
            {
                PCMPreliminaryViewModel pre = new PCMPreliminaryViewModel();
                pre.PCM_Preliminary_Id = item.PCM_Preliminary_Id;
                pre.PCM_Case_Id = item.PCM_Case_Id;
                pre.Client_Id = item.Client_Id;
                pre.Intake_Assessment_Id = item.Intake_Assessment_Id;
                pre.PCM_Outcome_Reason = item.PCM_Outcome_Reason;
                pre.PCM_Offence_Id = item.PCM_Offence_Id;
                pre.PCM_Recommendation_Id = item.PCM_Recommendation_Id;
                pre.PCM_Preliminary_Status_Id = item.Preliminary_Status_Id;
                pre.Preliminary_Status = item.Preliminary_Status;
                pre.Recommendation = item.Recommendation;

                pvm.Add(pre);
            }
            return pvm;
        }


        public List<PCMPreliminaryViewModel> GetRecommendation()
        {
            List<PCMPreliminaryViewModel> vm = new List<PCMPreliminaryViewModel>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var rRec = (from r in db.PCM_Recommendation
                        join o in db.PCM_Orders
                        on r.Recommendation_Id equals o.Recommendation_Id
                        join p in db.PCM_Placement
                        on r.Recommendation_Id equals p.Recommendation_Id

                        select new
                        {
                            r.Recommendation_Id,
                            r.Recommendation_Type_Id,
                            r.Placement_Type_Id,
                            r.Comments_For_Recommendation,
                            r.Intake_Assessment_Id,
                            o.Recomendation_Order_Id,
                            p.Person_Id,
                            p.Type_Of_Center_Id,
                            p.Facility_Id,
                        }).ToList();

            foreach (var item in rRec)
            {
                PCMPreliminaryViewModel pvm = new PCMPreliminaryViewModel();
                pvm.Recommendation_Id = item.Recommendation_Id;
                pvm.Recommendation_Type_Id = item.Recommendation_Type_Id;
                pvm.Placement_Type_Id = item.Placement_Type_Id;
                pvm.Comments_For_Recommendation = item.Comments_For_Recommendation;
                pvm.Intake_Assessment_Id = item.Intake_Assessment_Id;
                pvm.Recomendation_Order_Id = item.Recomendation_Order_Id;
                pvm.Person_Id = item.Person_Id;
                pvm.Type_Of_Center_Id = item.Type_Of_Center_Id;
                pvm.Facility_Id = item.Facility_Id;

                vm.Add(pvm);
            }
            return vm;
        }


        public List<PCMPreliminaryViewModel> GetOffence()
        {
            List<PCMPreliminaryViewModel> vm = new List<PCMPreliminaryViewModel>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var oList = (from OL in db.PCM_Offence_Details
                         join OC in db.Offence_Categories
                         on OL.Offence_Category_Id equals OC.Offence_Category_Id
                         join CD in db.PCM_Case_Details
                         on OL.PCM_Case_Id equals CD.PCM_Case_Id
                         join c in db.Courts
                         on CD.Court_id equals c.Court_Id
                         select new
                         {
                             OL.PCM_Offence_Id,
                             OL.PCM_Case_Id,
                             OL.Intake_Assessment_Id,
                             OL.Offence_Category_Id,
                             //OL.Charge_Id,
                             OL.Offence_Circumstance,
                             OL.Value_Of_Goods,
                             OL.Value_Recovered,
                             OL.IsChild_Responsible,
                             OL.Responsibility_Details,
                             //OC.Description,
                             OC.Source,
                             OC.Definition,
                             CD.Date_Arrested,
                             CD.Investigate_Officer_Name,
                             CD.Court_id,
                             c.Description
                         }).ToList();

            foreach (var item in oList)
            {
                PCMPreliminaryViewModel objO = new PCMPreliminaryViewModel();
                objO.PCM_Offence_Id = item.PCM_Offence_Id;
                objO.PCM_Case_Id = item.PCM_Case_Id;
                objO.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objO.Date_Arrested = item.Date_Arrested;
                objO.Investigate_Officer_Name = item.Investigate_Officer_Name;
                objO.Description = item.Description;

                vm.Add(objO);
            }
            return vm;
        }


        //Method for Updating Employee record  

        public void Add(PCMPreliminaryViewModel vm, int userId, int assID)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Preliminary_Details newPre = new PCM_Preliminary_Details();
            
                    
                   
                    newPre.Intake_Assessment_Id = assID;
                    newPre.PCM_Preliminary_Status_Id = vm.PCM_Preliminary_Status_Id;
                    newPre.PreInquiryConducted = vm.PreInquiryConducted;
                    newPre.ReasonPreInquiryConducted = vm.ReasonPreInquiryConducted;
                    newPre.PCM_Preliminary_Date = vm.PCM_Preliminary_Date;
                    newPre.PCM_Outcome_Reason = vm.PCM_Outcome_Reason;
                    newPre.PCM_Offence_Id = vm.PCM_Offence_Id;
                    newPre.PCM_Recommendation_Id = vm.PCM_Recommendation_Id;
                    newPre.Modified_By = userId;
                    newPre.Date_Modified = DateTime.Now;
                    newPre.Modified_By = userId;
                    newPre.Date_Created = DateTime.Now;

                    db.PCM_Preliminary_Details.Add(newPre);
                    db.SaveChanges();
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        public int GetPCMPreliminaryIdDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Intake_Assessments 
                        join Case in db.PCM_Preliminary_Details on p.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where p.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.PCM_Preliminary_Id).FirstOrDefault();
            }
        }

        public PCMPreliminaryViewModel Display(int id)
        {
            PCMPreliminaryViewModel vm = new PCMPreliminaryViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var pdata = db.PCM_Preliminary_Details.Find(id);

           
            // Updated by Yanga...........................
            if (pdata.Intake_Assessment_Id != null)
            {
                vm.Intake_Assessment_Id = pdata.Intake_Assessment_Id;
            }

           
            if (pdata.PCM_Preliminary_Date != null)
            {
                vm.PCM_Preliminary_Date = pdata.PCM_Preliminary_Date;
            }
           
            if (pdata.PCM_Preliminary_Status_Id != null)
            {
                vm.Preliminary_Status_Id = pdata.PCM_Preliminary_Status_Id;
            }
           
            if (pdata.PCM_Outcome_Reason != null)
            {
                vm.PCM_Outcome_Reason = pdata.PCM_Outcome_Reason;
            }
           
            if (pdata.PCM_Recommendation_Id != null)
            {
                vm.PCM_Recommendation_Id = pdata.PCM_Recommendation_Id;
            }

            if (pdata.Placement_Pre_Recommended_Id != null)
            {
                vm.Placement_Type_Id = pdata.Placement_Pre_Recommended_Id;
            }
            

            return vm;
        }

        public PCMPreliminaryViewModel Update(PCMPreliminaryViewModel pre,int userId, int PcmPreliID, int AssID)
        {
            PCMPreliminaryViewModel vm = new PCMPreliminaryViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
               
                    PCM_Preliminary_Details fc = db.PCM_Preliminary_Details.Find(PcmPreliID);
                    if (fc != null)
                    {
                       
                        fc.PCM_Recommendation_Id= pre.PCM_Recommendation_Id;
                        fc.PCM_Preliminary_Status_Id= pre.Preliminary_Status_Id;
                        fc.Placement_Pre_Recommended_Id = pre.Placement_Type_Id;
                        fc.PCM_Preliminary_Date = pre.PCM_Preliminary_Date;
                        fc.PCM_Outcome_Reason = pre.PCM_Outcome_Reason;

                        fc.Date_Modified = pre.Date_Modified;
                        fc.Modified_By = pre.Modified_By;
                        fc.Date_Created = pre.Date_Created;
                        db.SaveChanges();

                        PCM_HB_Supervision exist1 = db.PCM_HB_Supervision.OrderByDescending(x => x.HomeBasedSupervision_Id).FirstOrDefault(x => x.PCM_Preliminary_Id == PcmPreliID);

                        //PCM_HB_Supervision exist1 = db.PCM_HB_Supervision.Where(x => x.PCM_Preliminary_Id == PcmPreliID).SingleOrDefault();
                        if (fc.Placement_Pre_Recommended_Id == 187 && exist1 == null)
                        {


                            PCM_HB_Supervision add = new PCM_HB_Supervision();


                            add.PCM_Preliminary_Id = PcmPreliID;
                            add.Intake_Assessment_Id = AssID;
                            add.Court_Type_Id = 1;

                            db.PCM_HB_Supervision.Add(add);
                            db.SaveChanges();


                        }

                        PCM_Diversion exist12 = db.PCM_Diversion.OrderByDescending(x => x.Diversion_Id).FirstOrDefault(x => x.PCM_Preliminary_Id == PcmPreliID);

                        if (fc.PCM_Preliminary_Id == 1 && exist12 == null)

                        {

                            PCM_Diversion newhomebase = new PCM_Diversion();

                            newhomebase.PCM_Preliminary_Id = fc.PCM_Preliminary_Id;
                            newhomebase.Intake_Assessment_Id = AssID;
                            newhomebase.Court_Type_Id = 1;

                            db.PCM_Diversion.Add(newhomebase);
                            db.SaveChanges();

                        }


                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

                return vm;
            }
        }



        public List<StatusTypeLookup> GetStatusType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Status_Type = db.PCM_Preliminary_Status.Select(o => new StatusTypeLookup
                {
                    Preliminary_Status_Id = o.Preliminary_Status_Id,
                    Preliminary_Status = o.Preliminary_Status
                }).ToList();

                return Status_Type;
            }
        }


        public List<RecommendationTypeLookup> GetRecommendationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Recommendation_Type = db.apl_Recommendation_Type.Select(o => new RecommendationTypeLookup
                {
                    PCM_Recommendation_Id = o.Recommendation_Type_Id,
                    Recommendation = o.Description
                }).ToList();

                return Recommendation_Type;
            }
        }

        public List<PlacementRecomendationTypeLookupPcm> GetPlacementRecomendation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var P = db.apl_Placement_Type.Select(o => new PlacementRecomendationTypeLookupPcm
                {
                    Placement_Type_Id = o.Placement_Type_Id,
                    Description = o.Description
                }).ToList();

                return P;
            }
        }


        public List<PreliminaryStatusTypeLookupPcm> GetPreliminaryStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var P = db.apl_PCM_Preliminary_Status.Select(o => new PreliminaryStatusTypeLookupPcm
                {
                    Preliminary_Status_Id = o.Preliminary_Status_Id,
                    Description = o.Description
                }).ToList();

                return P;
            }
        }


        public int? GetId(int IntakeAssessmentId)
        {
            PCMPreliminaryViewModel vm = new PCMPreliminaryViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var prem = db.PCM_Preliminary_Details.Where(o => o.Intake_Assessment_Id == IntakeAssessmentId);
                int? xID = prem.FirstOrDefault().PCM_Recommendation_Id;
                return xID;
            }
        }

        public int? GetPlacementId(int IntakeAssessmentId)
        {
            PCMPreliminaryViewModel vm = new PCMPreliminaryViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var prem = db.PCM_Preliminary_Details.Where(o => o.Intake_Assessment_Id == IntakeAssessmentId);
                int? xID = prem.FirstOrDefault().Placement_Pre_Recommended_Id;
                return xID;
            }
        }


    }
}
