using Core.Network;
using Redux;
using Refit;

namespace Core.Domain.App
{
    public static class App
    {
        public static IStore<AppState> Store { get; } = new Store<AppState>(AppReducer.Reduce, new AppState());
        public static IFakeApi Api { get; } = RestService.For<IFakeApi>("https://jsonplaceholder.typicode.com");
    }
}