using Microsoft.EntityFrameworkCore;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Infrastructure.DatabaseContext;
using PracticeAPI.Infrastructure.RepositoriesImplementations;

namespace PracticeAPI.Tests.RepositoriesTests
{
    public class EmployeeRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Use a unique database name for each test to avoid conflicts
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_WhenEmployeesExist_ReturnsAllEmployees()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            // Seed test data
            var employees = new List<Employee>
            {
                new Employee { Id = 1, EmployeeId = 101, Name = "John Doe", Salary = 50000m },
                new Employee { Id = 2, EmployeeId = 102, Name = "Jane Smith", Salary = 60000m },
                new Employee { Id = 3, EmployeeId = 103, Name = "Bob Johnson", Salary = 55000m }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            var repository = new EmployeeRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());

            var resultList = result.ToList();
            Assert.Contains(resultList, e => e.Name == "John Doe" && e.EmployeeId == 101);
            Assert.Contains(resultList, e => e.Name == "Jane Smith" && e.EmployeeId == 102);
            Assert.Contains(resultList, e => e.Name == "Bob Johnson" && e.EmployeeId == 103);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoEmployees_ReturnsEmptyCollection()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EmployeeRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_VerifiesCorrectSalaryValues()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var employee = new Employee
            {
                Id = 1,
                EmployeeId = 101,
                Name = "Test Employee",
                Salary = 75000.50m
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var repository = new EmployeeRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            var retrievedEmployee = result.First();
            Assert.Equal(75000.50m, retrievedEmployee.Salary);
            Assert.Equal("Test Employee", retrievedEmployee.Name);
            Assert.Equal(101, retrievedEmployee.EmployeeId);
        }

        [Fact]
        public async Task AddAsync_WhenEmployeeIsNotNull_AddsEmployeeOnDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var employeeToAdd = new Employee
            { 
                Id = 1, 
                EmployeeId = 101,
                Name = "John Doe", 
                Salary = 50000m
            };

            var repository = new EmployeeRepository(context);

            // Act
            await repository.AddAsync(employeeToAdd);

            // Assert
            var addedEmployee = context.Employees.First();

            Assert.NotNull(addedEmployee);
            Assert.Equal("John Doe", addedEmployee.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEmployeeWithPassedIdExist_ReturnsEmployee()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var employee = new Employee
            {
                Id = 1,
                EmployeeId = 101,
                Name = "John Doe",
                Salary = 50000m
            };

            context.Employees.Add(employee);

            var repository = new EmployeeRepository(context);

            // Act
            var employeeFoundById = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(employeeFoundById);
            Assert.Equal("John Doe", employeeFoundById.Name);
            Assert.Equal(50000m, employeeFoundById.Salary);
            Assert.Equal(101, employeeFoundById.EmployeeId);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEmployeeWithPassedIdDoesNotExist_ReturnsNull()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var repository = new EmployeeRepository(context);

            // Act
            var nonExistingEmployee = await repository.GetByIdAsync(1);

            // Assert
            Assert.Null(nonExistingEmployee);    
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenEmployeeWithPassedIdExists_RemovesEmployeeAndReturnsTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var employee = new Employee
            {
                Id = 1,
                EmployeeId = 101,
                Name = "John Doe",
                Salary = 50000m
            };

            context.Employees.Add(employee);

            var repository = new EmployeeRepository(context);

            // Act
            var results = await repository.DeleteByIdAsync(1);

            var firstFoundEmployee = context.Employees.FirstOrDefault();

            // Assert
            Assert.True(results);
            Assert.Null(firstFoundEmployee);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenEmployeeWithPassedIdDoesNotExists_ReturnsFalse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var repository = new EmployeeRepository(context);

            // Act
            var results = await repository.DeleteByIdAsync(1);

            // Assert
            Assert.False(results);
        }

        [Fact]
        public async Task UpdateAsync_WhenEmployeeExists_UpdatesEmployeeWithNewProperties()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var originalEmployee = new Employee
            {
                Id = 1,
                EmployeeId = 101,
                Name = "John Doe",
                Salary = 50000m
            };

            context.Employees.Add(originalEmployee);
            await context.SaveChangesAsync();

            var repository = new EmployeeRepository(context);

            // Get the tracked entity and modify it
            var employeeToUpdate = await context.Employees.FindAsync(1);

            employeeToUpdate.Name = "John Smith";
            employeeToUpdate.Salary = 60000m;

            // Act
            await repository.UpdateAsync(employeeToUpdate);

            // Assert
            var result = await context.Employees.FindAsync(1);
            Assert.NotNull(result);
            Assert.Equal("John Smith", result.Name);
            Assert.Equal(60000m, result.Salary);
        }

        [Fact]
        public async Task UpdateAsync_WhenEmployeeDoesNotExist_()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EmployeeRepository(context);

            var nonExistentEmployee = new Employee
            {
                Id = 999,
                EmployeeId = 999,
                Name = "Ghost Employee",
                Salary = 50000m
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => repository.UpdateAsync(nonExistentEmployee));
        }
    }
}



