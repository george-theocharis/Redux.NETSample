using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redux.App;
using ReduxNET.Posts;

namespace ReduxNET.Actions
{
    public static class AsyncActionCreators
    {
        public static AsyncActionsCreator<AppState> FetchPosts()
        {
            return async (dispatch, getState) =>
            {
                dispatch(new FetchPosts());

                var posts = await GetPosts();

                dispatch(new PostsFetched
                {
                    Posts = posts
                });
            };
        }

        private static async Task<List<Post>> GetPosts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");

                var posts = JsonConvert.DeserializeObject<List<Post>>(await response.Content.ReadAsStringAsync());

                return posts;
            }
        }
    }
}   