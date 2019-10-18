using Common.Application.Services;
using Common.Data;
using Common.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Application.Extensions
{
    public static class IocExtensions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddTransient<IItemToDoInMemoryService, ItemToDoInMemoryService>();
            services.AddTransient<IItemToDoRepository, ItemToDoRepository>();
        }

    }
}
