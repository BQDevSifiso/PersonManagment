using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrastructure.Repositories;
using PersonManagment.Infrustructure.Persistence;
using TVA.Infrastructure.Repositories;

namespace PersonManagment.Infrustructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrustructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PersonManagmentDb");
            services.AddDbContext<PersonManagmentDbContext>( options => options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<PersonService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TransactionService>();
            services.AddTransient<ISearchPersonAccounts, SearchPersonAccounts>();
            services.AddTransient<ISearchPerson, SearchPerson>();
            services.AddTransient<ISearchAccount, SearchAccount>();
            services.AddTransient<ISearchTransaction, SearchTransaction>();
        }
    }
}
