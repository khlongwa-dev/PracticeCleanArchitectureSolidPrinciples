using Microsoft.EntityFrameworkCore;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;
using PracticeAPI.Infrastructure.DatabaseContext;

namespace PracticeAPI.Infrastructure.RepositoriesImplementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            await _appDbContext.Employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var employee = await _appDbContext.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _appDbContext.Employees.Remove(employee);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _appDbContext.Employees.ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var employee = await _appDbContext.Employees.FindAsync(id);
            return employee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _appDbContext.Employees.Update(employee);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
