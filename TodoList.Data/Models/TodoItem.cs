using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Shared.Domain;
using TodoList.Shared.Extensions;

namespace TodoList.Data.Models
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
            internal init => Status = value.ParseEnum<Status>();
        }

        [NotMapped] 
        public Status Status { get; init; }

        public int Priority { get; init; }

        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return $"Todo Item: {Environment.NewLine} Id:'{Id}', Name:'{Name}', Status:'{Status}', Priority:'{Priority}'";
        }
    }
}