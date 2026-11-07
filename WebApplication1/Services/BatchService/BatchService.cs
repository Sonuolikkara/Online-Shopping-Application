using System.Diagnostics;

namespace WebApplication1.Services
{
    public class BatchService : IBatchService
    {
        public async Task<(int ExitCode, string Output, string Error)> RunBatchFile()
        {
            var batchFilePath =
                @"C:\Users\sabuv\OneDrive\Desktop\BatchTask\TestTask.bat";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{batchFilePath}\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,

                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            return (
                process.ExitCode,
                output,
                error
            );
        }

        public async Task<(int ExitCode, string Output, string Error)> RunScheduledTask()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                Arguments = "/run /tn \"Test Batch Execution\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,

                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            return (
                process.ExitCode,
                output,
                error
            );
        }
    }
}