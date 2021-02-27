namespace TodoList.Domain.Contract.Requests
{
    public class TodoRequest
    {
        public string Status { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}