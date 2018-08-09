using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using Core.Domain.Posts;
using Core.Network;

namespace Core.Actions
{
    internal class FetchPosts : IAsyncAction
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