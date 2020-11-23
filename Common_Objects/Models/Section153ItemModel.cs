using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class Section153ItemModel
    {
        public Section_153_Item GetSpecificSection153Item(int section153ItemId)
        {
            Section_153_Item section153Item;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var section153ItemList = (from r in dbContext.Section_153_Items
                                          where r.Section_153_Item_Id.Equals(section153ItemId)
                                          select r).ToList();

                section153Item = (from r in section153ItemList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return section153Item;
        }

        public List<Section_153_Item> GetListOfSection153Items()
        {
            List<Section_153_Item> section153Items;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var section153ItemList = (from r in dbContext.Section_153_Items
                                            select r).ToList();

                section153Items = (from r in section153ItemList
                                    select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return section153Items;
        }
    }
}
