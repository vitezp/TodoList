using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Enums;

namespace TodoList.Application.Validators
{
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