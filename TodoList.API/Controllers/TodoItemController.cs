using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.DAL;
using TodoList.DAL.Models;

namespace TodoList.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemController(ILogger<TodoItemController> logger, ITodoItemRepository todoItemRepository)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAsync()
        {
            return _todoItemRepository.GetAllTodoItems();
        }
        
        [HttpPost]
        public int Insert(TodoItem todoItem)
        {
            return _todoItemRepository.InsertTodoItem(todoItem);
        }
    }
}