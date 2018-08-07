using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ReduxNET.Actions;

namespace ReduxNET.Posts
{
    public class PostsAdapter : RecyclerView.Adapter
    {
        public ImmutableList<Post> Items { get; private set; }

        public PostsAdapter()
            =>  Items = ImmutableList<Post>.Empty;

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
         =>   (holder as PostViewHolder)?.Bind(Items[position]);

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            => new PostViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_post, parent, false));

        public void UpdateItems(ImmutableList<Post> posts)
        {
            Items = posts;
            NotifyDataSetChanged();
        }
    }

    internal class PostViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Body { get; set; }

        public Post Post { get; private set; }

        public PostViewHolder(View itemView) : base(itemView)
        {
            Title = ItemView.FindViewById<TextView>(Resource.Id.title);
            Body = ItemView.FindViewById<TextView>(Resource.Id.msg);

            Observable.FromEventPattern(itemView, "Click")
                .Subscribe(_ => App.App.Store.Dispatch(new PostSelected(Post.Id)));
        }

        internal void Bind(Post post)
        {
            Post = post;
            Title.Text = post.Title;
            Body.Text = post.Body;
        }
    }
}