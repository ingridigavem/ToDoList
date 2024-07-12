using FluentValidation;
using ToDoList.Domain.ToDoList.UseCases.CreateToDo;
using ToDoList.Domain.ToDoList.UseCases.DeleteToDo;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDoDescription;

namespace ToDoList.Domain.ToDoList.UseCases;
public class CreateToDoValidation : AbstractValidator<CreateToDoRequest> {
    public CreateToDoValidation() {
        RuleFor(request => request).NotNull();
        RuleFor(request => request).Equals(typeof(CreateToDoRequest));
        RuleFor(request => request.Description).NotNull();
        RuleFor(request => request.Description).NotEmpty();
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

public class UpdateToDoDescriptionValidation : AbstractValidator<UpdateToDoDescriptionRequest> {
    public UpdateToDoDescriptionValidation() {
        RuleFor(request => request).NotNull();
        RuleFor(request => request).Equals(typeof(UpdateToDoDescriptionRequest));
        RuleFor(request => request.Description).NotNull();
        RuleFor(request => request.Description).NotEmpty();
        RuleFor(request => request.Description).MinimumLength(3);
        RuleFor(request => request.Description).MaximumLength(150);
    }
}

public class CompleteToDoValidation : AbstractValidator<CompleteToDoRequest> {
    public CompleteToDoValidation() {
        RuleFor(request => request).NotNull();
        RuleFor(request => request).Equals(typeof(CompleteToDoRequest));
    }
}