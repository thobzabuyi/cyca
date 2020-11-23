using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCM_D_ModulesModels
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<PCMDiversionViewModel> GetDiversion_M()
        {
            List<PCMDiversionViewModel> vm = new List<PCMDiversionViewModel>();
           

            var mList = (from m1 in db.PCM_Diversion_M
                         join d1 in db.PCM_Diversion_P
                         on m1.P_Id equals d1.P_Id
                         join sp1 in db.PCM_Diversion_SP
                         on d1.S_P_Id equals sp1.S_P_Id
                         join sp2 in db.PCM_D_ServicesProvider
                         on sp1.Services_Provider_Id equals sp2.Services_Provider_Id
                         join p in db.Provinces
                         on sp2.Province_Id equals p.Province_Id
                         join pr1 in db.PCM_D_Programme
                         on d1.Programme_Id equals pr1.Programme_Id
                         select new {
                                    m1.M_Id,
                                    //m1.Module,
                                    m1.Sessions,
                                    m1.Session_StartDate,
                                    m1.Session_EndDate,
                                    pr1.Programme_name,
                                    pr1.Programme_Status}).ToList();

            foreach(var item in mList)
            {
                PCMDiversionViewModel obj = new PCMDiversionViewModel();
                obj.M_Id = item.M_Id;
                //obj.Module = item.Module;
                obj.Sessions = item.Sessions;
                obj.Session_StartDate = item.Session_StartDate;
                obj.Session_EndDate = item.Session_EndDate;
                obj.Programme_name = item.Programme_name;
                obj.Programme_Status = item.Programme_Status;

                vm.Add(obj);
            }
            return vm;
        }

        public List<PCMDiversionViewModel> getProgramme()
        {
            List<PCMDiversionViewModel> vm = new List<PCMDiversionViewModel>();
            var p = (from p1 in db.PCM_Diversion_P
                        join p2 in db.PCM_D_Programme
                        on p1.Programme_Id equals p2.Programme_Id
                        join p3 in db.PCM_Diversion_SP
                        on p1.S_P_Id equals p3.S_P_Id
                        select new {p1.P_Id,
                                    p1.S_P_Id,
                                    p1.Programme_Id,
                                    p2.Programme_name,
                                    p2.Programme_Status,
                                    p2.Programme_Expiry_Date,
                                    p2.Services_Provider_Id}).ToList();
            foreach(var item in p)
            {
                PCMDiversionViewModel obj = new PCMDiversionViewModel();
                obj.P_Id = item.P_Id;
                //obj.S_P_Id = item.S_P_Id;
                obj.Programme_Id = item.Programme_Id;
                obj.Programme_name = item.Programme_name;
                obj.Programme_Expiry_Date = item.Programme_Expiry_Date;
                obj.Services_Provider_Id = item.Services_Provider_Id;

                vm.Add(obj);
            }

            return vm;
        }


        public void CreateDiversion_M(PCMDiversionViewModel vm)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Diversion_M m = new PCM_Diversion_M();
                    m.P_Id = vm.P_Id;
                    m.Modules_Id = vm.Modules_Id;
                    m.No_Module = vm.No_Module;
                    m.Sessions = vm.Sessions;
                    m.No_Sessions = vm.No_Sessions;
                    m.Session_StartDate = vm.Session_StartDate;
                    m.Session_EndDate = vm.Session_EndDate;

                    db.PCM_Diversion_M.Add(m);
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

        public PCMDiversionViewModel GetUpdateDiversion_M(int mId)
        {
            PCMDiversionViewModel vm = new PCMDiversionViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from m in db.PCM_Diversion_M
                               where (m.M_Id == mId)
                               select m.M_Id).FirstOrDefault();

                    PCM_Diversion_M mm = db.PCM_Diversion_M.Find(id);

                    if(mm != null)
                    {
                        vm.M_Id = db.PCM_Diversion_M.Find(mm.M_Id).M_Id;
                        vm.P_Id = mm.P_Id;
                        vm.Modules_Id = mm.Modules_Id;
                        vm.No_Module = mm.No_Module;
                        vm.Sessions = mm.Sessions;
                        vm.No_Sessions = mm.No_Sessions;
                        vm.Session_StartDate = mm.Session_StartDate;
                        vm.Session_EndDate = mm.Session_EndDate;

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

        public void UpdateDiversion_M(PCMDiversionViewModel vm, int mId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Diversion_M mm = db.PCM_Diversion_M.Find(mId);
                    mm.P_Id = vm.P_Id;
                    mm.Modules_Id = vm.Modules_Id;
                    mm.No_Module = vm.No_Module;
                    mm.Sessions = vm.Sessions;
                    mm.No_Sessions = vm.No_Sessions;
                    mm.Session_StartDate = vm.Session_StartDate;
                    mm.Session_EndDate = vm.Session_EndDate;

                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }
    }
}
