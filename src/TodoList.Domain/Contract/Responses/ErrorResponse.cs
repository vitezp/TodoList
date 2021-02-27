using System.Collections.Generic;

namespace TodoList.Domain.Contract.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(ErrorModel error)
        {
            Errors.Add(error);
        }

        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }

    public class ErrorModel
    {
        public string Message { get; set; }
    }
}