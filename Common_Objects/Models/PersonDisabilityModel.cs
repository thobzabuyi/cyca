using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PersonDisabilityModel
    {

        public int Create(int selected_DisabilityId, int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var personDisabilityRecord = new Int_Person_Disability();

                personDisabilityRecord.Person_Id = personId;
                personDisabilityRecord.Disability_Id = selected_DisabilityId;

                dbContext.Int_Person_Disability.Add(personDisabilityRecord);
                dbContext.SaveChanges();

                return personDisabilityRecord.Person_Disability_Id;
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

        public List<Int_Person_Disability> PersonDisability(int personId)
        {
            
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var disabilityList = dbContext.Int_Person_Disability.Where(a => a.Person_Id == personId).ToList();
                                    

                return disabilityList;
            }
            catch (Exception ex)
            {
                return null;
            }

            
        }

        public int Delete(int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var personDisabilityRecord = dbContext.Int_Person_Disability.Where(a => a.Person_Id == personId);
                dbContext.Int_Person_Disability.RemoveRange(personDisabilityRecord);
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
