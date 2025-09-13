using PracticeAPI.Domain.Entities;

namespace PracticeAPI.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task AddAsync(Employee employee);
        Task<bool> DeleteByIdAsync(int id);
        Task<Employee?> GetByIdAsync(int id);
        Task UpdateAsync(Employee employee);
    }
}
