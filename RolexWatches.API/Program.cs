
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RolexWatches.API.Middleware;
using RolexWatches.Application.Mapper;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Infrastructure.Data;
using RolexWatches.Infrastructure.Services;
using System.Text;

namespace RolexWatches.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //automapper
            builder.Services.AddAutoMapper(p=>p.AddProfile<AuthMappingProfile>());

            //registering service
            builder.Services.AddScoped<IAuthService, AuthService>();

            //token
            var jwtSection = builder.Configuration.GetSection("Jwt");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSection["Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();//for exception handling

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();   // <-- register it here, early

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.MapControllers();

            app.Run();
        }
    }
}
