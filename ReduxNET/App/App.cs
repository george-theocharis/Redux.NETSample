using Android.App;
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


        public override void OnCreate()
        {
            base.OnCreate();

        }
    }
}