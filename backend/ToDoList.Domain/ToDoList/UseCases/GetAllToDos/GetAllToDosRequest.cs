using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.GetAllToDos;
public record GetAllToDosRequest(int PageNumber = 1, int PageSize = 10) : IRequest<Result<GetAllToDosResponse>>;