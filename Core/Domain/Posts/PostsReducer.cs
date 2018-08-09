using System.Collections.Immutable;
using Core.Actions;
using Core.Network;
using Redux;

namespace Core.Domain.Posts
{
    internal static class PostsReducer
    {
        internal static PostsState Reduce(PostsState state, IAction action)
        {
            switch(action)
            {
                case FetchPosts asyncAction when asyncAction.Status == Status.Pending:
                    return state.With(true);

                case FetchPosts asyncAction when asyncAction.Status == Status.Success:
                    return state.With(false, asyncAction.Posts, Error: string.Empty);

                case FetchPosts asyncAction when asyncAction.Status == Status.Failure:
                    return state.With(false, ImmutableList<Post>.Empty, Error: asyncAction.Reason);

                case SelectPost post:
                    return state.With(SelectedPostId: post.Id);

                case SearchPosts search:
                    return state.With(SearchQuery: search.Query);
            }

            return state;
        }
    }
}