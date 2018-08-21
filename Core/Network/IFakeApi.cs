using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redux.Core;


namespace Core.Network
{
    public interface IFakeApi
    {
        [Get("/posts")]
        Task<ApiResponse<IEnumerable<Post>>> GetPosts();
    }
}