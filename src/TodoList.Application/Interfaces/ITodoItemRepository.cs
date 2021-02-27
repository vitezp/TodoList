using System.Collections.Generic;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface ITodoItemRepository
    {
        bool InsertTodoItem(TodoItem todoItem);
        TodoItem GetTodoItemById(int id);
        TodoItem GetTodoItemByName(string name);
        IEnumerable<TodoItem> GetAllTodoItems();
        bool UpdateTodoItem(TodoItem todoItem);
        bool DeleteTodoItem(TodoItem todoItem);
    }
}