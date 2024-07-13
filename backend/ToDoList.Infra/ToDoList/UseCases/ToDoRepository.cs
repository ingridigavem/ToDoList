using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Infra.Data;

namespace ToDoList.Infra.ToDoList.UseCases;
public class ToDoRepository(AppDbContext context) : IToDoRepository {
    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await context.ToDos.CountAsync(cancellationToken);


    public async Task<List<ToDo>> GetAllAsync(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default)
        => await context.ToDos.AsNoTracking().ToListAsync(cancellationToken);

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
