using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYCA_Module_V2.Common_Objects
{
    public class DynamicFormSearchViewModel
    {
        public int ChildId { get; set; }
        public int DynamicFormId { get; set; }
        public List<DynamicFormGridMain> DynamicFormGridMains { get; set; }

    }
    public partial class DynamicFormGridMain
    {
        public int DynamicFormDataId { get; set; }

        public int ChildId { get; set; }
        public int UserId { get; set; }
        public string ChildName { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
