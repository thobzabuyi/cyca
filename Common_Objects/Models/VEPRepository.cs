using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPRepository
    {
        private SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        public VEP_VictimsConditions UpdateVictimCondition(VictimRecord Obj, int Id)
        {
            int CheckIfExistVEP_VictimsConditions = (from x in dbContext.VEP_VictimsConditions
                                                     where x.Caseid == Id
                                                     select x.Id).FirstOrDefault();
            if (CheckIfExistVEP_VictimsConditions == 0)
            {
                VEP_VictimsConditions VCModel = new VEP_VictimsConditions();
                VCModel.Caseid = Id;
                //VCModel.Angry = Obj.VictimConditions.Angry;
                //VCModel.ExecivelyAlert = Obj.VictimConditions.ExecivelyAlert;
                //VCModel.FullyMobile = Obj.VictimConditions.FullyMobile;
                //VCModel.LookAfraid = Obj.VictimConditions.LookAfraid;
                //VCModel.Panic = Obj.VictimConditions.Panic;
                //VCModel.VictimsConditionsID = 1;
                dbContext.VEP_VictimsConditions.Add(VCModel);
                dbContext.SaveChanges();
                return VCModel;

            }
            else
            {
                VEP_VictimsConditions VCModel = (from k in dbContext.VEP_VictimsConditions
                                                 where k.Caseid == Id
                                                 select k).FirstOrDefault();
                VCModel.Caseid = Id;
                //VCModel.Angry = Obj.VictimConditions.Angry;
                //VCModel.ExecivelyAlert = Obj.VictimConditions.ExecivelyAlert;
                //VCModel.FullyMobile = Obj.VictimConditions.FullyMobile;
                //VCModel.LookAfraid = Obj.VictimConditions.LookAfraid;
                //VCModel.Panic = Obj.VictimConditions.Panic;
                //VCModel.VictimsConditionsID = 1;
                dbContext.SaveChanges();
                return VCModel;

            }
        }

        public int GetCaseId(int UseThis)
        {
            return (from c in dbContext.VEP_Cases
                    where c.ClientId == UseThis
                    select c.CaseId).FirstOrDefault();
        }

        public VEP_VictimsConditions GetVEP_VictimsConditions(int Case_Id)
        {
            return (from s in dbContext.VEP_VictimsConditions
                    where s.Caseid == Case_Id
                    select s).FirstOrDefault();
        }

        public VEP_IncidentInformation GetVEP_IncidentInformation(int Case_Id)
        {
            return (from s in dbContext.VEP_IncidentInformation
                    where s.Caseid == Case_Id
                    select s).FirstOrDefault();
        }
        public int GetVEP_IncidentInformationIncidentId(int Case_Id)
        {
            return (from s in dbContext.VEP_IncidentInformation
                    where s.Caseid == Case_Id
                    select s.IncidentID).FirstOrDefault();
        }

        public void UpdateIncidentInformation(VictimRecord Obj, int Id)
        {

            if (Obj.VictimTypeSelectedList!=null) { 


            for (int i =0; i < Obj.VictimTypeSelectedList.Count(); ++i)
            {

                int newInt = Obj.VictimTypeSelectedList[i];
                int Test = (from a in dbContext.VEP_VictimizationTypeDetails
                            where a.Case_Id == Id && a.VictimizationType == newInt
                            select a.Id).FirstOrDefault();
                if (Test == 0)
                {
                    VEP_VictimizationTypeDetails SubObj = new VEP_VictimizationTypeDetails();
                    SubObj.Case_Id = Id;
                    SubObj.VictimizationType = newInt;
                    SubObj.IsItSelected = true;
                    dbContext.VEP_VictimizationTypeDetails.Add(SubObj);

                    dbContext.SaveChanges();
                }
                if (Test > 0)
                {
                    VEP_VictimizationTypeDetails SubObj = (from p in dbContext.VEP_VictimizationTypeDetails
                                                          where p.Case_Id==Id && p.VictimizationType == newInt
                                                           select p).FirstOrDefault();
                    SubObj.Case_Id = Id;
                    SubObj.VictimizationType = newInt;
                    SubObj.IsItSelected = true;
                    dbContext.SaveChanges();
                }
            }
            }

            int CheckIfExistVEP_IncidentInformation = (from s in dbContext.VEP_IncidentInformation
                                                       where s.Caseid == Id
                                                       select s.IncidentID).FirstOrDefault();
            if (CheckIfExistVEP_IncidentInformation == 0)
            {
                VEP_IncidentInformation VCModel = new VEP_IncidentInformation();
                VCModel.Caseid = Id;
                VCModel.Incident_Details = Obj.IncidentInformation.IncidentDetails;
                (VCModel.Victimisation_Type) = Convert.ToString(Obj.IncidentInformation.VictimisationType);
                VCModel.PlaceofIncident = Obj.IncidentInformation.PlaceOfIncident;
                VCModel.TypeofSettlement = Convert.ToString(Obj.IncidentInformation.VEP_SettlementType);
                VCModel.DateofIncident = Obj.IncidentInformation.DateOfIncident;
                VCModel.DateofReporting = Obj.IncidentInformation.DateOfReporting;
                VCModel.PoliceCaseNumber = Obj.IncidentInformation.PoliceCaseNumber;
                VCModel.PoliceOBnumber = Obj.IncidentInformation.PoliceOBnumber;
                //VCModel.TypeofSettlement = dbContext.VEP_SettlementType.Find(Obj.IncidentInformation.SettlementId).Settlement;
                if (Obj.IncidentInformation.KnowPerpetrator == 1)
                {
                    VCModel.DoyouknowtheallegedPerpetrator = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.DoyouknowtheallegedPerpetrator = "No";
                    dbContext.SaveChanges();
                }

                if (Obj.IncidentInformation.PerpFamilyMemeber == 1)
                {
                    VCModel.Perpetratorrelatedtoyou = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.Perpetratorrelatedtoyou = "No";
                    dbContext.SaveChanges();
                }
                if (Obj.IncidentInformation.PerpCommunityMember == 1)
                {
                    VCModel.immediatecommunity = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.immediatecommunity = "No";
                    dbContext.SaveChanges();
                }
                if (Obj.IncidentInformation.ReportedToPolice == 1)
                {
                    VCModel.ReportedtothePolice = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.ReportedtothePolice = "No";
                    dbContext.SaveChanges();
                }
                //VCModel.immediatecommunity = Obj.IncidentInformation.PerpCommunityMember;
                //VCModel.ReportedtothePolice = Obj.IncidentInformation.ReportedToPolice;
                VCModel.PoliceCaseNumber = Obj.IncidentInformation.PoliceCaseNumber;
                VCModel.PoliceOBnumber = Obj.IncidentInformation.PoliceOBnumber;
                dbContext.VEP_IncidentInformation.Add(VCModel);
                dbContext.SaveChanges();

            }
            else
            {
                VEP_IncidentInformation VCModel = (from k in dbContext.VEP_IncidentInformation
                                                   where k.Caseid == Id
                                                   select k).FirstOrDefault();
                VCModel.Caseid = Id;
                VCModel.Incident_Details = Obj.IncidentInformation.IncidentDetails;
                VCModel.Victimisation_Type = Convert.ToString(Obj.IncidentInformation.VictimisationType);
                VCModel.PlaceofIncident = Obj.IncidentInformation.PlaceOfIncident;
                VCModel.DateofIncident = Obj.IncidentInformation.DateOfIncident;
                VCModel.DateofReporting = Obj.IncidentInformation.DateOfReporting;
                VCModel.DateofReporting = Obj.IncidentInformation.DateOfReporting;
                //VCModel.TypeofSettlement = dbContext.VEP_SettlementType.Find(Obj.IncidentInformation.SettlementId).Settlement;
                if (Obj.IncidentInformation.KnowPerpetrator == 1)
                {
                    VCModel.DoyouknowtheallegedPerpetrator = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.DoyouknowtheallegedPerpetrator = "No";
                    dbContext.SaveChanges();
                }
                if (Obj.IncidentInformation.PerpFamilyMemeber == 1)
                {
                    VCModel.Perpetratorrelatedtoyou = "Yes";
                    dbContext.SaveChanges();
                }
                else
                {
                    VCModel.Perpetratorrelatedtoyou = "No";
                    dbContext.SaveChanges();
                }
                //VCModel.immediatecommunity = Obj.IncidentInformation.;
                //VCModel.ReportedtothePolice = Obj.IncidentInformation.ReportedToPolice;
                VCModel.PoliceCaseNumber = Obj.IncidentInformation.PoliceCaseNumber;
                VCModel.PoliceOBnumber = Obj.IncidentInformation.PoliceOBnumber;
                dbContext.SaveChanges();
            }

        }

        public List<VEP_VictimizatimTypeListVM> GetListFromDB(int Id)
        {
            var InfFromTable= (from g in dbContext.VEP_VictimizationTypeDetails
                               where g.Case_Id == Id && g.IsItSelected==true
                               select g).ToList();
            List<VEP_VictimizatimTypeListVM> ReturnedObj = new List<VEP_VictimizatimTypeListVM>();
            foreach (var item in InfFromTable)
            {
                VEP_VictimizatimTypeListVM NewObj = new VEP_VictimizatimTypeListVM();
                NewObj.Id = item.Id;
                NewObj.Case_Id = item.Case_Id;
                NewObj.VictimizationType = dbContext.VEP_VictimizationType.Find(item.VictimizationType).VictimizationType;
                NewObj.IsChecked = item.IsItSelected;
                ReturnedObj.Add(NewObj);

            }
            return ReturnedObj;
        }


        public void DeleteRecord(string String, int id)
        {
            int UseThisId = (from t in dbContext.VEP_VictimizationType
                             where t.VictimizationType == String
                             select t.Id).FirstOrDefault();
            VEP_VictimizationTypeDetails Obj = (from j in dbContext.VEP_VictimizationTypeDetails
                                                where j.Case_Id == id && j.VictimizationType == UseThisId
                                                select j).FirstOrDefault();
            dbContext.VEP_VictimizationTypeDetails.Remove(Obj);
            dbContext.SaveChanges();
        }

        public List<int> GetListOfVictimTypeSelection(int Id)
        {
           return  (from r in dbContext.VEP_VictimizationTypeDetails
                        where r.Case_Id == Id
                        select r.VictimizationType).ToList();
        }

        public VEPServicesViewModel GetVEPServices(int Id)
        {
            var DataOnDB = (from a in dbContext.VEP_Services
                            where a.CaseId == Id
                            select a).FirstOrDefault();

            VEPServicesViewModel Model = new VEPServicesViewModel();
            if (DataOnDB != null)
            {
                Model.Serviceid = DataOnDB.ServiceId;
                Model.ServiceTypeid = DataOnDB.ServiceTypeId;
                Model.ServiceNotes = DataOnDB.ServiceNotes;
                Model.DateCreated = DataOnDB.DateCreated;
                Model.Caseid = DataOnDB.CaseId;
            }
            return Model;

        }

        public VEPReferalsViewModel GetVEPReferrals(int Id)
        {
            var DataOnDB = (from a in dbContext.VEP_Referals
                            where a.CaseId == Id
                            select a).FirstOrDefault();
            VEPReferalsViewModel Model = new VEPReferalsViewModel();

            //Model.CreateDate = DataOnDB.CreateDate;
            Model.CreateDate = DateTime.Now;
            Model.Createdby = DataOnDB.Createdby;
            //Model.CreatedByString = dbContext.Users.Find(DataOnDB.Createdby).User_Name;
            Model.CreatedByString = "";
            Model.FromDepartment = DataOnDB.FromDepartment.ToString();
            Model.ToDepartment = DataOnDB.ToDepartment.ToString();
            Model.Notes = DataOnDB.Notes;
            return Model;
        }

        public void UpdateReferrals(VEPReferalsViewModel vmodel)
        {
            var OldObj = (from c in dbContext.VEP_Referals
                          where c.CaseId == (vmodel.Caseid)
                          select c).FirstOrDefault();
            if (OldObj != null)
            {
                OldObj.CreateDate = vmodel.CreateDate;
                OldObj.Createdby = (from r in dbContext.Users
                                   where r.User_Name==vmodel.CreatedByString
                                   select r.User_Id).FirstOrDefault();
                OldObj.FromDepartment = Convert.ToInt32(vmodel.FromDepartment);
                OldObj.ToDepartment = Convert.ToInt32(vmodel.ToDepartment);
                OldObj.Notes = vmodel.Notes;
                dbContext.SaveChanges();
            }
            else
            {
                var NewObj = new VEP_Referals();
                NewObj.CreateDate = vmodel.CreateDate;
                NewObj.Createdby = vmodel.Createdby;
                NewObj.FromDepartment = Convert.ToInt32(vmodel.FromDepartment);
                NewObj.ToDepartment = Convert.ToInt32(vmodel.ToDepartment);
                NewObj.Notes = vmodel.Notes;
                NewObj.CaseId = vmodel.Caseid;
                //NewObj.CaseId = Convert.ToInt32(Session["CaseID"]);
                dbContext.VEP_Referals.Add(NewObj);
                dbContext.SaveChanges();
            }

        }

        public int GetSettleMentType(string test)
        {
            int Result = (from t in dbContext.VEP_SettlementType
                          where t.Settlement == test
                          select t.Id).FirstOrDefault();

            return Result;
        }

        public VEP_Services GetServiceById(int serviceId)
        {
            try
            {
                return dbContext.VEP_Services.Find(serviceId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public VEP_Referals GetReferralById(int referralId)
        {
            try
            {
                return dbContext.VEP_Referals.Find(referralId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int updateServiceDetails(int serviceId,string ServiceNotes)
        {
            VEP_Services serviceToUpdate;
           

            try
            {
                serviceToUpdate = (from s in dbContext.VEP_Services
                    where s.ServiceId.Equals(serviceId)
                    select s).FirstOrDefault();

                if (serviceToUpdate == null) return -1;

                serviceToUpdate.ServiceId = serviceId;
                serviceToUpdate.ServiceNotes = ServiceNotes;

                return dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public List<VEP_Referals> GetListOfSpecificReferrals(int caseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Referals.Where(a => a.CaseId == caseId).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public VEP_ReferralType GetSpecificReferralType(int id)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_ReferralType.Find(id);
            }
            catch (Exception e)
            {
                var Test = e.Message;
                return null;
            }
        }



    }
}
