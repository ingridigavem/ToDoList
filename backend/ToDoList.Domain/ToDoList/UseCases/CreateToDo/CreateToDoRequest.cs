using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.CreateToDo;
public record CreateToDoRequest(string Description) : IRequest<Result<CreateToDoResponse>>;
