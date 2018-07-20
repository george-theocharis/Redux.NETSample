using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Redux;
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

namespace ReduxNET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private FloatingActionButton _fab;
        private ConstraintLayout _container;
        private ProgressBar _loading;
        private RecyclerView _posts;
        private PostsAdapter _adapter;

        private CompositeDisposable _disposables = new CompositeDisposable();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            _container = FindViewById<ConstraintLayout>(Resource.Id.container);
            _loading = FindViewById<ProgressBar>(Resource.Id.loading);

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
                App.App.Store.Dispatch(AsyncActionCreators.FetchPosts());
        }

        private void SetupEventHandlers()
        {
            Observable.FromEventPattern(_fab, "Click")
                      .Subscribe(e =>
                      {
                          if (_adapter.ItemCount > 0)
                              App.App.Store.Dispatch(new PostsFetched() { Posts = new System.Collections.Generic.List<Post>() });
                          else
                              App.App.Store.Dispatch(AsyncActionCreators.FetchPosts());
                      })
                      .DisposeWith(_disposables);
        }

        private void SetupSubscriptions()
        {
            App.App.Store
                .DistinctUntilChanged(state => state.PostsState.Posts)
                .Subscribe(state => Render(state.PostsState))
                .DisposeWith(_disposables);


            App.App.Store
                .DistinctUntilChanged(state => state.PostsState.SelectedPostId)
                .Skip(1)
                .Subscribe(state => Render(state.PostsState.SelectedPostId))
                .DisposeWith(_disposables);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _disposables.Clear();
        }

        private void Render(PostsState state)
        {
            TransitionManager.BeginDelayedTransition(_container);
            _loading.Visibility = state.Loading ? ViewStates.Visible : ViewStates.Gone;
            _posts.Visibility = state.Loading ? ViewStates.Gone : ViewStates.Visible;

            _adapter.UpdateItems(state.Posts);
        }

        private void Render(int selectedPostId)
        {
            StartActivity(new Android.Content.Intent(this, typeof(PostDetailsActivity)));
        }
    }
}

