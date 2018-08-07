using Redux;
using System.Net;

namespace ReduxNET.Actions
{
    public class PostsFetchFailed : IAction
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public PostsFetchFailed(HttpStatusCode statusCode, string reason)
        {
            StatusCode = statusCode;
            Message = reason;
        }
    }
}