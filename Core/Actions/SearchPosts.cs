using Redux;

namespace Core.Actions
{
    internal class SearchPosts: IAction
    {
        public string Query { get; }

        public SearchPosts(string query)
        {
            Query = query;
        }
    }
}