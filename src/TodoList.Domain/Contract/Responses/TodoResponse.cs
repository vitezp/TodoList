namespace TodoList.Domain.Contract.Responses
{
    public class TodoResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public ErrorResponse ErrorResponse { get; set; }

        public TodoResponse()
        {
        }

        public TodoResponse(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
}