using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
public class CompleteToDoHandler(IToDoRepository repository, MemoryCache cache) : IRequestHandler<CompleteToDoRequest, Result<CompleteToDoResponse>> {
    public async Task<Result<CompleteToDoResponse>> Handle(CompleteToDoRequest request, CancellationToken cancellationToken) {
        #region Validate Request
        if (request is null) return new Result<CompleteToDoResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        var validator = new CompleteToDoValidation();
        var validations = validator.Validate(request);

        if (!validations.IsValid) {
            var errorsList = new List<string>();
            validations.Errors?.ForEach(error => errorsList.Add(error.ErrorMessage));
            return new Result<CompleteToDoResponse>(errors: errorsList, status: HttpStatusCode.BadRequest);
        }
        #endregion

        #region Complete ToDo and Reset Cache
        try {

            await repository.CompleteToDoAsync(request.Id, cancellationToken);
            cache.Clear();
        } catch (Exception ex) {
            return new Result<CompleteToDoResponse>(error: "Failed to complete To-Do", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion

        return new Result<CompleteToDoResponse>(new CompleteToDoResponse(true), HttpStatusCode.OK);
    }
}
