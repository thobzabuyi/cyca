using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class AddressModel
    {
        public Address GetSpecificAddress(int addressId)
        {
            Address address;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var addressList = (from r in dbContext.Addresses
                                   where r.Address_Id.Equals(addressId)
                                   select r).ToList();

                address = (from r in addressList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return address;
        }

        public List<Address> GetListOfAddresses()
        {
            List<Address> addresses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var addressList = (from r in dbContext.Addresses
                                       select r).ToList();

                    addresses = (from r in addressList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return addresses;
        }

        //public Address CreateAddress(int addressTypeId, string addressLine1, string addressLine2, int? townId, string postalCode)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();

        //    var address = new Address() { Address_Type_Id = addressTypeId, Address_Line_1 = addressLine1, Address_Line_2 = addressLine2, Town_Id = townId, Postal_Code = postalCode };

        //    try
        //    {
        //        var newAddress = dbContext.Addresses.Add(address);

        //        dbContext.SaveChanges();

        //        return newAddress;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public Address EditAddress(int addressId, int addressTypeId, string addressLine1, string addressLine2, int? townId, string postalCode)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editAddress = (from a in dbContext.Addresses
                                   where a.Address_Id.Equals(addressId)
                                   select a).FirstOrDefault();

                if (editAddress == null) return null;

                editAddress.Address_Type_Id = addressTypeId;
                editAddress.Address_Line_1 = addressLine1;
                editAddress.Address_Line_2 = addressLine2;
                editAddress.Town_Id = townId;
                editAddress.Postal_Code = postalCode;

                dbContext.SaveChanges();

                return editAddress;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;

            }
        }

        public CPR_Incident EditAddressCPR(int IncidentId, int addressId, int addressTypeId, string addressLine1, string addressLine2, int? townId, string postalCode)
        {
            CPR_Incident editIncident;
            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editIncident = (from p in dbContext.CPR_Incidents
                                    where p.Incident_Id.Equals(IncidentId)
                                    select p).FirstOrDefault();

                    //var editAddress = (from a in dbContext.Addresses
                    //                   where a.Address_Id.Equals(addressId)
                    //                   select a).FirstOrDefault();

                    //if (editAddress == null) {
                    //    editAddress = new Address();
                    //    editAddress.Address_Type_Id = addressTypeId;
                    //    editAddress.Address_Line_1 = addressLine1;
                    //    editAddress.Address_Line_2 = addressLine2;
                    //    editAddress.Town_Id = townId;
                    //    editAddress.Postal_Code = postalCode;
                    //    dbContext.Addresses.Add(editAddress);
                    //    dbContext.SaveChanges();
                    //};
                    if (editIncident == null) return null;
                    editIncident.Addresses.Add(new Address() { Address_Type_Id = addressTypeId, Address_Line_1 = addressLine1, Address_Line_2 = addressLine2, Town_Id = townId, Postal_Code = postalCode });
                    //editAddress.Address_Type_Id = addressTypeId;
                    //editAddress.Address_Line_1 = addressLine1;
                    //editAddress.Address_Line_2 = addressLine2;
                    //editAddress.Town_Id = townId;
                    //editAddress.Postal_Code = postalCode;

                    dbContext.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return editIncident;
        }

    }
}
