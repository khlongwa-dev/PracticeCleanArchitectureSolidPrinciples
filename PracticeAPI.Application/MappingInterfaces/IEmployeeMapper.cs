using PracticeAPI.Application.DTOs;
using PracticeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Application.MappingInterfaces
{
    public interface IEmployeeMapper
    {
        EmployeeDto MapToEmployeeDto(Employee employee);
        Employee MapToEmployeeEntity(CreateEmployeeDto createDto);
        Employee UpdateEmployeeEntity(UpdateEmployeeDto createDto, Employee employee);
    }
}
