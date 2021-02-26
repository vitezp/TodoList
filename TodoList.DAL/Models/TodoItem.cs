using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.DAL.Models
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

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Priority { get; init; }

        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return $"Todo Item: {Environment.NewLine} Id:'{Id}', Name:'{Name}', Status:'{Status}', Priority:'{Priority}'";
        }
    }
}