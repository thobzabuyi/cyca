using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PersonDisabilityTypeModel
    {
        public int Create(int selected_DisabilitySubTypeId, int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var personDisabilityRecord = new int_Person_Disability_Category();

                personDisabilityRecord.Person_Id = personId;
                personDisabilityRecord.DisabilityType_Id = selected_DisabilitySubTypeId;
                var disabilityids = dbContext.apl_DisabilityType.Where(a => a.DisabilityType_Id.Equals(selected_DisabilitySubTypeId));
                personDisabilityRecord.Disability_Id = disabilityids.FirstOrDefault().DisabilityId;

                dbContext.int_Person_Disability_Category.Add(personDisabilityRecord);
                dbContext.SaveChanges();

                return personDisabilityRecord.PersonSubDisability_Id;
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

        public List<int_Person_Disability_Category> PersonDisabilityType(int personId)
        {

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var disabilityTypeList = dbContext.int_Person_Disability_Category.Where(a => a.Person_Id == personId).ToList();


                return disabilityTypeList;
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
                var personDisabilityRecord = dbContext.int_Person_Disability_Category.Where(a => a.Person_Id == personId);
                dbContext.int_Person_Disability_Category.RemoveRange(personDisabilityRecord);
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
