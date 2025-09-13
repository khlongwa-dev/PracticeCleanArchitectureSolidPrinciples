using Moq;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.UseCaseImplementations;
using PracticeAPI.Application.UseCaseInterfaces;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Tests.EmployeeServiceTests
{
    public class GetAllEmployeesAsyncTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IEmployeeMapper> _employeeMapperMock;
        private IEmployeeServices _employeeServices;

        public GetAllEmployeesAsyncTests() 
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeMapperMock = new Mock<IEmployeeMapper>();
            _employeeServices = new EmployeeServices(_employeeRepositoryMock.Object, _employeeMapperMock.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WhenEmployeesExist_ReturnsListOfEmployees()
        {
            // Arrange
            // Setup the mock list of employees
            var employeeList = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", EmployeeId = 101, Salary = 20000m },
                new Employee { Id = 2, Name = "Jane Smith", EmployeeId = 102, Salary = 30000m }
            };


            _employeeRepositoryMock.Setup(r => r.GetAllAsync())
                                .ReturnsAsync(employeeList);
            // Act
            var employees = await _employeeServices.GetAllEmployeesAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(employees);
            Assert.NotEmpty(employees);
            Assert.Equal(2, employees.Count());
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WhenEmployeesDonotExist_ReturnsEmptyList()
        {
            // Arrange
            // Setup empty mock list of employees
            var employeeList = new List<Employee>();

            _employeeRepositoryMock.Setup(r => r.GetAllAsync())
                                .ReturnsAsync(employeeList);
            // Act
            var employees = await _employeeServices.GetAllEmployeesAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(employees);
            Assert.Empty(employees);
        }
    }
}
