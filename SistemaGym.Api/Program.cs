using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SistemaGym.Api.Filters;
using SistemaGym.Core.CustomEntities;
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
            // Configuración base
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // ¡ESTO ES CLAVE PARA AZURE



            // Configuracion base
            builder.Configuration.Sources.Clear();
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

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
            builder.Services.AddTransient<ISecurityService, SecurityService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();

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

            builder.Services.Configure<PasswordOptions>(
                builder.Configuration.GetSection("PasswordOptions"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            builder.Configuration["Authentication:SecretKey"] ?? string.Empty))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = new ErrorResponse
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Message = "No tiene autorización para acceder a este recurso. Inicie sesión y envíe un token JWT válido con el formato: Bearer {token}.",
                            TraceId = context.HttpContext.TraceIdentifier
                        };

                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(
                            response,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                            }));
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var response = new ErrorResponse
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Message = "No tiene permisos suficientes para ejecutar esta acción.",
                            TraceId = context.HttpContext.TraceIdentifier
                        };

                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(
                            response,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                            }));
                    }
                };
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

            // Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Backend Sistema Gym API",
                    Version = "v1",
                    Description = "Documentacion de la API de Sistema Gym",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipo de desarrollo",
                        Email = "desarrollo@sistemagym.com"
                    }
                });

                options.EnableAnnotations();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT. Ejemplo: Bearer {token}"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document, null),
                        new List<string>()
                    }
                });
            });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Usar Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Sistema Gym API v1");
                options.RoutePrefix = "swagger";
            });

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
