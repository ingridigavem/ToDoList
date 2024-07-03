using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.UpdateToDoDescription;
public record UpdateToDoDescriptionRequest(Guid Id, string Description) : IRequest<Result<UpdateToDoDescriptionResponse>>;
