using DigitalPhysio.Business.Interfaces;
using DigitalPhysio.Business.Services;
using DigitalPhysio.Domain.Interfaces;
using DigitalPhysio.Infrastructure.Data;
using DigitalPhysio.Infrastructure.Data.DigitalPhysio.Infrastructure.Data;
using DigitalPhysio.Infrastructure.DigitalPhysio.Infrastructure.Data;
using DigitalPhysio.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Digital Physiotherapy API",
        Version = "v1",
        Description = "API for the Digital Physiotherapy prototype application"
    });
});

// Configure DbContext
builder.Services.AddDbContext<PhysioDbContext>(options =>
    options.UseInMemoryDatabase("PhysioDB"));

// Register repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<ISessionResultRepository, SessionResultRepository>();

// Register services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<ISessionResultService, SessionResultService>();

// Register data seeder
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:50207")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
});

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddNewtonsoftJson(
        options =>
        {
            options.SerializerSettings.Culture = new System.Globalization.CultureInfo("en-GB");
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
        });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DataSeeder>();
    await seeder.SeedDataAsync();
}

app.Run();