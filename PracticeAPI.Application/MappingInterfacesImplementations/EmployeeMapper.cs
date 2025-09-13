using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Application.MappingInterfacesImplementations
{
    public class EmployeeMapper : IEmployeeMapper
    {
        public EmployeeDto MapToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                EmployeeId = employee.Id,
                Salary = employee.Salary
            };
        }

        public Employee MapToEmployeeEntity(CreateEmployeeDto createDto)
        {
            return new Employee
            {
                Name = createDto.Name,
                Salary = createDto.Salary,
                EmployeeId = createDto.EmployeeId
            };
        }

        public Employee UpdateEmployeeEntity(UpdateEmployeeDto createDto, Employee employee)
        {
            employee.Salary = createDto.Salary;

            return employee;
        }
    }
}
