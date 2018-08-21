using System;
using System.Collections.Immutable;
using System.Linq;
using DomainF;
using App = DomainF.App;

namespace Core.Selectors
{
    public static class Selectors
    {
        public static Func<App.AppState, Posts.Post> GetPostById
            => x => x.PostsState.Posts.SingleOrDefault(p => p.Id.Equals(x.PostsState.SelectedPostId));

        public static Func<App.AppState, ImmutableList<Posts.Post>> SearchPosts
            => x => string.IsNullOrEmpty(x.PostsState.Query) ? x.PostsState.Posts.ToImmutableList() : x.PostsState.Posts.Where(p => 
                p.Title.ToLower().Contains(x.PostsState.Query.ToLower()) || p.Body.ToLower().Contains(x.PostsState.Query.ToLower())
            ).ToImmutableList();
    }
}