using Redux;

namespace ReduxNET.Actions
{
    internal class PostSelected : IAction
    {
        public int Id { get; set; }

        public PostSelected(int id)
        {
            Id = id;
        }
    }
}