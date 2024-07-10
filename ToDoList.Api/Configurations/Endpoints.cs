using MediatR;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.UseCases.CreateToDo;
using ToDoList.Domain.ToDoList.UseCases.GetAllToDos;

namespace ToDoList.Api.Configurations;

public static class Endpoints {
    public static List<RouteHandlerBuilder> GetEndpointsV1(WebApplication app) {
        var endpoints = new List<RouteHandlerBuilder>();

        #region Get All ToDos

        endpoints.Add(
            app.MapGet("api/v1/getalltodos", async (int? page_number, int? page_size, IRequestHandler<GetAllToDosRequest, Result<GetAllToDosResponse>> handler) => {
                page_number ??= 1;
                page_size ??= 10;

                var request = new GetAllToDosRequest(page_number.Value, page_size.Value);
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        #region Create ToDo

        endpoints.Add(
            app.MapPost("api/v1/toDos", async (CreateToDoRequest request, IRequestHandler<CreateToDoRequest, Result<CreateToDoResponse>> handler) => {
                var result = await handler.Handle(request, new CancellationToken());
                return Results.Json(result, statusCode: (int)result.Status);
            })
        );
        #endregion

        return endpoints;
    }
}
