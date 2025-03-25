using System.Threading.Channels;

namespace CertifyMe.Services
{
    public class TaskQueueService : ITaskQueueService
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();

        public void Enqueue(Func<CancellationToken, Task> workItem)
        {
            _queue.Writer.TryWrite(workItem);
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}