using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.GetAllToDos;
public class GetAllToDosHandler {
    private readonly IToDoRepository _repository;
    private readonly MemoryCache _cache;

    public GetAllToDosHandler(IToDoRepository repository, MemoryCache cache) {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Result<GetAllToDosResponse>> Handle(GetAllToDosRequest request) {

        if (request is null) return new Result<GetAllToDosResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        #region Get ToDos and Set Cache
        IList<ToDo>? toDos = new List<ToDo>();
        int totalCount = 0;

        string cacheToDosKey = $"toDosList_page_{request.PageNumber}_size_{request.PageSize}";
        string cacheTotalCountKey = "totalCount";

        try {
            if (!_cache.TryGetValue(cacheTotalCountKey, out totalCount)) {
                totalCount = await _repository.Count();
                _cache.Set(cacheTotalCountKey, totalCount);
            }

            if (!_cache.TryGetValue(cacheToDosKey, out toDos)) {
                toDos = await _repository.GetAll(request.PageNumber, request.PageSize);
                _cache.Set(cacheToDosKey, toDos);
            }
        } catch (Exception ex) {
            new Result<GetAllToDosResponse>(error: "Failed to Get ToDos and Total of ToDos", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion

        return new Result<GetAllToDosResponse>(new GetAllToDosResponse(toDos ?? new List<ToDo>(), totalCount), HttpStatusCode.OK);
    }
}
