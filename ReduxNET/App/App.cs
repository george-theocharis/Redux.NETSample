using Android.App;
using Redux.App;
using Redux;
using Android.Runtime;
using System;
using Refit;
using ReduxNET.Network;

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
        public static IFakeApi Api { get; private set; }

        public override void OnCreate()
        {
            base.OnCreate();

            Api = RestService.For<IFakeApi>("https://jsonplaceholder.typicode.com");
        }
    }
}