using Microsoft.EntityFrameworkCore;
using SistemaGym.Api.Filters;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using SistemaGym.Infrastructure.Mappings;
using SistemaGym.Infrastructure.Repositories;
using SistemaGym.Services.Interfaces;
using SistemaGym.Services.Services;
using SistemaGym.Services.Validators;

namespace SistemaGym.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configurar la BD SqlServer
            //var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            //builder.Services.AddDbContext<SistemaGymContext>(options =>
            //    options.UseSqlServer(connectionString));
            #endregion

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");

            builder.Services.AddDbContext<SistemaGymContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            // Registrar repositorios
            builder.Services.AddScoped(
                typeof(IBaseRepository<>),
                typeof(BaseRepository<>));

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Registrar Dapper
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            // Registrar servicios
            builder.Services.AddTransient<IClienteService, ClienteService>();
            builder.Services.AddTransient<IUsuarioService, UsuarioService>();
            builder.Services.AddTransient<IPlanMembresiaService, PlanMembresiaService>();
            builder.Services.AddTransient<IMembresiaService, MembresiaService>();
            builder.Services.AddTransient<IPagoService, PagoService>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            // Registrar AutoMapper
            builder.Services.AddAutoMapper(typeof(ClienteProfile).Assembly);

            // Registrar validadores de FluentValidation

            builder.Services.AddScoped<ClienteDtoValidator>();
            builder.Services.AddScoped<CrearClienteDtoValidator>();
            builder.Services.AddScoped<ActualizarClienteDtoValidator>();

            builder.Services.AddScoped<UsuarioDtoValidator>();
            builder.Services.AddScoped<CrearUsuarioDtoValidator>();
            builder.Services.AddScoped<ActualizarUsuarioDtoValidator>();

            builder.Services.AddScoped<PlanMembresiaDtoValidator>();
            builder.Services.AddScoped<CrearPlanMembresiaDtoValidator>();
            builder.Services.AddScoped<ActualizarPlanMembresiaDtoValidator>();

            builder.Services.AddScoped<MembresiaDtoValidator>();
            builder.Services.AddScoped<CrearMembresiaDtoValidator>();
            builder.Services.AddScoped<ActualizarMembresiaDtoValidator>();

            builder.Services.AddScoped<PagoDtoValidator>();
            builder.Services.AddScoped<CrearPagoDtoValidator>();
            builder.Services.AddScoped<ActualizarPagoDtoValidator>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}