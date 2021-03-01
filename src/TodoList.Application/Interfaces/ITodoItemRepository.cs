using System.Collections.Generic;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    
    /// <summary>
    /// Represents storage of elements. It is completely decoupled from the system
    /// so there are numerous ways how to implement the persistence.
    /// </summary>
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Inserts a Todo item into the configured storage
        /// </summary>
        /// <param name="todoItem">Item to create</param>
        /// <returns>Creation succeeded or not</returns>
        bool InsertTodoItem(TodoItem todoItem);
        
        /// <summary>
        /// Gets Todo item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>todo item from the domain</returns>
        TodoItem GetTodoItemById(int id);
        
        /// <summary>
        /// Gets Todo item by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>todo item from the domain</returns>
        TodoItem GetTodoItemByName(string name);
        
        /// <summary>
        /// Gets all the todo items from the storage
        /// </summary>
        /// <returns>Collection of all Todo items</returns>
        IEnumerable<TodoItem> GetAllTodoItems();
        
        /// <summary>
        /// Updates the Todo item
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns>Update succeeded or not</returns>
        bool UpdateTodoItem(TodoItem todoItem);
        
        /// <summary>
        /// Deletes the todo item.
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns>Deletion succeeded or not</returns>
        bool DeleteTodoItem(TodoItem todoItem);
    }
}