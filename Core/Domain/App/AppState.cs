using Core.Domain.Posts;

namespace Core.Domain.App
{
    public class AppState
    {
        public AppState()
        {
            PostsState = new PostsState();
        }

        public AppState(PostsState posts)
        {
            PostsState = posts;
        }

        public PostsState PostsState { get; }
    }
}
