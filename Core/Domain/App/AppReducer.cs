using Core.Domain.Posts;
using Redux;

namespace Core.Domain.App
{
    public static class AppReducer 
    {
        public static AppState Reduce(AppState previous, IAction action)
        {
            return new AppState
            (
                 PostsReducer.Reduce(previous.PostsState, action)
            );
        }
    }
}