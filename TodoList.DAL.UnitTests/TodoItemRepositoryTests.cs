using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TodoList.DAL.Models;
using Xunit;

namespace TodoList.DAL.Tests
{
    public class TodoItemRepositoryTests
    {
        //consider testing interface here
        private readonly TodoItemRepository _sut;

        public TodoItemRepositoryTests()
        {
            _sut = new TodoItemRepository(new NullLogger<TodoItemRepository>());
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task Name éíáýžťčšľ", 50, Status.Completed)]
        [InlineData("Test Name", 0, Status.NotStarted)]
        [InlineData("Test Name", 99, null)]
        public void InsertItem_ShouldCreateItem_WhenAllParametersAreValid(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem() {Name = name, Priority = priority, Status = status};

            // Act
            var rowsAff = _sut.InsertTodoItem(item);
            var created = _sut.GetAllTodoItems();
            var first = created.First(m => m.Name == item.Name);

            // Assert
            rowsAff.Should().Be(1);
            first.Should().BeEquivalentTo(item, options =>
                options.Excluding(o => o.Id));
        }

        [Theory]
        [InlineData(null, 10, Status.InProgress)]
        [InlineData("Test Name", -10, Status.InProgress)]
        [InlineData("Test Name", 110, Status.InProgress)]
        public void InsertItem_ShouldNotCreateItem_WhenParametersAreInvalid(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem() {Name = name, Priority = priority, Status = status};

            // Act
            var rowsAff = _sut.InsertTodoItem(item);

            // Assert
            rowsAff.Should().Be(0);
            var createdCount = _sut.GetAllTodoItems().Count();
            createdCount.Should().Be(2);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        public void InsertItem_ShouldNotCreateItem_WhenItemWithTheSameNameExists(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem {Name = name, Priority = priority, Status = status};

            // Act
            var rowsAff = _sut.InsertTodoItem(item);

            // Assert
            rowsAff.Should().Be(1);
            rowsAff = _sut.InsertTodoItem(item);
            rowsAff.Should().Be(0);

            var createdCount = _sut.GetAllTodoItems().Count();
            createdCount.Should().Be(3);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task Name éíáýžťčšľ", 50, Status.Completed)]
        [InlineData("Test Name", 0, Status.NotStarted)]
        [InlineData("Test Name", 99, null)]
        public void UpdateItem_ShouldUpdateItem_WhenAllParametersAreValid(string name, int priority,
            Status status)
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;
            var item = new TodoItem {Id = origin.Id, Name = name, Priority = priority, Status = status};

            // Act
            var rowsAff = _sut.UpdateTodoItem(item);

            // Assert
            var created = _sut.GetAllTodoItems();
            var selected = created.First(m => m.Name == item.Name);
            rowsAff.Should().Be(1);
            selected.Should().BeEquivalentTo(item);
        }

        [Theory]
        [InlineData(null, 10, Status.InProgress)]
        [InlineData("Test Name", -10, Status.InProgress)]
        [InlineData("Test Name", 110, Status.InProgress)]
        public void UpdateItem_ShouldNotUpdateItem_WhenParametersAreInvalid(string name, int priority,
            Status status)
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;
            var item = new TodoItem {Id = origin.Id, Name = name, Priority = priority, Status = status};

            // Act
            var rowsAff = _sut.UpdateTodoItem(item);

            // Assert
            rowsAff.Should().Be(0);
            var createdCount = _sut.GetAllTodoItems();
            createdCount.First(m => m.Name == origin.Name).Should().BeEquivalentTo(origin);
        }

        [Theory]
        [InlineData(2, "Task Name", 10, Status.InProgress)]
        public void UpdateItem_ShouldNotUpdateItem_WhenItemWithTheSameNameExists(int id, string name, int priority,
            Status status)
        {
            _sut.InsertTodoItem(new TodoItem {Name = "Task Exists"});

            var item = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            var rowsAff = _sut.InsertTodoItem(item);
            rowsAff.Should().Be(1);
            item.Name = "Task Exists";
            rowsAff = _sut.UpdateTodoItem(item);
            rowsAff.Should().Be(0);
        }

        [Fact]
        public void DeleteItem_ShouldDeleteItem_WhenTheSpecifiedItemExists()
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;

            // Act
            var rowsAff = _sut.DeleteTodoItem(origin);

            // Assert
            rowsAff.Should().Be(1);
            var created = _sut.GetAllTodoItems();
            created.Where(m => m.Name == origin.Name).Should().HaveCount(0);
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