using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Redux.App;
using ReduxNET.Extensions;

namespace ReduxNET.Posts
{
    public static class PostsInteractor
    {
        public static IObservable<AppState> InitialFetch => App.App.Store
                                                                    .Where(state => state.PostsState.FirstTime)
                                                                    .ManageThreading();

        public static IObservable<bool> Loading => App.App.Store
                                                          .Select(state => state.PostsState.Loading)
                                                          .DistinctUntilChanged()
                                                          .ManageThreading();

        public static IObservable<ImmutableList<Post>> Posts => App.App.Store
                                                                        .Select(Selectors.Selectors.SearchPosts)
                                                                        .DistinctUntilChanged()
                                                                        .ManageThreading();

        public static IObservable<int> SelectedPostId => App.App.Store
                                                                .Select(state => state.PostsState.SelectedPostId)
                                                                .DistinctUntilChanged()
                                                                .Skip(1)
                                                                .ManageThreading();

        public static IObservable<string> Error => App.App.Store
                                                          .Select(state => state.PostsState.Error)
                                                          .DistinctUntilChanged()
                                                          .ManageThreading();
    }
}