using System.Threading;
using AutoMapper;
using Castle.Core.Internal;
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
    public class UpdateTodoItemHandlerTests
    {
        private readonly IRequestHandler<UpdateTodoCommand, TodoResponse> _sut;
        private readonly ITodoItemRepository _todoItemRepo;

        public UpdateTodoItemHandlerTests()
        {
            _todoItemRepo = Substitute.For<ITodoItemRepository>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _sut = new UpdateTodoItemHandler(new NullLogger<UpdateTodoItemHandler>(), _todoItemRepo, mapper);
        }

        [Theory]
        [InlineData(1324513, "Name", Status.Completed, 2)]
        [InlineData(50, "Item Name", Status.InProgress, 10)]
        [InlineData(50, "Item Name", Status.NotStarted, 99)]
        public async void Handle_ShouldUpdateItem_WhenParametersAreInvalid(int id, string name, Status status,
            int priority)
        {
            // Arrange
            var todo = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            TodoItem receivedArgs = null;
            _todoItemRepo.GetTodoItemById(Arg.Is<int>(m => m == todo.Id))
                .Returns(todo);
            _todoItemRepo.UpdateTodoItem(
                    Arg.Do<TodoItem>(x => receivedArgs = x))
                .Returns(true);

            // Act
            var res = await _sut.Handle(
                new UpdateTodoCommand(new UpdateTodoRequest
                    {Id = id.ToString(), Name = name, Status = status.ToString(), Priority = priority.ToString()}),
                CancellationToken.None);

            // Assert
            receivedArgs.Should().BeEquivalentTo(todo);
            res.Should()
                .Match<TodoResponse>((x) =>
                        x.Name == todo.Name &&
                        x.Status == todo.Status.ToString() &&
                        x.Priority == todo.Priority.ToString()
                );
        }

        [Theory]
        [InlineData(1234)]
        public async void Handle_ShouldReturnError_WhenItemIsNotPresent(int id)
        {
            // Arrange
            var todo = new TodoItem {Id = id};
            _todoItemRepo.GetTodoItemById(Arg.Any<int>())
                .ReturnsNull();

            // Act
            var res = await _sut.Handle(
                new UpdateTodoCommand(new UpdateTodoRequest
                    {Id = id.ToString()}),
                CancellationToken.None);

            // Assert
            res.Should()
                .Match<TodoResponse>((x) =>
                    x.ErrorResponse.Errors.Contains($"Todo item with id: '{todo.Id} not found'")
                );
        }
        
        [Theory]
        [InlineData(1234, "Name", Status.Completed, 12)]
        public async void Handle_ShouldReturnError_WhenUpdateFails(int id, string name, Status status,
            int priority)
        {
            // Arrange
            var todo = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            _todoItemRepo.GetTodoItemById(Arg.Any<int>())
                .Returns(todo);
            _todoItemRepo.UpdateTodoItem(Arg.Any<TodoItem>())
                .Returns(false);

            // Act
            var res = await _sut.Handle(
                new UpdateTodoCommand(new UpdateTodoRequest
                    {Id = id.ToString(), Name = name, Status = status.ToString(), Priority = priority.ToString()}),
                CancellationToken.None);

            // Assert
            res.Should()
                .Match<TodoResponse>((x) =>
                    x.ErrorResponse.Errors.Contains($"Failed when updating todo item '{todo}'")
                );
        }
        
    }
}