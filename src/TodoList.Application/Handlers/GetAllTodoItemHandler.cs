using System.Collections.Generic;
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
    public class GetAllTodoItemHandler : IRequestHandler<GetAllTodoCommand, IEnumerable<TodoResponse>>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTodoItemHandler> _logger;

        public GetAllTodoItemHandler(ILogger<GetAllTodoItemHandler> logger,
            ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoResponse>> Handle(GetAllTodoCommand request,
            CancellationToken cancellationToken)
        {
            var selectedTodoItems = await _todoItemRepository.GetAllTodoItems().ConfigureAwait(false);

            _logger.LogInformation($"Got: '{selectedTodoItems.Count()} items from the system'");
            return _mapper.Map<List<TodoResponse>>(selectedTodoItems.OrderByDescending(m=>m.Priority));
        }
    }
}