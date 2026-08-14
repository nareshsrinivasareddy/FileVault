
using FileVault.Api.Endpoints;
using FileVault.Application;
using FileVault.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "FileVault API",
    status = "running",
    openApi = "/openapi/v1.json",
    scalar = "/scalar",
    uploadEndpoint = "/api/documents/upload"
}));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDocumentsEndpoints();

app.Run();
