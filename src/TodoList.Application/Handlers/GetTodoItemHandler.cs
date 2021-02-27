using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Commands;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Responses;

namespace TodoList.Application.Handlers
{
    public class GetTodoItemHandler : IRequestHandler<GetTodoCommand, TodoResponse>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTodoItemHandler> _logger;

        public GetTodoItemHandler(ILogger<GetTodoItemHandler> logger,
            ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<TodoResponse> Handle(GetTodoCommand request, CancellationToken cancellationToken)
        {
            if (!int.TryParse(request.Id, out var todoId) || todoId < 1)
            {
                var error = $"Unable to select, TodoId should be an Integer > 0. Got: '{request.Id}'";
                _logger.LogError(error);
                return new TodoResponse(new ErrorResponse(error));
            }

            //TODO await
            var selectedTodoItem = _todoItemRepository.GetTodoItemById(todoId);

            _logger.LogInformation($"Got: '{selectedTodoItem}' from the system'");
            return _mapper.Map<TodoResponse>(selectedTodoItem);
        }
    }
}