using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Core.Extensions;
using Redux.Core;

namespace Core
{
    public static class AsyncActionCreator
    {
        public static AsyncActionsCreator<AppState> Fetch()
        {
            return (dispatch, getState) =>
                Observable.Return(dispatch(ActionCreators.FetchPosts))
                    .StartWith()
                    .SelectMany(async _ => await Domain.App.Api.GetPosts())
                    .Where(response => response.IsSuccessStatusCode)
                    .Select(response => response.Content)
                    .SubscribeOn(TaskPoolScheduler.Default)
                    .Subscribe(posts => dispatch(ActionCreators.PostsFetched(posts.ToArray())),
                        error => dispatch(ActionCreators.PostsFailed(error)));
        }
    }
}