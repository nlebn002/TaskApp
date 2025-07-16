using FluentValidation;

namespace TaskApp.Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
    }
}