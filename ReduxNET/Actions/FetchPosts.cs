using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using ReduxNET.Network;
using ReduxNET.Posts;

namespace ReduxNET.Actions
{
    public class FetchPosts : IAsyncAction
    {
        public FetchPosts(Status status) => Status = status;

        public FetchPosts(Status status, IEnumerable<Post> posts) : this(status)
        {
            Posts = posts.ToImmutableList();
        }

        public FetchPosts(Status status, HttpStatusCode statusCode, string reasonPhrase) : this(status)
        {
            StatusCode = statusCode;
            Reason = reasonPhrase;
        }

        public HttpStatusCode StatusCode { get; }
        public string Reason { get; }

        public Status Status { get; }
        public ImmutableList<Post> Posts { get; }
    }
}