using ToDoList.Domain.Shared.Entities;

namespace ToDoList.Domain.ToDoList.Entities;
public class ToDo : Entity {
    public string Description { get; set; } = null!;
    public bool Done { get; private set; }
    public bool Deleted { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public void CompleteToDo() => Done = !Done;
    public void DeleteToDo() => Deleted = true;
}
