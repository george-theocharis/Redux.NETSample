using System.Collections.Immutable;

namespace Core.Domain.Posts
{
    public class PostsState
    {
        public PostsState()
        {
            Posts = ImmutableList<Post>.Empty;
            FirstTime = true;
        }

        private PostsState(bool loading, ImmutableList<Post> posts, int selectedPostId, string searchQuery, string error)
        {
            Posts = posts;
            Loading = loading;
            SelectedPostId = selectedPostId;
            SearchQuery = searchQuery;
            Error = error;
            FirstTime = false;
        }

        public PostsState With(bool? Loading = null, ImmutableList<Post> Posts = null, int? SelectedPostId = null, string SearchQuery = null, string Error = null)
            => new PostsState
            (
                Loading ?? this.Loading,
                Posts ?? this.Posts,
                SelectedPostId ?? this.SelectedPostId,
                SearchQuery ?? this.SearchQuery,
                Error
            );


        public ImmutableList<Post> Posts { get; }
        public bool Loading { get; }
        public int SelectedPostId { get; }
        public string SearchQuery { get; }
        public string Error { get; }
        public bool FirstTime { get; }
    }
}