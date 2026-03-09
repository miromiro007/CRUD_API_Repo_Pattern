using CRUD_API.data;
using CRUD_API.Extentions;
using CRUD_API.Logging;
using CRUD_API.Repository;
using CRUD_API.Repository.Base;
using CRUD_API.Seed;
using CRUD_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUD API", Version = "v1" });
});

builder.Services.AddControllers();
builder.Services.AddIdentityCore<CRUD_API.Models.AppUser>(options => { })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
/*.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});*/

builder.Services.AddDbContext<CRUD_API.data.AppDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Repository and UnitOfWork
builder.Services.AddScoped(typeof(IRepository<>), typeof(MainRepository<>));
builder.Services.AddScoped<IUnitWork, UnitOfWork>();
// Enregistre le service JWT ici
builder.Services.AddScoped<JwtService>();



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
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),

        ClockSkew = TimeSpan.Zero
    };
});





builder.Services.AddAuthorization();

//Appel de la méthode qu'on a créée pour configurer Serilog.
SerilogConfiguration.ConfigureSerilog(builder);





var app = builder.Build();
app.UseSerilogRequestLogging();

// --- AJOUT DU SEED ICI ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("🚀 Tentative de seeding de la base de données...");
        await IdentitySeeder.SeedAdminUser(services);
        Log.Information("✅ Seeding terminé avec succès.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "❌ Une erreur est survenue lors du seeding de la base de données.");
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API v1");
    });
}
    




//app.UseRequestLogging1(); // Utilise le middleware de logging des requêtes 1
//app.UseApiKeyMiddleware();// Utilise le middleware de validation de l'API Key
//app.UseExceptionHandlingMiddleware(); // Utilise le middleware de gestion des exceptions 1
//app.UseRequestLogging(); // Utilise le middleware de logging des requêtes 2







app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();