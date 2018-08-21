module Reducers 

open Redux.Core
open Actions
open Redux

    
 let PostsReduce (state: PostsState, action: IAction) : PostsState =
     match downcast action : Action with
         | PostsPending _ -> { state with Loading = true }
         | PostsResult posts -> { state with Posts = posts; Loading = false; }
         | PostsFailure error -> { state with Loading = false; Error = error; }
         | SelectPost id -> { state with SelectedPostId = id; }
         | SearchPost query -> { state with Query = query; }

  let AppReduce (state: AppState, action: IAction) : AppState = { state with PostsState = PostsReduce(state.PostsState, action); }

