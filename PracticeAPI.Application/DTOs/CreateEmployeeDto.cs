using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Application.DTOs
{
    public class CreateEmployeeDto
    {
        public required string Name { get; set; }
        public int EmployeeId { get; set; }
        public decimal Salary { get; set; }
    }
}
