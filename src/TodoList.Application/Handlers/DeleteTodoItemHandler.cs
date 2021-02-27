using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Application.Commands;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Domain.Extensions;

namespace TodoList.Application.Handlers
{
    public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoCommand, DeletedTodoResponse>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTodoItemHandler> _logger;

        public DeleteTodoItemHandler(ILogger<DeleteTodoItemHandler> logger,
            ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<DeletedTodoResponse> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            if (!int.TryParse(request.TodoId, out var todoId) || todoId < 1)
            {
                var error = $"Unable to delete, TodoId should be an Integer >0. Got: {request.TodoId}";
                _logger.LogError(error);
                return new DeletedTodoResponse(error);
            }

            //TODO await
            var success = _todoItemRepository.DeleteTodoItem(new TodoItem {Id = todoId});

            _logger.LogInformation($"Deleted item with name: '{request.TodoId}'");
            return new DeletedTodoResponse(success);
        }
    }
}