using Redux;
using Redux.App;
using ReduxNET.Actions;
using ReduxNET.Posts;
using System;
using System.Collections.Generic;
using System.Net;

namespace ReduxNET.ActionCreators
{
    public static class PostsActionsCreator
    {
        public static IAction FetchPosts => new FetchPosts();

        public static IAction PostsFetched(List<Post> posts) => 
            new PostsFetched {
                Posts = posts
            };

        internal static IAction PostsFetchFailed(HttpStatusCode statusCode, string reasonPhrase)
            => new PostsFetchFailed(statusCode, reasonPhrase);

        public static AsyncActionsCreator<AppState> Fetch()
        {
            return async (dispatch, getState) =>
            {
                dispatch(FetchPosts);
                try
                {
                    var response = await App.App.Api.GetPosts();

                    dispatch(response.IsSuccessStatusCode ? PostsFetched(response.Content) : PostsFetchFailed(response.StatusCode, response.ReasonPhrase));
                }

                catch(Exception e)
                {
                    dispatch(PostsFetchFailed(HttpStatusCode.ExpectationFailed, e.Message));
                }
            };
        }
    }
}