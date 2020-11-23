using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PersonSpecialNeedModel
    {

        public int Create(int selected_SpecialNeedId, int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var personSpecialNeedRecord = new Int_Person_SpecialNeed();

                personSpecialNeedRecord.Person_Id = personId;
                personSpecialNeedRecord.SpecialNeed_Id = selected_SpecialNeedId;

                dbContext.Int_Person_SpecialNeed.Add(personSpecialNeedRecord);
                dbContext.SaveChanges();

                return personSpecialNeedRecord.Person_SpecialNeed_Id;
            }
            //catch (Exception ex)
            //{
            //    return -1;
            //}
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        var msg = validationError.PropertyName + " Error: " + validationError.ErrorMessage;
                        
                    }
                }
            }
            return -1;
        }

        public int Delete(int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var personSpecialNeedRecord = dbContext.Int_Person_SpecialNeed.Where(a => a.Person_Id == personId);
                dbContext.Int_Person_SpecialNeed.RemoveRange(personSpecialNeedRecord);
                dbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
