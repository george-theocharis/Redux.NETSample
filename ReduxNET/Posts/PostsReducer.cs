using ReduxNET.Actions;
using ReduxNET.Posts;
using System.Collections.Immutable;
using ReduxNET.Network;

namespace Redux
{
    internal class PostsReducer
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

                case PostSelected post:
                    return state.With(SelectedPostId: post.Id);

                case SearchPosts search:
                    return state.With(SearchQuery: search.Query);
            }

            return state;
        }
    }
}