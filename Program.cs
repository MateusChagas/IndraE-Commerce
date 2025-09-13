using EstoqueService.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Configurar Serilog
Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("logs/inventory-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Host.UseSerilog();

// Configurar EF Core
builder.Services.AddDbContext<EstoqueDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configurar MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});


// Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
  
    var jwtKey = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("A chave JWT não está configurada. Verifique a configuração 'Jwt:Key'.");
    }
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});


builder.Services.AddAuthorization();


builder.Services.AddControllers();


var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();