using MediatR;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    public class DeleteTodoCommand : IRequest<DeletedTodoResponse>
    {
        public string TodoId { get; set; }

        public DeleteTodoCommand(string todoId)
        {
            TodoId = todoId;
        }
    }
}