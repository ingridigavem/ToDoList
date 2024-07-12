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
        );
        #endregion

        #region Create ToDo

        endpoints.Add(
            app.MapPost("api/v1/toDo", async ([FromBody] CreateToDoRequest request, IRequestHandler<CreateToDoRequest, Result<CreateToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        #region Delete ToDo
        endpoints.Add(
            app.MapDelete("api/v1/toDo/{Id}", async ([AsParameters] DeleteToDoRequest request, IRequestHandler<DeleteToDoRequest, Result<DeleteToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        #region Complete ToDo
        endpoints.Add(
            app.MapPatch("api/v1/toDo/complete/{Id}", async ([AsParameters] CompleteToDoRequest request, IRequestHandler<CompleteToDoRequest, Result<CompleteToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        #region Update ToDo Description
        endpoints.Add(
            app.MapPatch("api/v1/toDo/{id}", async ([FromRoute] Guid id, UpdateToDoDescriptionRequestDto requestDto, IRequestHandler<UpdateToDoDescriptionRequest, Result<UpdateToDoDescriptionResponse>> handler) => {
                var request = new UpdateToDoDescriptionRequest(id, requestDto.Description);

                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        return endpoints;
    }
}
