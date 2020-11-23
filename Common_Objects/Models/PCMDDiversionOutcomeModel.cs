using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMDDiversionOutcomeModel
    {
        public List<PCMDSessionOutcomeViewModel> GetOutcomeList()
        {
            List<PCMDSessionOutcomeViewModel> vm = new List<PCMDSessionOutcomeViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var oList = (from o in db.PCM_D_Diversion_Outcome
                         select new {
                                    o.Diversion_Outotcome_Id,
                                    o.Intake_Assessment_Id,
                                    o.Court_Date,
                                    o.Remand,
                                    o.Reason_Remand,
                                    o.Next_Court_Date,
                                    o.Court_Outcome,
                                    o.Case_Status}).ToList();

            foreach(var item in oList)
            {
                PCMDSessionOutcomeViewModel obj = new PCMDSessionOutcomeViewModel();
                obj.Diversion_Outotcome_Id = item.Diversion_Outotcome_Id;
                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Court_Date = item.Court_Date;
                obj.Remand = item.Remand;
                obj.Reason_Remand = item.Reason_Remand;
                obj.Next_Court_Date = item.Next_Court_Date;
                obj.Court_Outcome = item.Court_Outcome;
                obj.Case_Status = item.Case_Status;

                vm.Add(obj);
            }

            return vm;

        }

        public void CreateOutcome(PCMDSessionOutcomeViewModel vm, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_D_Diversion_Outcome newOutcome = new PCM_D_Diversion_Outcome();
                    newOutcome.Intake_Assessment_Id = Intake_Assessment_Id;
                    newOutcome.Court_Date = vm.Court_Date;
                    newOutcome.Remand = vm.Remand;
                    newOutcome.Reason_Remand = vm.Reason_Remand;
                    newOutcome.Next_Court_Date = vm.Next_Court_Date;
                    newOutcome.Court_Outcome = vm.Court_Outcome;
                    newOutcome.Case_Status = vm.Case_Status;

                    db.PCM_D_Diversion_Outcome.Add(newOutcome);
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


        public PCMDSessionOutcomeViewModel GetOutcomeEditDetails(int Diversion_Outotcome_Id)
        {
            PCMDSessionOutcomeViewModel vm = new PCMDSessionOutcomeViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from c in db.PCM_D_Diversion_Outcome
                               where (c.Diversion_Outotcome_Id == Diversion_Outotcome_Id)
                               select c.Diversion_Outotcome_Id).FirstOrDefault();

                    PCM_D_Diversion_Outcome o = db.PCM_D_Diversion_Outcome.Find(id);
                    if (o != null)
                    {
                        vm.Diversion_Outotcome_Id = db.PCM_D_Diversion_Outcome.Find(o.Diversion_Outotcome_Id).Diversion_Outotcome_Id;
                        vm.Intake_Assessment_Id = o.Intake_Assessment_Id;
                        vm.Court_Date = o.Court_Date;
                        vm.Remand = o.Remand;
                        vm.Reason_Remand = o.Reason_Remand;
                        vm.Next_Court_Date = o.Next_Court_Date;
                        vm.Court_Outcome = o.Court_Outcome;
                        vm.Case_Status = o.Case_Status;

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

        public void UpdateOutcome(PCMDSessionOutcomeViewModel vm, int Intake_Assessment_Id, int Diversion_Outotcome_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_D_Diversion_Outcome oo = db.PCM_D_Diversion_Outcome.Find(Diversion_Outotcome_Id);

                    //cases.PCM_Case_Id = caseID;
                    oo.Diversion_Outotcome_Id = vm.Diversion_Outotcome_Id;
                    oo.Intake_Assessment_Id = Intake_Assessment_Id;
                    oo.Court_Date = vm.Court_Date;
                    oo.Remand = vm.Remand;
                    oo.Reason_Remand = vm.Reason_Remand;
                    oo.Next_Court_Date = vm.Next_Court_Date;
                    oo.Court_Outcome = vm.Court_Outcome;
                    oo.Case_Status = vm.Case_Status;

                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

    }
}
