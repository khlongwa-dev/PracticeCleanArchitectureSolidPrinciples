using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeAPI.Application.DTOs;
using PracticeAPI.Application.UseCaseInterfaces;

namespace PracticeAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeDto createDto)
        {
            await _employeeServices.AddEmployeeAsync(createDto);
            return Ok(new { success = true, message = "Employee successfully added."});
        }


    }
}
