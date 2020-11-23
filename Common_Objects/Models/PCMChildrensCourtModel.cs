using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMChildrensCourtModel
    {
        public List<PCMChildrensCourtViewModel> GetPCMChildrensCourtList()
        {
            List<PCMChildrensCourtViewModel> cVM = new List<PCMChildrensCourtViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var cList = (from cc in db.PCM_Childrens_Court
                         select new { cc.Children_Court_Id, cc.PCM_Recommendation_Id, cc.Court_Expiry_Date, cc.Prelim_Enquiry_Date }).ToList();

            foreach (var item in cList)
            {
                PCMChildrensCourtViewModel objR = new PCMChildrensCourtViewModel();

                objR.Children_Court_Id = item.Children_Court_Id;
                objR.PCM_Recommendation_Id = item.PCM_Recommendation_Id;
                objR.Court_Expiry_Date = item.Court_Expiry_Date;
                objR.Prelim_Enquiry_Date = item.Prelim_Enquiry_Date;

                cVM.Add(objR);
            }

            return cVM;
        }

        public List<PCMChildrensCourtViewModel> GetPCMChildrensOutcometList()
        {
            List<PCMChildrensCourtViewModel> vm = new List<PCMChildrensCourtViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var outcomeList = (from co in db.PCM_Childrens_Court_Outcome
                               select new { co.Outcome_Id, co.Children_Court_Id, co.Court_Date, co.Remand, co.Next_Court_Date, co.Court_Outcome, co.Court_Case_Status_Id }).ToList();

            foreach (var item in outcomeList)
            {
                PCMChildrensCourtViewModel objR = new PCMChildrensCourtViewModel();

                objR.Outcome_Id = item.Outcome_Id;
                //objR.Children_Court_Id = item.Children_Court_Id;
                objR.Court_Date = item.Court_Date;
                objR.Remand = item.Remand;
                objR.Next_Court_Date = item.Next_Court_Date;
                objR.Court_Outcome = item.Court_Outcome;

                vm.Add(objR);
            }

            return vm;
        }


        public PCMChildrensCourtViewModel GetChildrensCourtOutcometById(int CourtId)
        {
            PCMChildrensCourtViewModel vm = new PCMChildrensCourtViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Childrens_Court_Outcome act = db.PCM_Childrens_Court_Outcome.Find(CourtId);
                    if (act != null)
                    {
                        vm.Outcome_Id = act.Outcome_Id;
                        vm.Court_Case_Status_Id = act.Court_Case_Status_Id;
                        vm.Placement_Type_Id = act.Placement_Type_Id;
                        vm.Recommendation_Type_Id = act.Recommendation_Type_Id;
                        vm.Court_Outcome = act.Court_Outcome;
                        vm.Intake_Assessment_Id = act.Intake_Assessment_Id;



                        vm.Next_Court_Date = act.Next_Court_Date;
                        vm.Reason_Remand= act.Reason_Remand;
                        vm.Remand = act.Remand;
                        vm.Court_Date = act.Court_Date;
                        if (act.Recommendation_Type_Id > 0)
                        {
                            vm.descrRecommendation = db.apl_Recommendation_Type.Find(act.Recommendation_Type_Id).Description;
                        }
                        if (act.Placement_Type_Id > 0)
                        {
                            vm.descrPlacement = db.apl_Placement_Type.Find(act.Placement_Type_Id).Description;
                        }
                        if (act.Court_Case_Status_Id > 0)
                        {
                            vm.descrStatusCourt = db.apl_PCM_Court_Case_Status.Find(act.Court_Case_Status_Id).Description;
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


        public void UpdatePCMChildrensCourt(PCMChildrensCourtViewModel vm,  int Children_Court_Id, int userid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    

                    PCM_Childrens_Court newCC = db.PCM_Childrens_Court.Find(Children_Court_Id);

                    newCC.Court_Expiry_Date = vm.Court_Expiry_Date;
                    newCC.Prelim_Enquiry_Date = vm.Prelim_Enquiry_Date;
                    newCC.Child_Need_Care = vm.Child_Need_Care;
                    newCC.Substance_Abuse_Treat = vm.Substance_Abuse_Treat;
                    newCC.Referral_Date = vm.Referral_Date;
                    newCC.Alternative_Placement = vm.Alternative_Placement;
                    newCC.Case_Manager = vm.Case_Manager;
                    newCC.Service_Provider = vm.Service_Provider;
                    vm.Modified_By = userid;
                    vm.Date_Modified = DateTime.Now;


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


        public void CreatePCMChildrensCourt(PCMChildrensCourtViewModel vm, int RecomendationID, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Childrens_Court newCC = new PCM_Childrens_Court();
                    newCC.PCM_Recommendation_Id = RecomendationID;
                    newCC.Intake_Assessment_Id = Intake_Assessment_Id;
                    newCC.Court_Expiry_Date = vm.Court_Expiry_Date;
                    newCC.Prelim_Enquiry_Date = vm.Prelim_Enquiry_Date;
                    newCC.Child_Need_Care = vm.Child_Need_Care;
                    newCC.Substance_Abuse_Treat = vm.Substance_Abuse_Treat;
                    //newCC.Substance_Abuse_Treat = subAbuseTreat;
                    newCC.Referral_Date = vm.Referral_Date;
                    newCC.Alternative_Placement = vm.Alternative_Placement;
                    newCC.Case_Manager = vm.Case_Manager;
                    newCC.Service_Provider = vm.Service_Provider;

                    db.PCM_Childrens_Court.Add(newCC);
                    db.SaveChanges();

                    PCM_Childrens_Court_Outcome newOutcome = new PCM_Childrens_Court_Outcome();

                    newOutcome.Intake_Assessment_Id = Intake_Assessment_Id;
          
                    newOutcome.Children_Court_Id = newCC.Children_Court_Id;

                    db.PCM_Childrens_Court_Outcome.Add(newOutcome);
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





        public PCMChildrensCourtViewModel GetPCMChildrensCourtEditDetails(int ChildrenCourtId)
        {
            PCMChildrensCourtViewModel vm = new PCMChildrensCourtViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from c in db.PCM_Childrens_Court
                               where (c.Children_Court_Id == ChildrenCourtId)
                               select c.Children_Court_Id).FirstOrDefault();

                    PCM_Childrens_Court cc = db.PCM_Childrens_Court.Find(id);
                    if (cc != null)
                    {
                        vm.Children_Court_Id = db.PCM_Childrens_Court.Find(cc.Children_Court_Id).Children_Court_Id;
                        vm.PCM_Recommendation_Id = cc.PCM_Recommendation_Id;
                        vm.Court_Expiry_Date = cc.Court_Expiry_Date;
                        vm.Prelim_Enquiry_Date = cc.Prelim_Enquiry_Date;
                        vm.Child_Need_Care = cc.Child_Need_Care;
                        vm.Substance_Abuse_Treat = cc.Substance_Abuse_Treat;
                        vm.Referral_Date = cc.Referral_Date;
                        vm.Intake_Assessment_Id = cc.Intake_Assessment_Id;

                        db.SaveChanges();
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

       
        public List<PCMChildrensCourtViewModel> GetOutcometList( int assID)
        {
            List<PCMChildrensCourtViewModel> cVM = new List<PCMChildrensCourtViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();


       

            var cList = (from cc in db.PCM_Childrens_Court
                         join outc in db.PCM_Childrens_Court_Outcome
                         on cc.Children_Court_Id equals outc.Children_Court_Id
                         where cc.Intake_Assessment_Id == assID
                         select new
                         {
                             outc.Outcome_Id,
                             outc.Court_Date,
                             outc.Remand,
                             outc.Reason_Remand,
                             outc.Next_Court_Date,
                             outc.Court_Outcome,
                             outc.Intake_Assessment_Id,
                             
                             outc.Children_Court_Id

                         }).ToList();

            foreach (var item in cList)
            {
                PCMChildrensCourtViewModel objR = new PCMChildrensCourtViewModel();

                objR.Outcome_Id = item.Outcome_Id;
                objR.Court_Date = item.Court_Date;
                objR.Remand = item.Remand;
                objR.Reason_Remand = item.Reason_Remand;
                objR.Next_Court_Date = item.Next_Court_Date;
                objR.Court_Outcome = item.Court_Outcome;
                objR.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objR.Court_Expiry_Date=db.PCM_Childrens_Court.Find(item.Children_Court_Id).Court_Expiry_Date;
                objR.Children_Court_Id = Convert.ToInt32( item.Children_Court_Id);

                cVM.Add(objR);
            }

            return cVM;
        }


        public List<PCMChildrensCourtViewModel> GetCourtList(int Intake_Assessment_Id)
        {
            List<PCMChildrensCourtViewModel> cVM = new List<PCMChildrensCourtViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var cList = (from cc in db.PCM_Childrens_Court
                         select new
                         {
                             cc.Children_Court_Id,
                             cc.Court_Expiry_Date,
                             cc.Child_Need_Care,
                             cc.Referral_Date,
                             cc.Service_Provider,
                             cc.Substance_Abuse_Treat,
                             cc.Intake_Assessment_Id,
                             cc.Alternative_Placement
                         }).ToList();

            foreach (var item in cList)
            {
                PCMChildrensCourtViewModel objR = new PCMChildrensCourtViewModel();

                objR.Children_Court_Id = item.Children_Court_Id;
                objR.Court_Expiry_Date = item.Court_Expiry_Date;
                objR.Child_Need_Care = item.Child_Need_Care;
                objR.Referral_Date = item.Referral_Date;
                objR.Service_Provider = item.Service_Provider;
                objR.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objR.Substance_Abuse_Treat = item.Substance_Abuse_Treat;
                objR.Alternative_Placement = item.Alternative_Placement;

                cVM.Add(objR);
            }

            return cVM; 
        }




        public void CreatePCMChildrensCourtOutcome(PCMChildrensCourtViewModel vm, int RecomendationID, int Outcome_Id, int assID)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Childrens_Court_Outcome newOutcome = db.PCM_Childrens_Court_Outcome.Find(Outcome_Id);
                    newOutcome.Court_Date = vm.Court_Date;
                    newOutcome.Remand = vm.Remand;
                    newOutcome.Reason_Remand = vm.Reason_Remand;
                    newOutcome.Next_Court_Date = vm.Next_Court_Date;
                    newOutcome.Court_Outcome = vm.Court_Outcome;
                    newOutcome.Placement_Type_Id = vm.Type_Of_Placement_Id;
                    newOutcome.Recommendation_Type_Id = vm.Recommendation_Type_Id;
                    newOutcome.Court_Case_Status_Id = vm.Court_Case_Status_Id;


                    db.SaveChanges();

                   

                    PCM_HB_Supervision exist1 = db.PCM_HB_Supervision.OrderByDescending(x => x.HomeBasedSupervision_Id).FirstOrDefault(x => x.Outcome_Id == Outcome_Id);
                    if (newOutcome.Placement_Type_Id == 187 && exist1== null)
                 
                    {

                        PCM_HB_Supervision newhomebase = new PCM_HB_Supervision();

                        newhomebase.Outcome_Id = newOutcome.Outcome_Id;
                        newhomebase.Intake_Assessment_Id = assID;
                        newhomebase.Court_Type_Id = 3;

                        db.PCM_HB_Supervision.Add(newhomebase);
                        db.SaveChanges();


                    }

                    PCM_Diversion exist12 = db.PCM_Diversion.OrderByDescending(x => x.Diversion_Id).FirstOrDefault(x => x.Childrens_Court_Outcome_Id == Outcome_Id);
                    if (newOutcome.Recommendation_Type_Id == 1 && exist12 == null)

                    {

                        PCM_Diversion newhomebase = new PCM_Diversion();

                        newhomebase.Childrens_Court_Outcome_Id = newOutcome.Outcome_Id;
                        newhomebase.Intake_Assessment_Id = assID;
                        newhomebase.Court_Type_Id = 3;

                        db.PCM_Diversion.Add(newhomebase);
                        db.SaveChanges();


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
            }
        }

        public List<CourtOutcomeCaseStatusTypeLookup> GetCourtOutcomeCaseStatusType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Status_Type = db.apl_PCM_Court_Case_Status.Select(o => new CourtOutcomeCaseStatusTypeLookup
                {
                    CourtOutcome_CaseStatus_ID = o.Court_Case_Status_Id,
                    CourtOutcome_CaseStatus = o.Description
                }).ToList();

                return Status_Type;
            }
        }

        public PCMChildrensCourtViewModel GetCCDetails(int Id)
        {
            PCMChildrensCourtViewModel vm = new PCMChildrensCourtViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            //var cc = db.PCM_Childrens_Court.Find(Id);
            PCM_Childrens_Court model = db.PCM_Childrens_Court.Where(x => x.Intake_Assessment_Id == Id).SingleOrDefault();
            vm.Children_Court_Id = model.Children_Court_Id;
            vm.Court_Expiry_Date = model.Court_Expiry_Date;
            vm.Prelim_Enquiry_Date = model.Prelim_Enquiry_Date;
            //vm.Child_Need_Care = model.Child_Need_Care;
            vm.Alternative_Placement = model.Alternative_Placement;
            //vm.kidAddress = gd.kidAddress;

            return vm;
        }

        public PCMChildrensCourtViewModel GetCCOutComeByID(int Outcome_Id)
        {
            PCMChildrensCourtViewModel vm = new PCMChildrensCourtViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from c in db.PCM_Childrens_Court_Outcome
                               where (c.Intake_Assessment_Id == Outcome_Id)
                               select c.Intake_Assessment_Id).FirstOrDefault();

                    PCM_Childrens_Court_Outcome cc = db.PCM_Childrens_Court_Outcome.Find(id);
                    if (cc != null)
                    {
                        vm.Intake_Assessment_Id = db.PCM_Childrens_Court_Outcome.Find(cc.Intake_Assessment_Id).Intake_Assessment_Id;
                        vm.Outcome_Id = cc.Outcome_Id;
                        vm.Intake_Assessment_Id = cc.Intake_Assessment_Id;
                        //vm.barcode = ass.barcode;
                        //vm.serial = ass.serial;
                        //vm.reason = ass.reason;
                        //vm.official = ass.official;
                        //vm.datereceived = ass.datereceived;
                        //vm.datereturned = ass.datereturned;

                        db.SaveChanges();
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
