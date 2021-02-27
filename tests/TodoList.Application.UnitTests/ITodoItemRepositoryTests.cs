using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Infrastructure.Persistence;
using Xunit;

namespace TodoList.Application.UnitTests
{
    public class TodoItemRepositoryTests
    {
        private readonly ITodoItemRepository _sut;

        public TodoItemRepositoryTests()
        {
            //We could replace the instantiation with any implementation of ITodoItemRepository, tests should still pass
            //ass they are decoupled from the Infrastructure
            _sut = new TodoItemRepository(new NullLogger<ITodoItemRepository>());
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
            var success = _sut.InsertTodoItem(item);
            var created = _sut.GetAllTodoItems();
            var first = created.First(m => m.Name == item.Name);

            // Assert
            success.Should().BeTrue();
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
            var success = _sut.InsertTodoItem(item);

            // Assert
            success.Should().BeFalse();
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
            var success = _sut.InsertTodoItem(item);

            // Assert
            success.Should().BeTrue();
            success = _sut.InsertTodoItem(item);
            success.Should().BeFalse();

            var createdCount = _sut.GetAllTodoItems().Count();
            createdCount.Should().Be(3);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task Name éíáýžťčšľ", 50, Status.Completed)]
        [InlineData("Test Name", 0, Status.NotStarted)]
        [InlineData("Test Name", 99)]
        public void UpdateItem_ShouldUpdateItem_WhenAllParametersAreValid(string name, int priority,
            Status status = default)
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;
            var item = new TodoItem {Id = origin.Id, Name = name, Priority = priority, Status = status};

            // Act
            var success = _sut.UpdateTodoItem(item);

            // Assert
            var created = _sut.GetAllTodoItems();
            var selected = created.First(m => m.Name == item.Name);
            success.Should().BeTrue();
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
            var success = _sut.UpdateTodoItem(item);

            // Assert
            success.Should().BeFalse();
            var createdCount = _sut.GetAllTodoItems();
            createdCount.First(m => m.Name == origin.Name).Should().BeEquivalentTo(origin);
        }

        [Theory]
        [InlineData(2, "Task Name", 10, Status.InProgress)]
        public void UpdateItem_ShouldNotUpdateItem_WhenItemWithTheSameNameExists(int id, string name, int priority,
            Status status)
        {
            // Arrange
            _sut.InsertTodoItem(new TodoItem {Name = "Task Exists"});
            var item = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            var success = _sut.InsertTodoItem(item);
            success.Should().BeTrue();
            item.Name = "Task Exists";

            // Act
            success = _sut.UpdateTodoItem(item);

            // Assert
            success.Should().BeFalse();
        }

        [Fact]
        public void DeleteItem_ShouldDeleteItem_WhenTheSpecifiedItemExists()
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;

            // Act
            var success = _sut.DeleteTodoItem(origin);

            // Assert
            success.Should().BeTrue();
            var created = _sut.GetAllTodoItems();
            created.Where(m => m.Name == origin.Name).Should().HaveCount(0);
        }

        [Fact]
        public void DeleteItem_ShouldNotDeleteItem_WhenTheSpecifiedItemNotExists()
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;

            // Act
            var success = _sut.DeleteTodoItem(new TodoItem {Id = origin.Id + 1});

            // Assert
            success.Should().BeFalse();
            var created = _sut.GetAllTodoItems();
            created.Where(m => m.Name == origin.Name).Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task123", 10)]
        [InlineData("Task")]
        public void GetItem_ShouldReturnItem_WhenSpecifiedItemExists(string name, int priority = 0,
            Status status = default)
        {
            // Arrange
            var origin = new TodoItem {Name = name, Priority = priority, Status = status};
            _sut.InsertTodoItem(origin);
            origin.Id = _sut.GetAllTodoItems().First(m => m.Name == origin.Name).Id;

            // Act
            var getById = _sut.GetTodoItemById(origin.Id);
            var getByName = _sut.GetTodoItemByName(origin.Name);

            // Assert
            getById.Should().BeEquivalentTo(getByName);
            getById.Should().BeEquivalentTo(origin);
        }

        [Fact]
        public void GetItem_ShouldNotReturnItem_WhenSpecifiedItemNotExists()
        {
            // Arrange
            const int arbitraryId = 12345;
            const string arbitraryName = "Non Existent Item Name";

            // Act
            var getById = _sut.GetTodoItemById(arbitraryId);
            var getByName = _sut.GetTodoItemByName(arbitraryName);

            // Assert
            getById.Should().BeNull();
            getById.Should().BeNull();
        }
    }
}