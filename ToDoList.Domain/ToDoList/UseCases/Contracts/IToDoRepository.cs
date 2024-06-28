using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.Contracts;
public interface IToDoRepository {
    Task SaveToDoAsync(ToDo toDo);
    Task DeleteToDoAsync(Guid id);
    Task<List<ToDo>> GetAllAsync(int PageNumber = 0, int PageSize = 10);
    Task<int> CountAsync();
    Task<ToDo> CompleteToDoAsync(Guid id, bool complete);
    Task<ToDo> UpdateToDoDescriptionAsync(Guid id, string description);
}
