using Redux;

namespace Core.Network
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