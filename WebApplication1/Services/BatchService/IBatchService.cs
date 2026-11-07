namespace WebApplication1.Services
{
    public interface IBatchService
    {
        Task<(int ExitCode, string Output, string Error)> RunBatchFile();

        Task<(int ExitCode, string Output, string Error)> RunScheduledTask();
    }
}