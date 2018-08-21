namespace Core.Domain.Posts
{
    public interface IPostsView : ISubscribeView
    {
        void InitialFetch();
        void Loading();
        void Posts();
        void SelectedPostId();
        void Error();
    }
}