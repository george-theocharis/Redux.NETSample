using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System.Reactive.Linq;
using Android.Support.V7.Widget;
using ReduxNET.Posts;
using ReduxNET.Actions;
using Android.Support.Transitions;
using Android.Support.Constraints;
using ReduxNET.PostDetails;
using Android.Support.Design.Widget;
using System.Reactive.Disposables;
using ReduxNET.Extensions;
using ReduxNET.ActionCreators;
using System.Linq;
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

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Snackbar _snackbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
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
            SetupSubscriptions();
            SetupEventHandlers();
            Start();
        }

        private void Start()
        {
            if (_adapter.ItemCount == 0)
                App.App.Store.Dispatch(PostsActionsCreator.Fetch());
        }

        private void SetupEventHandlers()
        {
            Observable.FromEventPattern(_fab, "Click")
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .Subscribe(e =>
                      {
                          if (_adapter.ItemCount > 0)
                              App.App.Store.Dispatch(new PostsFetched() { Posts = new System.Collections.Generic.List<Post>() });
                          else
                              App.App.Store.Dispatch(PostsActionsCreator.Fetch());
                      })
                      .DisposeWith(_disposables);

            Observable.FromEventPattern(_search, "QueryTextChange")
                      .Subscribe(e => App.App.Store.Dispatch(PostsActionsCreator.SearchPosts(_search.Query)))
                      .DisposeWith(_disposables);
        }

        private void SetupSubscriptions()
        {
            App.App.Store
                .Select(Selectors.Selectors.SearchPosts)
                .DistinctUntilChanged()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(list => Render(list))
                .DisposeWith(_disposables);

            App.App.Store
                .Select(state => state.PostsState.SelectedPostId)
                .DistinctUntilChanged()
                .Skip(1)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(id => Render(id))
                .DisposeWith(_disposables);

            App.App.Store
              .Select(state => state.PostsState.Error)
              .DistinctUntilChanged()
              .SubscribeOn(TaskPoolScheduler.Default)
              .ObserveOn(SynchronizationContext.Current)
              .Subscribe(error => Render(error))
              .DisposeWith(_disposables);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _disposables.Clear();
        }

        private void Render(ImmutableList<Post> list)
        {
            TransitionManager.BeginDelayedTransition(_container);
            _loading.Visibility = ViewStates.Gone;
            _posts.Visibility = ViewStates.Visible;

            _adapter.UpdateItems(list);
        }

        private void Render(int selectedPostId)
            => StartActivity(new Android.Content.Intent(this, typeof(PostDetailsActivity)));

        private void Render(string error)
        {
            if(string.IsNullOrEmpty(error))
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
    }
}

