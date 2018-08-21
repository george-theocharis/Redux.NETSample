using System;
using System.Collections.Immutable;
using System.Reactive.Disposables;
using Android.OS;
using Android.Support.Constraints;
using Android.Support.Design.Widget;
using Android.Support.Transitions;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Core.Domain.Posts;
using Core.Extensions;
using ReduxNET.PostDetails;

namespace ReduxNET.Posts
{
    public class PostsView : Fragment, IPostsView
    {
        private ConstraintLayout _container;
        private ProgressBar _loading;
        private RecyclerView _posts;
        private PostsAdapter _adapter;
        private Snackbar _snackbar;
        
        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.content_main, container, false);
 
            _container = view.FindViewById<ConstraintLayout>(Resource.Id.container);
            _loading = view.FindViewById<ProgressBar>(Resource.Id.loading);

            _posts = view.FindViewById<RecyclerView>(Resource.Id.posts);
            _posts.SetLayoutManager(new LinearLayoutManager(Context.ApplicationContext, LinearLayoutManager.Vertical, false));
            _adapter = new PostsAdapter();
            _posts.SetAdapter(_adapter);
            
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            SetupSubscriptions();
        }

        public override void OnStop()
        {
            base.OnStop();
            Unsubscribe();
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

        private void Render(ImmutableList<DomainF.Posts.Post> list)
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

        private void Render(int id) => StartActivity(new Android.Content.Intent(Context.ApplicationContext, typeof(PostDetailsActivity)));
    }
}