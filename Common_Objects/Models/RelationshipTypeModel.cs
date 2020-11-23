using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class RelationshipTypeModel
    {
        public Relationship_Type GetSpecificRelationshipType(int relationshipTypeId)
        {
            Relationship_Type relationshipType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var relationshipTypeList = (from r in dbContext.Relationship_Types
                                            where r.Relationship_Type_Id.Equals(relationshipTypeId)
                                            select r).ToList();

                relationshipType = (from r in relationshipTypeList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return relationshipType;
        }

        public List<Relationship_Type> GetListOfRelationshipTypes()
        {
            List<Relationship_Type> relationshipTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var relationshipTypeList = (from r in dbContext.Relationship_Types
                                                select r).ToList();

                    relationshipTypes = (from r in relationshipTypeList
                                         select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return relationshipTypes;
        }
    }
}
