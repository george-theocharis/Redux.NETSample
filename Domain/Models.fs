namespace DomainF

open Newtonsoft.Json

module Posts =

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


module App =
    open Posts

    type AppState =
         { 
           PostsState: PostsState }


