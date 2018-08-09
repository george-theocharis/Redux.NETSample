using Redux;

namespace Core.Actions
{
    public enum Status
    {
        Pending,
        Success,
        Failure
    }

    public interface IAsyncAction : IAction
    {
        Status Status { get; }
    }
}