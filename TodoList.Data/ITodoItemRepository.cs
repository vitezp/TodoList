using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Data.Models;

namespace TodoList.Data
{
    public interface ITodoItemRepository
    {
        int InsertTodoItem(TodoItem todoItem);
        IEnumerable<TodoItem> GetAllTodoItems();
        int UpdateTodoItem(TodoItem todoItem);
        int DeleteTodoItem(TodoItem todoItem);
    }
}