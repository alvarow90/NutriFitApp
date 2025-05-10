using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NutriFitApp.API.Data;
using NutriFitApp.API.Helpers;
using NutriFitApp.Shared.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔑 Conexión a base de datos
builder.Services.AddDbContext<NutriFitDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🧩 Identity + Roles
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<NutriFitDbContext>()
.AddDefaultTokenProviders();

// 🔐 JWT Authentication Services Configuration
// Esto configura los servicios necesarios para la autenticación JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Para producción, considera validar Issuer y Audience
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!) // Asegúrate que "jwtKey" exista en tu config
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

// Es importante registrar los servicios de autorización también.
// Identity lo hace en parte, pero AddAuthorization() es explícito.
builder.Services.AddAuthorization();

// 🛠 Helpers
builder.Services.AddScoped<IUserHelper, UserHelper>();

// 🔁 CORS (por si usas frontend separado)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 🧪 Swagger y Controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "NutriFitApp.API", Version = "v1" });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Introduce tu token JWT como: Bearer {token}",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = "Bearer",
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// 🚀 CREACIÓN AUTOMÁTICA DE ROLES (Seed)
async Task SeedRolesAsync(WebApplication webApp) // Renombrado el parámetro para claridad
{
    using var scope = webApp.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    string[] roles = new[] { "Administrador", "Nutriologo", "Entrenador", "Usuario" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}
await SeedRolesAsync(app); // Ejecuta el seeder de roles.

// 🌐 Configuración del Pipeline de Middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Útil para ver errores detallados en desarrollo
}
else
{
    // Considera añadir un manejador de excepciones global para producción aquí
    // app.UseExceptionHandler("/Error");
    app.UseHsts(); // Solo si siempre usas HTTPS en producción
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Aplicar la política CORS

// --- ORDEN IMPORTANTE DEL MIDDLEWARE DE AUTENTICACIÓN Y AUTORIZACIÓN ---
app.UseRouting(); // 1. Habilita el enrutamiento para que los endpoints puedan ser encontrados.

app.UseAuthentication(); // 2. AÑADIDO: Habilita el middleware de autenticación.
                         //    Este middleware identifica al usuario basado en la información de la solicitud (ej. token JWT).

app.UseAuthorization(); // 3. Habilita el middleware de autorización.
                        //    Este middleware verifica si el usuario autenticado tiene permiso para acceder al recurso solicitado.

// Mapea los endpoints definidos en tus controladores.
// Debe ir DESPUÉS de UseRouting, UseAuthentication y UseAuthorization.
app.MapControllers();

app.Run();
