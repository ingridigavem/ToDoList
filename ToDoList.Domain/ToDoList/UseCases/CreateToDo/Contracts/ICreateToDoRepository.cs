using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.CreateToDo.Contracts;
public interface ICreateToDoRepository {
    Task SaveToDo(ToDo toDo);
}
