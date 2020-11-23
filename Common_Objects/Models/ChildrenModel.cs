using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
  public class ChildrenModel
    {
        public ChildrenModel()
        { }
      private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

     
      
        //Get All Children in facility 
        public List<CYCAChildAllocationViewModel> getAllChildren(int facilityID, User currentUser)
        {

         var allChildren = (from p in db.Persons
                               join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                               from imleft in pim.DefaultIfEmpty()
                               join c in db.Clients on p.Person_Id equals c.Person_Id
                               //Active Admissions for Facility
                               join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                               join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                            where gg.Is_Active
                            join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                               join ca in db.CYCA_Child_Allocation on new { person = p.Person_Id, admission = a.Admission_Id } equals new { person = ca.Person_Id ?? 0, admission = ca.Admission_Id ?? 0 }
                               where gg.ReAdmission_Id == null
                               select new CYCAChildAllocationViewModel
                               {
                                   GangMembership = ggg.Description,
                                   ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                                   Child_Allocation_Id = ca.Child_Allocation_Id,
                                   Person_Id = p.Person_Id,
                                   User_Id = ca.User_Id,
                                   Child_First_Name = p.First_Name,
                                   Child_Last_First_Name = p.Last_Name,
                                   Child_ID_No = p.Identification_Number,
                                   LoggedInUserName = currentUser.First_Name + " " + currentUser.Last_Name,
                                   Child_Name = p.First_Name + " " + p.Last_Name,
                                   Date_Transferred = ca.Date_Allocated,
                                   Child_Status = "1",
                                   OtherGangDescription = a.OtherGangMemberDescription
                               }).ToList();

            return allChildren;
        }
         public IQueryable<CYCAChildAllocationViewModel> GetActiveChildrenInFacility(int FacilityId)
        {
            var ACtiveChildren =
               (from p in db.Persons
                join c in db.Clients on p.Person_Id equals c.Person_Id
                join add in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = FacilityId } equals new { client = add.Client_Id, facility = add.Facility_Id }
                join disc in db.CYCA_Admissions_Discharge on add.Admission_Id equals disc.AdmissionId into adddisc
                from disc in adddisc.DefaultIfEmpty()
                where disc == null
                select new CYCAChildAllocationViewModel
                {
                    Person_Id = p.Person_Id,
                    Child_First_Name = p.First_Name,
                    Child_Last_First_Name = p.Last_Name,
                    Child_ID_No = p.Identification_Number,
                    Child_Name = p.First_Name + " " + p.Last_Name,
                });
            return ACtiveChildren;
        }

        //Get active children with no assignments
        public void ActiveChildrenWithNoAssignmentsFormat(int facilityID, User currentUser)
        {
            var children = getAllChildren(facilityID, currentUser).ToList();
            var nonAssigned = children.Where(x => x.Child_Allocation_Id == 0);
        }
        public List<CYCAChildAllocationViewModel> getActiveChildrenWithNoAssignments(int facilityID, User currentUser)
        {
            //First Get all Kids with no active Allocation
            var UnAssignedKids = (from a in db.CYCA_Admissions_AdmissionDetails
                                  join ca in db.CYCA_Child_Allocation on a.Admission_Id equals ca.Admission_Id
                                  into acca
                                  from ca in acca.DefaultIfEmpty()
                                  where a.Facility_Id == facilityID && a.Is_Active == true && ca ==null
                                  select a);


            var cc = (from u in UnAssignedKids
                      join c in db.Clients on u.Client_Id equals c.Client_Id
                      join p in db.Persons on c.Person_Id equals p.Person_Id
                      join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                      from imleft in pim.DefaultIfEmpty()
                      join gg in db.CYCA_Admissions_GangMembership on u.Admission_Id equals gg.Admission_Id where gg.Is_Active == true
                      join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                      where gg.ReAdmission_Id == null
                      select new CYCAChildAllocationViewModel
                      {
                          GangMembership = ggg.Description,
                          ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                          Person_Id = p.Person_Id,
                          Child_First_Name = p.First_Name,
                          Child_Last_First_Name = p.Last_Name,
                          Child_ID_No = p.Identification_Number,
                          //LoggedInUserName = u.First_Name + " " + u.Last_Name,
                          Child_Name = "*" + p.First_Name + " " + p.Last_Name,
                          OtherGangDescription = gg.OtherGangMemberDescription,
                      }).Distinct().ToList();

            var children =
              (from p in db.Persons
               join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
               from imleft in pim.DefaultIfEmpty()
               join c in db.Clients on p.Person_Id equals c.Person_Id
               //join a in db.CYCA_Admissions_AdmissionDetails on c.Client_Id equals a.Client_Id
               join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }           
               join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
               where gg.Is_Active == true
               join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
               join ca in db.CYCA_Child_Allocation on new { person = p.Person_Id, admission = a.Admission_Id } equals new { person = ca.Person_Id ?? 0, admission = ca.Admission_Id ?? 0 }
               into acca
               from ca in acca.DefaultIfEmpty()
               where ca == null && gg.ReAdmission_Id == null
               select new CYCAChildAllocationViewModel
               {
                   GangMembership = ggg.Description,
                   ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                   Person_Id = p.Person_Id,
                   Child_First_Name = p.First_Name,
                   Child_Last_First_Name = p.Last_Name,
                   Child_ID_No = p.Identification_Number,
                   //LoggedInUserName = u.First_Name + " " + u.Last_Name,
                   Child_Name = "*" + p.First_Name + " " + p.Last_Name,
                   OtherGangDescription = gg.OtherGangMemberDescription
               }).Distinct().ToList();

            return children;
        }
        public List<CYCAChildAllocationViewModel> getListP(int facilityID,int userId)
        {
            var children =
               (from p in db.Persons
                join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                from imleft in pim.DefaultIfEmpty()
                join cc in db.Clients on p.Person_Id equals cc.Person_Id
                join a in db.CYCA_Admissions_AdmissionDetails on new { client = cc.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                where gg.Is_Active
                join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                join ca in db.CYCA_Child_Allocation on a.Admission_Id equals ca.Admission_Id
                join u in db.Users on ca.User_Id equals u.User_Id
                where u.User_Id == userId && gg.ReAdmission_Id == null
                select new CYCAChildAllocationViewModel
                {
                    GangMembership = ggg.Description,
                    ImgUrl = imleft.Image_Filename ?? "/images/unknown.png"??imleft.Image_Filename , 
                    Child_Allocation_Id = ca.Child_Allocation_Id,
                    Person_Id = p.Person_Id,
                    Child_First_Name = p.First_Name,
                    Child_Last_First_Name = p.Last_Name,
                    Child_ID_No = p.Identification_Number,
                    LoggedInUserName = u.First_Name + " " + u.Last_Name,
                    Child_Name = p.First_Name + " " + p.Last_Name,
                    Date_Transferred = ca.Date_Allocated,
                    OtherGangDescription = gg.OtherGangMemberDescription
                }).ToList();

            return children;
        }
        //get ListP for HomeController.getTeamLeaderChildren()
        //Get All Children in facility that are assisgned to someone except the center manager
        public List<CYCAChildAllocationViewModel> getListPForTeamLeaderChildren(int userId, int facilityID)
        {
           var listP = (from p in db.Persons
                       join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                       from imleft in pim.DefaultIfEmpty()
                       join ca in db.CYCA_Child_Allocation on p.Person_Id equals ca.Person_Id
                       join a in db.CYCA_Admissions_AdmissionDetails on new { admission = ca.Admission_Id ?? 0, facility = facilityID, active = true } equals new { admission = a.Admission_Id, facility = a.Facility_Id, active = a.Is_Active }
                       join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id where gg.Is_Active == true
                        join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                        join cc in db.Clients on p.Person_Id equals cc.Person_Id
                       join u in db.Users on ca.User_Id equals u.User_Id
                       where u.User_Id == userId && gg.ReAdmission_Id == null
                       select new CYCAChildAllocationViewModel
                       {
                           GangMembership = ggg.Description,
                           ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                           Child_Allocation_Id = ca.Child_Allocation_Id,
                           Person_Id = p.Person_Id,
                           Child_First_Name = p.First_Name,
                           Child_Last_First_Name = p.Last_Name,
                           Child_ID_No = p.Identification_Number,
                           LoggedInUserName = u.First_Name + " " + u.Last_Name,
                           Child_Name = p.First_Name + " " + p.Last_Name,
                           Date_Transferred = ca.Date_Allocated,
                           OtherGangDescription = gg.OtherGangMemberDescription
                       }).ToList();

            return listP;
        }
      
        public List<CYCAChildAllocationViewModel> getChildrenAssignedOtherThanCentreManager(int facilityID, int userId, User currentUser)
                {
                   var children = getAllChildren(facilityID, currentUser).Where(x => x.User_Id != userId).ToList();

                /*var allChildren = (from p in db.Persons
                                    join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                                    from imleft in pim.DefaultIfEmpty()
                                    join c in db.Clients on p.Person_Id equals c.Person_Id
                                    //Active Admissions for Facility
                                    join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                                    join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                                    join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                                    join ca in db.CYCA_Child_Allocation on new { person = p.Person_Id, admission = a.Admission_Id } equals new { person = ca.Person_Id ?? 0, admission = ca.Admission_Id ?? 0 }
                                    where ca.User_Id != userId
                                    select new CYCAChildAllocationViewModel
                                    {
                                        GangMembership = ggg.Description,
                                        ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                                        Child_Allocation_Id = ca.Child_Allocation_Id,
                                        Person_Id = p.Person_Id,
                                        User_Id = ca.User_Id,
                                        Child_First_Name = p.First_Name,
                                        Child_Last_First_Name = p.Last_Name,
                                        Child_ID_No = p.Identification_Number,
                                        LoggedInUserName = currentUser.First_Name + " " + currentUser.Last_Name,
                                        Child_Name = p.First_Name + " " + p.Last_Name,
                                        Date_Transferred = ca.Date_Allocated,
                                        Child_Status = "1"
                                    }).ToList();*/
            return children;
                }

        public List<CYCAChildAllocationViewModel> getListByMe(int userId, int facilityID)
        {
            var listByMe = (from c in db.CYCA_Child_Transfer
                            join p in db.Persons on c.Person_Id equals p.Person_Id
                            join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                            from imleft in pim.DefaultIfEmpty()
                            join cc in db.Clients on p.Person_Id equals cc.Person_Id
                            join a in db.CYCA_Admissions_AdmissionDetails on new { client = cc.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                            join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                            where gg.Is_Active
                            join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                            join u in db.Users on c.Tranferred_To equals u.User_Id
                            where c.Transferred_By == userId && c.Tansfer_Status_Id == 1 && gg.ReAdmission_Id == null
                            select new CYCAChildAllocationViewModel
                            {
                                GangMembership = ggg.Description,
                                ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                                Transfer_Id = c.Transfer_Id,
                                Transferred_By = c.Transferred_By,
                                Person_Id = c.Person_Id,
                                Child_First_Name = p.First_Name,
                                Child_Last_First_Name = p.Last_Name,
                                Child_ID_No = p.Identification_Number,
                                Child_Name = p.First_Name + " " + p.Last_Name,
                                Date_Transferred = c.Date_Created,
                                transferredbyName = u.First_Name + " " + u.Last_Name,
                                OtherGangDescription = gg.OtherGangMemberDescription


                            }).ToList();

            return listByMe;
        }
        public List<CYCAChildAllocationViewModel> getListToMe(int userId, int facilityID)
        {
            var childrenListToMe = (from c in db.CYCA_Child_Transfer
                            join p in db.Persons on c.Person_Id equals p.Person_Id
                            join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                            from imleft in pim.DefaultIfEmpty()
                            join cc in db.Clients on p.Person_Id equals cc.Person_Id
                            join a in db.CYCA_Admissions_AdmissionDetails on new { client = cc.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                            join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                                    where gg.Is_Active
                                    join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                            join u in db.Users on c.Transferred_By equals u.User_Id
                            where c.Tranferred_To == userId && c.Tansfer_Status_Id == 1 && gg.ReAdmission_Id == null
                            select new CYCAChildAllocationViewModel
                            {
                                ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                                GangMembership = ggg.Description,
                                Transfer_Id = c.Transfer_Id,
                                Transferred_By = c.Transferred_By,
                                Person_Id = c.Person_Id,
                                Child_First_Name = p.First_Name,
                                Child_Last_First_Name = p.Last_Name,
                                Child_ID_No = p.Identification_Number,
                                Child_Name = p.First_Name + " " + p.Last_Name,
                                Date_Transferred = c.Date_Created,
                                transferredbyName = u.First_Name + " " + u.Last_Name,
                                OtherGangDescription = gg.OtherGangMemberDescription
                            }).ToList();

            return childrenListToMe;
        }
        public CYCA_Child_Allocation getChildAllocation(int TransferId)
        {
            //var ca = (from a in db.CYCA_Child_Allocation
            //          join p in db.Persons on a.Person_Id equals p.Person_Id
            //          join cc in db.Clients on p.Person_Id equals cc.Person_Id
            //          join tt in db.CYCA_Child_Transfer on p.Person_Id equals tt.Person_Id
            //          where tt.Transfer_Id == TransferId
            //          select a).SingleOrDefault();
            var ca = (from tt in db.CYCA_Child_Transfer
                      join p in db.Persons on tt.Person_Id equals p.Person_Id
                      join cc in db.Clients on p.Person_Id equals cc.Person_Id
                      join a in db.CYCA_Child_Allocation on new {admission=tt.Admission_Id,child=tt.Person_Id,active=true} equals new {admission=a.Admission_Id,child=a.Person_Id,active=a.Is_Active}
                      where tt.Transfer_Id == TransferId
                      select a).FirstOrDefault();
            return ca;
        }
        public CYCA_Child_Transfer GetChildTranfer(int TransferId)
        {
            return db.CYCA_Child_Transfer.Where(tt => tt.Transfer_Id == TransferId).SingleOrDefault();
        }
        public List<CYCAChildAllocationViewModel> getTeamLeaderChildren(int UserId,int facilityId)
        {

            //from p in db.Persons
            //join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
            //from imleft in pim.DefaultIfEmpty()
            //join c in db.Clients on p.Person_Id equals c.Person_Id
            ////Active Admissions for Facility
            //join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
            //join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
            //join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
            //join ca in db.CYCA_Child_Allocation on new { person = p.Person_Id, admission = a.Admission_Id } equals new { person = ca.Person_Id ?? 0, admission = ca.Admission_Id ?? 0 }
            //where gg.ReAdmission_Id == null


            var children = (from p in db.Persons
                            join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                            from imleft in pim.DefaultIfEmpty()
                            join c in db.Clients on p.Person_Id equals c.Person_Id
                            join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityId, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                            join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                            where gg.Is_Active
                            join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                            join ca in db.CYCA_Child_Allocation on a.Admission_Id equals ca.Admission_Id
                            join u in db.Users on ca.User_Id equals u.User_Id
                            //join cc in db.Clients on p.Person_Id equals cc.Person_Id
                            where ca.User_Id == UserId && gg.ReAdmission_Id == null
                            select new CYCAChildAllocationViewModel
                            {
                                GangMembership = ggg.Description,
                                ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                                Child_Allocation_Id = ca.Child_Allocation_Id,
                                Person_Id = p.Person_Id,
                                User_Id = ca.User_Id,
                                Child_First_Name = p.First_Name,
                                Child_Last_First_Name = p.Last_Name,
                                Child_ID_No = p.Identification_Number,
                                //LoggedInUserName = currentUser.First_Name + " " + currentUser.Last_Name,
                                Child_Name = p.First_Name + " " + p.Last_Name,
                                Date_Transferred = ca.Date_Allocated,
                                Child_Status = "1",
                                OtherGangDescription = gg.OtherGangMemberDescription
                            }).ToList();
            return children;
        }
        public List<CYCAChildAllocationViewModel> getSupervisorChildren(int UserId)
        {
            var children = (from p in db.Persons
                            join ca in db.CYCA_Child_Allocation on p.Person_Id equals ca.Person_Id
                            join a in db.CYCA_Admissions_AdmissionDetails on new { admission = ca.Admission_Id ?? 0, active = true } equals new { admission = a.Admission_Id, active = a.Is_Active }
                            join u in db.Users on ca.User_Id equals u.User_Id
                            join e in db.Employees on u.User_Id equals e.User_Id
                            join cc in db.Clients on p.Person_Id equals cc.Person_Id
                            where e.CYCA_Supervisor == UserId
                            select new CYCAChildAllocationViewModel
                            {
                                Child_Allocation_Id = ca.Child_Allocation_Id,
                                Person_Id = p.Person_Id,
                                Child_First_Name = p.First_Name,
                                Child_Last_First_Name = p.Last_Name,
                                Child_ID_No = p.Identification_Number,
                                LoggedInUserName = u.First_Name + " " + u.Last_Name,
                                Child_Name = p.First_Name + " " + p.Last_Name,
                                Date_Transferred = ca.Date_Allocated,
                                Child_Status = "2",
                                OtherGangDescription = a.OtherGangMemberDescription
                            }).ToList();
            return children;
        }
        public List<CYCAChildAllocationViewModel> getCareWorkerChildren(int facilityID, User currentUser, int UserId)
        {
            var children = (from p in db.Persons
                           join im in db.Person_Images on p.Person_Id equals im.Person_Id into pim
                           from imleft in pim.DefaultIfEmpty()
                           join c in db.Clients on p.Person_Id equals c.Person_Id
                           //Active Admissions for Facility
                           join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityID, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                           join gg in db.CYCA_Admissions_GangMembership on a.Admission_Id equals gg.Admission_Id
                            where gg.Is_Active
                            join ggg in db.apl_Cyca_Gang_Membership_Type on gg.Gang_Membership_Type_Id equals ggg.Gang_Membership_Type_Id
                           join ca in db.CYCA_Child_Allocation on new { person = p.Person_Id, admission = a.Admission_Id } equals new { person = ca.Person_Id ?? 0, admission = ca.Admission_Id ?? 0 }
                           where ca.User_Id == UserId && gg.ReAdmission_Id == null
                            select new CYCAChildAllocationViewModel
                           {
                               GangMembership = ggg.Description,
                               ImgUrl = imleft.Image_Filename ?? "/images/unknown.png",
                               Child_Allocation_Id = ca.Child_Allocation_Id,
                               Person_Id = p.Person_Id,
                               User_Id = ca.User_Id,
                               Child_First_Name = p.First_Name,
                               Child_Last_First_Name = p.Last_Name,
                               Child_ID_No = p.Identification_Number,
                               LoggedInUserName = currentUser.First_Name + " " + currentUser.Last_Name,
                               Child_Name = p.First_Name + " " + p.Last_Name,
                               Date_Transferred = ca.Date_Allocated,
                               Child_Status = "1",
                                OtherGangDescription = gg.OtherGangMemberDescription
                            }).ToList();
            return children;
        }
        public List<CYCA_Child_Transfer> getPendingList(int person_Id)
        {
            var children = (from c in db.CYCA_Child_Transfer
                            join p in db.Persons on c.Person_Id equals p.Person_Id
                            join cc in db.Clients on p.Person_Id equals cc.Person_Id
                            join u in db.Users on c.Transferred_By equals u.User_Id
                            where  person_Id == p.Person_Id  && c.Tansfer_Status_Id == 1
                            select c).ToList();
            return children;
        }
        public CYCA_Child_Allocation GetChildByAllocationId(int Child_Allocation_Id)
        {
            return db.CYCA_Child_Allocation.Where(ca => ca.Child_Allocation_Id == Child_Allocation_Id).Single();
        }
        public CYCA_Child_Allocation GetChildByAdmissionId(int AdmissionId)
        {
            return db.CYCA_Child_Allocation.Where(ca => ca.Admission_Id == AdmissionId).OrderByDescending(ca=>ca.Child_Allocation_Id).FirstOrDefault();
        }

        public List<apl_Cyca_Transfer_Status> GetTransferStatusList()
        {
            return db.apl_Cyca_Transfer_Status.ToList();
        }
        public void removeChild(CYCA_Child_Allocation child)
        {
            db.CYCA_Child_Allocation.Remove(child);
        }
        public void SaveChildrenChanges()
        {
            db.SaveChanges();
        }
        public IQueryable<dynamic> GetDishcargeHistoryByClienIDint(int clientId)
        {


            var data =  (from hist in db.CYCA_Admissions_Discharge
                    join add in db.CYCA_Admissions_AdmissionDetails on hist.AdmissionId equals add.Admission_Id
                    join child in db.Clients on add.Client_Id equals child.Client_Id
                    join person in db.Persons on child.Person_Id equals person.Person_Id
                    join empOver in db.Users on hist.UserHandedOverId equals empOver.User_Id
                    join userOver in db.Employees on empOver.User_Id equals userOver.User_Id
                    join empOverPos in db.Job_Positions on userOver.Job_Position_Id equals empOverPos.Job_Position_Id
                    join empReceive in db.Employees on hist.UserHandedOverId equals empReceive.User_Id
                    join empRecivedPos in db.Job_Positions on hist.PersonReceivingDesignationId equals empRecivedPos.Job_Position_Id
                    join rOrg in db.Organizations on empReceive.Organization_Id equals rOrg.Organization_Id
                    join reason in db.apl_Cyca_Admission_Discharge_Reason on hist.DischargeReasonId equals reason.DischargeReasonId
                    where add.Client_Id == clientId
                    select new 
                    {
                        admission_ID = add.Admission_Id,
                        clientID = add.Client_Id,
                        childFullName = person.First_Name + " " + person.Last_Name,
                        personID = person.Person_Id,
                        userHandedOverId = hist.UserHandedOverId,
                        selectedUserHandedOver = empOver.User_Name,
                        UserHandedOverDesignationId = userOver.Job_Position_Id,
                        selectedUserHandedOverDesignation = empOverPos.Description,
                        UserReceivedDesignationId = hist.PersonReceivingDesignationId, 
                        selectedUserReceivedDesignation = empRecivedPos.Description,
                        userReceivedName = hist.PersonReceivingName,
                        dischargeReasonId = hist.DischargeReasonId,
                        selectedDischargeReason = reason.Description,
                        dischargeDate = hist.DischargeDate,
                        keepBedSpace = hist.KeepBedSpace,
                        expectedReturnDate = hist.ReturnDate,
                        comments = hist.Comments,
                        userReceivedOrganisationId = empReceive.Organization_Id,
                        selectedUserReceivedOrganisation = rOrg.Description
                    });
            return data;
    }
        public List<CYCA_Admissions_Document> GetFilesByAdmissionID(int admissionID)
        {
            //Get files mathing to discharge doc
            return db.CYCA_Admissions_Document.Where(i => i.Admission_Id == admissionID && i.ReAdmission_Id == null && i.DischargeId == null).ToList();              
        }
        public List<CYCA_Admissions_Document> GetFilesByDischargeID(int dischargeID)
        {

            ////Get files mathing to discharge doc
            return db.CYCA_Admissions_Document.Where(i => i.DischargeId == dischargeID).ToList();

        }

        public List<CYCA_Admissions_Document> GetFilesByReAdmissionID(int readmissionID)
        {
            //Get files mathing to readmission doc
            return db.CYCA_Admissions_Document.Where(i => i.ReAdmission_Id == readmissionID).ToList();
        }

        public List<CYCA_BodilySearch_Document> GetFilesByBodySearchID(int bodysearchID)
        {


            return db.CYCA_BodilySearch_Document.Where(i => i.Bodily_Search_Id == bodysearchID).ToList();
        }
        public List<CYCA_IllegalItems_Document> GetFilesByIllegalItemID(int illegalitemID)
        {

            return db.CYCA_IllegalItems_Document.Where(i => i.Item_Found_Id == illegalitemID).ToList();
        }
    }
}
