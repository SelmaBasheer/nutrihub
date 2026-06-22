using IdentityService.API.Extensions;
using IdentityService.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.AddSerilog();

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Seed Admin Data
using (var scope = app.Services.CreateScope())
{
    var seeder = new DataSeeder(
        scope.ServiceProvider.GetRequiredService<IdentityDbContext>());
    await seeder.SeedAsync();
}

//  HTTP request middleware pipeline.
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
