using Common_Objects.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class NisisParticipantViewModel
    {
        public int Participant_Id { get; set; }
        public int Person_Id { get; set; }
        public int Profiling_Instance_Id { get; set; }
        public NisisParticipantSearchViewModel Search_Participant { get; set; }
        public Person Participant_Person { get; set; }
        public int Selected_Relationship_Type_Id { get; set; }

        [Display(Name = "Relation to Head")]
        public SelectList Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listofRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypes = (from x in listofRelationshipTypes
                                         select new SelectListItem()
                                         {
                                             Text = x.Description,
                                             Value = x.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = x.Relationship_Type_Id.Equals(Selected_Relationship_Type_Id)
                                         }).ToList();

                var selectList = new SelectList(relationshipTypes, "Value", "Text", Selected_Relationship_Type_Id);

                return selectList;
            }
        }
    }
}
