using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Infra.Data;

namespace ToDoList.Infra.ToDoList.UseCases;
public class ToDoRepository(AppDbContext context) : IToDoRepository {
    public async Task<int> CountAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
        await context.ToDos.Where(t => includeDeleted || !t.Deleted).CountAsync(cancellationToken);


    public async Task<List<ToDo>> GetAllAsync(int pageNumber = 1, int pageSize = 10, bool includeDeleted = false, CancellationToken cancellationToken = default)
        => await context.ToDos.Where(t => includeDeleted || !t.Deleted).OrderByDescending(t => t.CreatedAt).AsNoTracking()
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

    public async Task SaveToDoAsync(ToDo toDo, CancellationToken cancellationToken = default) {
        await context.ToDos.AddAsync(toDo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ToDo?> UpdateToDoDescriptionAsync(Guid id, string description, CancellationToken cancellationToken = default) {
        var toDo = await context.ToDos.FindAsync([id], cancellationToken: cancellationToken);

        if (toDo is null)
            return null;

        toDo.Description = description;
        await context.SaveChangesAsync(cancellationToken);
        return toDo;
    }
    public async Task CompleteToDoAsync(Guid id, CancellationToken cancellationToken = default) {
        var toDo = await context.ToDos.FindAsync([id], cancellationToken: cancellationToken);
        toDo?.CompleteToDo();
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteToDoAsync(Guid id, CancellationToken cancellationToken = default) {
        var toDo = await context.ToDos.FindAsync([id], cancellationToken: cancellationToken);
        toDo?.DeleteToDo();
        await context.SaveChangesAsync(cancellationToken);
    }
}
