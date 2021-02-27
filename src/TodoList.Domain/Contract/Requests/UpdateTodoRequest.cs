namespace TodoList.Domain.Contract.Requests
{
    public class UpdateTodoRequest : TodoRequest
    {
        public string Id { get; set; }
    }
}