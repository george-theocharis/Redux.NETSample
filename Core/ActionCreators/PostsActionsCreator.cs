using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Core.Extensions;
using DomainF;
using Redux;

namespace Core.ActionCreators
{
    public static class PostsActionsCreator
    {
        private static IAction FetchPosts => Actions.Action.PostsPending;

        private static IAction PostsFetched(IEnumerable<Posts.Post> posts) =>
            Actions.Action.NewPostsResult(posts.ToArray());

        private static IAction PostsFetchFailed(HttpStatusCode statusCode, string reasonPhrase) =>
            Actions.Action.NewPostsFailure(new Exception(reasonPhrase));

        public static IAction SelectPost(int id) => Actions.Action.NewSelectPost(id);
        public static IAction DeselectPost => Actions.Action.NewSelectPost(0);

        public static AsyncActionsCreator<App.AppState> Fetch()
        {
            return (dispatch, getState) =>
                Observable.Return(dispatch(FetchPosts))
                    .StartWith()
                    .SelectMany(async _ => await Domain.App.App.Api.GetPosts())
                    .Where(response => response.IsSuccessStatusCode)
                    .Select(response => response.Content)
                    .SubscribeOn(TaskPoolScheduler.Default)
                    .Subscribe(posts => dispatch(PostsFetched(posts)),
                        error => dispatch(PostsFetchFailed(HttpStatusCode.ExpectationFailed, error.Message)));
        }
    }
}