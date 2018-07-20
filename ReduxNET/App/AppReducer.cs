namespace Redux.App
{
    public static class AppReducer 
    {
        public static AppState Reduce(AppState previous, IAction action)
        {
            return new AppState
            {
                PostsState = PostsReducer.Reduce(previous.PostsState, action)
            };
        }
    }
}