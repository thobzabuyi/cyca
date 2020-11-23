using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class EmployeeService_RoleVM
    {
        public int Employee_Id { get; set; }
        public int? EmployeeServiceId { get; set; }
        public int? EmployeeRoleId { get; set; }
        public string ServiceDescription { get; set; }
        public string RoleDescription { get; set; }
    }
}
