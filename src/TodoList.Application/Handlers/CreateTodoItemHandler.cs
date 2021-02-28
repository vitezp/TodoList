using System.Collections.Generic;
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
    public class CreateTodoItemHandler : IRequestHandler<CreateTodoCommand, TodoResponse>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTodoItemHandler> _logger;

        public CreateTodoItemHandler(ILogger<CreateTodoItemHandler> logger,
            ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<TodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todoItem = _mapper.Map<TodoItem>(request.TodoRequest);

            var itemAlreadyExists = _todoItemRepository.GetTodoItemByName(todoItem.Name);
            if (itemAlreadyExists != null)
            {
                var error = $"Todo item '{request.TodoRequest.Name}' already exists";
                _logger.LogInformation(error);
                return new TodoResponse {ErrorResponse = new ErrorResponse(error)}; 
            }

            //TODO await
            var success = _todoItemRepository.InsertTodoItem(todoItem);
            if (!success)
            {
                var error = $"Unable to create item with name: '{request.TodoRequest.Name}'";
                _logger.LogInformation(error);
                return new TodoResponse {ErrorResponse = new ErrorResponse(error)};
            }

            _logger.LogInformation($"Created item with name: '{request.TodoRequest.Name}'");
            return _mapper.Map<TodoResponse>(todoItem);
        }
    }
}