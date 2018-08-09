using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace ReduxNET.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<TSource> ManageThreading<TSource>(this IObservable<TSource> observable) 
            =>   observable.SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(SynchronizationContext.Current);

        public static IObservable<TSource> ManageThreading<TSource>(this IObservable<TSource> observable, IScheduler backgroundScheduler)
            => observable.SubscribeOn(backgroundScheduler)
                .ObserveOn(SynchronizationContext.Current);
    }
}