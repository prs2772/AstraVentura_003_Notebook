using System.Text;
using AstraVenturaNotebook.API.Services;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using AstraVenturaNotebook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar MongoDB
var mongoConnectionString = builder.Configuration.GetValue<string>(
    "MongoDbSettings:ConnectionString"
);
var mongoDatabaseName = builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName");
builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString, mongoDatabaseName));

// 2. Inyección de Dependencias
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// 3. Registrar MediatR (Busca automáticamente los Handlers en la capa Application)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(AstraVenturaNotebook.Application.Dtos.SearchResultDto).Assembly
    )
);

// 4. Configurar Autenticación JWT (AstraVenturaAuth)
var jwtSecret = builder.Configuration.GetValue<string>("JwtSettings:SecretKey");
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
