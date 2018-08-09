using System;
using System.Reactive.Disposables;

namespace Core.Extensions
{
    public static class DisposableExtensions
    {
        public static TDisposable DisposeWith<TDisposable>(this TDisposable observable, CompositeDisposable disposables) where TDisposable : class, IDisposable
        {
            if (observable != null)
                disposables.Add(observable);

            return observable;
        }
    }
}