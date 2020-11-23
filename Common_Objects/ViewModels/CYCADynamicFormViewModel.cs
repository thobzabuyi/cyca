
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CYCADynamicFormViewModel
    {
        public int DynamicFormId { get; set; }
        public int DynamicFormTypeId { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public string Definition { get; set; }
        public string SummaryDefinition { get; set; }
        public string Answer { get; set; }
        public int ChildId { get; set; }
        public int UserId { get; set; }
        public int AnswerId { get; set; }
    }



   
}

