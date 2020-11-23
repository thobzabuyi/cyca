using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class AttachmentViewModel
    {
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public int AttachmentId { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
