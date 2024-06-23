using FluentValidation;
using ToDoList.Domain.ToDoList.UseCases.CreateToDo;
using ToDoList.Domain.ToDoList.UseCases.DeleteToDo;

namespace ToDoList.Domain.ToDoList.UseCases;
public class CreateToDoValidation : AbstractValidator<CreateToDoRequest> {
    public CreateToDoValidation() {
        RuleFor(request => request).NotNull();
        RuleFor(request => request).Equals(typeof(CreateToDoRequest));
        RuleFor(request => request.Description).NotNull();
        RuleFor(request => request.Description).MinimumLength(3);
        RuleFor(request => request.Description).MaximumLength(150);
    }
}

public class DeleteToDoValidation : AbstractValidator<DeleteToDoRequest> {
    public DeleteToDoValidation() {
        RuleFor(request => request).NotNull();
        RuleFor(request => request).Equals(typeof(DeleteToDoRequest));
    }
}