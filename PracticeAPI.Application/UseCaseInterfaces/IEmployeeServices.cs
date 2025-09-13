using PracticeAPI.Application.DTOs;
using PracticeAPI.Domain.Entities;

namespace PracticeAPI.Application.UseCaseInterfaces
{
    public interface IEmployeeServices
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(CreateEmployeeDto createDto);
        Task<bool> DeleteEmployeeByIdAsync(int id);
        Task UpdateEmployeeAsync(UpdateEmployeeDto updateDto, Employee employee);
    }
}
