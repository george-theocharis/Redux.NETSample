using ReduxNET.Posts;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReduxNET.Network
{
    public interface IFakeApi
    {
        [Get("/posts")]
        Task<List<Post>> GetPosts();
    }
}