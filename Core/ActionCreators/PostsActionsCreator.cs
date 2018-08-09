using Redux;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Core.Actions;
using Core.Domain.App;
using Core.Domain.Posts;
using Core.Extensions;

namespace Core.ActionCreators
{
    public static class PostsActionsCreator
    {
        private static IAction FetchPosts => new FetchPosts(Status.Pending);

        private static IAction PostsFetched(IEnumerable<Post> posts) => new FetchPosts(Status.Success, posts);

        private static IAction PostsFetchFailed(HttpStatusCode statusCode, string reasonPhrase) =>
            new FetchPosts(Status.Failure, statusCode, reasonPhrase);

        public static IAction SearchPosts(string query) => new SearchPosts(query);

        public static IAction SelectPost(int id) => new SelectPost(id);
        public static IAction DeselectPost => new SelectPost(0);

        public static AsyncActionsCreator<AppState> Fetch()
        {
            return (dispatch, getState) =>
                Observable.Return(dispatch(FetchPosts))
                    .StartWith()
                    .SelectMany(async _ => await Task.Run(async () => await App.Api.GetPosts()))
                    .Where(response => response.IsSuccessStatusCode)
                    .Select(response => response.Content)
                    .SubscribeOn(TaskPoolScheduler.Default)
                    .Subscribe(posts => dispatch(PostsFetched(posts)),
                        error => dispatch(PostsFetchFailed(HttpStatusCode.ExpectationFailed, error.Message)));
        }
    }
}