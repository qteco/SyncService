using Microsoft.EntityFrameworkCore;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Services;
using SyncService.Data;
using SyncService.Data.DataContext;
using SyncService.Data.Repositories;
using SyncService.ExternalServices.ApiClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientSiteRepository, ClientSiteRepository>();
builder.Services.AddScoped<IClientSiteRepository, ClientSiteRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientSiteService, ClientSiteService>();
builder.Services.AddScoped<IExactService, ExactService>();

builder.Services.AddScoped<ISuperopsApiClient, SuperopsApiClient>();
builder.Services.AddScoped<IExactRepository, ExactRepository>();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();