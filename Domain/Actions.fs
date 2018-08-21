namespace Redux.Core

module Actions = 
    open System
    open Redux
    
    type Action =
        | PostsResult of Posts: Post array
        | PostsPending
        | PostsFailure of Error: Exception
        | SelectPost of Id: int
        | SearchPost of Query: string
        interface IAction

open Actions

 module ActionCreators = 
    let FetchPosts = Action.PostsPending
    let PostsFetched posts = Action.PostsResult(posts)
    let PostsFailed error = Action.PostsFailure(error)
    let PostSelected id = Action.SelectPost(id)
    let QueryChanged query = Action.SearchPost(query)

