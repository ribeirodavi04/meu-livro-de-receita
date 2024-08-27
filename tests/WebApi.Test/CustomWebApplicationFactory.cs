﻿using Common.TestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private MyRecipeBook.Domain.Entities.User _user = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));
                    if(descriptor is not null) 
                        services.Remove(descriptor);

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                    services.AddDbContext<MyRecipeBookDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

                    dbContext.Database.EnsureDeleted();

                    StartDatabase(dbContext);
                   
                });
        }

        private void StartDatabase(MyRecipeBookDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();

            dbContext.Users.Add(_user);
            
            dbContext.SaveChanges();
        }

        public string GetEmail() => _user.Email;
        public string GetName() => _user.Name;
        public string GetPassword() => _password;
    }
}
