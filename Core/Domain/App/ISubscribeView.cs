namespace Core.Domain.App
{
    public interface ISubscribeView
    {
        void SetupSubscriptions();
        void Unsubscribe();
    }
}