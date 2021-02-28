using System.Threading;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReturnsExtensions;
using TodoList.Application.Commands;
using TodoList.Application.Handlers;
using TodoList.Application.Interfaces;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using Xunit;

namespace TodoList.Application.IntegrationTests
{
    public class CreateTodoItemHandlerTests
    {
        private readonly IRequestHandler<CreateTodoCommand, TodoResponse> _sut;
        private readonly ITodoItemRepository _todoItemRepo;

        public CreateTodoItemHandlerTests()
        {
            _todoItemRepo = Substitute.For<ITodoItemRepository>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _sut = new CreateTodoItemHandler(new NullLogger<CreateTodoItemHandler>(), _todoItemRepo, mapper);
        }

        [Theory]
        [InlineData(1324513, "Name", Status.Completed, 2)]
        [InlineData(50, "Item Name", Status.InProgress, 10)]
        [InlineData(50, "Item Name", Status.NotStarted, 99)]
        [InlineData(1, "Item Name")]
        public async void CreateTodoItemHandler_ShouldCreateTodoItem_WhenAllParametersAreValid(int id, string name,
            Status status = default, int priority = 0)
        {
            // Arrange
            var todo = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            TodoItem receivedArgs = null;
            _todoItemRepo.GetTodoItemByName(Arg.Is<string>(m=>m==todo.Name))
                .ReturnsNull();
            _todoItemRepo.InsertTodoItem(
                    Arg.Do<TodoItem>(x => receivedArgs = x))
                .Returns(true);

            // Act
            var res = await _sut.Handle(
                new CreateTodoCommand(new TodoRequest
                    {Name = name, Status = status.ToString(), Priority = priority.ToString()}),
                CancellationToken.None);

            // Assert
            receivedArgs.Should().BeEquivalentTo(todo, options =>
                options.Excluding(o => o.Id));
            res.Should()
                .Match<TodoResponse>((x) =>
                    x.Name == todo.Name &&
                    x.Status == todo.Status.ToString() &&
                    x.Priority == todo.Priority.ToString() &&
                    x.ErrorResponse == null
                );
        }

        [Theory]
        [InlineData("asdf", "Name", Status.Completed, 2)]
        public async void CreateTodoItemHandler(string id, string name, Status status, int priority)
        {
            // Arrange
            _todoItemRepo.GetTodoItemByName(Arg.Any<string>())
                .ReturnsNull();
            _todoItemRepo.InsertTodoItem(Arg.Any<TodoItem>())
                .Returns(false);

            // Act
            var res = await _sut.Handle(
                new CreateTodoCommand(new TodoRequest
                    {Name = name, Status = status.ToString(), Priority = priority.ToString()}),
                CancellationToken.None);

            // Assert
            res.ErrorResponse.Errors.Should().Contain($"Unable to create item with name: '{name}'");
        }

        [Theory]
        [InlineData("Name", Status.Completed, 2)]
        public async void CreateTodoItemHandler_ShouldNotCreateItem_WhenItemAlreadyExists(string name, Status status, int priority)
        {
            // Arrange
            _todoItemRepo.GetTodoItemByName(Arg.Any<string>())
                .Returns(new TodoItem());

            // Act
            var res = await _sut.Handle(
                new CreateTodoCommand(new TodoRequest
                    {Name = name, Status = status.ToString(), Priority = priority.ToString()}),
                CancellationToken.None);

            // Assert
            res.ErrorResponse.Errors.Should().Contain($"Todo item '{name}' already exists");
        }
    }
}