using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Common_Objects.Models;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Common_Objects.ViewModels
{
    public class ACMInvestigationViewModel
    {
        public int Investigation_Id { get; set; }
        public int CaseWorklist_Id { get; set; }
        public Nullable<System.DateTime> DateOfInterview { get; set; }
       [Display(Name = "Interview_Type", Description = "Type of investigation interviews")]

        public SelectList Interview_Type_List
        {
            get
            {
                var interviewTypeModel = new InterviewTypeModel();
                var listOfInterviewTypes = interviewTypeModel.GetListOfInterviewTypes();
                var InterviewTypesList = (from i in listOfInterviewTypes
                                          select new SelectListItem()
                                          {
                                              Text = i.Description,
                                            Value = i.Interview_Type_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = i.Interview_Type_Id.Equals(InterviewType_Id)
                                          }).ToList();

                var selectList = new SelectList(InterviewTypesList, "Value", "Text", InterviewType_Id);
                return selectList;
            }
        }
        public int InterviewType_Id { get; set; }
        public string PurposeOfInterview { get; set; }
        public string Process { get; set; }
        public string Evaluation { get; set; }
        public string PlanOfAction { get; set; }

        public ACM_Investigation ACM_Investigation { get; set; }


        public Nullable<int> SocialWorker_Id { get; set; }
  
        public Nullable<System.DateTime> DateFormCompleted { get; set; }
        public string Signature { get; set; }

        public List<Person> Interviewed_Person_List { get; set; }

        public SelectList Relationship_Type_List
        {
            get
            {
                var RelationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = RelationshipTypeModel.GetListOfRelationshipTypes();
                var RelationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(Relationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(RelationshipTypesList, "Value", "Text", Relationship_Type_Id);
                return selectList;
            }
        }

        public int Relationship_Type_Id { get; set; }
        public virtual ACM_CaseWorkList ACM_CaseWorkList { get; set; } 
        public virtual List<ACM_InterviewedPersons> ACM_InterviewedPersons { get; set; }
        public virtual Social_Worker SocialWorker { get; set; }
        public virtual Person Person { get; set; }
     

    }
}

        