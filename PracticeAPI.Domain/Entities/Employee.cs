using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int EmployeeId { get; set; }
        public decimal Salary { get; set; }
    }
}
