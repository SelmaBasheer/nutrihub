using NotificationService.Application.Contracts;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register notification handler
// When someone asks for INotificationHandler ? give ConsoleNotificationHandler
builder.Services.AddScoped<INotificationHandler, ConsoleNotificationHandler>();

// Register background service
// Starts automatically when app starts ? watches notification.orders queue
builder.Services.AddHostedService<NotificationConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
