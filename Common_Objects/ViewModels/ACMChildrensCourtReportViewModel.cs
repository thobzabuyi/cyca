using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class ACMChildrensCourtReportViewModel
    {

        public int ChildrensCourtReport_Id { get; set; }

        public int ChildAssesment_Id { get; set; }

        public string FileNumber { get; set; }

        public string DepartmentOf { get; set; }

        public string CourtFileNumber { get; set; }

        public string Introduction { get; set; }
        //public string FamilyHistory { get; set; }
        //public string EmploymentHistory { get; set; }
        //public string PlaceOfBirth { get; set; }

        public List<Person> Relation_Person_List { get; set; }
        public List<Person> Client_Person_List { get; set; }

        public Client Client { get; set; }
        public Person Person { get; set; }
        public Address PhysicalAddress { get; set; }
        public Person_Education PersonEducation { get; set; }
        public ACM_FamilyProfile ACMFamilyProfile { get; set;}


        public int? Relation_Type_Id { get; set; }

        [Display(Name = "Relation Type", Description = "The Type of Relation")]
        public SelectList Relation_Type_List
        {
            get
            {
                var relationTypes = Helpers.GetRelationTypes();

                var selectList = new SelectList(relationTypes, "Value", "Description", Relation_Type_Id);

                return selectList;
            }
        }

       public IntakeClientViewModel IntakeClientViewModel { get; set; }


}
}
