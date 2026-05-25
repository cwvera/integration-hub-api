using Asp.Versioning;
using IntegrationHub.Infrastructure.Persistence;
using IntegrationHub.Infrastructure.Security;
using IntegrationHub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using IntegrationHub.Application.Operations.Handlers;
using IntegrationHub.Application.Operations.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de DB
builder.Services.AddDbContext<IntegrationHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<IntegrationHubDbContext>());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SyncOperationsHandler).Assembly));

builder.Services.AddMemoryCache();
builder.Services.AddTransient<OAuthDelegatingHandler>();

// Registro de múltiples sistemas de integración (REST y SOAP)
builder.Services.AddHttpClient("SystemARest", client =>
{
    client.BaseAddress = new Uri("https://api.systema.com/");
})
.AddHttpMessageHandler<OAuthDelegatingHandler>();

builder.Services.AddHttpClient("SystemCRest", client =>
{
    client.BaseAddress = new Uri("https://api.systemc.com/");
})
.AddHttpMessageHandler<OAuthDelegatingHandler>();

// Registro de Adaptadores
builder.Services.AddTransient<IIntegrationAdapter>(sp => 
    new RestIntegrationAdapter(sp.GetRequiredService<IHttpClientFactory>().CreateClient("SystemARest"), "SYSTEM_A"));

builder.Services.AddTransient<IIntegrationAdapter>(sp => 
    new RestIntegrationAdapter(sp.GetRequiredService<IHttpClientFactory>().CreateClient("SystemCRest"), "SYSTEM_C"));

builder.Services.AddTransient<IIntegrationAdapter>(sp => 
    new SoapIntegrationAdapter("SYSTEM_B_SOAP"));

// Registro de la Factoría
builder.Services.AddTransient<IntegrationAdapterFactory>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
