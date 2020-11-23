using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CheckListRepository
    {
        public static CheckBoxListItems Get(int id, List<VEP_PresentationCondition> conditions)
        {
            return GetConditions(conditions).FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// for get all fruits
        /// </summary>
        public static IEnumerable<CheckBoxListItems> GetConditions(List<VEP_PresentationCondition> conditions)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            var listItems = new List<CheckBoxListItems>();

            foreach(var item in conditions)
            {
                listItems.Add(new CheckBoxListItems { Id = item.Id, Name = item.Conditions });
            }
            return listItems;
        }

        public static IEnumerable<CheckBoxListItems> GetVictimizationType(List<VEP_VictimizationType> victimizationTyp)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            var listItems = new List<CheckBoxListItems>();

            foreach (var item in victimizationTyp)
            {
                listItems.Add(new CheckBoxListItems { Id = item.Id, Name = item.VictimizationType });
            }
            return listItems;
        }
    }
}
