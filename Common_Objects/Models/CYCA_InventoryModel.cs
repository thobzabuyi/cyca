using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.ViewModels;


namespace Common_Objects.Models
{
    public class CYCA_InventoryModel
    {
        #region INVENTORY REGISTRATION
        //GET PERSONAL INVENTORY FROM DATABASE
        public List<IntakeDataViewModel> GetListOfPersonalInventory(int Id)
        {            
            List<IntakeDataViewModel> avm = new List<IntakeDataViewModel>();            
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();     
            var ListP = (
               (from pf in db.CYCA_PersonalAndFacilityInventory                
                join p in db.Persons on pf.Person_Id equals p.Person_Id
                join it in db.apl_Cyca_Inventory_Type on pf.Inventory_Type_Id equals it.Inventory_Type_Id
                join r in db.apl_Cyca_Inventory_Return_Status on pf.Return_Status_Id equals r.Return_Status_Id
                where p.Person_Id == Id && pf.Inventory_Type_Id == 1
                select new
                {
                    pf.Inventory_Id,
                    it.Inventory_Type_Id,
                    r.Return_Status_Id
                }).ToList());
            ;
            foreach (var item in ListP)
            {                
                IntakeDataViewModel obj = new IntakeDataViewModel();
                obj.Inventory_Id = item.Inventory_Id;
                obj.selectedInventoryType = db.apl_Cyca_Inventory_Type.Find(item.Inventory_Type_Id).Description;
                obj.Item_Color = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Color;
                obj.Item_Type = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Type;
                obj.Item_Quantity = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Quantity.ToString();
                obj.Item_Description = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Description;
                obj.Return_Status_Id = item.Return_Status_Id;
                obj.selectedReturnStatus = db.apl_Cyca_Inventory_Return_Status.Find(item.Return_Status_Id).Description;
                obj.Date_Handed_In = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Date_Handed_In;
                obj.Date_Returned = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Date_Returned;
                obj.Reason_Not_Returned = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Reason_Not_Returned;
                obj.Quantity_Returned = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Quantity_Returned.ToString();
                avm.Add(obj);
            }

            return avm;
        }

        public List<IntakeDataViewModel> GetListOfFacilityInventory(int Id)
        {
            List<IntakeDataViewModel> avm = new List<IntakeDataViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var ListP = (
               (from pf in db.CYCA_PersonalAndFacilityInventory
                join p in db.Persons on pf.Person_Id equals p.Person_Id
                join it in db.apl_Cyca_Inventory_Type on pf.Inventory_Type_Id equals it.Inventory_Type_Id
                join r in db.apl_Cyca_Inventory_Return_Status on pf.Return_Status_Id equals r.Return_Status_Id
                where p.Person_Id == Id && pf.Inventory_Type_Id == 2
                select new
                {
                    pf.Inventory_Id,
                    it.Inventory_Type_Id,
                    r.Return_Status_Id
                }).ToList());
            ;
            foreach (var item in ListP)
            {
                IntakeDataViewModel obj = new IntakeDataViewModel();
                obj.Inventory_Id = item.Inventory_Id;
                obj.selectedInventoryType = db.apl_Cyca_Inventory_Type.Find(item.Inventory_Type_Id).Description;
                obj.Item_Color = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Color;
                obj.Item_Type = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Type;
                obj.Item_Quantity = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Quantity.ToString();
                obj.Item_Description = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Item_Description;
                obj.Return_Status_Id = item.Return_Status_Id;
                obj.selectedReturnStatus = db.apl_Cyca_Inventory_Return_Status.Find(item.Return_Status_Id).Description;
                obj.Date_Handed_In = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Date_Handed_In;
                obj.Date_Returned = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Date_Returned;
                obj.Reason_Not_Returned = db.CYCA_PersonalAndFacilityInventory.Find(item.Inventory_Id).Reason_Not_Returned;
                avm.Add(obj);
            }

            return avm;
        }
        //ADD NEW INVENTORY
        public void AddInventory(IntakeDataViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())

            {
                //try
                //{
                    CYCA_PersonalAndFacilityInventory newob = new CYCA_PersonalAndFacilityInventory();
                    newob.Person_Id = Id;
                    newob.Item_Handed_By = userId;
                    newob.Admission_Id = (from a in db.CYCA_Admissions_AdmissionDetails
                                         join c in db.Clients on a.Client_Id equals c.Client_Id
                                         join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                                         where c.Person_Id == Id && a.Is_Active == true
                                         select a.Admission_Id).FirstOrDefault();                     
                    newob.Facility_Id = (from a in db.CYCA_Admissions_AdmissionDetails
                                                              join c in db.Clients on a.Client_Id equals c.Client_Id
                                                              join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                                                              where c.Person_Id == Id && a.Is_Active == true
                                                              select f.Facility_Id).FirstOrDefault();                   
                    newob.Inventory_Type_Id = Convert.ToInt32(vm.Inventory_Type_Id);
                    newob.Item_Color = vm.Item_Color;
                    newob.Item_Type = vm.Item_Type;
                    newob.Item_Description = vm.Item_Description;
                    newob.Item_Quantity = Convert.ToInt32(vm.Item_Quantity);
                    newob.Date_Handed_In = Convert.ToDateTime(vm.Date_Handed_Inn);
                    newob.Item_Handed_To = Convert.ToInt32(vm.Item_Handed_To);
                    newob.Return_Status_Id = 1;
                    newob.Date_Created = DateTime.Now;
                    newob.Created_By = userId;                                                        
                    newob.Is_Active = true;
                    newob.Is_Deleted = false;

                    db.CYCA_PersonalAndFacilityInventory.Add(newob);
                    db.SaveChanges();
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                //{
                //    Exception raise = dbEx;
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            string message = string.Format("{0}:{1}",
                //                validationErrors.Entry.Entity.ToString(),
                //                validationError.ErrorMessage);
                //            // raise a new exception nesting
                //            // the current instance as InnerException
                //            raise = new InvalidOperationException(message, raise);
                //        }
                //    }
                //    throw raise;
                //}
            }
        }

        //POPULATE INVENTORY FORM
        public IntakeDataViewModel GetInventoryByInventoryId(int InventoryId)
        {
            IntakeDataViewModel vm = new IntakeDataViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_PersonalAndFacilityInventory act = db.CYCA_PersonalAndFacilityInventory.Find(InventoryId);
                    if (act != null)
                    {
                        vm.Inventory_Id = InventoryId;
                        vm.Inventory_Type_Id = act.Inventory_Type_Id;
                        vm.selectedInventoryType = db.apl_Cyca_Inventory_Type.Find(act.Inventory_Type_Id).Description;
                        vm.Admission_Id = act.Admission_Id;                                                                
                        vm.Item_Type = act.Item_Type;
                        vm.Item_Color = act.Item_Color;
                        vm.Item_Quantity = act.Item_Quantity.ToString();                       
                        vm.Item_Description = act.Item_Description;
                        vm.Date_Handed_In = act.Date_Handed_In;
                        vm.Date_Handed_Inn = act.Date_Handed_In.ToString("yyyy/MM/dd");
                        vm.Item_Handed_To = act.Item_Handed_To;                                                
                        vm.selectedUser = db.Users.Find(act.Item_Handed_To).First_Name +" " + db.Users.Find(act.Item_Handed_To).Last_Name;
                        vm.Return_Status_Id = act.Return_Status_Id;
                        vm.selectedReturnStatus = db.apl_Cyca_Inventory_Return_Status.Find(act.Return_Status_Id).Description;                        
                        vm.Returned_By = act.Returned_By;
                        DateTime dateReturned = Convert.ToDateTime(act.Date_Returned);
                        vm.Reason_Not_Returned = act.Reason_Not_Returned;
                        vm.Quantity_Returned = act.Quantity_Returned.ToString();
                        if (vm.Return_Status_Id == 2)
                        {
                            vm.Date_Returnedd = dateReturned.ToString("yyyy/MM/dd");
                            vm.selectedReturnedBy = db.Users.Find(act.Returned_By).First_Name + " " + db.Users.Find(act.Returned_By).Last_Name;
                            
                        }
                        else if (vm.Reason_Not_Returned != null)
                        {
                            vm.Date_Returnedd = dateReturned.ToString("yyyy/MM/dd");
                            vm.selectedReturnedBy = db.Users.Find(act.Returned_By).First_Name + " " + db.Users.Find(act.Returned_By).Last_Name;
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
            }
            return vm;
        }

        //EDIT INVENTORY 
        public void UpdateInventory(IntakeDataViewModel vm, int Inventory_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_PersonalAndFacilityInventory edit = db.CYCA_PersonalAndFacilityInventory.Find(Inventory_Id);
                    
                    edit.Inventory_Type_Id = Convert.ToInt32(vm.Inventory_Type_Id);
                    edit.Item_Type = vm.Item_Type;
                    edit.Item_Color = vm.Item_Color;
                    edit.Item_Description = vm.Item_Description;
                    edit.Item_Quantity =Convert.ToInt32(vm.Item_Quantity);
                    edit.Date_Handed_In = Convert.ToDateTime(vm.Date_Handed_In);
                    edit.Item_Handed_To = Convert.ToInt32(vm.Item_Handed_To);
                    edit.Return_Status_Id = vm.Return_Status_Id;
                    edit.Quantity_Returned =Convert.ToInt32(vm.Quantity_Returned);
                    edit.Reason_Not_Returned = vm.Reason_Not_Returned;
                    edit.Returned_By = vm.Returned_By;
                    //edit.Date_Returned = Convert.ToDateTime(vm.Date_Returnedd);
                    edit.Date_Returned = string.IsNullOrEmpty(vm.Date_Returnedd) ? (DateTime?)null : DateTime.Parse(vm.Date_Returnedd);
                    edit.Date_Last_Modified = DateTime.Now;
                    edit.Modified_By = userId;


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

        #region INVENTORY RETURNS
        public void ReturnInventory(IntakeDataViewModel vm, int Inventory_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_PersonalAndFacilityInventory edit = db.CYCA_PersonalAndFacilityInventory.Find(Inventory_Id);
                  
                    edit.Return_Status_Id =vm.Return_Status_Id;
                    edit.Quantity_Returned = Convert.ToInt32(vm.Quantity_Returned);
                    edit.Date_Returned = Convert.ToDateTime(vm.Date_Returnedd);
                    edit.Returned_By = vm.Returned_By;
                    edit.Reason_Not_Returned = vm.Reason_Not_Returned;                    
                    edit.Date_Last_Modified = DateTime.Now;
                    edit.Modified_By = userId;

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
    }
}
