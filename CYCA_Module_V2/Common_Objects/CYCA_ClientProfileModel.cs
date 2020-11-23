using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCA_ClientProfileModel
    {
        public CYCA_ClientBiometricViewModel GetBiometricViewModel(int PersonId)
        {
            CYCA_ClientBiometricViewModel returnModel = new CYCA_ClientBiometricViewModel();
            using (SDIIS_DatabaseEntities _context = new SDIIS_DatabaseEntities())
            {
                var afis = _context.int_DSD_Afis.Where(af => af.Person_Id.Equals(PersonId)).SingleOrDefault();
                var person = _context.Persons.Where(p => p.Person_Id.Equals(PersonId)).Single();
                if(afis!=null)
                {
                    returnModel.HasBiometric = true;
                    returnModel.IsPivaVerified = person.Is_Piva_Validated;
                    returnModel.IsVerified = afis.Is_Verified;
                    returnModel.UniqueIdentifier = afis.Uid.ToString();
                }
                else
                {
                    returnModel.HasBiometric = false;
                    returnModel.IsPivaVerified = false;
                    returnModel.IsVerified = false;
                    returnModel.UniqueIdentifier = "00000000-0000-0000-0000-000000000000";
                }
            }
            return returnModel;
        }
    }
}