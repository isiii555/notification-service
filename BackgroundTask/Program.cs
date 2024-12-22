using BackgroundTask.API.Extensions;
using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.Jobs;
using BackgroundTask.Infrastructure;
using BackgroundTask.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MessagingProviderOptions>(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
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


