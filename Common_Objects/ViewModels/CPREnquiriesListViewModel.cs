using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class CPREnquiriesListViewModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<CPR_EnquiriesByEmployer> EmployerEnquiries { get; set; }
        public List<CPR_EnquiriesByIndividual> IndividualEnquiries { get; set; }

        public string searchEmployer { get; set; }
        public string searchIndv { get; set; }

        public CPR_EnquiriesByEmployer GetCPR_EnquiriesByEmployer(int id)
        {
            return db.CPR_EnquiriesByEmployer.Where(x => x.Enquiry_Id == id).FirstOrDefault();
        }

        public List<CPR_EquiriesPersonDetails> GetCPR_EquiriesPersonDetails(int Enquiry_Id)
        {
            return db.CPR_EquiriesPersonDetails.Where(x => x.Enquiry_Id == Enquiry_Id).ToList();
        }
        public Person GetPersonDetails(string FirstName, string Surname, string IdNumber)
        {
            return db.Persons.Where(x => x.First_Name == FirstName && x.Last_Name == Surname && x.Identification_Number == IdNumber).FirstOrDefault();
        }
        public CPR_Unsuitability GetCPR_Unsuitability(int Person_Id)
        {
            return db.CPR_Unsuitability.Where(x => x.Person_Id == Person_Id).FirstOrDefault();
        }

        public CPR_Unsuitability_Ruiling GetCPR_Unsuitability_Ruiling(int Unsuitablity_Id)
        {
            return db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == Unsuitablity_Id).FirstOrDefault();
        }
        public CPR_Unsuitability_Findings GetCPR_Unsuitability_Findings(int Unsuitablity_Id)
        {
            return db.CPR_Unsuitability_Findings.Where(x => x.Unsuitability_Id == Unsuitablity_Id).FirstOrDefault();
        }
        public CPR_EnquiriesByIndividual GetCPR_EnquiriesByIndividual(int id)
        {
            return db.CPR_EnquiriesByIndividual.Where(x => x.Enquiry_Id == id).FirstOrDefault();
        }
        public List<CPR_EnquiriesByEmployer> GetCPR_EnquiriesByEmployer()
        {
            return (from emp in db.CPR_EnquiriesByEmployer
                    select emp).ToList();
        }
        public List<CPR_EnquiriesByIndividual> GetCPR_EnquiriesByIndividual()
        {
            return (from ind in db.CPR_EnquiriesByIndividual
                    select ind).ToList();
        }
        public List<Town> GetTowns(int myValue)
        {
            return db.Towns.Where(x => x.Local_Municipality.District.District_Id == myValue).ToList();
        }
        public Employee GetEmployees(int User_Id)
        {
            return db.Employees.FirstOrDefault(p => p.User_Id == User_Id);
        }
        public void AddCPR_EquiriesPersonDetails(CPR_EquiriesPersonDetails newPerson)
        {
            db.CPR_EquiriesPersonDetails.Add(newPerson);
        }
        public void SaveInfo()
        {
            db.SaveChanges();
        }

        public List<CPR_EquiriesPersonDetails> GetCPR_EquiriesPersonDetailsList(int retrieveId)
        {
            return db.CPR_EquiriesPersonDetails.Where(x => x.Enquiry_Id == retrieveId).ToList();
        }

        public void RemoveCPR_EnquiriesByEmployer(CPR_EnquiriesByEmployer recordToRemove)
        {
            db.CPR_EnquiriesByEmployer.Remove(recordToRemove);
        }
        public List<CPR_EnquiriesByEmployer> GetallRecords()
        {
            return db.CPR_EnquiriesByEmployer.Where(x => x.ReferenceNumber != null).ToList();
        }
        public void AddCPR_EnquiriesByEmployer(CPR_EnquiriesByEmployer tempRecord)
        {
            db.CPR_EnquiriesByEmployer.Add(tempRecord);
        }
        public List<CPR_EnquiriesByIndividual> GetallRecordsIndv()
        {
            return db.CPR_EnquiriesByIndividual.Where(x => x.ReferenceNumber != null).ToList();
        }

        public CPR_EnquiriesEmployerDetails GetCPR_EnquiriesEmployerDetails(int Enquiry_Id)
        {
            return (from a in db.CPR_EnquiriesEmployerDetails
                    where a.Enquiry_Id == Enquiry_Id
                    select a).FirstOrDefault();
        }

        public void AddCPR_EnquiriesEmployerDetails(CPR_EnquiriesEmployerDetails business)
        {
            db.CPR_EnquiriesEmployerDetails.Add(business);

        }

        public void AddCPR_EnquiriesByIndividual(CPR_EnquiriesByIndividual enquiry)
        {
            db.CPR_EnquiriesByIndividual.Add(enquiry);
        }

        public CPR_EquiriesPersonDetails GetCPR_EquiriesPersonDetailsForPersondetails(int Enquiry_Id)
        {
            return db.CPR_EquiriesPersonDetails.Where(x => x.Enquiry_Id == Enquiry_Id).FirstOrDefault();
        }

        public int GetLocal_Municipality_Id(int? CityTown_Id)
        {
            return db.Towns.Find(CityTown_Id).Local_Municipality_Id;
        }

        public List<CPR_EnquiriesByEmployer> GetEmployerEnquiries(int CurrentUserId)
        {
            return (from emp in db.CPR_EnquiriesByEmployer
                    where emp.LoggedInUserId == CurrentUserId
                    select emp).ToList();
        }
        public List<CPR_EnquiriesByIndividual> GetIndividualEnquiries(int CurrentUserId)
        {
            return (from ind in db.CPR_EnquiriesByIndividual
                    where ind.LoggedInUserId == CurrentUserId
                    select ind).ToList();
        }

    }
}
