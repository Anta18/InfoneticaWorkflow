using Application.Services;
using Application.Requests;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApi(this WebApplication app)
        {
            app.UseExceptionHandler("/error");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            return app;
        }

        public static void MapWorkflowEndpoints(this WebApplication app)
        {
            app.MapPost("/definitions", async (CreateDefinitionRequest req, IWorkflowService svc) =>
            {
                var stateTuples = req.States
                    .Select(s => (s.Id, s.Name, s.IsStart, s.IsEnd));

                var actionTuples = req.Actions
                    .Select(a => (a.Name, a.FromStates, a.ToState));

                var defId = await svc.CreateDefinitionAsync(req.Name, stateTuples, actionTuples);
                return Results.Created($"/definitions/{defId}", new { defId });
            });

            app.MapGet("/definitions", async (IWorkflowService svc) =>
            {
                var list = await svc.ListDefinitionsAsync();
                return Results.Ok(list);
            });

            app.MapGet("/definitions/{defId:guid}", async (Guid defId, IWorkflowService svc) =>
            {
                var dto = await svc.GetDefinitionAsync(defId);
                return Results.Ok(dto);
            });

            app.MapPost("/definitions/{defId:guid}/instances", async (Guid defId, IWorkflowService svc) =>
            {
                var instId = await svc.StartInstanceAsync(defId);
                return Results.Created($"/instances/{instId}", new { instId });
            });

            app.MapGet("/instances", async (IWorkflowService svc) =>
            {
                var list = await svc.ListInstancesAsync();
                return Results.Ok(list);
            });

            app.MapGet("/instances/{instId:guid}", async (Guid instId, IWorkflowService svc) =>
            {
                var dto = await svc.GetInstanceAsync(instId);
                return Results.Ok(dto);
            });

            app.MapPost("/instances/{instId:guid}/actions/{actionId:guid}",
                async (Guid instId, Guid actionId, IWorkflowService svc) =>
                {
                    await svc.PerformActionAsync(instId, actionId);
                    return Results.NoContent();
                });

            app.Map("/error", (HttpContext ctx) =>
            {
                var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                return Results.Problem(detail: ex?.Message, statusCode: 500);
            });

            app.MapPatch("/definitions/{defId:guid}/states/{stateId:guid}/disable",
                async (Guid defId, Guid stateId, IWorkflowService svc) =>
                {
                    await svc.DisableStateAsync(defId, stateId);
                    return Results.NoContent();
                });

            app.MapPatch("/definitions/{defId:guid}/states/{stateId:guid}/enable",
                async (Guid defId, Guid stateId, IWorkflowService svc) =>
                {
                    await svc.EnableStateAsync(defId, stateId);
                    return Results.NoContent();
                });

            app.MapPatch("/definitions/{defId:guid}/actions/{actionId:guid}/disable",
                async (Guid defId, Guid actionId, IWorkflowService svc) =>
                {
                    await svc.DisableActionAsync(defId, actionId);
                    return Results.NoContent();
                });

            app.MapPatch("/definitions/{defId:guid}/actions/{actionId:guid}/enable",
                async (Guid defId, Guid actionId, IWorkflowService svc) =>
                {
                    await svc.EnableActionAsync(defId, actionId);
                    return Results.NoContent();
                });
        }
    }
}
