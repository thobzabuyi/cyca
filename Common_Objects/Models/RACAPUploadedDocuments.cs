using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common_Objects.Models
{
    public class RACAPUploadedDocuments
    {
        public int RacapCaseId { get; set; }
        public int int_Assessment_Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string AdditionalInformations { get; set; }
        public int AdditionalInformationsId { get; set; }
        public IEnumerable<HttpPostedFile> Files { get; set; }
    }
}
