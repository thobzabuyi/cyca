using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMReferralsModel
    {
        //string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
        #region PCM_Referrals
        public List<PCMReferralsViewModel> GetPCMReferralsList(int Intake_Assessment_Id)
        {
            List<PCMReferralsViewModel> rVM = new List<PCMReferralsViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var rList = (from pf in db.PCM_Referrals
                         join rt in db.apl_Referral_Type
                         on pf.Type_Referral_Id equals rt.Type_Referral_Id
                         where pf.Intake_Assessment_Id == Intake_Assessment_Id
                         select new { pf.Referrals_Id, pf.Intake_Assessment_Id, pf.theClerk, pf.theParticular, pf.Type_Referral_Id, rt.Type_Referral }).ToList();

            foreach(var item in rList)
            {
                PCMReferralsViewModel objR = new PCMReferralsViewModel();

                objR.Referrals_Id = item.Referrals_Id;
                objR.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objR.theClerk = item.theClerk;
                objR.theParticular = item.theParticular;
                objR.Type_Referral_Id = item.Type_Referral_Id;
                objR.Type_Referral = item.Type_Referral;

                rVM.Add(objR);
            }

            return rVM;
        }

        public int GetreftypeByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from pf in db.PCM_Referrals
                         join rt in db.apl_Referral_Type
                         on pf.Type_Referral_Id equals rt.Type_Referral_Id
                         where pf.Intake_Assessment_Id == intAssessmentId
                        select rt.Type_Referral_Id).FirstOrDefault();
            }
        }

        public int? GetTypeReferralId(int IntakeAssessmentId)
        {
            PCMReferralsViewModel vm = new PCMReferralsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.PCM_Referrals
                     
                        where p.Intake_Assessment_Id.Equals(IntakeAssessmentId)
                        select p.Type_Referral_Id).FirstOrDefault();
            }

        }

        public List<ReferralTypeLookup> GetReferralType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Referrral_Type = db.apl_Referral_Type.Select(o => new ReferralTypeLookup
                {
                    Type_Referral_Id = o.Type_Referral_Id,
                    Type_Referral = o.Type_Referral
                }).ToList();

                return Referrral_Type;
            }
        }

        public void SaveReferralsData(PCMReferralsViewModel vM , int userid, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referrals ReferralNew = new PCM_Referrals();
                    ReferralNew.Client_Employee_Details_ID = vM.Client_Employee_Details_ID;
                    ReferralNew.Intake_Assessment_Id = Intake_Assessment_Id;
                    ReferralNew.Type_Referral_Id = vM.Type_Referral_Id;
                    ReferralNew.theClerk = vM.theClerk;
                    ReferralNew.theParticular = vM.theParticular;
                    ReferralNew.Created_By = userid;
                    ReferralNew.Date_Created = DateTime.Now;

                    db.PCM_Referrals.Add(ReferralNew);
                    db.SaveChanges();


                    if (ReferralNew.Type_Referral_Id == 1)
                    {

                        PCM_Referral_To_Court nu = new PCM_Referral_To_Court();
                        nu.Referrals_Id = ReferralNew.Referrals_Id;
                        nu.Intake_Assessment_Id = Intake_Assessment_Id;
                        nu.Type_Referral_Id = vM.Type_Referral_Id;
                        nu.Created_By = userid;
                        nu.Date_Created = DateTime.Now;

                        db.PCM_Referral_To_Court.Add(nu);
                        db.SaveChanges();


                    }
                    else if(ReferralNew.Type_Referral_Id == 2)
                    {

                        PCM_Referral_To_Programme nu = new PCM_Referral_To_Programme();
                        nu.Referrals_Id = ReferralNew.Referrals_Id;
                        nu.Intake_Assessment_Id = Intake_Assessment_Id;
                        nu.Type_Referral_Id = vM.Type_Referral_Id;
                        nu.Created_By = userid;
                        nu.Date_Created = DateTime.Now;

                        db.PCM_Referral_To_Programme.Add(nu);
                        db.SaveChanges();
                    }
                    else if (ReferralNew.Type_Referral_Id == 3)
                    {
                        PCM_Referral_Counselling nu = new PCM_Referral_Counselling();
                        nu.Referrals_Id = ReferralNew.Referrals_Id;
                        nu.Intake_Assessment_Id = Intake_Assessment_Id;
                        nu.Type_Referral_Id = vM.Type_Referral_Id;
                        nu.Created_By = userid;
                        nu.Date_Created = DateTime.Now;

                        db.PCM_Referral_Counselling.Add(nu);
                        db.SaveChanges();

                    }
                    else if (ReferralNew.Type_Referral_Id == 4)
                    {
                        PCM_Referral_SocilaWorker nu = new PCM_Referral_SocilaWorker();
                        nu.Referrals_Id = ReferralNew.Referrals_Id;
                        nu.Intake_Assessment_Id = Intake_Assessment_Id;
                        nu.Type_Referral_Id = vM.Type_Referral_Id;
                        nu.Created_By = userid;
                        nu.Date_Created = DateTime.Now;

                        db.PCM_Referral_SocilaWorker.Add(nu);
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


        public PCMReferralsViewModel ViewReferralData(int Referrals_Id = 1011)
        {
            PCMReferralsViewModel vm = new PCMReferralsViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var gd = db.PCM_Referrals.Find(Referrals_Id);
                    vm.theClerk = gd.theClerk;
                    vm.theParticular = gd.theParticular;
                    //if (r != null)
                    //{
                    //vm.Referrals_Id = db.PCM_Referrals.Find(r.Referrals_Id).Referrals_Id;
                    //vm.theClerk = vm.theClerk;
                    //
                    //vm.Prelim_Enquiry_Date = cc.Prelim_Enquiry_Date;
                    //vm.Child_Need_Care = cc.Child_Need_Care;
                    //vm.Substance_Abuse_Treat = cc.Substance_Abuse_Treat;
                    //vm.Referral_Date = cc.Referral_Date;
                    return vm;
                        //db.SaveChanges();
                    //}
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

        public void UpdateReferralsData(PCMReferralsViewModel prVM, int id, int userid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referrals rr = db.PCM_Referrals.Find(id);
                    rr.PCM_Case_Id = prVM.PCM_Case_Id;
                    rr.Client_Employee_Details_ID = prVM.Client_Employee_Details_ID;
                    rr.theClerk = prVM.theClerk;
                    rr.theParticular = prVM.theParticular;
                    rr.Type_Referral_Id = prVM.Type_Referral_Id;
                    rr.Modified_By = userid;
                    rr.Date_Modified = DateTime.Now;

                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }



        #endregion

        #region PCM_Referral_To_Court

        public List<PCMReferralsViewModel> GetPCMCourtReferralsList()
        {
            List<PCMReferralsViewModel> rVM = new List<PCMReferralsViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var cList = (from cf in db.PCM_Referral_To_Court
                         select new {cf.Court_Referrals_Id, cf.Referrals_Id, cf.Child_Need_Care_Protection, cf.Stay_At_Parents_Home, cf.Comitted_Minor_Offence }).ToList();

            foreach(var item in cList)
            {
                PCMReferralsViewModel objC = new PCMReferralsViewModel();
                objC.Court_Referrals_Id = item.Court_Referrals_Id;
                //objC.Referrals_Id = item.Referrals_Id;
                //objC.Child_Need_Care_Protection = item.Child_Need_Care_Protection;
                //objC.Stay_At_Parents_Home = item.Stay_At_Parents_Home;

                rVM.Add(objC);
            }
            return rVM;
        }



        public void SaveCourtData(PCMReferralsViewModel prVM, int Court_Referrals_Id)
        {
        
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
               
                    PCM_Referral_To_Court nu = db.PCM_Referral_To_Court.Find(Court_Referrals_Id);
                    nu.Child_Need_Care_Protection = prVM.Child_Need_Care_Protection;
                    nu.Stay_At_Parents_Home = prVM.Stay_At_Parents_Home;
                    nu.Comitted_Minor_Offence = prVM.Comitted_Minor_Offence;
                    nu.Client_Employee_Details_ID = prVM.Client_Employee_Details_ID;
                    nu.Refferal_Date = prVM.Refferal_Date;
                    nu.Created_By = prVM.Created_By;
                    nu.Date_Created = DateTime.Now;
                    nu.Modified_By = prVM.Modified_By;
                    nu.Date_Modified = DateTime.Now;
                     
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


        #endregion


        #region Referrals to Counselling/Accredited Programme/Social worker

        public List<PCMReferralsViewModel> GetPCMReferralCAPSW()
        {
            List<PCMReferralsViewModel> rVM = new List<PCMReferralsViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var rList = (from pf in db.PCM_Referrals
                         join prt in db.PCM_Referral_Types
                         on pf.Referrals_Id equals prt.Referrals_Id
                         join rt in db.apl_Referral_Type
                         on pf.Type_Referral_Id equals rt.Type_Referral_Id
                        //where pf.Referrals_Id == Referrals_Id
                         select new {
                                        prt.Category_Referrals_Id,
                                        pf.Referrals_Id,
                                        pf.PCM_Case_Id,
                                        pf.Client_Employee_Details_ID,
                                        pf.theClerk,
                                        pf.theParticular,
                                        pf.Type_Referral_Id,
                                        rt.Type_Referral,
                                        prt.Refferal_Date,
                                        prt.Period_From,
                                        prt.Period_To}).ToList();

            foreach (var item in rList)
            {
                PCMReferralsViewModel objR = new PCMReferralsViewModel();
                objR.Category_Referrals_Id = item.Category_Referrals_Id;
                objR.Referrals_Id = item.Referrals_Id;
                objR.PCM_Case_Id = item.PCM_Case_Id;
                objR.Client_Employee_Details_ID = item.Client_Employee_Details_ID;
                objR.theClerk = item.theClerk;
                objR.theParticular = item.theParticular;
                //objR.Type_Referral_Id = item.Type_Referral_Id;
                objR.Type_Referral = item.Type_Referral;
                objR.Refferal_Date = item.Refferal_Date;
                objR.Period_From = item.Period_From;
                objR.Period_To = item.Period_To;

                rVM.Add(objR);
            }

            return rVM;
        }

        public void SaveReferalToCouncelling(PCMReferralsViewModel vm, int Referrals_Id, int Type_Referral_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referral_Types newRefType = new PCM_Referral_Types();

                    newRefType.Referrals_Id = Referrals_Id;
                    newRefType.Type_Referral_Id = Type_Referral_Id;
                    //newRefType.Client_Employee_Details_ID = Type_Referral_Id;
                    newRefType.Refferal_Date = vm.Refferal_Date;
                    newRefType.Period_From = vm.Period_From;
                    newRefType.Period_To = vm.Period_To;
                    newRefType.Progress_Report = vm.Progress_Report;
                    newRefType.Employee_Id_Copy_To = vm.Employee_Id_Copy_To;
                    newRefType.Modified_By = userId;
                    newRefType.Date_Modified = DateTime.Now;
                    newRefType.Modified_By = userId;
                    newRefType.Date_Created = DateTime.Now;

                    db.PCM_Referral_Types.Add(newRefType);
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

        public PCMReferralsViewModel GetCAPSW(int Category_Referrals_Id)
        {
            PCMReferralsViewModel vm = new PCMReferralsViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from c in db.PCM_Referral_Types
                               where ( c.Referrals_Id != null && c.Category_Referrals_Id == Category_Referrals_Id)
                               select c.Category_Referrals_Id).FirstOrDefault();

                    PCM_Referral_Types ass = db.PCM_Referral_Types.Find(id);
                    if (ass != null)
                    {
                        vm.Category_Referrals_Id = db.PCM_Referral_Types.Find(ass.Category_Referrals_Id).Category_Referrals_Id;
                        //vm.Referrals_Id = ass.Referrals_Id;
                        //vm.Type_Referral_Id = ass.Type_Referral_Id;
                        vm.Client_Employee_Details_ID = ass.Client_Employee_Details_ID;
                        vm.Refferal_Date = ass.Refferal_Date;
                        vm.Period_From = ass.Period_From;
                        vm.Period_To = ass.Period_To;
                        vm.Progress_Report = ass.Progress_Report;
                        vm.Employee_Id_Copy_To = ass.Employee_Id_Copy_To;

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

        public void UpdateReferalToCouncelling(PCMReferralsViewModel vm, int userId, int Referrals_Councelling_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referral_Counselling ass = db.PCM_Referral_Counselling.Find(Referrals_Councelling_Id);

                    //cases.PCM_Case_Id = cas
                    ass.Referrals_Id = vm.Referrals_Id;
                    ass.Type_Referral_Id = vm.Type_Referral_Id;
                    ass.Referral_of_Child_To_Counselling = vm.Referral_of_Child_To_Counselling;
                    ass.Period_From = vm.Period_From;
                    ass.Period = vm.Period_To;
                    ass.Child_Progress = vm.Progress_Report;
                    ass.Modified_By = userId;
                    ass.Date_Modified = DateTime.Now;


                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }


        public void UpdateReferalToProgramme(PCMReferralsViewModel vm, int userId, int Referrals_Programme_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referral_To_Programme ass = db.PCM_Referral_To_Programme.Find(Referrals_Programme_Id);

                    //cases.PCM_Case_Id = cas
                    ass.Referrals_Id = vm.Referrals_Id;
                    ass.Type_Referral_Id = vm.Type_Referral_Id;
                    ass.Accredited_Programme = vm.Accredited_Programme;
                    ass.Programme_Period_From = vm.Period_From;
                    ass.Programme_Period = vm.Period_To;
                    ass.Child_Progress = vm.Progress_Report;
                    ass.Modified_By = userId;
                    ass.Date_Modified = DateTime.Now;


                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void UpdateReferalToSocialWorker(PCMReferralsViewModel vm, int userId, int Referrals_SocilaWorker_Id)
        {


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Referral_SocilaWorker ass = db.PCM_Referral_SocilaWorker.Find(Referrals_SocilaWorker_Id);

                    //cases.PCM_Case_Id = cas
                    ass.Referrals_Id = vm.Referrals_Id;
                    ass.Type_Referral_Id = vm.Type_Referral_Id;
                    ass.Referral_Child_To_Social_Worker = vm.Referral_Child_To_Social_Worker;
                    ass.Programme_Period_From = vm.Period_From;
                    ass.Programme_Period = vm.Period_To;
                    ass.Child_Progress = vm.Progress_Report;
                    ass.Modified_By = userId;
                    ass.Date_Modified = DateTime.Now;


                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public int GetCourtIdByReferalId(int Referrals_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from pf in db.PCM_Referrals
                        join rt in db.PCM_Referral_To_Court on pf.Referrals_Id equals rt.Referrals_Id
                        where rt.Referrals_Id == Referrals_Id
                        select rt.Court_Referrals_Id).FirstOrDefault();
            }
        }


        public int GetProgrammeIdByReferalId(int Referrals_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from pf in db.PCM_Referrals
                        join rt in db.PCM_Referral_To_Programme on pf.Referrals_Id equals rt.Referrals_Id
                        where rt.Referrals_Id == Referrals_Id
                        select rt.Referrals_Programme_Id).FirstOrDefault();
            }
        }

        public int GetCouncellingIdByReferalId(int Referrals_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from pf in db.PCM_Referrals
                        join rt in db.PCM_Referral_Counselling on pf.Referrals_Id equals rt.Referrals_Id
                        where rt.Referrals_Id == Referrals_Id
                        select rt.Referrals_Councelling_Id).FirstOrDefault();
            }
        }

        public int GetSocilaworkerIdByReferalId(int Referrals_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from pf in db.PCM_Referrals
                        join rt in db.PCM_Referral_SocilaWorker on pf.Referrals_Id equals rt.Referrals_Id
                        where rt.Referrals_Id == Referrals_Id
                        select rt.Referrals_SocilaWorker_Id).FirstOrDefault();
            }
        }

        #endregion

    }
}
