using Moq;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.UseCaseImplementations;
using PracticeAPI.Domain.Entities;
using PracticeAPI.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Tests.EmployeeServiceTests
{
    public class DeleteEmployeeAsyncTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IEmployeeMapper> _employeeMapperMock;
        private EmployeeServices _employeeServices;
        private int employeeEntityId;
        public DeleteEmployeeAsyncTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeMapperMock = new Mock<IEmployeeMapper>();
            _employeeServices = new EmployeeServices(_employeeRepositoryMock.Object, _employeeMapperMock.Object);
            employeeEntityId = 1;
        }

        [Fact]
        public async Task DeleteEmployeeByIdAsync_ValidId_CallsRepositoryOnce()
        {
            // Setup the mock to return true (employee deleted successfully)
            _employeeRepositoryMock.Setup(r => r.DeleteByIdAsync(employeeEntityId))
                                .ReturnsAsync(true);

            // Act
            var success = await _employeeServices.DeleteEmployeeByIdAsync(employeeEntityId);

            // Assert
            _employeeRepositoryMock.Verify(r => r.DeleteByIdAsync(employeeEntityId), Times.Once());
            Assert.True(success);
        }

        [Fact]
        public async Task DeleteEmployeeByIdAsync_InvalidId_CallsRepositoryOnce()
        {
            // Setup the mock to return false (employee not found)
            _employeeRepositoryMock.Setup(r => r.DeleteByIdAsync(employeeEntityId))
                                .ReturnsAsync(false);

            // Act
            var success = await _employeeServices.DeleteEmployeeByIdAsync(employeeEntityId);
            
            // Assert
            _employeeRepositoryMock.Verify(r => r.DeleteByIdAsync(employeeEntityId), Times.Once);
            Assert.False(success);
        }
    }
}