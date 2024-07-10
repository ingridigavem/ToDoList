using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Infra.Data;

namespace ToDoList.Infra.ToDoList.UseCases;
public class ToDoRepository(AppDbContext context) : IToDoRepository {
    public Task<ToDo> CompleteToDoAsync(Guid id, bool complete, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default) {
        return await context.ToDos.CountAsync(cancellationToken);
    }

    public async Task DeleteToDoAsync(Guid id, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public async Task<List<ToDo>> GetAllAsync(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default)
        => await context.ToDos.AsNoTracking()
                              .ToListAsync(cancellationToken);

    public async Task SaveToDoAsync(ToDo toDo, CancellationToken cancellationToken = default) {
        await context.ToDos.AddAsync(toDo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ToDo> UpdateToDoDescriptionAsync(Guid id, string description, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }
}
