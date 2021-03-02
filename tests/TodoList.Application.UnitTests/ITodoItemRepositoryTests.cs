using System.Linq;
using System.Threading.Tasks;
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
        public async Task InsertItem_ShouldCreateItem_WhenAllParametersAreValid(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem() {Name = name, Priority = priority, Status = status};

            // Act
            var success = await _sut.InsertTodoItem(item);
            var created = await _sut.GetAllTodoItems();
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
        public async Task InsertItem_ShouldNotCreateItem_WhenParametersAreInvalid(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem {Name = name, Priority = priority, Status = status};

            // Act
            var success = await _sut.InsertTodoItem(item);

            // Assert
            success.Should().BeFalse();
            var items = await _sut.GetAllTodoItems();
            var createdCount = items.Count();
            createdCount.Should().Be(2);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        public async Task ReInsertItem_ShouldCreateItem_WhenItWasOnceDeleted(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem() {Name = name, Priority = priority, Status = status};
            var success = await _sut.InsertTodoItem(item);
            var todoItem = await _sut.GetTodoItemByName(item.Name);
            item.Id = todoItem.Id;
            success &= await _sut.DeleteTodoItem(item);

            // Act
            success &= await _sut.InsertTodoItem(item);
            var created = await _sut.GetTodoItemById(item.Id + 1);

            // Assert
            success.Should().BeTrue();
            created.Should().BeEquivalentTo(item, options =>
                options.Excluding(o => o.Id));
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        public async Task InsertItem_ShouldNotCreateItem_WhenTheSameExistsDifferentCase(string name, int priority,
            Status status)
        {
            // Arrange
            var item = new TodoItem() {Name = name, Priority = priority, Status = status};
            await _sut.InsertTodoItem(item);
            item.Name = item.Name.ToUpper();

            // Act
            var success = await _sut.InsertTodoItem(item);

            // Assert
            success.Should().BeFalse();
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task Name éíáýžťčšľ", 50, Status.Completed)]
        [InlineData("Test Name", 0, Status.NotStarted)]
        [InlineData("Test Name", 99)]
        public async Task UpdateItem_ShouldUpdateItem_WhenAllParametersAreValid(string name, int priority,
            Status status = default)
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            await _sut.InsertTodoItem(origin);
            var itemByName = await _sut.GetTodoItemByName(origin.Name);
            origin.Id = itemByName.Id;
            var item = new TodoItem {Id = origin.Id, Name = name, Priority = priority, Status = status};

            // Act
            var success = await _sut.UpdateTodoItem(item);

            // Assert
            var created = await _sut.GetAllTodoItems();
            var selected = created.First(m => m.Name == item.Name);
            success.Should().BeTrue();
            selected.Should().BeEquivalentTo(item);
        }

        [Theory]
        [InlineData(null, 10, Status.InProgress)]
        [InlineData("Test Name", -10, Status.InProgress)]
        [InlineData("Test Name", 110, Status.InProgress)]
        public async Task UpdateItem_ShouldNotUpdateItem_WhenParametersAreInvalid(string name, int priority,
            Status status)
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            await _sut.InsertTodoItem(origin);
            var itemByName = await _sut.GetTodoItemByName(origin.Name);
            origin.Id = itemByName.Id;
            var item = new TodoItem {Id = origin.Id, Name = name, Priority = priority, Status = status};

            // Act
            var success = await _sut.UpdateTodoItem(item);

            // Assert
            success.Should().BeFalse();
            var createdCount = await _sut.GetAllTodoItems();
            createdCount.First(m => m.Name == origin.Name).Should().BeEquivalentTo(origin);
        }

        [Theory]
        [InlineData(2, "Task Name", 10, Status.InProgress)]
        public async Task UpdateItem_ShouldNotUpdateItem_WhenItemWithTheSameNameExists(int id, string name,
            int priority,
            Status status)
        {
            // Arrange
            await _sut.InsertTodoItem(new TodoItem {Name = "Task Exists"});
            var item = new TodoItem {Id = id, Name = name, Priority = priority, Status = status};
            var success = await _sut.InsertTodoItem(item);
            success.Should().BeTrue();
            item.Name = "Task Exists";

            // Act
            success = await _sut.UpdateTodoItem(item);

            // Assert
            success.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteItem_ShouldDeleteItem_WhenTheSpecifiedItemExists()
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            await _sut.InsertTodoItem(origin);
            var itemByName = await _sut.GetTodoItemByName(origin.Name);
            origin.Id = itemByName.Id;

            // Act
            var success = await _sut.DeleteTodoItem(origin);

            // Assert
            success.Should().BeTrue();
            var created = await _sut.GetAllTodoItems();
            created.Where(m => m.Name == origin.Name).Should().HaveCount(0);
        }

        [Fact]
        public async Task DeleteItem_ShouldNotDeleteItem_WhenTheSpecifiedItemNotExists()
        {
            // Arrange
            var origin = new TodoItem {Name = "TodoTask"};
            await _sut.InsertTodoItem(origin);
            var itemByName = await _sut.GetTodoItemByName(origin.Name);
            origin.Id = itemByName.Id;

            // Act
            var success = await _sut.DeleteTodoItem(new TodoItem {Id = origin.Id + 1});

            // Assert
            success.Should().BeFalse();
            var created = await _sut.GetAllTodoItems();
            created.Where(m => m.Name == origin.Name).Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Task Name", 10, Status.InProgress)]
        [InlineData("Task123", 10)]
        [InlineData("Task")]
        public async Task GetItem_ShouldReturnItem_WhenSpecifiedItemExists(string name, int priority = 0,
            Status status = default)
        {
            // Arrange
            var origin = new TodoItem {Name = name, Priority = priority, Status = status};
            await _sut.InsertTodoItem(origin);
            var itemByName = await _sut.GetTodoItemByName(origin.Name);
            origin.Id = itemByName.Id;

            // Act
            var getById = await _sut.GetTodoItemById(origin.Id);
            var getByName = await _sut.GetTodoItemByName(origin.Name);

            // Assert
            getById.Should().BeEquivalentTo(getByName);
            getById.Should().BeEquivalentTo(origin);
        }

        [Fact]
        public async Task GetItem_ShouldNotReturnItem_WhenSpecifiedItemNotExists()
        {
            // Arrange
            const int arbitraryId = 12345;
            const string arbitraryName = "Non Existent Item Name";

            // Act
            var getById = await _sut.GetTodoItemById(arbitraryId);
            var getByName = _sut.GetTodoItemByName(arbitraryName);

            // Assert
            getById.Should().BeNull();
            getById.Should().BeNull();
        }
    }
}