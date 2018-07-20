using ReduxNET;
using ReduxNET.Actions;

namespace Redux
{
    internal class PostsReducer
    {
        internal static PostsState Reduce(PostsState state, IAction action)
        {
            switch(action)
            {
                case FetchPosts _:
                    return state.With(Loading: true);

                case PostsFetched result:
                    return state.With(Loading: false, Posts: result.Posts);

                case PostSelected post:
                    return state.With(SelectedPostId: post.Id);
            }

            return state;
        }
    }
}