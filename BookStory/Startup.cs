using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using BookStory.Data.Contexts;
using BookStory.Data.Seeders;
using BookStory.Extensions;
using BookStory.Helpers;
using BookStory.MapperProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace BookStory
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "BookStory", Version = "v1"}); });

            services.AddAutoMapper(typeof(MainProfile));

            services.AddSwaggerGen(options => { options.EnableAnnotations(); });

            services.AddMainContext(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation("Экземпляр приложения подготовлен");

            var migrateAuto = Configuration.GetSection("MigrateAuto").Get<bool>();

            if (migrateAuto)
            {
                MigrateDatabase(app, logger);
            }

            SeedData(app, logger);

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStory v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void SeedData(IApplicationBuilder app, ILogger logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MainContext>();
                if (context == null)
                {
                    throw ErrorHelper.ContextIsNotRegistered<MainContext>();
                }

                var seed = new MainContextSeeder();

                seed.Seed(context, logger);
            }
        }

        private static void MigrateDatabase(IApplicationBuilder app, ILogger logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MainContext>();
                if (context == null)
                {
                    throw ErrorHelper.ContextIsNotRegistered<MainContext>();
                }

                try
                {
                    if (context.Database.IsRelational() && context.Database.GetPendingMigrations().Any())
                    {
                        logger.LogInformation("Имеются не примененные миграции для реляционной БД. Применяем ...");
                        context.Database.Migrate();
                        logger.LogInformation("Миграции успешно применены");
                    }
                }
                catch (NpgsqlException postgreException)
                {
                    logger.LogError(postgreException, "Произошла ошибка при соединении к БД.");
                    throw;
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, "Возникла ошибка при попытке применения миграций.");
                    throw;
                }
            }
        }
    }
}