using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.GetAllToDos;
public class GetAllToDosHandler(IToDoRepository repository, MemoryCache cache) : IRequestHandler<GetAllToDosRequest, Result<GetAllToDosResponse>> {
    public async Task<Result<GetAllToDosResponse>> Handle(GetAllToDosRequest request, CancellationToken cancellationToken) {

        if (request is null) return new Result<GetAllToDosResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        #region Get ToDos and Set Cache
        IList<ToDo>? toDos;
        int totalCount;

        string cacheToDosKey = $"toDosList_page_{request.PageNumber}_size_{request.PageSize}";
        string cacheTotalCountKey = "totalCount";

        try {
            if (!cache.TryGetValue(cacheTotalCountKey, out totalCount)) {
                totalCount = await repository.CountAsync();
                cache.Set(cacheTotalCountKey, totalCount);
            }

            if (!cache.TryGetValue(cacheToDosKey, out toDos)) {
                toDos = await repository.GetAllAsync(request.PageNumber, request.PageSize);
                cache.Set(cacheToDosKey, toDos);
            }
        } catch (Exception ex) {
            return new Result<GetAllToDosResponse>(error: "Failed to Get ToDos and Total of ToDos", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion

        return new Result<GetAllToDosResponse>(new GetAllToDosResponse(toDos ?? new List<ToDo>(), totalCount), HttpStatusCode.OK);
    }
}
