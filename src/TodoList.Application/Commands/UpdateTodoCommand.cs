using MediatR;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    public class UpdateTodoCommand : IRequest<TodoResponse>
    {
        public UpdateTodoRequest TodoRequest { get; }

        public UpdateTodoCommand(UpdateTodoRequest todoRequest)
        {
            TodoRequest = todoRequest;
        }
    }
}