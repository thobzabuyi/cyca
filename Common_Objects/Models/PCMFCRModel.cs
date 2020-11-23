using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMFCRModel
    {
        #region formal court

        public List<PCMFCRViewModel> GetPCMFormalCourtList(int assID)
        {
            List<PCMFCRViewModel> vm = new List<PCMFCRViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var fList = (from p in db.PCM_FCR_Proceeding
                         join c in db.Courts
                         on p.Formal_Court_Id equals c.Court_Id
                         where p.Intake_Assessment_Id == assID
                         select new {p.FormalCourt_Id, p.Intake_Assessment_Id, p.Formal_Court_Id, p.Appearance_Date, c.Description }).ToList();

            foreach(var item in fList)
            {
                PCMFCRViewModel obj = new PCMFCRViewModel();

                obj.FormalCourt_Id = item.FormalCourt_Id;
                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Court_Id = item.Formal_Court_Id;
                obj.Appearance_Date = item.Appearance_Date;
                obj.Court_Name = item.Description;

                vm.Add(obj);
            }

            return vm;
        }

        public List<CourtsTypeLookup> GetCourtsType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Courts_Type = db.Courts.Select(o => new CourtsTypeLookup
                {
                    Court_Id = o.Court_Id,
                    Court_Name = o.Description
                }).ToList();

                return Courts_Type;
            }
        }

        public void AddProceeding(PCMFCRViewModel pVM, int Intake_Assessment_Id,int  userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_FCR_Proceeding newProceeding = new PCM_FCR_Proceeding();
                    newProceeding.Intake_Assessment_Id = Intake_Assessment_Id;
                    newProceeding.Formal_Court_Id = pVM.Court_Id;
                    newProceeding.Appearance_Date = pVM.Appearance_Date;
                   
                    db.PCM_FCR_Proceeding.Add(newProceeding);
                    db.SaveChanges();

                    PCM_FCR_Outcome newOutcome = new PCM_FCR_Outcome();
                    newOutcome.Intake_Assessment_Id = Intake_Assessment_Id;
                    newOutcome.FormalCourt_Id = newProceeding.FormalCourt_Id;

                    db.PCM_FCR_Outcome.Add(newOutcome);
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

        public void UpdateProceeding(PCMFCRViewModel vm, int FormalCourt_Id, int Intake_Assessment_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_FCR_Proceeding pp = db.PCM_FCR_Proceeding.Find(FormalCourt_Id);
                    pp.Intake_Assessment_Id = Intake_Assessment_Id;
                    pp.Formal_Court_Id = vm.Court_Id;
                    pp.Appearance_Date = vm.Appearance_Date;
                    
                    
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public PCMFCRViewModel GetFormalCourtById(int CourtId)
        {
            PCMFCRViewModel vm = new PCMFCRViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_FCR_Outcome act = db.PCM_FCR_Outcome.Find(CourtId);
                    if (act != null)
                    {
                        vm.FormalCourtOutcome_Id = act.FormalCourtOutcome_Id;
                        vm.FormalCaseStatus_Id = act.FormalCaseStatus_Id;
                        vm.Placement_Type_Id = act.Placement_Type_Id;
                        vm.Recommendation_Type_Id = act.Recommendation_Type_Id;
                        vm.CourtOutcome = act.CourtOutcome;
                        


                        vm.NextCourtDate = act.NextCourtDate;
                        vm.RemandReason = act.RemandReason;
                        vm.Remand = act.Remand;
                        vm.CourtDate = act.CourtDate;
                        if (act.Recommendation_Type_Id > 0)
                        {
                            vm.descrRecommendation = db.apl_Recommendation_Type.Find(act.Recommendation_Type_Id).Description;
                        }
                        if (act.Placement_Type_Id > 0)
                        {
                            vm.descrPlacement = db.apl_Placement_Type.Find(act.Placement_Type_Id).Description;
                        }
                        if (act.FormalCaseStatus_Id > 0)
                        {
                            vm.descrStatusCourt = db.apl_PCM_Court_Case_Status.Find(act.FormalCaseStatus_Id).Description;
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

        #endregion

        public List<PCMFCRViewModel> GetPCMFCRoutcome(int assID)
        {
            List<PCMFCRViewModel> vm = new List<PCMFCRViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var outList = (from o in db.PCM_FCR_Outcome
                           
                           join r in db.PCM_FCR_Proceeding on o.FormalCourt_Id  equals r.FormalCourt_Id
                           join co in db.Courts on r.Formal_Court_Id equals co.Court_Id
                           where o.Intake_Assessment_Id == assID
                           select new { o.FormalCourtOutcome_Id, o.Intake_Assessment_Id,
                               o.CourtDate, o.Remand, o.RemandReason, o.NextCourtDate, o.CourtOutcome,
                             
                               r.FormalCourt_Id,
                               co.Court_Id
                           }).ToList();

            foreach(var item in outList)
            {
                PCMFCRViewModel obj = new PCMFCRViewModel();

                obj.FormalCourtOutcome_Id = item.FormalCourtOutcome_Id;
              
                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.CourtDate = item.CourtDate;
                obj.Remand = item.Remand;
                obj.RemandReason = item.RemandReason;
                obj.NextCourtDate = item.NextCourtDate;
                obj.CourtOutcome = item.CourtOutcome;
                obj.FormalCourt_Id = item.FormalCourt_Id;
                obj.Appearance_Date = db.PCM_FCR_Proceeding.Find(item.FormalCourt_Id).Appearance_Date;
                obj.Court_Name = db.Courts.Find(item.Court_Id).Description;
                vm.Add(obj);
            }

            return vm;
        }

        public List<CaseTypeLookup> GetCaseStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Case_Type = db.apl_PCM_Court_Case_Status.Select(o => new CaseTypeLookup
                {
                    FormalCaseStatus_Id = o.Court_Case_Status_Id,
                    FormalCaseStatus = o.Description
                }).ToList();

                return Case_Type;
            }
        }

        public List<CaseTypeLookup> GetCaseType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Case_Type = db.apl_PCM_Court_Case_Status.Select(o => new CaseTypeLookup
                {
                    FormalCaseStatus_Id = o.Court_Case_Status_Id,
                    FormalCaseStatus = o.Description
                }).ToList();

                return Case_Type;
            }
        }

        public void AddOutcome(PCMFCRViewModel pVM, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_FCR_Outcome newOutcome = new PCM_FCR_Outcome();
                    newOutcome.Intake_Assessment_Id = Intake_Assessment_Id;
                    newOutcome.CourtDate = pVM.CourtDate;
                    newOutcome.Remand = pVM.Remand;
                    newOutcome.RemandReason = pVM.RemandReason;
                    newOutcome.NextCourtDate = pVM.NextCourtDate;
                    newOutcome.CourtOutcome = pVM.CourtOutcome;
                    newOutcome.FormalCaseStatus_Id = pVM.FormalCaseStatus_Id;

                    db.PCM_FCR_Outcome.Add(newOutcome);
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

        public void UpdateOutcome(PCMFCRViewModel vm, int FormalCourtOutcome_Id, int assID)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_FCR_Outcome pp = db.PCM_FCR_Outcome.Find(FormalCourtOutcome_Id);
                    pp.CourtDate = vm.CourtDate;
                    pp.Remand = vm.Remand;
                    pp.RemandReason = vm.RemandReason;
                    pp.NextCourtDate = vm.NextCourtDate;
                    pp.CourtOutcome = vm.CourtOutcome;
                    pp.FormalCaseStatus_Id = vm.FormalCaseStatus_Id;
                    pp.Placement_Type_Id = vm.Placement_Type_Id;
                    pp.Recommendation_Type_Id = vm.PCM_Recommendation_Id;

                    db.SaveChanges();


                    PCM_HB_Supervision exist1 =db.PCM_HB_Supervision.OrderByDescending(x => x.HomeBasedSupervision_Id).FirstOrDefault(x => x.FormalCourtOutcome_Id == FormalCourtOutcome_Id);


                    //PCM_HB_Supervision exist1 = db.PCM_HB_Supervision.OrderByDescending.Where(x => x.FormalCourtOutcome_Id == FormalCourtOutcome_Id).FirstOrDefault();

                    if (pp.Placement_Type_Id == 187 && exist1 == null)

                    {

                        PCM_HB_Supervision newhomebase = new PCM_HB_Supervision();

                        newhomebase.FormalCourtOutcome_Id = pp.FormalCourtOutcome_Id;
                        newhomebase.Intake_Assessment_Id = assID;
                        newhomebase.Court_Type_Id = 2;

                        db.PCM_HB_Supervision.Add(newhomebase);
                        db.SaveChanges();


                    }

                    PCM_Diversion exist12 = db.PCM_Diversion.OrderByDescending(x => x.Diversion_Id).FirstOrDefault(x => x.Formal_Courtcome_Id == FormalCourtOutcome_Id);
                    //PCM_Diversion exist12 = db.PCM_Diversion.Where(x => x.Formal_Courtcome_Id == FormalCourtOutcome_Id).FirstOrDefault();

                    if (pp.Recommendation_Type_Id == 1 && exist12 == null)

                    {

                        PCM_Diversion newhomebase = new PCM_Diversion();

                        newhomebase.Formal_Courtcome_Id = pp.FormalCourtOutcome_Id;
                        newhomebase.Intake_Assessment_Id = assID;
                        newhomebase.Court_Type_Id = 2;

                        db.PCM_Diversion.Add(newhomebase);
                        db.SaveChanges();

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int? GetPlacementId(int IntakeAssessmentId)
        {
            PCMPreliminaryViewModel vm = new PCMPreliminaryViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var prem = db.PCM_Preliminary_Details.Where(o => o.Intake_Assessment_Id == IntakeAssessmentId);
                int? xID = prem.FirstOrDefault().PCM_Recommendation_Id;
                return xID;
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
    }
}
