using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;using Microsoft.EntityFrameworkCore;
using ApiBase.DAL.Modelos_BD_Universidad; // Ajusta al namespace real del scaffold



var builder = WebApplication.CreateBuilder(args);

// ---------------------
// Definir una política de CORS
// ---------------------
var corsPolicy = "_allowAll"; // puedes darle otro nombre

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // policy.WithOrigins("https://localhost:7243", "http://localhost:5271")
            //     .AllowAnyHeader()
            //     .AllowAnyMethod();
        });
        
});

// ---------------------
// Configuración del DbContext
// ---------------------
builder.Services.AddDbContext<BD_UniversidadContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BD_Universidad")));

// ---------------------
// Configuración de servicios
// ---------------------

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiBase", Version = "v1" });

    // ✅ Esquema HTTP Bearer (Swagger agrega "Bearer " solo)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Autenticación JWT. Pega solo el token (sin 'Bearer ')."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Configuración JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// Controllers (para que puedas usar atributos como [Authorize])
builder.Services.AddControllers();

var app = builder.Build();

// Activar CORS (antes de Authentication/Authorization)
app.UseCors(corsPolicy);

// ---------------------
// Middleware
// ---------------------
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // 👈 antes de UseAuthorization
app.UseAuthorization();

app.MapControllers(); // para tus controladores

// Ruta raíz "/" con un texto simple
app.MapGet("/", () => Results.Text("API Base corriendo correctamente."));

app.Run();


