using BackgroundTask.API.Extensions;
using BackgroundTask.API.Jobs;
using BackgroundTask.Application;
using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.Validators;
using BackgroundTask.Infrastructure;
using BackgroundTask.Persistence;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddValidatorsFromAssemblyContaining<SendNotificationRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MessagingProviderOptions>(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<NotificationRetryJob>();


var app = builder.Build();
app.ConfigureExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


