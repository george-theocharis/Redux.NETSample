using System.Collections.Generic;
using Redux;
using ReduxNET.Posts;

namespace ReduxNET.Actions
{
    internal class PostsFetched : IAction
    {
        public List<Post> Posts { get; set; }
    }
}