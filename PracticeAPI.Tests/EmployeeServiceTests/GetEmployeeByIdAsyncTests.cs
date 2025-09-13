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
    public class GetEmployeeByIdAsyncTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IEmployeeMapper> _employeeMapperMock;
        private IEmployeeServices _employeeServices;
        private int employeeEntityId;
        public GetEmployeeByIdAsyncTests()
        { 
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeMapperMock = new Mock<IEmployeeMapper>();
            _employeeServices = new EmployeeServices(_employeeRepositoryMock.Object, _employeeMapperMock.Object);
            employeeEntityId = 1;
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WhenTheEmployeeExist_ReturnsTheEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            var expectedDto = new EmployeeDto
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            // Setup the return of the employee dto which exactly maps the employee
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(employeeEntityId))
                                .ReturnsAsync(employee);

            _employeeMapperMock.Setup(m => m.MapToEmployeeDto(employee))
                      .Returns(expectedDto);
            // Act
            var employeeDto = await _employeeServices.GetEmployeeByIdAsync(employeeEntityId);

            _employeeRepositoryMock.Verify(r => r.GetByIdAsync(employeeEntityId), Times.Once);

            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(employeeEntityId, employeeDto.Id);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WhenTheEmployeeNotExist_ReturnsNull()
        {
            // Arrange
            // Setup the return of the employee dto which exactly maps the employee
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(employeeEntityId))
                                .ReturnsAsync((Employee?)null);
            // Act
            var employeeDto = await _employeeServices.GetEmployeeByIdAsync(employeeEntityId);

            // Assert
            Assert.Null(employeeDto);
        }
    }
}
