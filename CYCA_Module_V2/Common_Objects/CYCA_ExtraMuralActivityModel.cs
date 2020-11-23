using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCA_ExtraMuralActivityModel
    {
        public List<CYCAAdmissionExtraMuralActivityViewModel> GetExtraMuralActivityList(int Id)
        {
            List<CYCAAdmissionExtraMuralActivityViewModel> avm = new List<CYCAAdmissionExtraMuralActivityViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var ListP = (
                 (from a in db.CYCA_Admissions_AdmissionDetails
                  join ae in db.CYCA_Admissions_ExtraMuralActivity on a.Admission_Id equals ae.Admission_Id                 
                  join aec in db.Eye_Colors on ae.Eye_Color_Id equals aec.Eye_Color_Id
                  join ahc in db.Hair_Colors on ae.Hair_Color_Id equals ahc.Hair_Color_Id
                  join apb in db.apl_Cyca_Physical_Build on ae.Physical_Build_Id equals apb.Physical_Build_Id
                  join c in db.Clients on a.Client_Id equals c.Client_Id
                  where c.Person_Id == Id
                  select new
                {
                    a.Admission_Id,
                    ae.Extra_Mural_Activity_Id,                     
                    ahc.Hair_Color_Id,
                    apb.Physical_Build_Id,
                    aec.Eye_Color_Id

                }).ToList());
            ;
            foreach (var item in ListP)
            {                
                CYCAAdmissionExtraMuralActivityViewModel obj = new CYCAAdmissionExtraMuralActivityViewModel();
                obj.Admission_Id = item.Admission_Id;
                obj.Extra_Mural_Activity_Id = item.Extra_Mural_Activity_Id;
                obj.selectedHairColor = db.Hair_Colors.Find(item.Hair_Color_Id).Description;                
                obj.selectedEyeColor = db.Eye_Colors.Find(item.Eye_Color_Id).Description;          
                obj.selectedPhysicalBuild = db.apl_Cyca_Physical_Build.Find(item.Physical_Build_Id).Description;
                obj.DateCreated = db.CYCA_Admissions_ExtraMuralActivity.Find(item.Extra_Mural_Activity_Id).Date_Created.ToString();             
                obj.Hobby_Id = db.CYCA_Admissions_ExtraMuralActivity.Find(item.Extra_Mural_Activity_Id).Hobby_Id.Split(',').ToArray();
                obj.Activity_Id = db.CYCA_Admissions_ExtraMuralActivity.Find(item.Extra_Mural_Activity_Id).Activity_Id.Split(',').ToArray();
                for (int i = 0; i < obj.Hobby_Id.Length; i++)
                {
                    int hobbyId = Convert.ToInt32(obj.Hobby_Id[i]);
                    string hobbyDescription = db.apl_Cyca_Child_Hobbies.Find(hobbyId).Description;
                    if (obj.selectedHobby == null || obj.selectedHobby == " ")
                    {
                        obj.selectedHobby = hobbyDescription;
                    }
                    else
                    {
                        obj.selectedHobby = obj.selectedHobby + ", " + hobbyDescription;
                    }
                    
                }
                for (int i = 0; i < obj.Activity_Id.Length; i++)
                {
                    int activityId = Convert.ToInt32(obj.Activity_Id[i]);
                    string activityDescription = db.apl_Cyca_Sport_Activity.Find(activityId).Description;
                    if (obj.selectedSportActivity == null || obj.selectedSportActivity == " ")
                    {
                        obj.selectedSportActivity = activityDescription;
                    }
                    else
                    {
                        obj.selectedSportActivity = obj.selectedSportActivity + ", " + activityDescription;
                    }
                }
                avm.Add(obj);
            }

            return avm;
        }
        //POPULATE MODAL
        public CYCAAdmissionExtraMuralActivityViewModel GetActivitybyActivityId(int ActivityId)
        {
            CYCAAdmissionExtraMuralActivityViewModel vm = new CYCAAdmissionExtraMuralActivityViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Admissions_ExtraMuralActivity act = db.CYCA_Admissions_ExtraMuralActivity.Find(ActivityId);
                    if (act != null)
                    {
                        vm.Extra_Mural_Activity_Id = ActivityId;
                        vm.Admission_Id = Convert.ToInt16(act.Admission_Id);
                        vm.Weight = act.Weight;
                        vm.Physical_Build_Id = act.Physical_Build_Id;
                        vm.selectedPhysicalBuild = db.apl_Cyca_Physical_Build.Find(act.Physical_Build_Id).Description;
                        vm.Eye_Color_Id = act.Eye_Color_Id;
                        vm.selectedEyeColor = db.Eye_Colors.Find(act.Eye_Color_Id).Description;
                        vm.Hair_Color_Id = act.Hair_Color_Id;
                        vm.selectedHairColor = db.Hair_Colors.Find(act.Hair_Color_Id).Description;
                        vm.Hobby_Id = act.Hobby_Id.Split(',').ToArray();
                        vm.Activity_Id = act.Activity_Id.Split(',').ToArray();                       
                        vm.Additional_Description = act.Description;              
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
            return vm;
        }
    }
}