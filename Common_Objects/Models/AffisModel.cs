using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class AffisModel
    {
        private readonly SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

        public async Task<int> AddAffis(int_DSD_Afis afisRecord)
        {        
            db.int_DSD_Afis.Add(afisRecord);
            await db.SaveChangesAsync();
            return 1;
        }
        public async Task<int> RemoveAffis(Guid Uid)
        {
            var aa = await db.int_DSD_Afis.Where(aaa => aaa.Uid == Uid).SingleAsync();
            db.int_DSD_Afis.Remove(aa);
            await db.SaveChangesAsync();
            return 1;
        }
        public int_DSD_Afis GetRecord(Guid uuid)
        {
            return  db.int_DSD_Afis.Where(a => a.Uid == uuid).SingleOrDefault();      
        }
        public Person GetUserByRecord(int Person_Id)
        {
           return db.Persons.Where(pp => pp.Person_Id == Person_Id).SingleOrDefault();
        }

        public int_DSD_Afis CheckGuid(Guid guid)
        {
         return   db.int_DSD_Afis.Where(afis => afis.Uid == guid).SingleOrDefault();
        }
    }
}
