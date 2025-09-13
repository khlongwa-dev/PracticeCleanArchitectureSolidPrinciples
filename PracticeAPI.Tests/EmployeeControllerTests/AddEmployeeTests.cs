using Microsoft.AspNetCore.Mvc;
using Moq;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.UseCaseInterfaces;
using PracticeAPI.Presentation.Controllers;

namespace PracticeAPI.Tests.EmployeeControllerTests
{
    public class AddEmployeeTests
    {
        private readonly Mock<IEmployeeServices> _employeeServicesMock;
        private readonly EmployeeController _controller;

        public AddEmployeeTests()
        {
            _employeeServicesMock = new Mock<IEmployeeServices>();
            _controller = new EmployeeController(_employeeServicesMock.Object);
        }

        [Fact]
        public async Task AddEmployee_ValidEmployee_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            _employeeServicesMock
                .Setup(x => x.AddEmployeeAsync(It.IsAny<CreateEmployeeDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddEmployee(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value;
           
            var successProperty = response?.GetType().GetProperty("success");
            var messageProperty = response?.GetType().GetProperty("message");

            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);
            Assert.Equal("Employee successfully added.", messageProperty.GetValue(response));

            // Verify service was called exactly once with the correct parameter
            _employeeServicesMock.Verify(x => x.AddEmployeeAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task AddEmployee_ServiceThrowsException_ExceptionBubblesUp()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 20000m
            };

            var expectedExceptionMessage = "Database connection failed";
            _employeeServicesMock
                .Setup(x => x.AddEmployeeAsync(It.IsAny<CreateEmployeeDto>()))
                .ThrowsAsync(new InvalidOperationException(expectedExceptionMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _controller.AddEmployee(createDto));

            Assert.Equal(expectedExceptionMessage, exception.Message);

            // Verify service was called exactly once
            _employeeServicesMock.Verify(x => x.AddEmployeeAsync(createDto), Times.Once);
        }

        [Theory]
        [InlineData("", 101, 40000)]
        [InlineData("John Doe", 101, 40000)]
        [InlineData("Jane Smith", 102, 50000)]
        [InlineData("Bob Johnson", 103, 35000)]
        public async Task AddEmployee_VariousInputs_CallsServiceAndReturnsOk(
            string name, int employeeId, decimal salary)
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Name = name,           // Use the parameter value
                EmployeeId = employeeId,  // Use the parameter value
                Salary = salary        // Use the parameter value
            };

            _employeeServicesMock
                .Setup(x => x.AddEmployeeAsync(It.IsAny<CreateEmployeeDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddEmployee(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _employeeServicesMock.Verify(x => x.AddEmployeeAsync(createDto), Times.Once);
        }
    }
}
