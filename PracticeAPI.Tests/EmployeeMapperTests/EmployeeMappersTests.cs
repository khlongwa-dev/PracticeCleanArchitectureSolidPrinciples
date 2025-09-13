using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.MappingInterfacesImplementations;
using PracticeAPI.Domain.Entities;

namespace PracticeAPI.Tests.EmployeeMapperTests
{
    public class EmployeeMappersTests
    {
        private readonly IEmployeeMapper _employeeMapper;
        public EmployeeMappersTests()
        {
            _employeeMapper = new EmployeeMapper();
        }

        [Fact]
        public void MapToEmployeeEntity_MapsTheDtoToEmployeeEntity()
        {
            // Arrange
            var createEmployeeDto = new CreateEmployeeDto
            {
                Name = "John Doe",
                EmployeeId = 123,
                Salary = 50000m
            };

            // Act
            var employee = _employeeMapper.MapToEmployeeEntity(createEmployeeDto);

            // Assert
            Assert.NotNull(employee);
            Assert.Equal("John Doe", employee.Name);
            Assert.Equal(123, employee.EmployeeId);
            Assert.Equal(50000m, employee.Salary);
        }

        [Fact]
        public void MapToEmployeeDto_MapsTheEmployeeEntityToDto()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 123,
                Salary = 50000m
            };

            // Act
            var employeeDto = _employeeMapper.MapToEmployeeDto(employee);

            // Assert
            Assert.NotNull(employeeDto);
            Assert.True(employeeDto.Id == 1);
        }

        [Fact]
        public void UpdateEmployeeEntity_ReturnaUpdatedEmployeeEntity()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 123,
                Salary = 50000m
            };

            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Salary = 70000m
            };

            // Act
            var updatedEmployee = _employeeMapper.UpdateEmployeeEntity(updateEmployeeDto, employee);

            // Assert
            Assert.True(updatedEmployee.Id == 1);
            Assert.True(updatedEmployee.Name == "John Doe");
            Assert.True(updatedEmployee.EmployeeId == 123);
            Assert.True(updatedEmployee.Salary == 70000m);
        }
    }
}


