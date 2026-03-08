using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using ParcelTracking.API.Hubs;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Application.Services;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Configuration;
using ParcelTracking.Infrastructure.Messaging;
using ParcelTracking.Infrastructure.Persistence;
using ParcelTracking.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// --------------------------------------------------
// BASIC SERVICES
// --------------------------------------------------

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Parcel Tracking API",
        Version = "v1",
        Description = "Event Driven Parcel Tracking System"
    });
});

// --------------------------------------------------
// CORS (React UI)
// --------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// --------------------------------------------------
// SIGNALR (Realtime updates)
// --------------------------------------------------

builder.Services.AddSignalR();


// --------------------------------------------------
// COSMOS DB
// --------------------------------------------------

builder.Services.Configure<CosmosDbOptions>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("COSMOS_CONNECTION");//"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==0;";
                                                                                       //Environment.GetEnvironmentVariable("COSMOS_CONNECTION");

    options.DatabaseName = "ParcelTrackingDB";
});

builder.Services.AddSingleton<CosmosDbContext>();

builder.Services.AddSingleton<CosmosDbInitializer>();


// --------------------------------------------------
// APPLICATION SERVICES
// --------------------------------------------------

builder.Services.AddScoped<IParcelService, ParcelService>();


// --------------------------------------------------
// REPOSITORIES
// --------------------------------------------------

builder.Services.AddScoped<IParcelRepository, CosmosParcelRepository>();

builder.Services.AddScoped<IParcelEventRepository, CosmosParcelEventRepository>();

builder.Services.AddScoped<ITrackingQueryRepository, CosmosTrackingQueryRepository>();

builder.Services.AddScoped<IProcessedEventRepository, CosmosProcessedEventRepository>();


// --------------------------------------------------
// KAFKA EVENT PUBLISHER
// --------------------------------------------------

builder.Services.AddScoped<IEventPublisher, KafkaEventPublisher>();


// --------------------------------------------------
// BUILD APP
// --------------------------------------------------

var app = builder.Build();


// --------------------------------------------------
// COSMOS INITIALIZATION
// --------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var initializer =
        scope.ServiceProvider.GetRequiredService<CosmosDbInitializer>();

    await initializer.InitializeAsync();
}


// --------------------------------------------------
// MIDDLEWARE
// --------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Parcel API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowReact");

app.UseAuthorization();


// --------------------------------------------------
// ENDPOINTS
// --------------------------------------------------

app.MapControllers();

app.MapHub<TrackingHub>("/trackingHub");


// --------------------------------------------------

app.Run();