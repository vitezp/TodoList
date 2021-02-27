using System.Collections.Generic;

namespace TodoList.Domain.Contract.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string error)
        {
            Errors.Add(error);
        }

        public List<string> Errors { get; } = new List<string>();
    }
}