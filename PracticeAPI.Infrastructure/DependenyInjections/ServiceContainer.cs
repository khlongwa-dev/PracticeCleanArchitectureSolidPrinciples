using Microsoft.Extensions.DependencyInjection;
using PracticeAPI.Domain.RepositoryInterfaces;
using PracticeAPI.Infrastructure.RepositoriesImplementations;

namespace PracticeAPI.Infrastructure.DependenyInjections
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return service;
        }
    }
}
