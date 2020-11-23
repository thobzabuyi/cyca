using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class CYCAChildAllocationModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        #region MOVEMENTS TAB
        #region SEARCH CHILD ASSIGENED TO CARE WORKER

        //GET CHILDREN ASSIGNED TO LOGGED IN USER
        public List<CYCAChildAllocationViewModel> GetPersonListByUserId(int userId)
        {
            List<CYCAChildAllocationViewModel> cvm = new List<CYCAChildAllocationViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var ListP = 
                (from p in db.Persons
                 join ca in db.CYCA_Child_Allocation on p.Person_Id equals ca.Person_Id
                 join cc in db.Clients on p.Person_Id equals cc.Person_Id
                 join u in db.Users on ca.User_Id equals u.User_Id
                 join a in db.CYCA_Admissions_AdmissionDetails on new { admission = ca.Admission_Id??0, active = true } equals new { admission = a.Admission_Id, active = a.Is_Active }
                 //join aa in db.CYCA_Admissions_AdmissionDetails on cc.Client_Id equals aa.Client_Id
                 join agm in db.CYCA_Admissions_GangMembership on a.Admission_Id equals agm.Admission_Id
                 join gm in db.apl_Cyca_Gang_Membership_Type on agm.Gang_Membership_Type_Id equals gm.Gang_Membership_Type_Id
                 where u.User_Id == userId && agm.ReAdmission_Id == null
                 select new CYCAChildAllocationViewModel
                 {
                     Child_Allocation_Id = ca.Child_Allocation_Id,
                     Person_Id = p.Person_Id,
                     Child_First_Name = p.First_Name,
                     Child_Last_First_Name = p.Last_Name,
                     Child_ID_No= p.Identification_Number,
                     LoggedInUserName = u.First_Name + " " + u.Last_Name,
                     Child_Name = p.First_Name + " " + p.Last_Name,
                     Date_Transferred = ca.Date_Allocated,
                     GangMembership = gm.Description
                                          
                 }).Distinct().ToList();
            //Get Transfer Status = 1
            var ListT = db.CYCA_Child_Transfer.Where(m=>m.Transferred_By==userId&&m.Tansfer_Status_Id==1).ToList();
            HashSet<int> userIds = new HashSet<int>(ListT.Select(x => x.Person_Id??0));
            ListP.RemoveAll(x => userIds.Contains(x.Person_Id??0));
            return ListP;
        }
        #endregion

        #region GET USER AND CHILD DETAILS IN VENUE

        public int GetFacilityIdByUserID(int UserId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Employees
                        join c in db.Users on p.User_Id equals c.User_Id
                        join i in db.Organizations on p.Organization_Id equals i.Organization_Id
                        join f in db.apl_Cyca_Facility on i.Organization_Id equals f.Organization_Id
                        where c.User_Id == (UserId)
                        select i.Organization_Id).SingleOrDefault();
            }
        }

        public int GetVenueIdByFacilityId(int facilityId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from v in db.apl_Cyca_Venue
                        join f in db.apl_Cyca_Facility on v.Facility_Id equals f.Facility_Id
                        join t in db.CYCA_Team_Allocation on v.Venue_Id equals t.Venue_Id
                        where f.Facility_Id == facilityId
                        select v.Venue_Id).FirstOrDefault();
            }
        }

        public List<CYCAChildAllocationViewModel> GetUserByVenueId(int venueId)
        {
            List<CYCAChildAllocationViewModel> cvm = new List<CYCAChildAllocationViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var ListP =
              (from t in db.CYCA_Child_Movement
               join u in db.Users on t.Moved_By equals u.User_Id
               //Active Admissions for Facility
               join a in db.CYCA_Admissions_AdmissionDetails on new { admission = t.Admission_Id??0, active = true } equals new { admission = a.Admission_Id, active = a.Is_Active }
               join c in db.CYCA_Child_Allocation on new { careworker = t.Moved_By,child = t.Person_Id, admission = t.Admission_Id } equals new { careworker =c.User_Id,child=c.Person_Id,admission = c.Admission_Id}
               join v in db.apl_Cyca_Venue on t.Venue_Id equals v.Venue_Id

               join p in db.Persons on t.Person_Id equals p.Person_Id
               where v.Venue_Id == venueId && t.Check_Out_Date == null
               select new 
               {
                  t.Movement_Id,
                   t.Moved_By,
                   u.User_Id,
                   u.First_Name,
                   u.Last_Name,
                   t.Date_Moved,
                   v.Venue_Id,
                   p.Person_Id,
                   ChildName = p.First_Name+ " " +p.Last_Name
               }).OrderBy(c=>c.User_Id).ToList();
            int count = 0;
            int lastUser = 0;
            foreach (var item in ListP)
            {
                if(lastUser!=item.User_Id)
                {
                    count = 1;
                    CYCAChildAllocationViewModel obj = new CYCAChildAllocationViewModel();

                    obj.Movement_Id = item.Movement_Id;
                    obj.CareWorker_Name = item.First_Name + " " + item.Last_Name;
                    obj.Checked_In_Date = item.Date_Moved;
                    obj.Person_Id = count; ;
                    obj.Child_Name = item.ChildName;

                    cvm.Add(obj);

                }else
                {
                    count++;
                    cvm[cvm.Count - 1].Person_Id = count;
                    cvm[cvm.Count - 1].Child_Name = cvm[cvm.Count - 1].Child_Name + "&#13;" + item.ChildName;
                }
                lastUser = item.User_Id;
            }

            return cvm;
        }

        public CYCAChildAllocationViewModel GetCareWorkerByVenueId(int venueId)
        {
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Team_Allocation act = db.CYCA_Team_Allocation.Find(venueId);
                    if (act != null)
                    {
                        vm.Team_Allocation_Id = act.Team_Allocation_Id;
                        vm.Venue_Id = act.Venue_Id;
                        vm.CareWorker_Name = db.Users.Find(act.Reporting_User).First_Name + " " + db.Users.Find(act.Reporting_User).Last_Name;
                        vm.Start_Date = act.Start_Date.ToString("dd/MM/yyyy");

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
        #endregion

        #region ADD CHILD MOVEMENT TO DATABASE
        public int GetAlloIdByPersId(int personId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from a in db.CYCA_Child_Allocation
                        join p in db.Persons on a.Person_Id equals p.Person_Id
                        where a.Person_Id == personId
                        select a.Child_Allocation_Id).FirstOrDefault();
            }
        }

        public void saveChldToAllocation(CYCA_Child_Allocation child)
        {
            db.CYCA_Child_Allocation.Add(child);
            db.SaveChanges();
        }
        public CYCAChildAllocationViewModel GetChildByAllocationId(int AllocationId)
        {
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCAChildAllocationModel model = new CYCAChildAllocationModel();
                    var person = model.GetAlloIdByPersId(AllocationId);

                    CYCA_Child_Allocation act = db.CYCA_Child_Allocation.Find(person);

                    if (act != null)
                    {
                        vm.Child_Allocation_Id = person;
                        vm.Person_Id = act.Person_Id;
                        vm.Moved_By = act.User_Id;
                        vm.Date_Moved = DateTime.Now.ToString("yyy/MM/dd");
                        vm.Time_Moved = DateTime.Now.ToString("HH:mm");

                        vm.Transferred_By = act.User_Id??0;
                        vm.Date_Transferred = DateTime.Now;
                        vm.Time_Transferred = DateTime.Now;                    
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

        public int GetPersonIdByAllocationId(int allocationId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from p in db.Persons
                        join a in db.CYCA_Child_Allocation on p.Person_Id equals a.Person_Id
                        where a.Child_Allocation_Id == allocationId
                        select p.Person_Id).SingleOrDefault();
            }
        }

        public int GetAllocationIdByUserId(int UserId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from a in db.CYCA_Child_Allocation
                        join u in db.Users on a.User_Id equals u.User_Id
                        where a.User_Id == UserId
                        select a.Child_Allocation_Id).FirstOrDefault();
            }
        }

        public int GetMovementID(int movedBy)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from m in db.CYCA_Child_Movement
                        join u in db.Users on m.Moved_By equals u.User_Id
                        where m.Moved_By == movedBy && m.Check_Out_Date == null
                        select m.Movement_Id).FirstOrDefault();
            }
        }



        public void SaveChildMovement(CYCAChildMovementTransfer vm, int userId)//, int movementId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    
                    //Is Child in another Venue
                    var closeMovements = (from m in db.CYCA_Child_Movement
                                         join p in db.Persons on m.Person_Id equals p.Person_Id
                                         where m.Person_Id == vm.Person_Id && m.Child_Check_Out_Date == null
                                         select m).ToList();
                    foreach(CYCA_Child_Movement closeMovement in closeMovements)
                    {
                        closeMovement.Check_Out_Date = DateTime.Now;
                        closeMovement.Check_Out_Time = DateTime.Now;
                        closeMovement.Child_Check_Out_Date = DateTime.Now;
                        closeMovement.Child_Check_Out_Time = DateTime.Now;

                        closeMovement.Date_Last_Modified = DateTime.Now;
                        closeMovement.Modified_By = userId.ToString();
                        db.SaveChanges();
                    }
                    //Open New Record
                    CYCA_Child_Movement add = new CYCA_Child_Movement();
                    add.Person_Id = vm.Person_Id;
                    add.Admission_Id = vm.Admission_Id;
                    add.Venue_Id = vm.Venue_Id;
                    add.Date_Moved = Convert.ToDateTime(DateTime.Now);
                    add.Time_Moved = Convert.ToDateTime(DateTime.Now);
                    add.Moved_By = userId;
                    add.Created_By = userId.ToString();
                    add.Date_Created = DateTime.Now;
                    add.Is_Active = true;
                    add.Is_Deleted = false;
                    db.CYCA_Child_Movement.Add(add);

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

        #endregion





        #region TRANSFER TAB

        //List of Children Assigend to User
        public List<CYCAChildAllocationViewModel> GetChildByUserId(int User_Id)
        {
            List<CYCAChildAllocationViewModel> cvm = new List<CYCAChildAllocationViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var ListP =
              (from t in db.CYCA_Child_Transfer
               join u in db.Users on t.Tranferred_To equals u.User_Id
               join p in db.Persons on t.Person_Id equals p.Person_Id
               where u.User_Id == User_Id
               select new
               {
                   t.Transfer_Id,
                   u.User_Id,
                   p.Person_Id
               }).ToList();

            foreach (var item in ListP)
            {
                CYCAChildAllocationViewModel obj = new CYCAChildAllocationViewModel();

                obj.Transfer_Id = item.Transfer_Id;
                obj.Child_Name = db.Persons.Find(item.Person_Id).First_Name + " " + db.Persons.Find(item.Person_Id).Last_Name;
                obj.Date_Transferred = db.CYCA_Child_Transfer.Find(item.Transfer_Id).Date_Transferred;




                cvm.Add(obj);
            }

            return cvm;
        }

        //Get Child Details/Populate modal
        public CYCAChildAllocationViewModel GetPersonByAllocationId(int AllocationId)
        {
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Child_Allocation act = db.CYCA_Child_Allocation.Find(AllocationId);

                    if (act != null)
                    {
                        vm.Child_Allocation_Id = AllocationId;
                        vm.Person_Id = act.Person_Id;                      
                        vm.Transferred_By = act.User_Id??0;
                        vm.Date_Transferred = DateTime.Now;
                        vm.Time_Transferred = DateTime.Now;
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

        //Add Child on transfer table
        public void AddChildTransferToDatabase(CYCAChildMovementTransfer vm, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Child_Transfer add = new CYCA_Child_Transfer();

                    add.Person_Id = vm.Person_Id;
                    add.Tansfer_Status_Id = 1;
                    add.Date_Transferred = DateTime.Now;
                    add.Time_Transferred = DateTime.Now;
                    add.Transferred_By = userId;
                    add.Tranferred_To = vm.TransferredTo_Id;
                    add.Created_By = userId.ToString();
                    add.Date_Created = DateTime.Now;
                    add.Is_Active = true;
                    add.Is_Deleted = false;
                    add.Admission_Id = vm.Admission_Id;
                    db.CYCA_Child_Transfer.Add(add);
             
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


        //Get Child Transferred by logged in User
        public List<CYCAChildAllocationViewModel> GetTransferredPersonByTransferredBy(int userId)
        {
            List<CYCAChildAllocationViewModel> cvm = new List<CYCAChildAllocationViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var ListP =
                (from p in db.Persons
                 join t in db.CYCA_Child_Transfer on p.Person_Id equals t.Person_Id
                 join u in db.Users on t.Transferred_By equals u.User_Id
                 join ts in db.apl_Cyca_Transfer_Status on t.Tansfer_Status_Id equals ts.Tansfer_Status_Id
                 where t.Transferred_By == userId && t.Tansfer_Status_Id == 1
                 select new
                 {
                     p.Person_Id,
                     t.Transfer_Id,
                     ts.Tansfer_Status_Id,
                     u.User_Id
                 }).ToList();
            foreach (var item in ListP)
            {
                CYCAChildAllocationViewModel obj = new CYCAChildAllocationViewModel();

                obj.Transfer_Id = item.Transfer_Id;
                obj.Child_First_Name = db.Persons.Find(item.Person_Id).First_Name + " " + db.Persons.Find(item.Person_Id).Last_Name;                
                obj.selectedTransferStatus = db.apl_Cyca_Transfer_Status.Find(item.Tansfer_Status_Id).Description;
                cvm.Add(obj);
            }
            return cvm;
        }

        //Get Child Transferred to logged in User
        public List<CYCAChildAllocationViewModel> GetTransferredPersonByTransferredTo(int userId)
        {
            List<CYCAChildAllocationViewModel> cvm = new List<CYCAChildAllocationViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();            

            var ListP =
                (from p in db.Persons
                 join t in db.CYCA_Child_Transfer on p.Person_Id equals t.Person_Id
                 join u in db.Users on t.Transferred_By equals u.User_Id
                 join ts in db.apl_Cyca_Transfer_Status on t.Tansfer_Status_Id equals ts.Tansfer_Status_Id
                 where t.Tranferred_To == userId && t.Tansfer_Status_Id == 1
                 select new
                 {
                     p.Person_Id,
                     t.Transfer_Id,
                     ts.Tansfer_Status_Id,
                     u.User_Id
                 }).ToList();
            foreach (var item in ListP)
            {
                CYCAChildAllocationViewModel obj = new CYCAChildAllocationViewModel();

                obj.Transfer_Id = item.Transfer_Id;
                obj.Child_First_Name = db.Persons.Find(item.Person_Id).First_Name + " " + db.Persons.Find(item.Person_Id).Last_Name;
                obj.selectedTransferStatus = db.apl_Cyca_Transfer_Status.Find(item.Tansfer_Status_Id).Description;
                cvm.Add(obj);
            }
            return cvm;
        }

        //Populate Transfer modal
        public CYCAChildAllocationViewModel GetTransferById(int TransferId)
        {
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    CYCA_Child_Transfer act = db.CYCA_Child_Transfer.Find(TransferId);

                    if (act != null)
                    {
                        //Get Request Details
                        vm.Transfer_Id = TransferId;
                        vm.Transfer_Status_Id = act.Tansfer_Status_Id;
                        vm.Child_First_Name = db.Persons.Find(act.Person_Id).First_Name + " " + db.Persons.Find(act.Person_Id).Last_Name;
                        vm.transferredbyName = db.Users.Find(act.Transferred_By).First_Name + " " + db.Users.Find(act.Transferred_By).Last_Name;
                        vm.Person_Id = db.Persons.Find(act.Person_Id).Person_Id;



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

        public CYCA_Child_Allocation GetAllocationIdByTransferId(int transferid)
        {
            using (SDIIS_DatabaseEntities db  = new SDIIS_DatabaseEntities())
            {
                return (from a in db.CYCA_Child_Allocation
                        join p in db.Persons on a.Person_Id equals p.Person_Id
                        join t in db.CYCA_Child_Transfer on p.Person_Id equals t.Person_Id
                        where t.Transfer_Id == transferid
                        select a).SingleOrDefault();
            }
        }
     
        //Update Transfer and Child Allocation
        public void UpdateTransferAndAllocation(CYCAChildAllocationViewModel vm, int Transfer_Id, int userId, int reid, int Allocation_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Child_Transfer edit = db.CYCA_Child_Transfer.Find(Transfer_Id);



                    edit.Tansfer_Status_Id = Convert.ToInt32(reid);
                    edit.Date_Last_Modified = DateTime.Today;


                    CYCA_Child_Allocation assign = db.CYCA_Child_Allocation.Find(Allocation_Id);
                    if (reid == 2)
                    {
                        assign.User_Id = userId;
                    }


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
