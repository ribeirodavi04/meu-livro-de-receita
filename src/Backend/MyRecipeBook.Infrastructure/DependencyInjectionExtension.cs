using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.Context;
using MyRecipeBook.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnnectionString();
            services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions => dbContextOptions.UseSqlServer(connectionString));
        }

        private static void AddRepositories(IServiceCollection services)
        {

        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.ConnnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddSqlServer().WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
            });
        }

    }
}
