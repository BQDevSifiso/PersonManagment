using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrastructure.Repositories;
using PersonManagment.Infrustructure.Persistence;
using System.Text;
using TVA.Infrastructure.Repositories;

namespace PersonManagment.Infrustructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrustructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PersonManagmentDb");
            services.AddDbContext<PersonManagmentDbContext>( options => options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = true;    
                option.Password.RequireLowercase = true;
                option.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<PersonManagmentDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                   // ValidAudience = "http://ahmadmozaffar.net",
                    //ValidIssuer = "http://ahmadmozaffar.net",
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is the key that we will use in the encryption")),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<PersonService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TransactionService>();
            services.AddTransient<ISearchPersonAccounts, SearchPersonAccounts>();
            services.AddTransient<ISearchPerson, SearchPerson>();
            services.AddTransient<ISearchAccount, SearchAccount>();
            services.AddTransient<ISearchTransaction, SearchTransaction>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
