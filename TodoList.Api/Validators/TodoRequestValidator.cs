using System;
using System.Linq;
using FluentValidation;
using TodoList.Api.Controllers;
using TodoList.Shared.Contract.Requests;
using TodoList.Shared.Domain;

namespace TodoList.Api.Validators
{
    public class TodoRequestValidator : AbstractValidator<TodoRequest>
    {
        public TodoRequestValidator()
        {
            RuleFor(x => x.Status)
                .IsEnumName(typeof(Status), false)
                .WithMessage($"Todo item field 'status' must be one of '{string.Join(", ", Enum.GetValues<Status>())}'");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Todo item field 'name' cannot be empty");
            RuleFor(x => x.Priority)
                .InclusiveBetween(0, 100)
                .WithMessage("Todo item field 'priority must be within range <0,100>'");
        }
        
    }
}