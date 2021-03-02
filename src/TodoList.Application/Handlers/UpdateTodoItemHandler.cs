using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Commands;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Domain.Extensions;

namespace TodoList.Application.Handlers
{
    public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoCommand, TodoResponse>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTodoItemHandler> _logger;

        public UpdateTodoItemHandler(ILogger<UpdateTodoItemHandler> logger,
            ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<TodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todoItem = _mapper.Map<TodoItem>(request.TodoRequest);

            var got = await _todoItemRepository.GetTodoItemById(todoItem.Id).ConfigureAwait(false);
            if (got == null)
            {
                var err = $"Todo item with id: '{todoItem.Id} not found'";
                _logger.LogError(err);
                return new TodoResponse(new ErrorResponse(err));
            }

            _logger.LogInformation($"Got todo item for id '{todoItem.Id}'");

            var success = await _todoItemRepository.UpdateTodoItem(todoItem).ConfigureAwait(false);
            if (!success)
            {
                var err = $"Failed when updating todo item '{todoItem}'";
                _logger.LogError(err);
                return new TodoResponse(new ErrorResponse(err));
            }

            _logger.LogInformation($"Updated item with name: '{request.TodoRequest.Name}'");
            return _mapper.Map<TodoResponse>(todoItem);
        }
    }
}