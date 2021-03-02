using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Application.Commands;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;

namespace TodoList.Web.Controllers
{
    
    /// <summary>
    /// Thin API controllers that leverage mediator to allow simple testability. 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly IMediator _mediator;

        public TodoItemController(ILogger<TodoItemController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the todo item by ID
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Todo.GetById)]
        [ProducesResponseType(typeof(TodoResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string todoId)
        {
            var query = new GetTodoCommand(todoId);
            var result = await _mediator.Send(query);
            if (result.ErrorResponse != null)
            {
                return NotFound($"Unable to get Todo item with id: '{todoId}'");
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets all todo items from the system
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Todo.GetAll)]
        [ProducesResponseType(typeof(List<TodoResponse>), 200)]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetAllTodoCommand();
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// Creates a TodoItem in the system
        /// </summary>
        /// <response code="201">Creates a TodoItem in the system</response>
        /// <response code="400">Unable to create a TodoItem due to a validation error</response>
        [HttpPost(ApiRoutes.Todo.Create)]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] TodoRequest request)
        {
            var query = new CreateTodoCommand(request);
            var result = await _mediator.Send(query);
            if (result.ErrorResponse != null)
            {
                return BadRequest(result.ErrorResponse);
            }

            return Created(ApiRoutes.Todo.Create, result);
        }

        /// <summary>
        /// Delete a TodoItem from the system
        /// </summary>
        /// <response code="204">Successfully deleted the item</response>
        /// <response code="404">Unable to delete a TodoItem as it probably does note exist</response>
        [HttpDelete(ApiRoutes.Todo.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> Delete([FromRoute] string todoId)
        {
            var query = new DeleteTodoCommand(todoId);
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpPut(ApiRoutes.Todo.Update)]
        [ProducesResponseType(typeof(TodoResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Update([FromBody] UpdateTodoRequest request)
        {
            var query = new UpdateTodoCommand(request);
            var result = await _mediator.Send(query);
            if (result.ErrorResponse != null)
            {
                return BadRequest(result.ErrorResponse);
            }

            return Created(ApiRoutes.Todo.Create, result);
        }
    }
}