using System.Threading;
using AutoMapper;
using Castle.Core.Internal;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.Core;
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
    public class DeleteTodoItemHandlerTests
    {
        private readonly IRequestHandler<DeleteTodoCommand, DeletedTodoResponse> _sut;
        private readonly ITodoItemRepository _todoItemRepo;

        public DeleteTodoItemHandlerTests()
        {
            _todoItemRepo = Substitute.For<ITodoItemRepository>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _sut = new DeleteTodoItemHandler(new NullLogger<DeleteTodoItemHandler>(), _todoItemRepo, mapper);
        }

        [Theory]
        [InlineData(1324513)]
        public async void DeleteTodoItemHandler_ShouldReturnNoError_WhenItemToDeleteIsPresent(int id)
        {
            // Arrange
            var todo = new TodoItem {Id = id};
            TodoItem receivedArgs = null;
            _todoItemRepo.DeleteTodoItem(
                    Arg.Do<TodoItem>(x => receivedArgs = x))
                .Returns(true);

            // Act
            var res = await _sut.Handle(
                new DeleteTodoCommand(id.ToString()),
                CancellationToken.None);

            // Assert
            receivedArgs.Should().BeEquivalentTo(todo, options =>
                options.Excluding(o => o.Id));
            res.Should()
                .Match<DeletedTodoResponse>((x) =>
                    x.Success == true &&
                    x.ErrorMessage == null
                );
        }

        [Theory]
        [InlineData("idshouldbeint")]
        public async void DeleteTodoItemHandler_ShouldReturnError_WhenItemToDeleteIsPresent(string id)
        {
            // Arrange

            // Act
            var res = await _sut.Handle(
                new DeleteTodoCommand(id),
                CancellationToken.None);

            // Assert
            res.Should()
                .Match<DeletedTodoResponse>((x) =>
                    x.Success == false &&
                    x.ErrorMessage.Errors.Contains($"Unable to delete, TodoId should be an Integer >0. Got: {id}")
                );
        }
    }
}