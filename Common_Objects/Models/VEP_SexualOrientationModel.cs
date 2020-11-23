using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEP_SexualOrientationModel
    {
        public VEP_Sexual_Orientation GetSpecificSexualOrientation(int sexualOrientationId)
        {
            VEP_Sexual_Orientation sexualOrientation;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var sexualOrientationList = (from g in dbContext.VEP_Sexual_Orientation
                                  where g.Sexual_Orientation_Id.Equals(sexualOrientationId)
                                  select g).ToList();

                sexualOrientation = (from g in sexualOrientationList
                                     select g).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return sexualOrientation;
        }

        public List<VEP_Sexual_Orientation> GetListOfsexualOrientation()
        {
            List<VEP_Sexual_Orientation> VEP_sexualOrientation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var VEPsexualOrientationList = (from g in dbContext.VEP_Sexual_Orientation
                                      select g).ToList();

                    VEP_sexualOrientation = (from g in VEPsexualOrientationList
                               select g).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return VEP_sexualOrientation;
        }
    }
}
