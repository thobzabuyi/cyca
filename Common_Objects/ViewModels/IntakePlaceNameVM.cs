using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class IntakePlaceNameVM
    {
        public int Id { get; set; }
        public int CountyId { get; set; }
        public string Country { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        public int MunicipalityId { get; set; }
        public string Municpality { get; set; }
        public int LocalMunicipalityId { get; set; }
        public string LocalMunicipality { get; set; }
        public int TownId { get; set; }
        public string Town { get; set; }
        public int WardId { get; set; }
        public string Ward { get; set; }

    }
}
