using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PCMDiversionModel
    {
        public void DiversionAdd(PCMDiversionViewModel vm, int userid, int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Diversion DiversionNew = db.PCM_Diversion.Find(Intake_Assessment_Id);
                    if (DiversionNew != null)
                    {

                        DiversionNew.Intake_Assessment_Id = vm.Intake_Assessment_Id;
                        DiversionNew.Source_Referral_Id = vm.Source_Referral;
                        DiversionNew.Province_Id = vm.Province_Id;
                        DiversionNew.Service_Provider_Category = vm.Service_Provider_Category;
                        DiversionNew.Services_Provider_Id = vm.Services_Provider_Id;
                        DiversionNew.Programme_id = vm.Programme_Id;
                        DiversionNew.No_Modules = vm.No_Modules;
                        DiversionNew.Programme_Level_Id = vm.Programme_Level_Id;
                        DiversionNew.Created_By = userid;
                        DiversionNew.Date_Created = DateTime.Now;
                        DiversionNew.Modified_By = userid;
                        DiversionNew.Date_Modified = DateTime.Now;
                 
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
            }
        }

        



        #region RECOMMENDED DIVESIONS
       

        public void PCM_Diversion_SPAddnew(PCMDiversionViewModel vm,int Intake_Assessment_Id, int userid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Diversion _SP = new PCM_Diversion();
                    _SP.Intake_Assessment_Id = Intake_Assessment_Id;
                    _SP.Source_Referral_Id = vm.Source_Referral;
                    _SP.Programme_id = vm.Programme_Id;
                    _SP.Created_By = userid;
                    _SP.Childrens_Court_Outcome_Id = vm.Childrens_Court_Outcome_Id;
                    _SP.PCM_Preliminary_Id = vm.PCM_Preliminary_Id;
                    _SP.Formal_Courtcome_Id = vm.Formal_Courtcome_Id;
                    _SP.Court_Type_Id = vm.Court_Type_Id;
                    _SP.Diversion_Id = vm.Diversion_Id;
                    
                    _SP.Date_Created = DateTime.Now;

                    db.PCM_Diversion.Add(_SP);
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

        public void UpdatePCMDivesionsDeatils(PCMDiversionViewModel vm, int Diversion_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Diversion DiversionNew = db.PCM_Diversion.Find(Diversion_Id);
                    if (DiversionNew != null)
                    {

                        
                        DiversionNew.Source_Referral_Id = vm.Source_Referral;
                        DiversionNew.Services_Provider_Id = vm.Services_Provider_Id;
                        DiversionNew.Programme_id = vm.Programme_Id;
                        DiversionNew.No_Modules = vm.No_Modules;
                        DiversionNew.Programme_Level_Id = vm.Programme_Level_Id;
                        DiversionNew.Date_Modified = DateTime.Now;
                        DiversionNew.Modified_By = userId;
                        db.SaveChanges();
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

        public List<PCMDiversionViewModel> GetSelectedDivesionFromDBDiv(int Intake_Assessment_Id)
        {

            List<PCMDiversionViewModel> avm = new List<PCMDiversionViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var orderList =
                (from r in db.PCM_Diversion
                 join rt in db.Intake_Assessments on r.Intake_Assessment_Id equals rt.Intake_Assessment_Id
                 join ot in db.apl_Programmes on r.Programme_id equals ot.Programme_Id into empty1
                 from ed in empty1.DefaultIfEmpty()

                 
                 join so in db.apl_PCM_Referral_Source on r.Source_Referral_Id equals so.Source_Referral_Id into empty2
                 from ed1 in empty2.DefaultIfEmpty()
                 join co in db.apl_PCM_Court_Type on r.Court_Type_Id equals co.Court_Type_Id into empty3
                 from ed3 in empty3.DefaultIfEmpty()
                 join le in db.apl_Programme_Level on r.Programme_Level_Id equals le.Programme_Level_Id into empty4
                 from ed4 in empty4.DefaultIfEmpty()

                 where (r.Intake_Assessment_Id == Intake_Assessment_Id)
                 select new
                 {
                     r.Intake_Assessment_Id,
                     Programme_Id = ed == null ? 0 : ed.Programme_Id,
                     Source_Referral_Id= ed1==null?0 : ed1.Source_Referral_Id,
                     r.Diversion_Id,
                     Court_Type_Id=ed3==null? 0: ed3.Court_Type_Id,
                     r.Childrens_Court_Outcome_Id,
                     r.Formal_Courtcome_Id,
                     r.PCM_Preliminary_Id,
                     Programme_Level_Id = ed4==null? 0: ed4.Programme_Level_Id
                 }).ToList();
            ;
            foreach (var item in orderList)
            {

                // initialising view model
                PCMDiversionViewModel obj = new PCMDiversionViewModel();

                obj.Diversion_Id = item.Diversion_Id;
                if (item.Programme_Id != 0)
                {
                    obj.DesrciptionDivesionPrograme = db.apl_Programmes.Find(item.Programme_Id).Programme_Name;

                }
                else
                {
                    obj.DesrciptionDivesionPrograme = "None";

                }
                obj.Court_Type_Id = item.Court_Type_Id;
                obj.DesrciptionCourttype = db.apl_PCM_Court_Type.Find(item.Court_Type_Id).Description;
                obj.PCM_Preliminary_Id = item.PCM_Preliminary_Id;
                obj.Formal_Courtcome_Id = item.Formal_Courtcome_Id;
                obj.Childrens_Court_Outcome_Id = item.Childrens_Court_Outcome_Id;

                if (item.Source_Referral_Id != 0)
                {
                    obj.RefSourcedesrciption = db.apl_PCM_Referral_Source.Find(item.Source_Referral_Id).Description;

                }
                else
                {
                    obj.RefSourcedesrciption = "None";

                }
                if (item.Programme_Level_Id != 0)
                {
                    obj.Levels = db.apl_Programme_Level.Find(item.Programme_Level_Id).Description;

                }
                else
                {
                    obj.Levels = 0;

                }
                if (item.Childrens_Court_Outcome_Id > 0 || item.Childrens_Court_Outcome_Id != null)
                {

                    obj.CourtDate = db.PCM_Childrens_Court_Outcome.Find(item.Childrens_Court_Outcome_Id).Court_Date;
                }
                else if (item.PCM_Preliminary_Id > 0 || item.PCM_Preliminary_Id != null)
                {

                    obj.CourtDate = db.PCM_Preliminary_Details.Find(item.PCM_Preliminary_Id).PCM_Preliminary_Date;
                }
                else if (item.Formal_Courtcome_Id > 0 || item.Formal_Courtcome_Id != null)
                {
                    if(db.PCM_FCR_Outcome.Find(item.Formal_Courtcome_Id).CourtDate!=null)
                        {
                        obj.CourtDate = db.PCM_FCR_Outcome.Find(item.Formal_Courtcome_Id).CourtDate;
                    }
                   
                }
                else
                {
                    obj.CourtDate = null;

                }
                avm.Add(obj);
            }

            return avm;
        }

       
        public PCMDiversionViewModel GetDivesionFromDBDivbyId(int Diversion_Id)
        {
            PCMDiversionViewModel vm = new PCMDiversionViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Diversion act = db.PCM_Diversion.Find(Diversion_Id);

                    if (act != null)
                    {
                        vm.Diversion_Id = act.Diversion_Id;
                        vm.Source_Referral_Id = act.Source_Referral_Id;
                        if (act.Source_Referral_Id > 0)
                        {
                            vm.RefSourcedesrciption = db.apl_PCM_Referral_Source.Find(act.Source_Referral_Id).Description;
                        }
                       
                        vm.Programme_Id = act.Programme_id;
                        if (act.Programme_id > 0)
                        {
                            vm.DesrciptionDivesionPrograme = db.apl_Programmes.Find(act.Programme_id).Programme_Name;
                        }

                        vm.No_Modules = act.No_Modules;
                        vm.Programme_Level_Id = act.Programme_Level_Id;
                        vm.Modified_By = act.Modified_By;
                        vm.Date_Modified = act.Date_Modified;
                        vm.Court_Type_Id = act.Court_Type_Id;
                        vm.PCM_Preliminary_Id = act.PCM_Preliminary_Id;
                        vm.Formal_Courtcome_Id = act.Formal_Courtcome_Id;
                        vm.Childrens_Court_Outcome_Id = act.Childrens_Court_Outcome_Id;
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

        public void DeleteProgrammeRecord(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Diversion_Recommendation Obj = (from j in db.PCM_Diversion_Recommendation
                                                    where j.PCM_Diversion_Recomm_Id == id
                                                    select j).FirstOrDefault();
                db.PCM_Diversion_Recommendation.Remove(Obj);
                db.SaveChanges();
            }
        }

        #endregion



        public List<DescriptionTypeLookup> GetDescriptionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Description_Type = db.Provinces.Select(o => new DescriptionTypeLookup
                {
                    Province_Id = o.Province_Id,
                    Description = o.Description
                }).ToList();

                return Description_Type;
            }
        }

        public List<ServicesProviderNameTypeLookup> GetServicesProviderNameType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var ServicesProviderName_Type = db.PCM_D_ServicesProvider.Select(o => new ServicesProviderNameTypeLookup
                {
                    Services_Provider_Id = o.Services_Provider_Id,
                    Services_Provider_Name = o.Services_Provider_Name
                }).ToList();

                return ServicesProviderName_Type;
            }
        }

        public List<SourceReferralLookupPCM> GetReferralSource()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var r = db.apl_PCM_Referral_Source.Select(o => new SourceReferralLookupPCM
                {
                    Source_Referral_Id = o.Source_Referral_Id,
                    Description = o.Description
                }).ToList();

                return r;
            }
        }

        public List<DiversionProgrammesLookupPcm> GetAllDiversion_Programmes()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Diversion_Programmes.Select(o => new DiversionProgrammesLookupPcm
                {
                    Div_Program_Id = o.Div_Program_Id,
                    Programme_Name = o.Programme_Name
                }).ToList();

                return Type;
            }
        }
        public List<OrganisationTypeLookupPCM> GetAllOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupPCM
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }
        public List<LocalMunicipalityLookupAdopt> GetAllLocalMunicipality()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Local_Mun_id = db.Local_Municipalities.Select(o => new LocalMunicipalityLookupAdopt
                {
                    Local_Municipality_Id = o.Local_Municipality_Id,
                    Description = o.Description
                }).ToList();

                return Local_Mun_id;


            }
        }

        public List<OrganizationLookupPcm> GetAllPCMOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.Organizations.Select(o => new OrganizationLookupPcm
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }

        public List<RecomendationOrderLookupPcm> GetAllPCMOrders()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var or = db.apl_PCM_Orders.Select(o => new RecomendationOrderLookupPcm
                {
                     Recomendation_Order_Id= o.Recomendation_Order_Id,
                    Description = o.Description
                }).ToList();

                return or;
            }
        }

        public List<ProgrammeLevelLookupPCM> GetAllPCMProgrammeLevels()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var or = db.apl_Programme_Level.Select(o => new ProgrammeLevelLookupPCM
                {
                    Programme_Level_Id = o.Programme_Level_Id,
                    Description = o.Description
                }).ToList();

                return or;
            }
        }
        public List<ProvinceLookupPCM> GetAllProvinces()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Province_id = db.Provinces.Select(o => new ProvinceLookupPCM
                {
                    Province_Id = o.Province_Id,
                    Description = o.Description
                }).ToList();

                return Province_id;
            }
        }

        public List<DistrictLookupPCM> GetAllDistrict()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var District_id = db.Districts.Select(o => new DistrictLookupPCM
                {
                    District_Id = o.District_Id,
                    Description = o.Description
                }).ToList();

                return District_id;


            }
        }

        public List<ProgrammeLookupPCM> GetAllPrograms()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var p = db.apl_Programmes.Select(o => new ProgrammeLookupPCM
                {
                    Div_Program_Id = o.Programme_Id,
                    Programme_Name = o.Programme_Name
                }).ToList();

                return p;


            }
        }


    }
}
