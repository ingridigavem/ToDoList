using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.UseCases.CreateToDo;
using ToDoList.Domain.ToDoList.UseCases.DeleteToDo;
using ToDoList.Domain.ToDoList.UseCases.GetAllToDos;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDoDescription;

namespace ToDoList.Api.Configurations;

public static class Endpoints {
    public static List<RouteHandlerBuilder> GetEndpointsV1(WebApplication app) {
        var endpoints = new List<RouteHandlerBuilder>();

        #region Get All ToDos

        endpoints.Add(
            app.MapGet("api/v1/toDos", async ([AsParameters] GetAllToDosRequest request, IRequestHandler<GetAllToDosRequest, Result<GetAllToDosResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
            .WithOpenApi(operation => new(operation) {
                Summary = "Get a list of ToDos",
                Description = "Return a list of ToDos. Can receive or not parameters for pagination",
            })
            .Produces<Result<GetAllToDosResponse>>(StatusCodes.Status200OK)
            .Produces<Result<GetAllToDosResponse>>(StatusCodes.Status400BadRequest)
            .Produces<Result<GetAllToDosResponse>>(StatusCodes.Status500InternalServerError)
        );
        #endregion

        #region Create ToDo

        endpoints.Add(
            app.MapPost("api/v1/toDo", async ([FromBody] CreateToDoRequest request, IRequestHandler<CreateToDoRequest, Result<CreateToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
            .WithOpenApi(operation => new(operation) {
                Summary = "Create a ToDo",
                Description = "Receives a body with a description to create a new ToDo",
            })
            .Produces<Result<CreateToDoResponse>>(StatusCodes.Status201Created)
            .Produces<Result<CreateToDoResponse>>(StatusCodes.Status400BadRequest)
            .Produces<Result<CreateToDoResponse>>(StatusCodes.Status500InternalServerError)
        );
        #endregion

        #region Delete ToDo
        endpoints.Add(
            app.MapDelete("api/v1/toDo/{Id}", async ([AsParameters] DeleteToDoRequest request, IRequestHandler<DeleteToDoRequest, Result<DeleteToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
            .WithOpenApi(operation => new(operation) {
                Summary = "Delete ToDo",
                Description = "Receives a ToDo ID and delete the ToDo informed",
            })
            .Produces<Result<DeleteToDoResponse>>(StatusCodes.Status204NoContent)
            .Produces<Result<DeleteToDoResponse>>(StatusCodes.Status400BadRequest)
            .Produces<Result<DeleteToDoResponse>>(StatusCodes.Status500InternalServerError)
        );
        #endregion

        #region Complete ToDo
        endpoints.Add(
            app.MapPatch("api/v1/toDo/complete/{Id}", async ([AsParameters] CompleteToDoRequest request, IRequestHandler<CompleteToDoRequest, Result<CompleteToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
            .WithOpenApi(operation => new(operation) {
                Summary = "Complete or Uncomplete ToDo",
                Description = "Receives a ToDo ID and Complete or Uncomplete the ToDo informed",
            })
            .Produces<Result<CompleteToDoResponse>>(StatusCodes.Status200OK)
            .Produces<Result<CompleteToDoResponse>>(StatusCodes.Status400BadRequest)
            .Produces<Result<CompleteToDoResponse>>(StatusCodes.Status500InternalServerError)
        );
        #endregion

        #region Update ToDo Description
        endpoints.Add(
            app.MapPatch("api/v1/toDo/{id}", async ([FromRoute] Guid id, UpdateToDoDescriptionRequestDto requestDto, IRequestHandler<UpdateToDoDescriptionRequest, Result<UpdateToDoDescriptionResponse>> handler) => {
                var request = new UpdateToDoDescriptionRequest(id, requestDto.Description);

                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
            .WithOpenApi(operation => new(operation) {
                Summary = "Update ToDo description",
                Description = "Receives a ToDo ID and a body with a new description",
            })
            .Produces<Result<UpdateToDoDescriptionResponse>>(StatusCodes.Status200OK)
            .Produces<Result<UpdateToDoDescriptionResponse>>(StatusCodes.Status400BadRequest)
            .Produces<Result<UpdateToDoDescriptionResponse>>(StatusCodes.Status404NotFound)
            .Produces<Result<UpdateToDoDescriptionResponse>>(StatusCodes.Status500InternalServerError)
        );
        #endregion

        return endpoints;
    }
}
