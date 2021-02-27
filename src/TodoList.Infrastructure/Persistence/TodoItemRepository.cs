using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;

namespace TodoList.Data.Persistence
{
    public class TodoItemRepository : IDisposable, ITodoItemRepository
    {
        private readonly SqliteConnection _connection;
        private readonly ILogger<ITodoItemRepository> _log;

        public TodoItemRepository(ILogger<ITodoItemRepository> logger)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() {DataSource = ":memory:"};
            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _connection.Open();
            _log = logger;
            InitialiseDatabase();
        }

        public ConnectionState GetConnectionState()
        {
            return _connection.State;
        }

        private void InitialiseDatabase()
        {
            const string createItem = @"CREATE TABLE IF NOT EXISTS TodoItem (
ID INTEGER PRIMARY KEY AUTOINCREMENT, 
Name TEXT NOT NULL UNIQUE, 
Status TEXT NOT NULL, 
Priority INTEGER NOT NULL CHECK (Priority >= 0 AND Priority <= 100), 
IsDeleted TINYINT NOT NULL DEFAULT 0);";

            using var cmd = new SqliteCommand(createItem, _connection);

            if (cmd.ExecuteNonQuery() != 0)
            {
                throw new ApplicationException("Unable to initialize database");
            }

            _log.LogInformation("Database initialized with table 'TodoItem'");
            InsertTodoItem(new TodoItem()
                {Name = "Send CV to the Barclays", Priority = 70, Status = Status.Completed});
            InsertTodoItem(new TodoItem()
                {Name = "Implement the coding taks", Priority = 60, Status = Status.InProgress});
        }

        public int InsertTodoItem(TodoItem todoItem)
        {
            //could be used Insert On Duplicate Update but separating makes sense
            const string insertItem =
                @"INSERT INTO TodoItem (Name, Status, Priority) VALUES (@Name, @Status, @Priority)";

            return ExecuteNonQuery(todoItem, insertItem, "Error inserting todoItem");
        }

        public int UpdateTodoItem(TodoItem todoItem)
        {
            const string updateItems =
                @"UPDATE TodoItem SET Name = @Name, Status = @Status, Priority = @Priority, IsDeleted = @IsDeleted WHERE Id = @Id;";

            return ExecuteNonQuery(todoItem, updateItems, "Error updating todoItem");
        }

        public IEnumerable<TodoItem> GetAllTodoItems()
        {
            const string getItems = @"SELECT * FROM TodoItem WHERE IsDeleted = 0;";

            using var cmd = new SqliteCommand(getItems, _connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var item = new TodoItem()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    StatusString = reader.GetString(2),
                    Priority = reader.GetInt32(3)
                };
                _log.LogDebug($"Reading todoItem: '{item}'");
                yield return item;
            }
        }

        public int DeleteTodoItem(TodoItem todoItem)
        {
            todoItem.IsDeleted = true;
            return UpdateTodoItem(todoItem);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _connection.Close();
        }

        private int ExecuteNonQuery(TodoItem todoItem, string updateItems, string errorMessage)
        {
            using var cmd = new SqliteCommand(updateItems, _connection);
            cmd.Parameters.AddWithValue("@Name", todoItem.Name);
            cmd.Parameters.AddWithValue("@Status", todoItem.Status);
            cmd.Parameters.AddWithValue("@Priority", todoItem.Priority);
            // Id for Update IsDeleted for Delete
            cmd.Parameters.AddWithValue("@Id", todoItem.Id);
            cmd.Parameters.AddWithValue("@IsDeleted", todoItem.IsDeleted);
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _log.LogError(errorMessage, e);
                return 0;
            }
        }
    }
}