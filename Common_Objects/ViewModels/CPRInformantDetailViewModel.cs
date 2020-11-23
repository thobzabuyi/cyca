using Common_Objects.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Globalization;

namespace Common_Objects.ViewModels
{
    public class CPRInformantDetailViewModel
    {
        public int Person_Id { get; set; }
        public int Incident_Id { get; set; }
        public CPRInformantSearchViewModel SearchPerson { get; set; }
        public Person AllegedOffenderPerson { get; set; }
        public PersonDetailViewModel CreatePerson { get; set; }
        public CPR_Informant CPRInformant { get; set; }

        public int SelectedRelationship_Type_Id { get; set; }
        [Display(Name = "Relationship to Child")]
        public SelectList Child_Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(SelectedRelationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(relationshipTypesList, "Value", "Text", SelectedRelationship_Type_Id);

                return selectList;
            }
        }
    }
}
