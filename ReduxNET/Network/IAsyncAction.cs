using Redux;

namespace ReduxNET.Network
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