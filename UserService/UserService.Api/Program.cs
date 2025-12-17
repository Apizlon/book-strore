using UserService.Application.Services;
using UserService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using UserService.Api.Configuration;
using UserService.Api.Filters;
using UserService.Application.Interfaces;
using UserService.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();

// Add services to the container
builder.Services.AddControllers(options => options.Filters.Add<CustomExceptionFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService API", Version = "v1" });

    // JWT Security для Bearer токенов
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            new string[] { }
        }
    });
});

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add application services
builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

// Add CORS
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

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    dbContext.Database.Migrate();
}
app.UseHttpMetrics();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
app.MapMetrics("/metrics");
app.Run();
