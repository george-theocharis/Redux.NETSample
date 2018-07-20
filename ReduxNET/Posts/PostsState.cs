using ReduxNET.Posts;
using System.Collections.Generic;

namespace Redux
{
    public class PostsState
    {
        public PostsState()
        {
            Posts = new List<Post>();
        }

        private PostsState(bool loading, List<Post> posts, int selectedPostId)
        {
            Posts = posts;
            Loading = loading;
            SelectedPostId = selectedPostId;
        }

        public PostsState With(bool? Loading = null, List<Post> Posts = null, int? SelectedPostId = null)
           =>  new PostsState
               (
                   Loading ?? this.Loading,
                   Posts ?? this.Posts,
                   SelectedPostId ?? this.SelectedPostId
               );

        public List<Post> Posts { get; }
        public bool Loading { get; }
        public int SelectedPostId { get; }
    }
}