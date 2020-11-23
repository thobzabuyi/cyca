using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class GenderModel
    {
        public Gender GetSpecificGender(int genderId)
        {
            Gender gender;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var genderList = (from g in dbContext.Genders
                                  where g.Gender_Id.Equals(genderId)
                                  select g).ToList();

                gender = (from g in genderList
                          select g).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return gender;
        }

        public List<Gender> GetListOfGenders()
        {
            List<Gender> genders;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var genderList = (from g in dbContext.Genders
                                      select g).ToList();

                    genders = (from g in genderList
                               select g).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return genders;
        }
    }
}