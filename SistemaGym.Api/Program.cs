using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;
using SistemaGym.Infrastructure.Mappings;
using SistemaGym.Infrastructure.Repositories;

namespace SistemaGym.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            // Registrar los servicios
            builder.Services.AddTransient<IClienteRepository, ClienteRepository>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling
                     = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
             );

            // Registra el profile del automapper para Cliente
            builder.Services.AddAutoMapper(typeof(ClienteProfile).Assembly);

            builder.Services.AddOpenApi();

            var app = builder.Build();

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