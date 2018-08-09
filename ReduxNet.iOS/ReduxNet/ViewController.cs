using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reactive.Disposables;
using Core.Domain.Posts;
using Core.Extensions;
using UIKit;

namespace ReduxNet
{
    public partial class ViewController : UIViewController, IPostsView
    {
        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetupSubscriptions();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
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
        }

        private void Render(ImmutableList<Post> list)
        {
            Debug.WriteLine($"Count: {list.Count}");
        }

        private void Render(string error)
        {
        }

        private void Render(int id)
        {

        }
    }
}
