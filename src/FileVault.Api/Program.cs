
using FileVault.Api.Endpoints;
using FileVault.Application;
using FileVault.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler(exceptionApp => exceptionApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    if (exception is ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        await Results.ValidationProblem(errors, statusCode: StatusCodes.Status400BadRequest)
            .ExecuteAsync(context);
        return;
    }

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
}));

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
