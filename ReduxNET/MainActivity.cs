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

namespace ReduxNET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ConstraintLayout _container;
        private ProgressBar _loading;
        private RecyclerView _posts;
        private PostsAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _container = FindViewById<ConstraintLayout>(Resource.Id.container);
            _loading = FindViewById<ProgressBar>(Resource.Id.loading);

            _posts = FindViewById<RecyclerView>(Resource.Id.posts);
            _posts.SetLayoutManager(new LinearLayoutManager(ApplicationContext, LinearLayoutManager.Vertical, false));
            _adapter = new PostsAdapter();
            _posts.SetAdapter(_adapter);

            App.App.Store.DistinctUntilChanged(state => state.PostsState.Posts).Subscribe(state => Render(state.PostsState));
            App.App.Store.DistinctUntilChanged(state => state.PostsState.SelectedPostId).Skip(1).Subscribe(state => Render(state.PostsState.SelectedPostId));
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_adapter.ItemCount == 0)
                App.App.Store.Dispatch(AsyncActionCreators.FetchPosts());
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

