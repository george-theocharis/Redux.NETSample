using Android.App;
using Redux.App;
using Redux;
using Android.Runtime;
using System;

namespace ReduxNET.App
{
    [Application]
    internal class App : Application
    {
        public App()
        {
        }

        protected App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public static IStore<AppState> Store { get; private set; } = new Store<AppState>(AppReducer.Reduce, new AppState());

    }
}