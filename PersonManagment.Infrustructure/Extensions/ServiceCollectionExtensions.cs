using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonManagment.Infrustructure.Persistence;

namespace PersonManagment.Infrustructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrustructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PersonManagmentDb");
            services.AddDbContext<PersonManagmentDbContext>( options => options.UseSqlServer(connectionString));
        }
    }
}
