using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Domain.Enums;
using TodoList.Domain.Extensions;

namespace TodoList.Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Column("Status")]
        public string StatusString
        {
            get => Status.ToString();
            init => Status = value.ParseEnum<Status>();
        }

        [NotMapped] 
        public Status Status { get; init; }

        public int Priority { get; init; }

        public override string ToString()
        {
            return $"Todo Item: {Environment.NewLine} Id:'{Id}', Name:'{Name}', Status:'{Status}', Priority:'{Priority}'";
        }
    }
}