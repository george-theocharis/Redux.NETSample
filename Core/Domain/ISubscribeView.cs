namespace Core.Domain
{
    public interface ISubscribeView
    {
        void SetupSubscriptions();
        void Unsubscribe();
    }
}