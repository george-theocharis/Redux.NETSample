using Redux;
using Redux.App;
using ReduxNET.Actions;
using ReduxNET.Posts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReduxNET.Extensions;
using ReduxNET.Network;

namespace ReduxNET.ActionCreators
{
    public static class PostsActionsCreator
    {
        private static IAction FetchPosts => new FetchPosts(Status.Pending);

        private static IAction PostsFetched(IEnumerable<Post> posts) =>
            new FetchPosts
            (
                Status.Success,
                posts
            );

        private static IAction PostsFetchFailed(HttpStatusCode statusCode, string reasonPhrase)
            => new FetchPosts(Status.Failure, statusCode, reasonPhrase);

        public static IAction SearchPosts(string query) =>
            new SearchPosts
            {
                Query = query
            };

        public static AsyncActionsCreator<AppState> Fetch()
        {
            return (dispatch, getState) =>
                Observable.Return(dispatch(FetchPosts))
                    .StartWith()
                    .SelectMany(async _ => await Task.Run(async () => await App.App.Api.GetPosts()))
                    .Where(response => response.IsSuccessStatusCode)
                    .Select(response => response.Content)
                    .SubscribeOn(TaskPoolScheduler.Default)
                    .Subscribe(posts => dispatch(PostsFetched(posts)),
                        error => dispatch(PostsFetchFailed(HttpStatusCode.ExpectationFailed, error.Message)));
        }
    }
}