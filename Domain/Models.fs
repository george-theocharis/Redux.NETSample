namespace Redux.Core

open Newtonsoft.Json

 type Post =
     { 
       [<JsonProperty(PropertyName = "id")>]
       Id: int
       [<JsonProperty(PropertyName = "userId")>]
       UserId: int
       [<JsonProperty(PropertyName = "title")>]
       Title: string 
       [<JsonProperty(PropertyName = "body")>]
       Body: string }
 
 type PostsState =
     { Posts : Post array
       Loading: bool
       Error: System.Exception
       SelectedPostId: int
       Query: string } 


 type AppState =
      { 
        PostsState: PostsState }


