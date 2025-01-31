﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Infrastructure.Persistence;

namespace TodoList.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddSingleton<ITodoItemRepository, TodoItemRepository>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            var dbService = serviceProvider.GetService<ITodoItemRepository>()
                            ?? throw new ApplicationException("Unable to obtain DB from service collection ");
            foreach (var task in await dbService.GetAllTodoItems())
            {
                logger.LogDebug($"Selecting all tasks: {task}");
            }

            for (var i = 0; i < 34; i++)
            {
                await dbService.InsertTodoItem(new TodoItem() {Name = $"{i}", Priority = 100, Status = Status.InProgress});
            }

            var tasks = await dbService.GetAllTodoItems();
            int count = tasks.Count();
            logger.LogInformation($"There is {count} tasks in progress");

            var first = tasks.First(m => m.Name.Equals("12"));

            logger.LogInformation(first.ToString());


            System.Console.ReadKey();
        }
    }
}