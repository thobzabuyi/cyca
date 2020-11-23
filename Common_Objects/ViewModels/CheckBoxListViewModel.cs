using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Common_Objects.ViewModels
{
    public class CheckBoxListViewModel
    {
        public IEnumerable<CheckBoxListItems> Available { get; set; }
        public IEnumerable<CheckBoxListItems> Selected { get; set; }
        public PostCheckBoxList Posted { get; set; }
    }
}
