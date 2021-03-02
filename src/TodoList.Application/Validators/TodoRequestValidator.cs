using System;
using System.Linq;
using FluentValidation;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Enums;

namespace TodoList.Application.Validators
{
    /// <summary>
    /// Fluent validation that is invoked once the requests are received on the API. 
    /// </summary>
    public class TodoRequestValidator : AbstractValidator<TodoRequest>
    {
        public TodoRequestValidator()
        {
            RuleFor(x => x.Status)
                .Custom((x, context) =>
                {
                    if (!string.IsNullOrEmpty(x) && Enum.GetValues<Status>().All(m =>
                        !m.ToString().Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        context.AddFailure(
                            $"Todo item field 'status' must be empty or one of '{string.Join(", ", Enum.GetValues<Status>())}'");
                    }
                });

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Todo item field 'name' cannot be empty");

            RuleFor(x => x.Priority)
                .Custom((x, context) =>
                {
                    if (!string.IsNullOrWhiteSpace(x) && (!int.TryParse(x, out var value) || value < 0 || value > 100))
                    {
                        context.AddFailure($"'{x}' is not a valid number or not within range <0,100>");
                    }
                });
        }
    }
}