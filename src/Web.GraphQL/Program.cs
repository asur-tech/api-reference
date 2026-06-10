using Application;
using HealthChecks.UI.Client;
using HotChocolate;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Web.GraphQL;
using Web.GraphQL.Extensions;
using Web.GraphQL.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

// Safety net for unhandled exceptions outside the GraphQL pipeline.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    // Failed Results already carry a domain error code (see ResultGraphQL); anything else is an
    // unexpected exception, which we surface as a stable, non-leaking error.
    .AddErrorFilter(error => string.IsNullOrEmpty(error.Code)
        ? error.WithMessage("An unexpected error occurred.").WithCode("INTERNAL_SERVER_ERROR")
        : error);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

// GraphQL replaces the REST endpoints. The Nitro IDE is served at this path in development.
app.MapGraphQL("/graphql");

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
public partial class Program;
