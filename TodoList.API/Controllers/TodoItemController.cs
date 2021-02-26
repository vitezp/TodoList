using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Data;
using TodoList.Data.Models;
using TodoList.Shared.Contract;
using TodoList.Shared.Contract.Requests;
using TodoList.Shared.Contract.Responses;
using TodoList.Shared.Domain;
using TodoList.Shared.Extensions;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;

        public TodoItemController(ILogger<TodoItemController> logger, ITodoItemRepository todoItemRepository,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _todoItemRepository = todoItemRepository;
        }

        [HttpGet(ApiRoutes.Todo.GetAll)]
        public async Task<IActionResult> GetAsync()
        {
            var allTodoItems = _todoItemRepository.GetAllTodoItems();
            return Ok(_mapper.Map<List<TodoResponse>>(allTodoItems));
        }

        [HttpDelete(ApiRoutes.Todo.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string todoName)
        {
            var deleted = _todoItemRepository.DeleteTodoItem(new TodoItem() {Name = todoName});

            if (deleted == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost(ApiRoutes.Todo.Update)]
        [ProducesResponseType(typeof(TodoResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Update([FromBody] TodoRequest request)
        {
            //TODO use mapper
            var todoItem = new TodoItem()
            {
                Status = request.Status.ParseEnum<Status>(),
                Name = request.Name,
                Priority = request.Priority
            };

            var get = _todoItemRepository.GetAllTodoItems().First(m => m.Name == request.Name);
            if (get == null)
            {
                return BadRequest(new ErrorResponse(new ErrorModel
                    {Message = "Todo item with given name is not present"}));
            }

            var rowsAffected = _todoItemRepository.UpdateTodoItem(todoItem);
            if (rowsAffected == 0)
            {
                return BadRequest(new ErrorResponse(new ErrorModel {Message = "Unable to update todo item"}));
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Todo.Get.Replace("{tagName}", todoItem.Name);
            return Created(locationUri, _mapper.Map<TodoResponse>(todoItem));
        }

        /// <summary>
        /// Creates a TodoItem in the system
        /// </summary>
        /// <response code="201">Creates a TodoItem in the system</response>
        /// <response code="400">Unable to create a TodoItem due to a validation error</response>
        [HttpPost(ApiRoutes.Todo.Create)]
        [ProducesResponseType(typeof(TodoResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] TodoRequest request)
        {
            //TODO use mapper
            var todoItem = new TodoItem()
            {
                Status = request.Status.ParseEnum<Status>(),
                Name = request.Name,
                Priority = request.Priority
            };

            var rowsAffected = _todoItemRepository.InsertTodoItem(todoItem);
            if (rowsAffected == 0)
            {
                return BadRequest(new ErrorResponse(new ErrorModel {Message = "Unable to create todo item"}));
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Todo.Get.Replace("{tagName}", todoItem.Name);
            return Created(locationUri, _mapper.Map<TodoResponse>(todoItem));
        }
    }
}