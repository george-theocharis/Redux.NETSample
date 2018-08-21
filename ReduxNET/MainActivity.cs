using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Reactive.Linq;
using ReduxNET.Posts;
using System.Reactive.Disposables;
using Android.Widget;
using Core.Extensions;
using Redux.Core;

namespace ReduxNET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private SearchView _search;
        private CompositeDisposable Disposables { get; } = new CompositeDisposable();
        private const string Posts = "Posts";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _search = FindViewById<SearchView>(Resource.Id.search);
            
            if (savedInstanceState == null)
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.nav_host_container, new PostsView(), Posts)
                    .CommitNow();
        }

        protected override void OnStart()
        {
            base.OnStart();
            SetupEventHandlers();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Disposables.Clear();
        }

        private void SetupEventHandlers()
        {
            Observable.FromEventPattern(_search, "QueryTextChange")
                .Merge(Observable.FromEventPattern(_search, "QueryTextSubmit"))
                .Select(e => _search.Query)
                .DistinctUntilChanged()
                .Subscribe(query => Core.Domain.App.Store.Dispatch(ActionCreators.QueryChanged(query)))
                .DisposeWith(Disposables);
        }
    }
}