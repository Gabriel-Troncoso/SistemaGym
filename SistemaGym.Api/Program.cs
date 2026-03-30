using Microsoft.EntityFrameworkCore;
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

  
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");

            builder.Services.AddDbContext<SistemaGymContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

      
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

       
            builder.Services.AddTransient<IClienteService, ClienteService>();
            builder.Services.AddTransient<IUsuarioService, UsuarioService>();
            builder.Services.AddTransient<IPlanMembresiaService, PlanMembresiaService>();
            builder.Services.AddTransient<IMembresiaService, MembresiaService>();
            builder.Services.AddTransient<IPagoService, PagoService>();

          
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

           
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

        
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