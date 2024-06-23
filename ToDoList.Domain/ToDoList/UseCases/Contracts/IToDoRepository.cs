using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.Contracts;
public interface IToDoRepository {
    Task SaveToDo(ToDo toDo);
    Task DeleteToDo(Guid id);
}
