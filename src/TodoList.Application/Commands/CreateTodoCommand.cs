using MediatR;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    public class CreateTodoCommand : IRequest<TodoResponse>
    {
        public TodoRequest TodoRequest { get; set; }

        public CreateTodoCommand(TodoRequest todoRequest)
        {
            TodoRequest = todoRequest;
        }
    }
}