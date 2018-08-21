using System;
using Core.Network;
using Redux;
using Refit;
using Redux.Core;

namespace Core.Domain
{
    public static class App
    {
        public static IStore<AppState> Store { get; } = new Store<AppState>(Reducers.AppReduce, new AppState(new PostsState(Array.Empty<Post>(), false, null, 0, string.Empty )));
        public static IFakeApi Api { get; } = RestService.For<IFakeApi>("https://jsonplaceholder.typicode.com");
    }
}