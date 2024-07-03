using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
public record CompleteToDoRequest(Guid Id, bool Complete) : IRequest<Result<CompleteToDoResponse>>;