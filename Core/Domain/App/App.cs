
using Core.Network;
using Redux;
using Refit;

namespace Core.Domain.App
{
    public static class App
    {
        public static IStore<DomainF.App.AppState> Store { get; } = new Store<DomainF.App.AppState>(Reducers.AppReduce, new DomainF.App.AppState(new DomainF.Posts.PostsState(new DomainF.Posts.Post[0], false, null, 0, string.Empty )));
        public static IFakeApi Api { get; } = RestService.For<IFakeApi>("https://jsonplaceholder.typicode.com");
    }
}