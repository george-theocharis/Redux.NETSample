using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Core.ActionCreators;
using Core.Extensions;

namespace Core.Domain.Posts
{


    public static class PostsInteractor
    {
        public static IObservable<DomainF.App.AppState> InitialFetch => App.App.Store
                                                                    .Where(state => state.PostsState.Posts.Length == 0)
                                                                    .DistinctUntilChanged()
                                                                    .Do(_ => App.App.Store.Dispatch(PostsActionsCreator.Fetch()))
                                                                    .ManageThreading();

        public static IObservable<bool> Loading => App.App.Store
                                                          .Select(state => state.PostsState.Loading)
                                                          .DistinctUntilChanged()
                                                          .ManageThreading();

        public static IObservable<ImmutableList<DomainF.Posts.Post>> Posts => App.App.Store
                                                                        .Select(Selectors.Selectors.SearchPosts)
                                                                        .DistinctUntilChanged()
                                                                        .ManageThreading();

        public static IObservable<int> SelectedPostId => App.App.Store
                                                                .Select(state => state.PostsState.SelectedPostId)
                                                                .Where(id => id > 0)
                                                                .ManageThreading();

        public static IObservable<string> Error => App.App.Store
                                                          .Select(state => state.PostsState.Error?.Message)
                                                          .DistinctUntilChanged()
                                                          .ManageThreading();
    }
}