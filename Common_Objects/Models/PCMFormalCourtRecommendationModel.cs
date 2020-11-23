using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMFormalCourtRecommendationModel
    {
        public List<PCMChildrensCourtViewModel> GetPCMFormalCourtRecommendationList()
        {
            List<PCMChildrensCourtViewModel> fVM = new List<PCMChildrensCourtViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var fList = (from fc in db.PCM_Formal_Court_Recommendation
                         select new { fc.PCM_Formal_Court_Recomm_Id, fc.PCM_Recommendation_Id, fc.Type_Of_Center_Id, fc.Type_Of_Placement_Id }).ToList();

            foreach (var item in fList)
            {
                PCMChildrensCourtViewModel objR = new PCMChildrensCourtViewModel();

                objR.PCM_Formal_Court_Recomm_Id = item.PCM_Formal_Court_Recomm_Id;
                objR.PCM_Recommendation_Id = item.PCM_Recommendation_Id;
                objR.Type_Of_Center_Id = item.Type_Of_Center_Id;
                objR.Type_Of_Placement_Id = item.Type_Of_Placement_Id;

                fVM.Add(objR);
            }

            return fVM;
        }


        public void CreatePCMFormalCourt(PCMChildrensCourtViewModel vm, int PcmReg, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Formal_Court_Recommendation newFC = new PCM_Formal_Court_Recommendation();
                    newFC.PCM_Recommendation_Id = PcmReg;
                    newFC.Type_Of_Center_Id = vm.Type_Of_Center_Id;
                    newFC.Type_Of_Placement_Id = vm.Type_Of_Placement_Id;
                    newFC.Personal_Details_Id = vm.Personal_Details_Id;
                    newFC.Facility_Id = vm.Facility_Id;
                    newFC.Created_By = userId;
                    newFC.Date_Modified = DateTime.Now;
                    newFC.Modified_By = userId;
                    newFC.Date_Created = DateTime.Now;
                    newFC.Withdrawal_Status = vm.Withdrawal_Status;

                    db.PCM_Formal_Court_Recommendation.Add(newFC);
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

        public PCMChildrensCourtViewModel GetPCMFormalCourtEditDetails(int FormalCourtRecommId)
        {
            PCMChildrensCourtViewModel vm = new PCMChildrensCourtViewModel();
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? id = (from c in db.PCM_Formal_Court_Recommendation
                               where (c.PCM_Formal_Court_Recomm_Id == FormalCourtRecommId)
                               select c.PCM_Formal_Court_Recomm_Id).FirstOrDefault();

                    PCM_Formal_Court_Recommendation fc = db.PCM_Formal_Court_Recommendation.Find(id);
                    if (fc != null)
                    {
                        vm.PCM_Formal_Court_Recomm_Id = db.PCM_Formal_Court_Recommendation.Find(fc.PCM_Formal_Court_Recomm_Id).PCM_Formal_Court_Recomm_Id;
                        vm.PCM_Recommendation_Id = fc.PCM_Recommendation_Id;
                        vm.Type_Of_Center_Id = fc.Type_Of_Center_Id;
                        vm.Type_Of_Placement_Id = fc.Type_Of_Placement_Id;
                        vm.Personal_Details_Id = fc.Personal_Details_Id;
                        vm.Facility_Id = fc.Facility_Id;
                        vm.Date_Modified = fc.Date_Modified;
                        vm.Modified_By = fc.Modified_By;
                        vm.Date_Created = fc.Date_Created;
                        vm.Withdrawal_Status = vm.Withdrawal_Status;
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
    }
}
