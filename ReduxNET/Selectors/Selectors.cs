using Redux.App;
using ReduxNET.Posts;
using System;
using System.Linq;

namespace ReduxNET.Selectors
{
    public static class Selectors
    {
        public static Func<AppState, Post> GetPostById
            => x => x.PostsState.Posts.SingleOrDefault(p => p.Id.Equals(x.PostsState.SelectedPostId));
    }
}