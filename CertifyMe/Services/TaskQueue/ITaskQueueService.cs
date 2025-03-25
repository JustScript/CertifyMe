namespace CertifyMe.Services
{
    public interface ITaskQueueService
    {
        void Enqueue(Func<CancellationToken, Task> workItem);
        
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}