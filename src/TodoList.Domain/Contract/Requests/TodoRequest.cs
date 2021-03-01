namespace TodoList.Domain.Contract.Requests
{
    public class TodoRequest
    {
        public string Status { get; set; } = "NotStarted";
        public string Name { get; set; }
        public string Priority { get; set; } = "0";
    }
}