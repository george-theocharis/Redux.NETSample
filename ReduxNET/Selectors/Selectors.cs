using Redux.App;
using ReduxNET.Posts;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace ReduxNET.Selectors
{
    public static class Selectors
    {
        public static Func<AppState, Post> GetPostById
            => x => x.PostsState.Posts.SingleOrDefault(p => p.Id.Equals(x.PostsState.SelectedPostId));

        public static Func<AppState, ImmutableList<Post>> SearchPosts
            => x => string.IsNullOrEmpty(x.PostsState.SearchQuery) ? x.PostsState.Posts : x.PostsState.Posts.Where(p => 
                p.Title.ToLower().Contains(x.PostsState.SearchQuery.ToLower()) || p.Body.ToLower().Contains(x.PostsState.SearchQuery.ToLower())
            ).ToImmutableList();
    }
}