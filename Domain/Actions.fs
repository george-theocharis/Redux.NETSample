namespace DomainF

module Actions = 
    open System
    open Posts
    open Redux
    
    type Action =
        | PostsResult of Posts: Post array
        | PostsPending
        | PostsFailure of Error: Exception
        | SelectPost of Id: int
        | SearchPost of Query: string
        interface IAction