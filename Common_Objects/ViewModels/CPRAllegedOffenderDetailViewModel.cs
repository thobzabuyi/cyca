using System.Collections.Generic;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class CPRAllegedOffenderDetailViewModel
    {
        public int Person_Id { get; set; }
        public int Incident_Id { get; set; }
        public CPRAllegedOffenderSearchViewModel SearchPerson { get; set; }
        public Person AllegedOffenderPerson { get; set; }
        public PersonDetailViewModel CreatePerson { get; set; }
        public Alleged_Offender AllegedOffender { get; set; }
        public CPR_Section_153 CPRSection153 { get; set; }
        public List<CPR_Incident> CrossReferences { get; set; }
        public List<CPR_Conviction> Convictions { get; set; }
        public CPR_Conviction ConvictionDetails { get; set; }
        public AddressViewModel PhyAdd { get; set; }
        public AddressViewModel PosAdd { get; set; }
    }
}
