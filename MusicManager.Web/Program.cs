using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Services;
using System;

namespace MusicManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IDataReadService, DataReadService>();
            builder.Services.AddScoped<IDataWriteService, DataWriteService>();
            builder.Services.AddDbContext<MusicManagerContext>(options => {
                if (builder.Environment.IsDevelopment())
                {
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                            .EnableSensitiveDataLogging();
                }
                options.UseSqlServer(builder.Configuration.GetConnectionString("MusicManagerContext"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            CreateDbIfNotExists(app.Services);

            app.Run();
        }

        private static void CreateDbIfNotExists(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var hostingEnvironment = services.GetService<IWebHostEnvironment>();

                if (hostingEnvironment.IsDevelopment())
                {
                    var dbContext = services.GetRequiredService<MusicManagerContext>();
                    DevDbInitialiser.Initialise(dbContext);
                }
            }
        }
    }
}