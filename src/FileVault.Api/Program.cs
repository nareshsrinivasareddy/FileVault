
using FileVault.Api.Endpoints;
using FileVault.Application;
using FileVault.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "FileVault API",
    status = "running",
    swagger = "/swagger",
    uploadEndpoint = "/api/documents/upload"
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDocumentsEndpoints();

app.Run();
