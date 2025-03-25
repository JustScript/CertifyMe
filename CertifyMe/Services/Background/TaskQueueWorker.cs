namespace CertifyMe.Services
{
    public class TaskQueueWorker : BackgroundService
    {
        private readonly ILogger<TaskQueueWorker> _logger;

        private readonly ITaskQueueService _taskQueue;

        public TaskQueueWorker(ILogger<TaskQueueWorker> logger, ITaskQueueService taskQueue)
        {
            _logger = logger;
            _taskQueue = taskQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background task queue worker started.");

                try
                {
                    var workItem = await _taskQueue.DequeueAsync(stoppingToken);
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BackgroundQueueWorker task failed.");
                }
            }
        }
    }
}