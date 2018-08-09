using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Posts;

namespace Core.Network
{
    public interface IFakeApi
    {
        [Get("/posts")]
        Task<ApiResponse<List<Post>>> GetPosts();
    }
}