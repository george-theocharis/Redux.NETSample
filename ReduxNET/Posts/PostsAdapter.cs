using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Core.ActionCreators;

namespace ReduxNET.Posts
{
    public class PostsAdapter : RecyclerView.Adapter
    {
        private ImmutableList<DomainF.Posts.Post> Items { get; set; }

        public PostsAdapter()
            =>  Items = ImmutableList<DomainF.Posts.Post>.Empty;

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
         =>  (holder as PostViewHolder)?.Bind(Items[position]);

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            => new PostViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_post, parent, false));

        public void UpdateItems(ImmutableList<DomainF.Posts.Post> posts)
        {
            Items = posts;
            NotifyDataSetChanged();
        }
    }

    internal class PostViewHolder : RecyclerView.ViewHolder
    {
        private TextView Title { get; }
        private TextView Body { get; }
        private DomainF.Posts.Post Post { get; set; }

        public PostViewHolder(View itemView) : base(itemView)
        {
            Title = ItemView.FindViewById<TextView>(Resource.Id.title);
            Body = ItemView.FindViewById<TextView>(Resource.Id.msg);

            Observable.FromEventPattern(itemView, "Click")
                      .Subscribe(_ => Core.Domain.App.App.Store.Dispatch(PostsActionsCreator.SelectPost(Post.Id)));
        }

        internal void Bind(DomainF.Posts.Post post)
        {
            Title.Text = post.Title;
            Body.Text = post.Body;
            Post = post;
        }
    }
}