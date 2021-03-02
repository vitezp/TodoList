using MediatR;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    
    /// <summary>
    /// MediatR -- Commands are used to handle the API requests from the controller and are passed to the mediator
    /// </summary>
    public class CreateTodoCommand : IRequest<TodoResponse>
    {
        public TodoRequest TodoRequest { get; set; }

        public CreateTodoCommand(TodoRequest todoRequest)
        {
            TodoRequest = todoRequest;
        }
    }
}