﻿using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;

namespace ToDoList.Domain.ToDoList.UseCases.UpdateToDoDescription;
public class UpdateToDoDescriptionHandler(IToDoRepository repository, MemoryCache cache) : IRequestHandler<UpdateToDoDescriptionRequest, Result<UpdateToDoDescriptionResponse>> {
    public async Task<Result<UpdateToDoDescriptionResponse>> Handle(UpdateToDoDescriptionRequest request, CancellationToken cancellationToken) {

        #region Validate Request
        if (request is null) return new Result<UpdateToDoDescriptionResponse>(error: "Request can not be null", status: HttpStatusCode.BadRequest);

        var validator = new UpdateToDoDescriptionValidation();
        var validations = validator.Validate(request);

        if (!validations.IsValid) {
            var errorsList = new List<string>();
            validations.Errors?.ForEach(error => errorsList.Add(error.ErrorMessage));
            return new Result<UpdateToDoDescriptionResponse>(errors: errorsList, status: HttpStatusCode.BadRequest);
        }
        #endregion        

        ToDo? toDo;
        #region Complete ToDo and Reset Cache
        try {

            toDo = await repository.UpdateToDoDescriptionAsync(request.Id, request.Description, cancellationToken);

            if (toDo is null)
                return new Result<UpdateToDoDescriptionResponse>(error: "To-Do not found", status: HttpStatusCode.NotFound);

            cache.Clear();
        } catch (Exception ex) {
            return new Result<UpdateToDoDescriptionResponse>(error: "Failed to complete To-Do", exceptionMessage: ex.Message, status: HttpStatusCode.InternalServerError);
        }
        #endregion
        return new Result<UpdateToDoDescriptionResponse>(new UpdateToDoDescriptionResponse(toDo), status: HttpStatusCode.OK);
    }
}
