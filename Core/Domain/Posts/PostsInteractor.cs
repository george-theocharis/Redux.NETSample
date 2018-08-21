using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Core.Extensions;
using Redux.Core;

namespace Core.Domain.Posts
{
    public static class PostsInteractor
    {
        public static IObservable<AppState> InitialFetch => App.Store
            .Where(state => state.PostsState.Posts.Length == 0)
            .DistinctUntilChanged()
            .Do(_ => App.Store.Dispatch(AsyncActionCreator.Fetch()))
            .ManageThreading();

        public static IObservable<bool> Loading => App.Store
            .Select(state => state.PostsState.Loading)
            .DistinctUntilChanged()
            .ManageThreading();

        public static IObservable<ImmutableList<Post>> Posts => App.Store
            .Select(Selectors.Selectors.SearchPosts)
            .DistinctUntilChanged()
            .ManageThreading();

        public static IObservable<int> SelectedPostId => App.Store
            .Select(state => state.PostsState.SelectedPostId)
            .Where(id => id > 0)
            .ManageThreading();

        public static IObservable<string> Error => App.Store
            .Select(state => state.PostsState.Error?.Message)
            .DistinctUntilChanged()
            .ManageThreading();
    }
}