namespace Redux.App
{
    public class AppState
    {
        public AppState()
        {
            PostsState = new PostsState();
        }

        public PostsState PostsState { get; set; }
    }
}
