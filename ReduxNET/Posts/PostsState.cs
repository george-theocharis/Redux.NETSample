using ReduxNET.Posts;
using System.Collections.Immutable;

namespace Redux
{
    public class PostsState
    {
        public PostsState()
        {
            Posts = ImmutableList<Post>.Empty;
        }

        private PostsState(bool loading, ImmutableList<Post> posts, int selectedPostId, string error)
        {
            Posts = posts;
            Loading = loading;
            SelectedPostId = selectedPostId;
            Error = error;
        }

        public PostsState With(bool? Loading = null, ImmutableList<Post> Posts = null, int? SelectedPostId = null, string Error = null)
           => new PostsState
               (
                   Loading ?? this.Loading,
                   Posts ?? this.Posts,
                   SelectedPostId ?? this.SelectedPostId,
                   Error ?? this.Error
               );


        public ImmutableList<Post> Posts { get; }
        public bool Loading { get; }
        public int SelectedPostId { get; }
        public string Error { get; }
    }
}