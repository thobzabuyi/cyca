using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class Section173Model
    {
        public Section_173_Item GetSpecificSection173Item(int section173ItemId)
        {
            Section_173_Item section173Item;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var section173ItemList = (from r in dbContext.Section_173_Items
                                          where r.Section_173_Item_Id.Equals(section173ItemId)
                                          select r).ToList();

                section173Item = (from r in section173ItemList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return section173Item;
        }

        public List<Section_173_Item> GetListOfSection173Items()
        {
            List<Section_173_Item> section173Items;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var section173ItemList = (from r in dbContext.Section_173_Items
                                              select r).ToList();

                    section173Items = (from r in section173ItemList
                                       select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return section173Items;
        }
    }
}
