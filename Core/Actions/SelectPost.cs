using Redux;

namespace Core.Actions
{
    internal class SelectPost : IAction
    {
        public int Id { get; }

        public SelectPost(int id)
        {
            Id = id;
        }
    }
}