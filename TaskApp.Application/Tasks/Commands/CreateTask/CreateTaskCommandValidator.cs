using FluentValidation;

namespace TaskApp.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        //RuleFor(x => x.DueDate)
        //    .GreaterThanOrEqualTo(DateTime.UtcNow).When(x => x.DueDate.HasValue);
    }
}