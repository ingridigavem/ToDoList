using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.DeleteToDo;
public record DeleteToDoRequest(Guid Id) : IRequest<Result<DeleteToDoResponse>>;
