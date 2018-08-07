using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using ReduxNET.Posts;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ReduxNET.PostDetails
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public  class PostDetailsActivity : AppCompatActivity
    {
        private TextView _title;
        private TextView _body;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_post_details);

            _title = FindViewById<TextView>(Resource.Id.title);
            _body = FindViewById<TextView>(Resource.Id.msg);

            using (var sub = App.App.Store.DistinctUntilChanged(state => state.PostsState.SelectedPostId)
                 .Select(Selectors.Selectors.GetPostById)
                 .Select(p => p)
                 .Subscribe(p => Render(p))) { }
        }

        private void Render(Post p)
        {
            _title.Text = p.Title;
            _body.Text = p.Body;
        }
    }
}