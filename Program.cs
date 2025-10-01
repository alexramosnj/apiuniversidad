using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ApiBase.DAL.Modelos_BD_Universidad;

var builder = WebApplication.CreateBuilder(args);

// ---------------------
// Variables de entorno / config
// ---------------------
// FRONTEND_ORIGINS: lista separada por coma con los dominios permitidos (prod y dev)
var frontendOrigins = (builder.Configuration["FRONTEND_ORIGINS"] ?? "")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var jwtKey = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado");
var jwtIssuer = jwt["Issuer"];
var jwtAudience = jwt["Audience"];

// ---------------------
// DbContext (PostgreSQL)
// ---------------------
// Render (o cualquier PaaS) suele requerir SSL. Ajusta si tu DB lo exige.
var conn = builder.Configuration.GetConnectionString("BD_Universidad")
           ?? builder.Configuration["DATABASE_URL"]; // fallback por si la defines así
builder.Services.AddDbContext<BD_UniversidadContext>(options =>
    options.UseNpgsql(conn, npgsql =>
    {
        // si tu proveedor lo requiere:
        // npgsql.SetPostgresVersion(16, 0);
    })
);

// ---------------------
// CORS
// ---------------------
const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        // 🔐 En producción: permitir solo el dominio del front
        policy.WithOrigins(
            "https://vue-universidad.onrender.com", // front en Render
            "http://localhost:5173",                // dev local
            "https://localhost:5173"                // dev local https
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ---------------------
// Auth (JWT)
// ---------------------
builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiBase", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pega solo el token (sin 'Bearer ')."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference
            { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] {} }
    });
});

var app = builder.Build();

// // ---------------------
// // Bind al puerto de Render
// // ---------------------
// var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// app.Urls.Add($"http://0.0.0.0:{port}");  // evita 502 por puerto incorrecto
// ---------------------
// Bind al puerto de Render (solo si PORT existe)
// ---------------------
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

// Si estás detrás de proxy (Render), respeta cabeceras X-Forwarded-*
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

// ---------------------
// Middleware
// ---------------------
// En Render puedes mantener Swagger habilitado
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(CorsPolicy);

// En Render, si HSTS/HTTPS te estorba, puedes comentarlo.
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ping simple
app.MapGet("/", () => Results.Text("API Base corriendo correctamente."));

app.Run();
