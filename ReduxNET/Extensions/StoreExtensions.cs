using System;
using Redux;

namespace ReduxNET.Extensions
{
    public delegate void AsyncActionsCreator<in TState>(Dispatcher dispatcher, Func<TState> getState);

    public static class StoreExtensions
    {
        public static void Dispatch<TState>(this IStore<TState> store, AsyncActionsCreator<TState> actionsCreator)
        {
             actionsCreator(store.Dispatch, store.GetState);
        }
    }
}