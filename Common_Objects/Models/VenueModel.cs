using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VenueModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public VenueModel()
        { }
        public List<apl_Cyca_Venue> GetAplCycaVenues()
        {
            return db.apl_Cyca_Venue.ToList();
        }

    }
}
