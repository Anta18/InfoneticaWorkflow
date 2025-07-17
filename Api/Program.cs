using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddApi();

var app = builder.Build();

app.UseApi();
app.MapWorkflowEndpoints();

app.Run();
