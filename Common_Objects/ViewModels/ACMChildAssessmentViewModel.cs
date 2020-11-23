using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Common_Objects.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class ACMChildAssessmentViewModel
    {           
        public string ReferalSource { get; set; }
        public string PresentingProblem { get; set; }

        public SelectList EngagementType_List
        {
            get
            {
                var engagementTypeModel = new EngagementTypeModel();
                var listOfEngagementTypes = engagementTypeModel.GetListOfEngagementTypes();
                var EngagementTypesList = (from i in listOfEngagementTypes
                                           select new SelectListItem()
                                          {
                                              Text = i.Description,
                                              Value = i.EngagementType_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = i.EngagementType_Id.Equals(TypeOfEngagement_Id)
                                          }).ToList();

                var selectList = new SelectList(EngagementTypesList, "Value", "Text", TypeOfEngagement_Id);
                return selectList;
            }
        }
        public Nullable<int> TypeOfEngagement_Id { get; set; }
        public string NameOfPersonsParticipating { get; set; }

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
                                                 Selected = r.Relationship_Type_Id.Equals(RelationshipType_Id)
                                             }).ToList();

                var selectList = new SelectList(RelationshipTypesList, "Value", "Text", RelationshipType_Id);
                return selectList;
            }
        }

        public int RelationshipType_Id { get; set; }

        //Investigate where CaseReferenceNumber is coming from
        public string  CaseReferenceNumber { get; set; }

        public int CaseWorklist_Id { get; set; }
        public virtual Person Person { get; set; }

        public virtual ACM_SocialHistory SocialHistory { get; set; }
        public virtual ACM_MedicalFactors MedicalFactors { get; set; }
        public virtual Relationship_Type Relationship_Type { get; set; }
    }
}
