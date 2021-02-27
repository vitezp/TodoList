using System.Collections.Generic;
using MediatR;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Commands
{
    public class GetAllTodoCommand : IRequest<IEnumerable<TodoResponse>>
    {
    }
}