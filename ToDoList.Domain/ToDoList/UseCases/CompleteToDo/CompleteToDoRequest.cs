namespace ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
public record CompleteToDoRequest(Guid Id, bool Complete);