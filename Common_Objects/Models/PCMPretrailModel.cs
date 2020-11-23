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
    public class PCMPretrailModel
    {
        List<PCMPretrailViewModel> ppVM = new List<PCMPretrailViewModel>();
        //string cs = "data source=.;initial catalog=SDIIS_Database_Tes;Integrated Security=True";
        public List<PCMPretrailViewModel> ListAll()
        {
            List<PCMPretrailViewModel> ppVM = new List<PCMPretrailViewModel>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities(); //db connection

            var pList = (from pT in db.PCM_Pretrial_Details
                         select new {
                                     pT.PCM_Pretrial_Id,
                                     pT.PCM_Case_Id,
                                     pT.PCM_Pretrial_Date,
                                     pT.PCM_Court_Outcome_Id,
                                     pT.Intake_Assessment_Id,
                                     pT.Educational_Summary,
                                     pT.Offence_Sammary,
                                     pT.Victims_Sammary,
                                     //pT.PCM_Offence_Id,
                                     //pT.PCM_Recommendation_Id,
                                     pT.PCM_Commemts,
                                     pT.Created_By,
                                     pT.Date_Created,
                                     pT.Modified_By,    
                                     pT.Date_Modified
                         }).ToList();

            foreach(var item in pList)
            {
                PCMPretrailViewModel objP = new PCMPretrailViewModel();
                objP.PCM_Pretrial_Id = item.PCM_Pretrial_Id;
                objP.PCM_Case_Id = item.PCM_Case_Id;
                objP.PCM_Pretrial_Date = item.PCM_Pretrial_Date;
                objP.Educational_Summary = item.Educational_Summary;
                objP.Offence_Sammary = item.Educational_Summary;
                objP.Victims_Sammary = item.Victims_Sammary;
                objP.PCM_Court_Outcome_Id = item.PCM_Court_Outcome_Id;
                objP.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objP.PCM_Commemts = item.PCM_Commemts;

                ppVM.Add(objP);
            }

            return ppVM;
        }


        public void Add(PCMPretrailViewModel pVM, int Intake_Assessment_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Pretrial_Details newPetrail = new PCM_Pretrial_Details();
                    newPetrail.Intake_Assessment_Id = Intake_Assessment_Id;
                    newPetrail.Educational_Summary = pVM.Educational_Summary;
                    newPetrail.Offence_Sammary = pVM.Offence_Sammary;
                    newPetrail.Victims_Sammary = pVM.Victims_Sammary;
                    newPetrail.Created_By = userId;
                    newPetrail.Date_Created = DateTime.Now;

                    db.PCM_Pretrial_Details.Add(newPetrail);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVM"></param>
        /// <returns></returns>
        /// 

        public List<OutcomeTypeLookup> GetOutcomeType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Outcome_Type = db.PCM_Pretrail_Outcome.Select(o => new OutcomeTypeLookup
                {
                    PCM_Court_Outcome_Id = o.PCM_Court_Outcome_Id,
                    PCM_Court_Outcome = o.PCM_Court_Outcome
                }).ToList();

                return Outcome_Type;
            }
        }



        //Method for Updating Employee record  
        //public int Update(PCMPretrailViewModel pVM)
        //{
        //    int i;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("InsertUpdateEmployee", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@PCM_Pretrial_Id", pVM.PCM_Pretrial_Id);
        //        com.Parameters.AddWithValue("@PCM_Case_Id", pVM.PCM_Case_Id);
        //        com.Parameters.AddWithValue("@Client_Id", pVM.Client_Id);
        //        com.Parameters.AddWithValue("@PCM_Preliminary_Id", pVM.PCM_Preliminary_Id);
        //        com.Parameters.AddWithValue("@PCM_Pretrial_Date", pVM.PCM_Pretrial_Date);
        //        com.Parameters.AddWithValue("@PCM_Court_Outcome_Id", pVM.PCM_Court_Outcome_Id);
        //        com.Parameters.AddWithValue("@PCM_Offence_Id", pVM.PCM_Offence_Id);
        //        com.Parameters.AddWithValue("@PCM_Recommendation_Id", pVM.PCM_Recommendation_Id);
        //        com.Parameters.AddWithValue("@PCM_Commemts", pVM.PCM_Commemts);
        //        com.Parameters.AddWithValue("@Created_By", pVM.Created_By);
        //        com.Parameters.AddWithValue("@Date_Created", pVM.Date_Created);
        //        com.Parameters.AddWithValue("@Modified_By", pVM.Modified_By);
        //        com.Parameters.AddWithValue("@Date_Modified", pVM.Date_Modified);
        //        com.Parameters.AddWithValue("@Action", "Update");
        //        i = com.ExecuteNonQuery();
        //    }
        //    return i;
        //}

        public void update(PCMPretrailViewModel vm,int PCM_Pretrial_Id, int Intake_Assessment_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Pretrial_Details pp = db.PCM_Pretrial_Details.Find(PCM_Pretrial_Id);
                    pp.Intake_Assessment_Id = Intake_Assessment_Id;
                    pp.Educational_Summary = vm.Educational_Summary;
                    pp.Offence_Sammary = vm.Offence_Sammary;
                    pp.Victims_Sammary = vm.Victims_Sammary;
                    pp.Modified_By = userId;
                    pp.Date_Modified=DateTime.Now;

                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public PCMPretrailViewModel Update(int PCM_Pretrial_Id)
        //{
        //    PCMPretrailViewModel vm = new PCMPretrailViewModel();
        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            //int? id = (from c in db.PCM_Pretrial_Details
        //            //           where (c.PCM_Pretrial_Id == PCM_Pretrial_Id)
        //            //           select c.PCM_Pretrial_Id).FirstOrDefault();

        //            PCM_Pretrial_Details p = db.PCM_Pretrial_Details.Find(id);
        //            if(p != null)
        //            {
        //                vm.PCM_Pretrial_Id = db.PCM_Pretrial_Details.Find(p.PCM_Pretrial_Id).PCM_Pretrial_Id;
        //                vm.PCM_Case_Id = p.PCM_Case_Id;
        //                vm.PCM_Pretrial_Date = p.PCM_Pretrial_Date;
        //                vm.PCM_Court_Outcome_Id = p.PCM_Court_Outcome_Id;         
        //                vm.PCM_Commemts = vm.PCM_Commemts;
        //                //vm.Created_By = DateTime.Now;
        //                vm.Date_Created = DateTime.Now; ;
        //                vm.Modified_By = p.Modified_By;
        //                vm.Date_Modified = DateTime.Now;

        //                db.SaveChanges();
        //            }
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {
        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    // raise a new exception nesting
        //                    // the current instance as InnerException
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }
        //    }
        //        return vm;
        //}

        public void View(PCMPretrailViewModel vm, int userId, int PCM_Pretrial_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Pretrial_Details p = db.PCM_Pretrial_Details.Find(PCM_Pretrial_Id);

                    vm.PCM_Case_Id = p.PCM_Case_Id;
                    vm.PCM_Pretrial_Date = p.PCM_Pretrial_Date;
                    vm.PCM_Court_Outcome_Id = p.PCM_Court_Outcome_Id;
                    vm.PCM_Commemts = vm.PCM_Commemts;
                    vm.Date_Created = DateTime.Now; ;
                    vm.Modified_By = p.Modified_By;
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
    }
}
