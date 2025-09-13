using Moq;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.MappingInterfacesImplementations;
using PracticeAPI.Application.UseCaseImplementations;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;

namespace PracticeAPI.Tests.EmployeeServiceTests
{
    public class UpdateEmployeeAsyncTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IEmployeeMapper> _employeeMapperMock;
        private EmployeeServices _employeeServices;

        public UpdateEmployeeAsyncTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeMapperMock = new Mock<IEmployeeMapper>();
            _employeeServices = new EmployeeServices(_employeeRepositoryMock.Object, _employeeMapperMock.Object);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenEmployeeExist_ReturnsTrue()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };
    
            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };
    
            var updatedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 30000m
            };
    
            // Setup mapper to return updated employee
            _employeeMapperMock.Setup(m => m.UpdateEmployeeEntity(updateDto, originalEmployee))
                              .Returns(updatedEmployee);
    
            // Setup repository
            _employeeRepositoryMock.Setup(r => r.UpdateAsync(updatedEmployee))
                                .Returns(Task.CompletedTask);
    
            // Act
            await _employeeServices.UpdateEmployeeAsync(updateDto, originalEmployee);
    
            // Assert
            _employeeMapperMock.Verify(m => m.UpdateEmployeeEntity(updateDto, originalEmployee), Times.Once);
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(updatedEmployee), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenMapperReturnsNull_ShouldStillCallRepository()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };

            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            // Setup mapper to return null (edge case)
            _employeeMapperMock.Setup(m => m.UpdateEmployeeEntity(updateDto, originalEmployee))
                              .Returns((Employee?)null);

            // Setup repository
            _employeeRepositoryMock.Setup(r => r.UpdateAsync(null))
                                .Returns(Task.CompletedTask);

            // Act
            await _employeeServices.UpdateEmployeeAsync(updateDto, originalEmployee);

            // Assert
            _employeeMapperMock.Verify(m => m.UpdateEmployeeEntity(updateDto, originalEmployee), Times.Once);
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(null), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };

            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            var updatedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 30000m
            };

            _employeeMapperMock.Setup(m => m.UpdateEmployeeEntity(updateDto, originalEmployee))
                              .Returns(updatedEmployee);

            _employeeRepositoryMock.Setup(r => r.UpdateAsync(updatedEmployee))
                                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _employeeServices.UpdateEmployeeAsync(updateDto, originalEmployee));

            // Verify mapper was called
            _employeeMapperMock.Verify(m => m.UpdateEmployeeEntity(updateDto, originalEmployee), Times.Once);
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(updatedEmployee), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenMapperThrows_PropagatesException()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };

            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            _employeeMapperMock.Setup(m => m.UpdateEmployeeEntity(updateDto, originalEmployee))
                              .Throws(new ArgumentException("Invalid update data"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _employeeServices.UpdateEmployeeAsync(updateDto, originalEmployee));

            // Verify mapper was called
            _employeeMapperMock.Verify(m => m.UpdateEmployeeEntity(updateDto, originalEmployee), Times.Once);

            // Verify repository was never called
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithRealMapper_ShouldUpdateCorrectly()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };

            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            // Use real mapper if you want to test integration
            var realMapper = new EmployeeMapper();
            var employeeService = new EmployeeServices(_employeeRepositoryMock.Object, realMapper);

            _employeeRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
                                .Returns(Task.CompletedTask);

            // Act
            await employeeService.UpdateEmployeeAsync(updateDto, originalEmployee);

            // Assert
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Employee>(e =>
                e.Id == originalEmployee.Id &&
                e.Name == originalEmployee.Name &&
                e.EmployeeId == originalEmployee.EmployeeId &&
                e.Salary == 30000m)), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldPassSameEmployeeObjectToRepository()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                Salary = 30000m
            };

            var originalEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            // Setup mapper to return the same employee object (simulating in-place update)
            _employeeMapperMock.Setup(m => m.UpdateEmployeeEntity(updateDto, originalEmployee))
                              .Returns(originalEmployee);

            _employeeRepositoryMock.Setup(r => r.UpdateAsync(originalEmployee))
                                .Returns(Task.CompletedTask);

            // Act
            await _employeeServices.UpdateEmployeeAsync(updateDto, originalEmployee);

            // Assert
            _employeeMapperMock.Verify(m => m.UpdateEmployeeEntity(updateDto, originalEmployee), Times.Once);
            _employeeRepositoryMock.Verify(r => r.UpdateAsync(originalEmployee), Times.Once);
        }
    }
}
