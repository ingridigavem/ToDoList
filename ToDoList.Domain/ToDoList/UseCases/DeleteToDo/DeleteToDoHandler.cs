using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.DeleteToDo;
public class DeleteToDoHandler {
    private readonly IToDoRepository _repository;
    private readonly IMemoryCache _cache;

    public DeleteToDoHandler(IToDoRepository repository, IMemoryCache cache) {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Result<DeleteToDoResponse>> Handle(DeleteToDoRequest request) {
        #region Validate Request
        var validator = new DeleteToDoValidation();

        if (request is null) return new Result<DeleteToDoResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        var validations = validator.Validate(request);

        if (!validations.IsValid) {
            var errorsList = new List<string>();
            validations.Errors?.ForEach(error => errorsList.Add(error.ErrorMessage));
            return new Result<DeleteToDoResponse>(errors: errorsList, status: HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete ToDo and Reset Cache
        try {
            await _repository.DeleteToDo(request.Id);
            _cache.Remove(key: "ToDoList");
        } catch (Exception ex) {

            return new Result<DeleteToDoResponse>(error: "Failed to delete To-Do", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion

        return new Result<DeleteToDoResponse>(data: new DeleteToDoResponse(true), status: HttpStatusCode.OK);
    }
}
