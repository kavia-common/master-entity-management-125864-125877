using System;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using dotnet.Dtos;
using dotnet.Services;

namespace dotnet.Endpoints
{
    /// <summary>
    /// Endpoint route mappings for MasterRace CRUD operations.
    /// </summary>
    public static class MasterRaceEndpoints
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Maps routes under /api/master-races for CRUD operations:
        /// - POST /api/master-races
        /// - GET /api/master-races
        /// - GET /api/master-races/{id}
        /// - PUT /api/master-races/{id}
        /// - DELETE /api/master-races/{id}
        /// </summary>
        public static IEndpointRouteBuilder MapMasterRaceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/master-races")
                .WithTags("MasterRaces");

            group.MapPost("/", async ([FromBody] MasterRaceCreateRequest body, IMasterRaceService svc) =>
            {
                var (created, problem) = await svc.CreateAsync(body);
                if (problem is not null)
                {
                    return Results.ValidationProblem(problem.Errors);
                }
                var location = $"/api/master-races/{created!.Id}";
                return Results.Created(location, created);
            })
            .WithName("CreateMasterRace")
            .Produces<MasterRaceResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/", async (IMasterRaceService svc) =>
            {
                var items = await svc.GetAllAsync();
                return Results.Ok(items);
            })
            .WithName("ListMasterRaces")
            .Produces<MasterRaceResponse[]>(StatusCodes.Status200OK);

            group.MapGet("/{id:guid}", async (Guid id, IMasterRaceService svc) =>
            {
                var item = await svc.GetByIdAsync(id);
                return item is null ? Results.NotFound() : Results.Ok(item);
            })
            .WithName("GetMasterRace")
            .Produces<MasterRaceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id:guid}", async (Guid id, [FromBody] MasterRaceUpdateRequest body, IMasterRaceService svc) =>
            {
                var (updated, notFound, problem) = await svc.UpdateAsync(id, body);
                if (notFound)
                {
                    return Results.NotFound();
                }
                if (problem is not null)
                {
                    return Results.ValidationProblem(problem.Errors);
                }
                return Results.Ok(updated);
            })
            .WithName("UpdateMasterRace")
            .Produces<MasterRaceResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", async (Guid id, IMasterRaceService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteMasterRace")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
