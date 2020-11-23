using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class CPRUnmonitoredCasesViewModel
    {
        public bool is_Filtered { get; set; }
        public int? pageNumber { get; set; }
        public string SearchRecordNumber { get; set; }
        public string SearchFirstName { get; set; }
        public string SearchLastName { get; set; }
        public string SearchTypeOfAbuse { get; set; }
        public string SearchDateRegistered { get; set; }
    }
}
