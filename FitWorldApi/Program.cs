using DbConnectionTools;

using FitWorld.BLL.Interfaces;
using FitWorld.BLL.Services;
using FitWorld.Dal.Interfaces;
using FitWorld.Dal.Repositories;

using FitWorldApi.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

namespace FitWorldApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Services.AddOptions();
            _ = builder.Services.Configure<MyOptions>(options =>
            {
                options.ConnectionString = builder.Configuration.GetConnectionString("FitWorld");
            });

            _ = builder.Services.AddCors(
                options => options.AddPolicy(
                    "MyPolicy",
                    policyBuilder =>
            {
                _ = policyBuilder.WithOrigins("localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));

            _ = builder.Services.AddControllers();

            _ = builder.Services.AddSignalR();
            _ = builder.Services.AddSingleton<TokenManager>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    OpenApiSecurityScheme openApiSecurityScheme = new()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    };

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        // Set up a key-value pair in a dictionary
                        [openApiSecurityScheme] = new List<string>()
                    });
                }
                );

            _ = builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "IsConnected",
                    policy => policy.RequireAuthenticatedUser());
            });

            _ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenManager.secret)),
                    ValidateIssuer = true,
                    ValidIssuer = TokenManager.issuer,
                    ValidateAudience = true,
                    ValidAudience = TokenManager.audience,
                };
            });

            // Add Singletons
            _ = builder.Services.AddSingleton(serviceProvider => new Connection(builder.Configuration.GetConnectionString("FitWorld")));

            // Add Repositories
            _ = builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            // Add Services...
            _ = builder.Services.AddScoped<IStudentService, StudentService>();

            WebApplication app = builder.Build();

            _ = app.UseCors("MyPolicy");

            /*
            // add an origin mentioning its domain name
            app.UseCors(options =>
            {
                options.WithOrigins("localhost:4200").AllowAnyMethod();
                options.WithOrigins("my other site...").AllowAnyMethod();
            });
            */

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }
            else
            {
                _ = app.UseDefaultFiles();
                _ = app.UseStaticFiles();
            }

            _ = app.UseHttpsRedirection();

            _ = app.UseRouting();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllers();

            app.Run();
        }
    }
}