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
using System.Collections.Immutable;
using Core.ActionCreators;
using Core.Domain.Posts;
using Core.Extensions;

namespace ReduxNET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IPostsView
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
            Unsubscribe();
        }

        private void SetupEventHandlers()
        {
            Observable.FromEventPattern(_fab, "Click")
                .Subscribe(e => Core.Domain.App.App.Store.Dispatch(PostsActionsCreator.Fetch()))
                .DisposeWith(Disposables);

            Observable.FromEventPattern(_search, "QueryTextChange")
                .Merge(Observable.FromEventPattern(_search, "QueryTextSubmit"))
                .Select(e => _search.Query)
                .DistinctUntilChanged()
                .Subscribe(e => Core.Domain.App.App.Store.Dispatch(PostsActionsCreator.SearchPosts(_search.Query)))
                .DisposeWith(Disposables);
        }

        public void InitialFetch() => PostsInteractor
            .InitialFetch
            .Subscribe()
            .DisposeWith(Disposables);

        public void Loading() => PostsInteractor
            .Loading
            .Subscribe(Render)
            .DisposeWith(Disposables);

        public void Posts() => PostsInteractor
            .Posts
            .Subscribe(Render)
            .DisposeWith(Disposables);

        public void SelectedPostId() => PostsInteractor
            .SelectedPostId
            .Subscribe(Render)
            .DisposeWith(Disposables);

        public void Error() => PostsInteractor
            .Error
            .Subscribe(Render)
            .DisposeWith(Disposables);

        public void SetupSubscriptions()
        {
            InitialFetch();
            Loading();
            Posts();
            SelectedPostId();
            Error();
        }

        public void Unsubscribe() => Disposables.Clear();

        private void Render(bool loading)
        {
            TransitionManager.BeginDelayedTransition(_container);
            _loading.Visibility = loading ? ViewStates.Visible : ViewStates.Gone;
            _posts.Visibility = !loading ? ViewStates.Visible : ViewStates.Gone;
        }

        private void Render(ImmutableList<Post> list)
        {
            TransitionManager.BeginDelayedTransition(_container, new ChangeBounds());
            _adapter.UpdateItems(list);
        }

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

        private void Render(int id) => StartActivity(new Android.Content.Intent(this, typeof(PostDetailsActivity)));
    }
}