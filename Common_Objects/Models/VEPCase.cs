using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPCase
    {
        public VEP_Cases GetSpecificServicese(int caseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Cases.SingleOrDefault(a => a.CaseId == caseId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public VEP_Cases GetSpecificByAssesmentId(int assessmentId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Cases.SingleOrDefault(a => a.ClientId == assessmentId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetCurrentUserProvince(int userId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {

                var currentProvince = (from province in dbContext.Provinces
                                       join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                                       join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                                       join serviceOffice in dbContext.Service_Offices on localMunicipality.Local_Municipality_Id equals serviceOffice.Local_Municipality_Id
                                       join employees in dbContext.Employees on serviceOffice.Service_Office_Id equals employees.Service_Office_Id
                                       join users in dbContext.Users on employees.User_Id equals users.User_Id
                                       where users.User_Id == userId
                                       select new NationalStatsGrid { provinceName = province.Abbreviation }).FirstOrDefault();



                return currentProvince.provinceName;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public int GetListOfCase()
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var maxId1 = (from r in dbContext.VEP_Cases
                              where r.VEPReference != null
                              orderby r.CaseId descending
                              select r.CaseId).ToList();
                var newId = maxId1 == null ? 1 : maxId1.FirstOrDefault() + 1;
                return newId;

            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }

        }

        public int Create(VEPCaseViewModel model)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                    VEP_Cases vep_case = new VEP_Cases();

                    vep_case.ClientId = model.ClientId;
                    vep_case.IncidentDetails = model.IncidentDetails;
                    vep_case.PlaceOfIncident = model.PlaceOfIncident;
                    vep_case.SettlementId = model.SettlementId;
                    vep_case.DateOfIncident = model.DateOfIncident;
                    vep_case.DateOfReporting = model.DateOfReporting;
                    vep_case.KnowPerpetrator = model.KnowPerpetrator;
                    vep_case.PerpFamilyMemeber = model.PerpFamilyMemeber;
                    vep_case.PerpCommunityMember = model.PerpCommunityMember;
                    vep_case.ReportedToPolice = model.ReportedToPolice;
                    vep_case.PoliceCaseNumber = model.PoliceCaseNumber;
                    vep_case.PoliceOBnumber = model.PoliceOBnumber;
                    //vep_case.VictimisationType = model.VictimisationType;
                    vep_case.VEPReference = model.VEPReference;
                    //vep_case.DateCaptured = model.DateCaptured;

                    dbContext.VEP_Cases.Add(vep_case);
                    dbContext.SaveChanges();

                    return vep_case.CaseId;
               
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        public int Update(VEPCaseViewModel model,int caseID)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var vep_case = dbContext.VEP_Cases.SingleOrDefault(a => a.CaseId == caseID);

                vep_case.ClientId = model.ClientId;
                vep_case.IncidentDetails = model.IncidentDetails;
                vep_case.PlaceOfIncident = model.PlaceOfIncident;
                vep_case.SettlementId = model.SettlementId;
                vep_case.DateOfIncident = model.DateOfIncident;
                vep_case.DateOfReporting = model.DateOfReporting;
                vep_case.KnowPerpetrator = model.KnowPerpetrator;
                vep_case.PerpFamilyMemeber = model.PerpFamilyMemeber;
                vep_case.PerpCommunityMember = model.PerpCommunityMember;
                vep_case.ReportedToPolice = model.ReportedToPolice;
                vep_case.PoliceCaseNumber = model.PoliceCaseNumber;
                vep_case.PoliceOBnumber = model.PoliceOBnumber;
                vep_case.SexualOrientationId = model.SexualOrientationId;
                //vep_case.VictimisationType = model.VictimisationType;
                vep_case.VEPReference = model.VEPReference;
                //vep_case.DateCaptured = model.DateCaptured;

                dbContext.SaveChanges();

                return vep_case.CaseId;

            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        //VEP Reporting

        public IEnumerable<NationalStatsGrid> GetNationalCasesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                //return (from province in dbContext.Provinces
                //        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                //        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                //        join serviceOffice in dbContext.Service_Offices on localMunicipality.Local_Municipality_Id equals serviceOffice.Local_Municipality_Id
                //        join employees in dbContext.Employees on serviceOffice.Service_Office_Id equals employees.Service_Office_Id
                //        join users in dbContext.Users on employees.User_Id equals users.User_Id
                //        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                //        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                //        group province by province.Description into g
                //        select new NationalStatsGrid { provinceName = g.Key, totalVictimsCaptured = g.Count() }).ToList();
                return (from province in dbContext.Provinces
                        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        group province by province.Description into g
                        select new NationalStatsGrid { provinceName = g.Key, totalVictimsCaptured = g.Count() }).ToList();

                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return (from province in dbContext.Provinces
                        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                        join sites in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals sites.Local_Municipality_Id
                        join employees in dbContext.Employees on sites.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        group sites by sites.Organization_Id into g
                        select new NationalStatsGrid { siteId = g.Key, totalVictimsCaptured = g.Count() }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetTotalNationalSitesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return (from province in dbContext.Provinces
                        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                        join sites in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals sites.Local_Municipality_Id
                        join employees in dbContext.Employees on sites.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        group sites by sites.Organization_Id into g
                        select new NationalStatsGrid { siteId = g.Key, totalVictimsCaptured = g.Count() }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesCasesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return (from site in dbContext.Organizations
                        join employees in dbContext.Employees on site.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                        group site by site.Site_Code into g
                        select new NationalStatsGrid { siteCode = g.Key }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesVictimCasesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return (from site in dbContext.Organizations
                        join employees in dbContext.Employees on site.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                        group site by site.Site_Code into g
                        select new NationalStatsGrid { siteCode = g.Key,totalVictimsCaptured = g.Count() }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesServicesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return (from site in dbContext.Organizations
                        join employees in dbContext.Employees on site.Organization_Id equals employees.Organization_Id
                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                        group site by site.Site_Code into g
                        select new NationalStatsGrid { siteCode = g.Key, totalServicesRendered = g.Count() }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesMaleStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList =  (from cases in dbContext.VEP_Cases
                                        join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                        join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                        join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                        join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                        join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                        join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                        join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                        join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                        join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                        join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                        join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                        join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                        where gender.Gender_Id == 1
                                        select new NationalStatsGrid { totalMales = person.Person_Id, siteCode = site.Site_Code  }).Distinct().GroupBy(g => g.siteCode).ToList();

                
                return distictMaleList.Select(m => new NationalStatsGrid {
                    siteCode = m.Key, totalMales = m.Count()
                });

                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesFemaleStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                       where gender.Gender_Id == 2
                                       select new NationalStatsGrid { totalFemales = person.Person_Id, siteCode = site.Site_Code }).Distinct().GroupBy(g => g.siteCode).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    siteCode = m.Key,
                    totalFemales = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesHeteroSexualStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       where orientation.Sexual_Orientation_Id == 1
                                       select new NationalStatsGrid { totalHeterosexual = person.Person_Id, siteCode = site.Site_Code }).Distinct().GroupBy(g => g.siteCode).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    siteCode = m.Key,
                    totalHeterosexual = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalSitesLGBTIQSexualStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from site in dbContext.Organizations
                                       join employees in dbContext.Employees on site.Organization_Id equals employees.Organization_Id
                                       join users in dbContext.Users on employees.User_Id equals users.User_Id
                                       join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                                       join client in dbContext.Clients on assesment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join sexualOrentation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals sexualOrentation.Sexual_Orientation_Id
                                       join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                                       join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                                       where sexualOrentation.Sexual_Orientation_Id == 2
                                       select new NationalStatsGrid { totalLGBTIQsexual = person.Person_Id, siteCode = site.Site_Code }).Distinct().GroupBy(g => g.siteCode).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    siteCode = m.Key,
                    totalLGBTIQsexual = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetProvincialSitesMaleStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                       where gender.Gender_Id == 1
                                       select new NationalStatsGrid { totalFemales = person.Person_Id, provinceName = province.Description }).Distinct().GroupBy(g => g.provinceName).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    provinceName = m.Key,
                    totalMales = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetProvincialSitesFemaleStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                       where gender.Gender_Id == 2
                                       select new NationalStatsGrid { totalFemales = person.Person_Id, provinceName = province.Description }).Distinct().GroupBy(g => g.provinceName).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    provinceName = m.Key,
                    totalFemales = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetProvincialSitesHeteroSexualStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       where orientation.Sexual_Orientation_Id == 1
                                       select new NationalStatsGrid { totalHeterosexual = person.Person_Id, provinceName = province.Description }).Distinct().GroupBy(g => g.provinceName).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    provinceName = m.Key,
                    totalHeterosexual = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetProvincialSitesLGBTIQSexualStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var distictMaleList = (from cases in dbContext.VEP_Cases
                                       join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                       join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                       join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                       join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                       join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                       join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                       join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                       join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                       join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                       join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                       join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                       where orientation.Sexual_Orientation_Id == 2
                                       select new NationalStatsGrid { totalLGBTIQsexual = person.Person_Id, provinceName = province.Description }).Distinct().GroupBy(g => g.provinceName).ToList();


                return distictMaleList.Select(m => new NationalStatsGrid
                {
                    provinceName = m.Key,
                    totalLGBTIQsexual = m.Count()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<NationalStatsGrid> GetNationalServicesStats()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                //return (from province in dbContext.Provinces
                //        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                //        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                //        join serviceOffice in dbContext.Service_Offices on localMunicipality.Local_Municipality_Id equals serviceOffice.Local_Municipality_Id
                //        join employee in dbContext.Employees on serviceOffice.Service_Office_Id equals employee.Service_Office_Id
                //        join users in dbContext.Users on employee.User_Id equals users.User_Id
                //        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                //        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                //        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                //        group province by province.Description into g
                //        select new NationalStatsGrid { provinceName = g.Key, totalServicesRendered = g.Count() }).ToList();

                return (from province in dbContext.Provinces
                        join district in dbContext.Districts on province.Province_Id equals district.Province_Id
                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                        join employee in dbContext.Employees on organisation.Organization_Id equals employee.Organization_Id
                        join users in dbContext.Users on employee.User_Id equals users.User_Id
                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                        group province by province.Description into g
                        select new NationalStatsGrid { provinceName = g.Key, totalServicesRendered = g.Count() }).ToList();


                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<LocalMunicipalityDataReport> GetLocalMunicipalityDataReport(string localMunicipalityId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var localMunicipalityCases = (from cases in dbContext.VEP_Cases
                                              join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                              join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                              join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                              join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                              join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                              join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                              join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                              join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                              join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                              join populationGroup in dbContext.Population_Groups on person.Population_Group_Id equals populationGroup.Population_Group_Id
                                              join maritalStatus in dbContext.Marital_Statusses on person.Marital_Status_Id equals maritalStatus.Marital_Status_Id
                                              join nationality in dbContext.Nationalities on person.Nationality_Id equals nationality.Nationality_Id
                                              join disabilityType in dbContext.Disabilities on person.Disability_Type_Id equals disabilityType.Disability_Id
                                              join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                              select new LocalMunicipalityDataReport
                                              {
                                                  RefNo = cases.VEPReference,
                                                  ProvinceName = province.Description,
                                                  DistrictName = district.Description,
                                                  LocalMunicipalityName = local.Description,
                                                  PopulationGroup = populationGroup.Description,
                                                  SexualOrientation = cases.SexualOrientationId.ToString(),
                                                  MaritalStatus = maritalStatus.Description,
                                                  Age = person.Age,
                                                  Gender = gender.Description,
                                                  DisabilityType = disabilityType.Description,
                                                  Citizenship = nationality.Description,
                                                  Settlement = cases.SettlementId.ToString(),
                                                  KnownPerpetrator = cases.KnowPerpetrator.ToString(),
                                                  PerpFamilyMember = cases.KnowPerpetrator.ToString(),
                                                  PerpCommunityMember = cases.PerpCommunityMember.ToString(),
                                                  ReportedToPolice = cases.ReportedToPolice.ToString(),
                                                  DateOfIncident = cases.DateOfIncident.ToString(),
                                                  DateOfReporting = cases.DateOfReporting.ToString()

                                              }).ToList();

                
                if (localMunicipalityId != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.LocalMunicipalityName == localMunicipalityId).ToList(); }

                var filterDataReport = new List<LocalMunicipalityDataReport>();

                foreach (var item in localMunicipalityCases)
                {
                    var reportItem = new LocalMunicipalityDataReport();

                    if (dbContext.VEP_Cases.Where(b => b.VEPReference == item.RefNo) != null)
                    {
                        var vCase = dbContext.VEP_Cases.FirstOrDefault(b => b.VEPReference == item.RefNo);

                        foreach (var link in dbContext.VEP_VictimizationTypeDetails.Where(a => a.Case_Id == vCase.CaseId).ToList())
                        {
                            item.VictimisationType += ", ";
                            item.VictimisationType += (dbContext.VEP_VictimizationType.Where(a => a.Id == link.VictimizationType).ToList().Count() == 0) ? "" : dbContext.VEP_VictimizationType.SingleOrDefault(a => a.Id == link.VictimizationType).VictimizationType;
                        }

                    }
  
                    reportItem.RefNo = item.RefNo;
                    reportItem.ProvinceName = item.ProvinceName;
                    reportItem.DistrictName = item.DistrictName;
                    reportItem.LocalMunicipalityName = item.LocalMunicipalityName;
                    reportItem.PopulationGroup = item.PopulationGroup;
                    reportItem.MaritalStatus = item.MaritalStatus;
                    reportItem.Gender = item.Gender;
                    reportItem.DisabilityType = item.DisabilityType;
                    reportItem.Citizenship = item.Citizenship;
                    reportItem.VictimisationType = item.VictimisationType;
                        if (reportItem.VictimisationType.Length > 0) { reportItem.VictimisationType.Substring(2); }
                   
                    reportItem.RefNo = item.RefNo;
                    if (item.SexualOrientation != String.Empty)
                    {
                        int oriention = Convert.ToInt32(item.SexualOrientation);
                        
                        if (dbContext.VEP_Sexual_Orientation.SingleOrDefault(a => a.Sexual_Orientation_Id == oriention) != null)
                        { reportItem.SexualOrientation = dbContext.VEP_Sexual_Orientation.SingleOrDefault(a => a.Sexual_Orientation_Id == oriention).Sexual_Orientation; }
                    }
                        reportItem.Age = item.Age;
                    if (item.Settlement != String.Empty)
                    {
                        int mSettlement = Convert.ToInt32(item.Settlement);

                        if (dbContext.VEP_SettlementType.SingleOrDefault(a => a.Id == mSettlement) != null) {
                            reportItem.Settlement = dbContext.VEP_SettlementType.SingleOrDefault(a => a.Id == mSettlement).Settlement; }
                    }

                    reportItem.KnownPerpetrator = (item.KnownPerpetrator == "0") ? "No" : "Yes";
                    reportItem.PerpFamilyMember = (item.PerpFamilyMember == "0") ? "No" : "Yes";
                    reportItem.PerpCommunityMember = (item.PerpCommunityMember == "0") ? "No" : "Yes";
                    reportItem.ReportedToPolice = (item.ReportedToPolice == "0") ? "No" : "Yes";
                    reportItem.DateOfReporting = item.DateOfReporting;
                    reportItem.DateOfIncident = item.DateOfIncident;

                    filterDataReport.Add(reportItem);
                }

                return localMunicipalityCases;
                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return null;
            }

        }


        public IEnumerable<LocalMunicipalityDataReport> GetSearchDataReport(int provinceID, int districtID, int localMunicipalityID, string siteID, List<string> vitimisationTypeID, int maritalStatusID,
            int nationalityID, int populationGroupID, int genderID, List<string> disabilityTypeID, int settlementTypeID, int sexualOrientationID, string IsAllegedPerpetrator,
            string IsPerpetratorMemberOfFamily, string IsPerpetratorMemberOfCommunity, string IsCaseReported, DateTime IncidentDateFrom, DateTime IncidentDateTo,
            DateTime CreatedDateFrom, DateTime CreatedDateTo, int ageFrom, int ageTo)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {

                var localMunicipalityCases = (from cases in dbContext.VEP_Cases
                                              join assessment in dbContext.Intake_Assessments on cases.ClientId equals assessment.Intake_Assessment_Id
                                              join client in dbContext.Clients on assessment.Client_Id equals client.Client_Id
                                              join person in dbContext.Persons on client.Person_Id equals person.Person_Id
                                              join orientation in dbContext.VEP_Sexual_Orientation on person.Sexual_Orientation_Id equals orientation.Sexual_Orientation_Id
                                              join users in dbContext.Users on assessment.Case_Manager_Id equals users.User_Id
                                              join employee in dbContext.Employees on users.User_Id equals employee.User_Id
                                              join site in dbContext.Organizations on employee.Organization_Id equals site.Organization_Id
                                              join office in dbContext.Service_Offices on employee.Service_Office_Id equals office.Service_Office_Id
                                              join local in dbContext.Local_Municipalities on office.Local_Municipality_Id equals local.Local_Municipality_Id
                                              join district in dbContext.Districts on local.District_Municipality_Id equals district.District_Id
                                              join province in dbContext.Provinces on district.Province_Id equals province.Province_Id
                                              join populationGroup in dbContext.Population_Groups on person.Population_Group_Id equals populationGroup.Population_Group_Id
                                              join maritalStatus in dbContext.Marital_Statusses on person.Marital_Status_Id equals maritalStatus.Marital_Status_Id
                                              join nationality in dbContext.Nationalities on person.Nationality_Id equals nationality.Nationality_Id
                                              //join disabilityType in dbContext.Int_Person_Disability on person.Disability_Type_Id equals disabilityType.Disability_Id                                              
                                              join gender in dbContext.Genders on person.Gender_Id equals gender.Gender_Id
                                              select new VEPFilterDataReport
                                              {
                                                  RefNo = cases.VEPReference,
                                                  CaseId = cases.CaseId,
                                                  SiteCode = site.Site_Code,
                                                  ProvinceID = province.Province_Id,
                                                  DistrictID = district.District_Id,
                                                  LocalMunicipalityID = local.Local_Municipality_Id,
                                                  PopulationGroupID = populationGroup.Population_Group_Id,
                                                  SexualOrientationID = orientation.Sexual_Orientation_Id,
                                                  MaritalStatusID = maritalStatus.Marital_Status_Id,
                                                  Age = person.Age,
                                                  GenderID = gender.Gender_Id,
                                                  PersonId = person.Person_Id,
                                                  CitizenshipID = nationality.Nationality_Id,
                                                  SettlementID = cases.SettlementId,
                                                  KnownPerpetrator = cases.KnowPerpetrator.ToString(),
                                                  PerpFamilyMember = cases.KnowPerpetrator.ToString(),
                                                  PerpCommunityMember = cases.PerpCommunityMember.ToString(),
                                                  ReportedToPolice = cases.ReportedToPolice.ToString(),
                                                  DateOfIncident = cases.DateOfIncident,
                                                  DateOfReporting = cases.DateOfReporting
                                              }).ToList();

                if (provinceID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.ProvinceID == provinceID).ToList(); }
                if (districtID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.DistrictID == districtID).ToList(); }
                if (localMunicipalityID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.LocalMunicipalityID == localMunicipalityID).ToList(); }
                if (siteID != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.SiteCode == siteID).ToList(); }

                if (maritalStatusID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.MaritalStatusID == maritalStatusID).ToList(); }
                if (nationalityID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.CitizenshipID == nationalityID).ToList(); }
                if (populationGroupID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.PopulationGroupID == populationGroupID).ToList(); }
                if (genderID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.GenderID == genderID).ToList(); }
                if (disabilityTypeID != null)
                {

                    bool foundDisability = false;

                    foreach (var record in localMunicipalityCases.GroupBy(a => a.PersonId).Select(a => a.FirstOrDefault()))
                    {
                        var personDisabilities = dbContext.Int_Person_Disability.Where(a => a.Person_Id == record.PersonId).ToList();
                        foundDisability = false;

                        foreach (var disability in personDisabilities)
                        {
                            if (dbContext.Disabilities.Find(disability.Disability_Id) != null)
                            {
                                if (disabilityTypeID.Contains(disability.Disability_Id.ToString()))
                                {
                                    foundDisability = true;
                                    break;
                                }
                            }
                        }

                        if (!foundDisability) { localMunicipalityCases.RemoveAll(a => a.PersonId == record.PersonId); }
                    };

                    //get list of persons disabilities

                    //localMunicipalityCases = localMunicipalityCases.Where(a => a.DisabilityTypeID == disabilityTypeID).ToList();
                }

                if (vitimisationTypeID != null)
                {

                    bool foundVictimization = false;
                    List<VEPFilterDataReport> victimizationCasesList = new List<VEPFilterDataReport>();
                    victimizationCasesList.AddRange(localMunicipalityCases);

                    foreach (var record in victimizationCasesList)
                    {
                        var caseVictimization = dbContext.VEP_VictimizationTypeDetails.Where(a => a.Case_Id == record.CaseId).ToList();
                        foundVictimization = false;

                        foreach (var victimization in caseVictimization)
                        {
                            if (dbContext.VEP_VictimizationType.Find(victimization.VictimizationType) != null)
                            {
                                if (vitimisationTypeID.Contains(victimization.VictimizationType.ToString()))
                                {
                                    foundVictimization = true;
                                    break;
                                }
                            }
                        }

                        if (!foundVictimization) { localMunicipalityCases.Remove(record); }
                    };

                    //get list of persons disabilities

                    //localMunicipalityCases = localMunicipalityCases.Where(a => a.DisabilityTypeID == disabilityTypeID).ToList();
                }

                if (settlementTypeID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.SettlementID == settlementTypeID).ToList(); }
                if (sexualOrientationID != -1) { localMunicipalityCases = localMunicipalityCases.Where(a => a.SexualOrientationID == sexualOrientationID).ToList(); }
                if (IsAllegedPerpetrator != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.KnownPerpetrator == IsAllegedPerpetrator).ToList(); }
                if (IsPerpetratorMemberOfFamily != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.PerpFamilyMember == IsPerpetratorMemberOfFamily).ToList(); }
                if (IsPerpetratorMemberOfCommunity != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.PerpCommunityMember == IsPerpetratorMemberOfCommunity).ToList(); }
                if (IsCaseReported != String.Empty) { localMunicipalityCases = localMunicipalityCases.Where(a => a.ReportedToPolice == IsCaseReported).ToList(); }
                if (IncidentDateFrom.Year != 1800 && IncidentDateTo.Year != 1800)
                {
                    localMunicipalityCases = localMunicipalityCases.Where(a => a.DateOfIncident >= IncidentDateFrom && a.DateOfIncident <= IncidentDateTo).ToList();
                }
                if (CreatedDateFrom.Year != 1800 && CreatedDateTo.Year != 1800)
                {
                    localMunicipalityCases = localMunicipalityCases.Where(a => a.DateOfReporting >= CreatedDateFrom && a.DateOfReporting <= CreatedDateTo).ToList();
                }
                if (ageFrom != -1)
                {
                    localMunicipalityCases = localMunicipalityCases.Where(a => a.Age >= ageFrom).ToList();
                }
                if (ageTo != -1)
                {
                    localMunicipalityCases = localMunicipalityCases.Where(a => a.Age <= ageTo).ToList();
                }

                var filterDataReport = new List<LocalMunicipalityDataReport>();

                foreach (var item in localMunicipalityCases)
                {
                    var reportItem = new LocalMunicipalityDataReport();
                    item.VictimisationType = String.Empty;


                    foreach (var link in dbContext.VEP_VictimizationTypeDetails.Where(a => a.Case_Id == item.CaseId).ToList())
                    {
                        if (item.VictimisationType.Length > 0) { item.VictimisationType += ", "; }

                        item.VictimisationType += (dbContext.VEP_VictimizationType.Where(a => a.Id == link.VictimizationType).ToList().Count() == 0) ? "" : dbContext.VEP_VictimizationType.SingleOrDefault(a => a.Id == link.VictimizationType).VictimizationType;
                    }


                    reportItem.VictimisationType = item.VictimisationType;

                    reportItem.RefNo = item.RefNo;
                    //if (dbContext.VEP_VictimizationType.Find(item.VictimisationTypeID) != null) { reportItem.VictimisationType = dbContext.VEP_VictimizationType.Find(item.VictimisationTypeID).VictimizationType; }
                    if (dbContext.Provinces.Find(item.ProvinceID) != null) { reportItem.ProvinceName = dbContext.Provinces.Find(item.ProvinceID).Description; }
                    if (dbContext.Districts.Find(item.DistrictID) != null) { reportItem.DistrictName = dbContext.Districts.Find(item.DistrictID).Description; }
                    if (dbContext.Local_Municipalities.Find(item.LocalMunicipalityID) != null) { reportItem.LocalMunicipalityName = dbContext.Local_Municipalities.Find(item.LocalMunicipalityID).Description; }
                    reportItem.SiteCode = item.SiteCode;
                    if (dbContext.Population_Groups.Find(item.PopulationGroupID) != null) { reportItem.PopulationGroup = dbContext.Population_Groups.Find(item.PopulationGroupID).Description; }
                    if (dbContext.Genders.Find(item.GenderID) != null) { reportItem.Gender = dbContext.Genders.Find(item.GenderID).Description; }
                    if (dbContext.VEP_Sexual_Orientation.Find(item.SexualOrientationID) != null) { reportItem.SexualOrientation = dbContext.VEP_Sexual_Orientation.Find(item.SexualOrientationID).Sexual_Orientation; }
                    if (dbContext.Marital_Statusses.Find(item.MaritalStatusID) != null) { reportItem.MaritalStatus = dbContext.Marital_Statusses.Find(item.MaritalStatusID).Description; }
                    reportItem.Age = item.Age;

                    var disabilities = new PersonDisabilityModel();
                    var personDiabilityList = disabilities.PersonDisability(item.PersonId);
                    reportItem.DisabilityType = String.Empty;

                    foreach (var personDisability in personDiabilityList)
                    {
                        if (dbContext.Disabilities.Find(personDisability.Disability_Id) != null)
                        {
                            if (dbContext.Disabilities.Find(personDisability.Disability_Id).Description.Trim() != String.Empty)
                            {
                                if (reportItem.DisabilityType.Length > 0) { reportItem.DisabilityType += ", "; }

                                reportItem.DisabilityType += (dbContext.Disabilities.Find(personDisability.Disability_Id) == null) ? "" : dbContext.Disabilities.Find(personDisability.Disability_Id).Description;
                            }
                        }
                    }

                    //if (dbContext.Int_Person_Disability.Find(item.DisabilityTypeID) != null) { reportItem.DisabilityType = dbContext.Int_Person_Disability.Find(item.DisabilityTypeID).; }
                    if (dbContext.Nationalities.Find(item.CitizenshipID) != null) { reportItem.Citizenship = dbContext.Nationalities.Find(item.CitizenshipID).Description; }
                    if (dbContext.VEP_SettlementType.Find(item.SettlementID) != null) { reportItem.Settlement = dbContext.VEP_SettlementType.Find(item.SettlementID).Settlement; }
                    reportItem.KnownPerpetrator = (item.KnownPerpetrator == "0") ? "No" : "Yes";
                    reportItem.PerpFamilyMember = (item.PerpFamilyMember == "0") ? "No" : "Yes";
                    reportItem.PerpCommunityMember = (item.PerpCommunityMember == "0") ? "No" : "Yes";
                    reportItem.ReportedToPolice = (item.ReportedToPolice == "0") ? "No" : "Yes";
                    reportItem.DateOfReporting = Convert.ToDateTime(item.DateOfReporting).ToShortDateString();
                    reportItem.DateOfIncident = Convert.ToDateTime(item.DateOfIncident).ToShortDateString();

                    filterDataReport.Add(reportItem);
                }

                return filterDataReport;
                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<DistrictStatsGrid> GetDistrictStatsList(string province)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var totalVictimsList = (from provinces in dbContext.Provinces
                                        join district in dbContext.Districts on provinces.Province_Id equals district.Province_Id
                                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                                        where provinces.Description == province
                                        group district by district.Description into g
                                        select new DistrictStatsGrid { districtName = g.Key, totalVictimsCaptured = g.Count() }).ToList();


                var totalServiceList = (from provinces in dbContext.Provinces
                                        join district in dbContext.Districts on provinces.Province_Id equals district.Province_Id
                                        join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join users in dbContext.Users on employees.User_Id equals users.User_Id
                                        join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
                                        join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
                                        join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
                                        where provinces.Description == province
                                        group district by district.Description into g
                                        select new DistrictStatsGrid { districtName = g.Key, totalServicesRendered = g.Count() }).ToList();

                foreach (var item in totalVictimsList)
                {
                    if (totalServiceList.Where(a => a.districtName == item.districtName).Count() != 0)
                    {
                        item.totalServicesRendered = totalServiceList.FirstOrDefault(a => a.districtName == item.districtName).totalServicesRendered;
                    }
                }

                return totalVictimsList;
                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        //public IEnumerable<DistrictStatsGrid> GetDistrictStatsList(string province)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();
        //    try
        //    {
        //        var totalVictimsList = (from provinces in dbContext.Provinces
        //                                join district in dbContext.Districts on provinces.Province_Id equals district.Province_Id
        //                                join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
        //                                join serviceOffice in dbContext.Service_Offices on localMunicipality.Local_Municipality_Id equals serviceOffice.Local_Municipality_Id
        //                                join employees in dbContext.Employees on serviceOffice.Service_Office_Id equals employees.Service_Office_Id
        //                                join users in dbContext.Users on employees.User_Id equals users.User_Id
        //                                join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
        //                                join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
        //                                where provinces.Description == province
        //                                group district by district.Description into g
        //                                select new DistrictStatsGrid { districtName = g.Key, totalVictimsCaptured = g.Count() }).ToList();


        //        var totalServiceList = (from provinces in dbContext.Provinces
        //                                join district in dbContext.Districts on provinces.Province_Id equals district.Province_Id
        //                                join localMunicipality in dbContext.Local_Municipalities on district.District_Id equals localMunicipality.District_Municipality_Id
        //                                join serviceOffice in dbContext.Service_Offices on localMunicipality.Local_Municipality_Id equals serviceOffice.Local_Municipality_Id
        //                                join employee in dbContext.Employees on serviceOffice.Service_Office_Id equals employee.Service_Office_Id
        //                                join users in dbContext.Users on employee.User_Id equals users.User_Id
        //                                join assesment in dbContext.Intake_Assessments on users.User_Id equals assesment.Case_Manager_Id
        //                                join vep in dbContext.VEP_Cases on assesment.Intake_Assessment_Id equals vep.ClientId
        //                                join service in dbContext.VEP_Services on vep.CaseId equals service.CaseId
        //                                where provinces.Description == province
        //                                group district by district.Description into g
        //                                select new DistrictStatsGrid {  districtName = g.Key, totalServicesRendered = g.Count() }).ToList();

        //        foreach (var item in totalVictimsList)
        //        {
        //            if (totalServiceList.Where(a => a.districtName == item.districtName).Count() != 0)
        //            {
        //                item.totalServicesRendered = totalServiceList.FirstOrDefault(a => a.districtName == item.districtName).totalServicesRendered;
        //            }
        //        }

        //        return totalVictimsList;
        //        //dbContext.VEP_Cases.GroupBy(p => p.)
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //}

        public IEnumerable<LocalMunicipalityStatsGrid> GetLocalMunicipalityStatsList(string districtId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var totalVictimsList = (from districts in dbContext.Districts
                                        join localMunicipality in dbContext.Local_Municipalities on districts.District_Id equals localMunicipality.District_Municipality_Id
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join assessment in dbContext.Intake_Assessments on employees.User_Id equals assessment.Case_Manager_Id
                                        join casses in dbContext.VEP_Cases on assessment.Intake_Assessment_Id equals casses.ClientId
                                        where districts.Description == districtId
                                        group localMunicipality by localMunicipality.Description into g
                                        select new LocalMunicipalityStatsGrid { localMunicipalityName = g.Key, totalVictimsCaptured = g.Count() }).Distinct().ToList();


                var totalServiceList = (from districts in dbContext.Districts
                                        join localMunicipality in dbContext.Local_Municipalities on districts.District_Id equals localMunicipality.District_Municipality_Id
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join assessment in dbContext.Intake_Assessments on employees.User_Id equals assessment.Case_Manager_Id
                                        join casses in dbContext.VEP_Cases on assessment.Intake_Assessment_Id equals casses.ClientId
                                        join service in dbContext.VEP_Services on casses.CaseId equals service.CaseId
                                        where districts.Description == districtId
                                        group localMunicipality by localMunicipality.Description into g
                                        select new LocalMunicipalityStatsGrid { localMunicipalityName = g.Key, totalServicesRendered = g.Count() }).ToList();

                foreach (var item in totalVictimsList)
                {
                    if (totalServiceList.Where(a => a.localMunicipalityName == item.localMunicipalityName).Count() != 0)
                    {
                        item.totalServicesRendered = totalServiceList.FirstOrDefault(a => a.localMunicipalityName == item.localMunicipalityName).totalServicesRendered;
                    }
                }

                return totalVictimsList;
                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<SiteStatsGrid> GetSiteStatsList(string localId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var totalVictimsList = (from localMunicipality in dbContext.Local_Municipalities 
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join assessment in dbContext.Intake_Assessments on employees.User_Id equals assessment.Case_Manager_Id
                                        join casses in dbContext.VEP_Cases on assessment.Intake_Assessment_Id equals casses.ClientId
                                        where localMunicipality.Description == localId
                                        group organisation by organisation.Site_Code into g
                                        select new SiteStatsGrid { siteCode = g.Key, totalVictimsCaptured = g.Count() }).Distinct().ToList();


                var totalServiceList = (from localMunicipality in dbContext.Local_Municipalities
                                        join organisation in dbContext.Organizations on localMunicipality.Local_Municipality_Id equals organisation.Local_Municipality_Id
                                        join employees in dbContext.Employees on organisation.Organization_Id equals employees.Organization_Id
                                        join assessment in dbContext.Intake_Assessments on employees.User_Id equals assessment.Case_Manager_Id
                                        join casses in dbContext.VEP_Cases on assessment.Intake_Assessment_Id equals casses.ClientId
                                        join service in dbContext.VEP_Services on casses.CaseId equals service.CaseId
                                        where localMunicipality.Description == localId
                                        group organisation by organisation.Site_Code into g
                                        select new SiteStatsGrid { siteCode = g.Key, totalServicesRendered = g.Count() }).ToList();

                foreach (var item in totalVictimsList)
                {
                    if (totalServiceList.Where(a => a.siteCode == item.siteCode).Count() != 0)
                    {
                        item.totalServicesRendered = totalServiceList.FirstOrDefault(a => a.siteCode == item.siteCode).totalServicesRendered;
                    }
                }

                return totalVictimsList;
                //dbContext.VEP_Cases.GroupBy(p => p.)
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
