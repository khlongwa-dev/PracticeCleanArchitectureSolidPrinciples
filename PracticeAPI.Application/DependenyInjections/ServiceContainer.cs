using Microsoft.Extensions.DependencyInjection;
using PracticeAPI.Application.MappingInterfaces;
using PracticeAPI.Application.MappingInterfacesImplementations;
using PracticeAPI.Application.UseCaseImplementations;
using PracticeAPI.Application.UseCaseInterfaces;

namespace PracticeAPI.Application.DependenyInjections
{
    public static class ServiceContainer
    {
        /*This is necess*/
        public static IServiceCollection AddApplicationServices(this IServiceCollection service)
        {
            service.AddScoped<IEmployeeServices, EmployeeServices>();
            service.AddScoped<IEmployeeMapper, EmployeeMapper>();
            return service;
        }
    }
}
