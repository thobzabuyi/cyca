using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMDSessionOutcomeModel
    {

        public List<PCMDSessionOutcomeViewModel> GetDSOList()
        {
            List<PCMDSessionOutcomeViewModel> vm = new List<PCMDSessionOutcomeViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var DSOList = (from d in db.PCM_D_Session_Outcome
                           select new
                           {
                               d.DSession_Id,
                               d.Intake_Assessment_Id,
                               d.Current_Module_Attended,
                               d.Session_Attend,
                               d.Session_Date,
                               d.Name_of_the_Facilitator,
                               d.Name_of_Co_Facilitator,
                               d.Process_Notes,
                               d.Next_Session_Date,
                               d.Compliance
                           }).ToList();

            foreach(var item in DSOList)
            {
                PCMDSessionOutcomeViewModel obj = new PCMDSessionOutcomeViewModel();

                obj.DSession_Id = item.DSession_Id;
                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Current_Module_Attended = item.Current_Module_Attended;
                obj.Session_Attend = item.Session_Attend;
                obj.Session_Date = item.Session_Date;
                obj.Name_of_the_Facilitator = item.Name_of_the_Facilitator;
                obj.Name_of_Co_Facilitator = item.Name_of_Co_Facilitator;
                obj.Process_Notes = item.Process_Notes;
                obj.Next_Session_Date = item.Next_Session_Date;
                obj.Compliance = item.Compliance;

                vm.Add(obj);
            }

            return vm;
        }

        public void CreateDSO(PCMDSessionOutcomeViewModel vm, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_D_Session_Outcome newdso = new PCM_D_Session_Outcome();
                    newdso.Intake_Assessment_Id = 28377;
                    newdso.Current_Module_Attended = vm.Current_Module_Attended;
                    newdso.Session_Attend = vm.Session_Attend;
                    newdso.Session_Date = vm.Session_Date;
                    newdso.Name_of_the_Facilitator = vm.Name_of_the_Facilitator;
                    newdso.Name_of_Co_Facilitator = vm.Name_of_Co_Facilitator;
                    newdso.Process_Notes = vm.Process_Notes;
                    newdso.Next_Session_Date = vm.Next_Session_Date;
                    newdso.Compliance = vm.Compliance;

                    db.PCM_D_Session_Outcome.Add(newdso);
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
