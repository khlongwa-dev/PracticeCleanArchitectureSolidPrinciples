using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.UseCaseInterfaces;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Application.UseCaseImplementations
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeMapper _employeeMapper;
        public EmployeeServices(IEmployeeRepository employeeRepository, IEmployeeMapper employeeMapper)
        {
            _employeeRepository = employeeRepository;
            _employeeMapper = employeeMapper;
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto createDto)
        {
            var employee = _employeeMapper.MapToEmployeeEntity(createDto);
            await _employeeRepository.AddAsync(employee);
        }

        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            var success = await _employeeRepository.DeleteByIdAsync(id);

            return success;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees?.Select(employee => _employeeMapper.MapToEmployeeDto(employee)).ToList() ?? new List<EmployeeDto>();
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            return employee == null? null : _employeeMapper.MapToEmployeeDto(employee);
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto updateDto, Employee employee)
        {
            var updatedEmployee = _employeeMapper.UpdateEmployeeEntity(updateDto, employee);
            await _employeeRepository.UpdateAsync(updatedEmployee);
        }
    }
}
