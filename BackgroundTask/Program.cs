using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.IServices;
using BackgroundTask.Application.Services;
using BackgroundTask.Domain.Interfaces;
using BackgroundTask.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageProvider, TwilioMessageProvider>();
builder.Services.AddTransient<IMessageProvider, AmazonSNSMessageProvider>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.Configure<MessagingProviderOptions>(builder.Configuration.GetSection("MessagingProviders"));


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
