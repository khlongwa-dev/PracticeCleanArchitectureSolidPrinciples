using Moq;
using PracticeAPI.Application.UseCaseInterfaces;
using PracticeAPI.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAPI.Tests.EmployeeControllerTests
{
    public class RemoveEmployeeTests
    {
        private readonly Mock<IEmployeeServices> _services;
        private readonly EmployeeController controller;

        public RemoveEmployeeTests()
        {
            _services = new Mock<IEmployeeServices>();
            controller = new EmployeeController(_services.Object);
        }

        [Fact]

    }
}
