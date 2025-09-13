using Moq;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.UseCaseImplementations;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;


namespace PracticeAPI.Tests.EmployeeServiceTests
{
    public class AddEmployeeAsyncTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IEmployeeMapper> _employeeMapperMock;
        private EmployeeServices _employeeServices;

        public AddEmployeeAsyncTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeMapperMock = new Mock<IEmployeeMapper>();
            _employeeServices = new EmployeeServices(_employeeRepositoryMock.Object, _employeeMapperMock.Object);
        }

        [Fact]
        public async Task AddProductAsync_ValidCreateDto_CallsMapperAndRepository()
        {
            var createDto = new CreateEmployeeDto
            {
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            var mappedEmployee = new Employee
            {
                Id = 1,
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            _employeeMapperMock.Setup(m => m.MapToEmployeeEntity(createDto))
                .Returns(mappedEmployee);

            _employeeRepositoryMock.Setup(r => r.AddAsync(mappedEmployee))
                .Returns(Task.CompletedTask);

            // Act
            await _employeeServices.AddEmployeeAsync(createDto);

            // Assert
            // Verify mapper was called with correct DTO
            _employeeMapperMock.Verify(m => m.MapToEmployeeEntity(createDto), Times.Once);

            // Verify repository was called with mapped product
            _employeeRepositoryMock.Verify(r => r.AddAsync(mappedEmployee), Times.Once);
        }

        [Fact]
        public async Task AddProductAsync_ValidCreateDto_CallsRepositoryWithCorrectProduct()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            var expectedEmployee = new Employee
            {
                Id = 1,
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            _employeeMapperMock.Setup(m => m.MapToEmployeeEntity(createDto))
                      .Returns(expectedEmployee);

            // Act
            await _employeeServices.AddEmployeeAsync(createDto);

            // Assert
            // Verify repository receives the exact employee from mapper
            _employeeRepositoryMock.Verify(r => r.AddAsync(
                It.Is<Employee>(e =>
                    e.Name == expectedEmployee.Name &&
                    e.EmployeeId == expectedEmployee.EmployeeId &&
                    e.Salary == expectedEmployee.Salary)),
                Times.Once);
        }

        [Fact]
        public async Task AddEmployeeAsync_MapperThrowsException_ExceptionPropagated()
        {
            // Arrange
            var createDto = new CreateEmployeeDto { Name = "Test" };
            var expectedException = new InvalidOperationException("Mapping failed");

            _employeeMapperMock.Setup(m => m.MapToEmployeeEntity(createDto))
                      .Throws(expectedException);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _employeeServices.AddEmployeeAsync(createDto));

            Assert.Equal("Mapping failed", actualException.Message);

            // Verify repository was never called due to mapper exception
            _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task AddEmployeeAsync_RepositoryThrowsException_ExceptionPropagated()
        {
            // Arrange
            var createDto = new CreateEmployeeDto { Name = "Test" };
            var mappedEmployee = new Employee { Name = "Test" };
            var expectedException = new InvalidOperationException("Database error");

            _employeeMapperMock.Setup(m => m.MapToEmployeeEntity(createDto))
                      .Returns(mappedEmployee);

            _employeeRepositoryMock.Setup(r => r.AddAsync(mappedEmployee))
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _employeeServices.AddEmployeeAsync(createDto));

            Assert.Equal("Database error", actualException.Message);

            // Verify mapper was still called before repository failed
            _employeeMapperMock.Verify(m => m.MapToEmployeeEntity(createDto), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeAsync_CallsMethodsInCorrectOrder()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            var mappedEmployee = new Employee
            {
                Id = 1,
                Name = "Test",
                EmployeeId = 123,
                Salary = 2000m
            };

            var callOrder = new List<string>();

            _employeeMapperMock.Setup(m => m.MapToEmployeeEntity(createDto))
                      .Returns(mappedEmployee)
                      .Callback(() => callOrder.Add("Mapper"));

            _employeeRepositoryMock.Setup(r => r.AddAsync(mappedEmployee))
                          .Returns(Task.CompletedTask)
                          .Callback(() => callOrder.Add("Repository"));

            // Act
            await _employeeServices.AddEmployeeAsync(createDto);

            // Assert
            Assert.Equal(new[] { "Mapper", "Repository" }, callOrder);
        }
    }
}


