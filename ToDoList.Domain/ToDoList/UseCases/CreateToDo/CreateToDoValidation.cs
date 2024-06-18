using FluentValidation;

namespace ToDoList.Domain.ToDoList.UseCases.CreateToDo;
public class CreateToDoValidation : AbstractValidator<CreateToDoRequest> {
    public CreateToDoValidation() {
        RuleFor(request => request.Description).NotNull();
        RuleFor(request => request.Description).MinimumLength(3);
        RuleFor(request => request.Description).MaximumLength(150);
    }
}