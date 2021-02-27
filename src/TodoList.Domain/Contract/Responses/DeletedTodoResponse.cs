namespace TodoList.Domain.Contract.Responses
{
    public class DeletedTodoResponse
    {
        public bool Success { get; }
        public ErrorResponse ErrorMessage { get; }

        public DeletedTodoResponse(string deleted)
        {
            ErrorMessage = new ErrorResponse(deleted);
        }

        public DeletedTodoResponse(bool success)
        {
            Success = success;
        }
    }
}