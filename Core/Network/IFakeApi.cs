using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainF;

namespace Core.Network
{
    public interface IFakeApi
    {
        [Get("/posts")]
        Task<ApiResponse<List<Posts.Post>>> GetPosts();
    }
}