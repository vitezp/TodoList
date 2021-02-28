using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
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
    public class GetAllTodoItemHandlerTests
    {
        private readonly IRequestHandler<GetAllTodoCommand, IEnumerable<TodoResponse>> _sut;
        private readonly ITodoItemRepository _todoItemRepo;

        public GetAllTodoItemHandlerTests()
        {
            _todoItemRepo = Substitute.For<ITodoItemRepository>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _sut = new GetAllTodoItemHandler(new NullLogger<GetAllTodoItemHandler>(), _todoItemRepo, mapper);
        }

        [Theory]
        [InlineData(1324513, "Name", Status.Completed, 2)]
        [InlineData(50, "Item Name", Status.InProgress, 10)]
        [InlineData(50, "Item Name", Status.NotStarted, 99)]
        public async void GetTodoItemHandler_(int id, string name, Status status, int priority)
        {
            // Arrange
            var todo = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            _todoItemRepo.GetAllTodoItems()
                .Returns(new List<TodoItem> {todo});

            // Act
            var res = await _sut.Handle(
                new GetAllTodoCommand(),
                CancellationToken.None);

            // Assert
            res.Should().OnlyContain(m => m.Id == todo.Id.ToString()
                                                  && m.Name == todo.Name
                                                  && m.Priority == todo.Priority.ToString()
                                                  && m.Status == todo.Status.ToString());
        }
    }
}