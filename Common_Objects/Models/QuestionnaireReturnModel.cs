using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class QuestionnaireReturnModel
    {
        public int ProfilingInstanceId { get; set; }
        public int ProfilingToolId { get; set; }
        public int SectionId { get; set; }
        public int QuestionId { get; set; }
        public int ColumnId { get; set; }
        public int PersonId { get; set; }
        public string AnswerValue { get; set; }
    }
}
