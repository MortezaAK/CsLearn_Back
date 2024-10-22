using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Services;
using ApplicationCore.Utilitis;
using AutoMapper;
using Infrastructure;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

namespace WebApI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            // AutoMapper configuration
            builder.Services.AddScoped<IMapper>(provider =>
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                return mappingConfig.CreateMapper();
            });

            // DbContext
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });

            // Services and UnitOfWork
            builder.Services.AddScoped<IServiceContainer, ServiceContainer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            }); ;
            builder.Services.AddEndpointsApiExplorer();


            //#region Mapper
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //builder.Services.AddSingleton(mapper);
            //#endregion

            //#region DataBase
            //builder.Services.AddDbContext<ApplicationContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"),
            //        sqlServerOptionsAction: sqlOptions =>
            //        {
            //            sqlOptions.EnableRetryOnFailure(
            //                maxRetryCount: 10,
            //                maxRetryDelay: TimeSpan.FromSeconds(30),
            //                errorNumbersToAdd: null);
            //        });
            //});
            //#endregion
            //#region Services
            //builder.Services.AddScoped<IServiceContainer, ServiceContainer>();
            //#endregion

            //#region UnitOfWork 
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddHttpContextAccessor();
            //builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //#endregion
            //builder.Services.AddControllers();
            //builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure CORS before routing
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.MapControllers();
            app.Run();
        }
    }
}
