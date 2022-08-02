using Quartz;

[DisallowConcurrentExecution]
public class TimeJob : IJob
{
    private readonly ILogger<TimeJob> _logger;

    public TimeJob(ILogger<TimeJob> logger)
    {
        this._logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation(DateTime.Now.ToString());
        return Task.CompletedTask;
    }
}