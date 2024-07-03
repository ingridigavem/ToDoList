using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.CreateToDo;
public class CreateToDoHandler(IToDoRepository repository, MemoryCache cache) : IRequestHandler<CreateToDoRequest, Result<CreateToDoResponse>> {
    public async Task<Result<CreateToDoResponse>> Handle(CreateToDoRequest request, CancellationToken cancellationToken) {

        #region Validate Request    
        var validator = new CreateToDoValidation();

        if (request is null) return new Result<CreateToDoResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        var validations = validator.Validate(request);

        if (!validations.IsValid) {
            var errorsList = new List<string>();
            validations.Errors?.ForEach(error => errorsList.Add(error.ErrorMessage));
            return new Result<CreateToDoResponse>(errors: errorsList, status: HttpStatusCode.BadRequest);
        }
        #endregion

        #region Generate ToDo
        var toDo = new ToDo { Description = request.Description };
        #endregion

        #region Save Data and Reset Cache
        try {
            await repository.SaveToDoAsync(toDo);
            cache.Clear();

        } catch (Exception ex) {
            return new Result<CreateToDoResponse>(error: "Failed to persist new To-Do", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion

        return new Result<CreateToDoResponse>(data: new CreateToDoResponse(toDo.Id), status: HttpStatusCode.Created);
    }
}
