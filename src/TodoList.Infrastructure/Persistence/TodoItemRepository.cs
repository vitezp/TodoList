using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;

namespace TodoList.Infrastructure.Persistence
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
Name TEXT NOT NULL UNIQUE COLLATE NOCASE, 
Status TEXT, 
Priority INTEGER DEFAULT 0 CHECK (Priority >= 0 AND Priority <= 100));";

            using var cmd = new SqliteCommand(createItem, _connection);

            if (cmd.ExecuteNonQuery() != 0)
            {
                throw new ApplicationException("Unable to initialize database");
            }

            _log.LogInformation("Database initialized with table 'TodoItem'");
            
            //Dummy seed to some initial data
            InsertTodoItem(new TodoItem()
                {Name = "Send CV to the Barclays", Priority = 70, Status = Status.Completed});
            InsertTodoItem(new TodoItem()
                {Name = "Implement the coding task", Priority = 60, Status = Status.InProgress});
        }


        public IEnumerable<TodoItem> GetAllTodoItems()
        {
            const string getItems = @"SELECT * FROM TodoItem;";

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

        public TodoItem GetTodoItemById(int id)
        {
            const string getItems = @"SELECT * FROM TodoItem WHERE Id = @Id;";

            using var cmd = new SqliteCommand(getItems, _connection);
            cmd.Parameters.AddWithValue("@Id", id);

            return ExecuteReader(cmd);
        }

        public TodoItem GetTodoItemByName(string name)
        {
            const string getItems = @"SELECT * FROM TodoItem WHERE Name = @Name COLLATE NOCASE;";

            using var cmd = new SqliteCommand(getItems, _connection);
            cmd.Parameters.AddWithValue("@Name", name);

            return ExecuteReader(cmd);
        }

        public bool InsertTodoItem(TodoItem todoItem)
        {
            //could be used Insert On Duplicate Update but separating makes sense
            const string insertItem =
                @"INSERT INTO TodoItem (Name, Status, Priority) VALUES (@Name, @Status, @Priority)";

            _log.LogInformation($"Inserting Todo item:'{todoItem}'");
            return ExecuteNonQuery(todoItem, insertItem, $"Error inserting todoItem: '{todoItem}'") > 0;
        }

        public bool UpdateTodoItem(TodoItem todoItem)
        {
            const string updateItems =
                @"UPDATE TodoItem SET Name = @Name, Status = @Status, Priority = @Priority WHERE Id = @Id;";

            _log.LogInformation($"Updating Todo item:'{todoItem}'");
            return ExecuteNonQuery(todoItem, updateItems, "Error updating todoItem: '{todoItem}") > 0;
        }

        public bool DeleteTodoItem(TodoItem todoItem)
        {
            const string deleteItems =
                @"DELETE FROM TodoItem WHERE Id = @Id;";

            _log.LogInformation($"Deleting Todo item with id:'{todoItem.Id}'");
            return ExecuteNonQuery(todoItem, deleteItems, $"Error deleting todoItem with id: '{todoItem.Id}'") > 0;
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
            // Id to update IsDeleted for Delete request
            cmd.Parameters.AddWithValue("@Id", todoItem.Id);
            try
            {
                _log.LogDebug($"Executing non query: '{cmd.CommandText}'");
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _log.LogError(e, errorMessage);
                return 0;
            }
        }

        private TodoItem ExecuteReader(SqliteCommand cmd)
        {
            using var reader = cmd.ExecuteReader();

            try
            {
                reader.Read();
            }
            catch (Exception e)
            {
                _log.LogError(e, $"Exception occured when executing query: '{cmd.CommandText}'");
                return null;
            }

            if (!reader.HasRows)
            {
                _log.LogError($"Unable to find todo item with query: '{cmd.CommandText}'");
                return null;
            }

            var item = new TodoItem()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                StatusString = reader.GetString(2),
                Priority = reader.GetInt32(3)
            };
            _log.LogInformation($"Reading todoItem: '{item}'");
            {
                return item;
            }
        }
    }
}