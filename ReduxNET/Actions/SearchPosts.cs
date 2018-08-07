using Redux;

namespace ReduxNET.Actions
{
    public class SearchPosts: IAction
    {
        public string Query { get; set; }
        
    }
}