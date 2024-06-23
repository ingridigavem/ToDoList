using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.GetAllToDos;

public record GetAllToDosResponse(IEnumerable<ToDo> ToDos, int TotalCount);
