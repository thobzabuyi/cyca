using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class PersonImageModel
    {
        public Person_Image GetSpecificPersonImage(int personImageId)
        {
            Person_Image personImage;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var personImageList = (from r in dbContext.Person_Images
                                       where r.Person_Image_Id.Equals(personImageId)
                                       select r).ToList();

                personImage = (from r in personImageList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return personImage;
        }

        public List<Person_Image> GetListOfPersonImages()
        {
            List<Person_Image> personImages;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var personImageList = (from r in dbContext.Person_Images
                                           select r).ToList();

                    personImages = (from r in personImageList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return personImages;
        }
    }
}
