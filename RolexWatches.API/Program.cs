using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RolexWatches.API.Middleware;
using RolexWatches.Application.Mapper;
using RolexWatches.Application.Mapping;
using RolexWatches.Application.Service;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Application.Validators;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using RolexWatches.Infrastructure.Repository;
using RolexWatches.Infrastructure.Services;
using System.Text;

namespace RolexWatches.API;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // =====================================================
        // CONTROLLERS
        // =====================================================

        builder.Services.AddControllers();

        // =====================================================
        // SWAGGER
        // =====================================================

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RolexWatches API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token like: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // =====================================================
        // DATABASE
        // =====================================================

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // =====================================================
        // AUTOMAPPER
        // =====================================================

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<CatalogMappingProfile>();
            cfg.AddProfile<AuthMappingProfile>();
        });

        // =====================================================
        // FLUENT VALIDATION
        // =====================================================

        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

        // =====================================================
        // REPOSITORIES
        // =====================================================

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IBrandRepository, BrandRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // =====================================================
        // SERVICES
        // =====================================================

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IDashboardService, DashboardService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // =====================================================
        // JWT AUTHENTICATION
        // =====================================================

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var jwtKey = jwtSection["Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT Key is missing in appsettings.json");
        }

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
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
                    Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        // =====================================================
        // AUTHORIZATION
        // =====================================================

        builder.Services.AddAuthorization();

        // =====================================================
        // CORS
        // =====================================================

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // =====================================================
        // BUILD APPLICATION
        // =====================================================

        var app = builder.Build();

        // =====================================================
        // EXCEPTION MIDDLEWARE
        // =====================================================

        app.UseMiddleware<ExceptionMiddleware>();

        // =====================================================
        // SWAGGER
        // =====================================================

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // =====================================================
        // HTTP PIPELINE
        // =====================================================

        app.UseHttpsRedirection();

        app.UseCors("AllowAngular");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        // =====================================================
        // RUN
        // =====================================================

        app.Run();
    }
}