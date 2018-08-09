using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System.Reactive.Linq;
using Android.Support.V7.Widget;
using ReduxNET.Posts;
using Android.Support.Transitions;
using Android.Support.Constraints;
using ReduxNET.PostDetails;
using Android.Support.Design.Widget;
using System.Reactive.Disposables;
using ReduxNET.Extensions;
using ReduxNET.ActionCreators;
using System.Collections.Immutable;
using System.Threading;
using System.Reactive.Concurrency;

namespace ReduxNET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private FloatingActionButton _fab;
        private ConstraintLayout _container;
        private ProgressBar _loading;
        private Android.Widget.SearchView _search;
        private RecyclerView _posts;
        private PostsAdapter _adapter;
        private Snackbar _snackbar;

        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            _container = FindViewById<ConstraintLayout>(Resource.Id.container);
            _loading = FindViewById<ProgressBar>(Resource.Id.loading);

            _search = FindViewById<Android.Widget.SearchView>(Resource.Id.search);

            _posts = FindViewById<RecyclerView>(Resource.Id.posts);
            _posts.SetLayoutManager(new LinearLayoutManager(ApplicationContext, LinearLayoutManager.Vertical, false));
            _adapter = new PostsAdapter();
            _posts.SetAdapter(_adapter);
        }

        protected override void OnStart()
        {
            base.OnStart();
            SetupEventHandlers();
            SetupSubscriptions();
        }
        
        protected override void OnStop()
        {
            base.OnStop();
            Disposables.Clear();
        }

        private void SetupEventHandlers()
        {
            Observable.FromEventPattern(_fab, "Click")
                .Subscribe(e => App.App.Store.Dispatch(PostsActionsCreator.Fetch()))
                .DisposeWith(Disposables);

            Observable.FromEventPattern(_search, "QueryTextChange")
                .Select(e => e.EventArgs.ToString())
                .DistinctUntilChanged()
                .Subscribe(e => App.App.Store.Dispatch(PostsActionsCreator.SearchPosts(_search.Query)))
                .DisposeWith(Disposables);
        }

        private void SetupSubscriptions()
        {
            App.App.Store
                .Where(state => state.PostsState.FirstTime)
                .ManageThreading()
                .Subscribe(_ => App.App.Store.Dispatch(PostsActionsCreator.Fetch()))
                .DisposeWith(Disposables);

            App.App.Store
                .Select(state => state.PostsState.Loading)
                .DistinctUntilChanged()
                .ManageThreading()
                .Subscribe(Render)
                .DisposeWith(Disposables);

            App.App.Store
                .Select(Selectors.Selectors.SearchPosts)
                .DistinctUntilChanged()
                .ManageThreading()
                .Subscribe(Render)
                .DisposeWith(Disposables);

            App.App.Store
                .Select(state => state.PostsState.SelectedPostId)
                .DistinctUntilChanged()
                .Skip(1)
                .ManageThreading()
                .Subscribe(id => Render())
                .DisposeWith(Disposables);

            App.App.Store
                .Select(state => state.PostsState.Error)
                .DistinctUntilChanged()
                .ManageThreading()
                .Subscribe(Render)
                .DisposeWith(Disposables);
        }

        private void Render(bool loading)
        {
            TransitionManager.BeginDelayedTransition(_container);
            _loading.Visibility = loading ? ViewStates.Visible : ViewStates.Gone;
            _posts.Visibility = !loading ? ViewStates.Visible : ViewStates.Gone;
        }

        private void Render(ImmutableList<Post> list) => _adapter.UpdateItems(list);

        private void Render(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                if (_snackbar == null) return;
                if (_snackbar.IsShownOrQueued) _snackbar.Dismiss();
                return;
            }

            if (_snackbar == null)
                _snackbar = Snackbar.Make(_container, error, Snackbar.LengthIndefinite);
            else _snackbar.SetText(error);

            _snackbar.Show();
        }

        private void Render() => StartActivity(new Android.Content.Intent(this, typeof(PostDetailsActivity)));
    }
}