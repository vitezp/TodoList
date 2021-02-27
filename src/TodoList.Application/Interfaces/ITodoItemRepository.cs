using System.Collections.Generic;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface ITodoItemRepository
    {
        int InsertTodoItem(TodoItem todoItem);
        IEnumerable<TodoItem> GetAllTodoItems();
        int UpdateTodoItem(TodoItem todoItem);
        int DeleteTodoItem(TodoItem todoItem);
    }
}