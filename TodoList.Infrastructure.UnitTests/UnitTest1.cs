using System.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TodoList.Data.Persistence;
using Xunit;

namespace TodoList.Infrastructure.UnitTests
{
    public class TodoItemRepositoryTests
    {
        private readonly TodoItemRepository _sut;

        public TodoItemRepositoryTests()
        {
            //We could replace the instantiation with any implementation of ITodoItemRepository, tests should still pass
            //ass they are decoupled from the Infrastructure
            _sut = new TodoItemRepository(new NullLogger<TodoItemRepository>());
        }
        
        [Fact]
        public void Dispose_ConnectionShouldBeClosed_WhenSutIsDisposed()
        {
            _sut.GetConnectionState().Should().Be(ConnectionState.Open);
            _sut.Dispose();
            _sut.GetConnectionState().Should().Be(ConnectionState.Closed);
        }
    }
}