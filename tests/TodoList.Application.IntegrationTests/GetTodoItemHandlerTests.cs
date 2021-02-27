using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TodoList.Application.Commands;
using TodoList.Application.Handlers;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using Xunit;

namespace TodoList.Application.IntegrationTests
{
    public class GetTodoItemHandlerTests
    {
        private readonly IRequestHandler<GetTodoCommand, TodoResponse> _sut;
        private readonly ITodoItemRepository _todoItemRepo;

        public GetTodoItemHandlerTests()
        {
            _todoItemRepo = Substitute.For<ITodoItemRepository>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _sut = new GetTodoItemHandler(new NullLogger<GetTodoItemHandler>(), _todoItemRepo, mapper);
        }

        [Theory]
        [InlineData(1324513, "Name", Status.Completed, 2)]
        [InlineData(50, "Item Name", Status.InProgress, 10)]
        [InlineData(50, "Item Name", Status.NotStarted, 99)]
        public async void GetTodoItemHandler_ShouldReturnItem_WhenRequestedIdMatchesExistingItem(int id, string name,
            Status status, int priority)
        {
            // Arrange
            var todo = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            _todoItemRepo.GetTodoItemById(id)
                .Returns(todo);

            // Act
            var res = await _sut.Handle(new GetTodoCommand(id.ToString()), CancellationToken.None);

            // Assert
            res.Should().Match<TodoResponse>(m => m.Id == todo.Id.ToString()
                                                  && m.Name == todo.Name
                                                  && m.Priority == todo.Priority.ToString()
                                                  && m.Status == todo.Status.ToString());
        }

        [Fact]
        public async void GetTodoItemHandler_ShouldReturnNothing_WhenSpecifiedItemIsNotPresent()
        {
            // Arrange
            const int id = 1234;
            _todoItemRepo.GetTodoItemById(id)
                .ReturnsNull();

            // Act
            var res = await _sut.Handle(new GetTodoCommand(id.ToString()), CancellationToken.None);

            // Assert
            res.Should().BeNull();
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData("-123")]
        public async void GetTodoItemHandler_ShouldReturnError_WhenInvalidIdIsProvided(string id)
        {
            // Arrange

            // Act
            var res = await _sut.Handle(new GetTodoCommand(id), CancellationToken.None);

            // Assert
            res.ErrorResponse.Errors.Should().Contain($"Unable to select, TodoId should be an Integer > 0. Got: '{id}'");
        }
    }
}