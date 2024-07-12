using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.Contracts;
public interface IToDoRepository {
    Task SaveToDoAsync(ToDo toDo, CancellationToken cancellationToken = default);
    Task DeleteToDoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ToDo>> GetAllAsync(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task CompleteToDoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ToDo?> UpdateToDoDescriptionAsync(Guid id, string description, CancellationToken cancellationToken = default);
}
