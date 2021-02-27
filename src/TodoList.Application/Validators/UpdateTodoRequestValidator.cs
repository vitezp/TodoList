using System;
using FluentValidation;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Enums;

namespace TodoList.Application.Validators
{
    public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
    {
        public UpdateTodoRequestValidator()
        {
            Include(new TodoRequestValidator());

            RuleFor(x => x.Id)
                .Custom((x, context) =>
                {
                    if (!int.TryParse(x, out var value) || value < 1)
                    {
                        context.AddFailure($"'{x}' is not a valid id or it's lower than 1");
                    }
                });
        }
    }
}