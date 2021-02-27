using MediatR;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    public class GetTodoCommand : IRequest<TodoResponse>
    {
        public string Id { get; set; }

        public GetTodoCommand(string id)
        {
            Id = id;
        }
    }
}