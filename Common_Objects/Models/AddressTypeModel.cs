using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class AddressTypeModel
    {
        public Address_Type GetSpecificAddressType(int addressTypeId)
        {
            Address_Type addressType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var addressTypeList = (from r in dbContext.Address_Types
                                       where r.Address_Type_Id.Equals(addressTypeId)
                                       select r).ToList();

                addressType = (from r in addressTypeList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return addressType;
        }

        public List<Address_Type> GetListOfAddressTypes()
        {
            List<Address_Type> addressTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var addressTypeList = (from r in dbContext.Address_Types
                                           select r).ToList();

                    addressTypes = (from r in addressTypeList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return addressTypes;
        }
    }
}
