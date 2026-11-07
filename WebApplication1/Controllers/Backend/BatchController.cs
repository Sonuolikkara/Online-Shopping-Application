using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpGet("run")]
        public async Task<IActionResult> RunBatch()
        {
            var result = await _batchService.RunBatchFile();

            if (result.ExitCode != 0)
            {
                return StatusCode(500, new
                {
                    message = "Batch file execution failed.",
                    exitCode = result.ExitCode,
                    output = result.Output,
                    error = result.Error
                });
            }

            return Ok(new
            {
                message = "Batch file executed successfully.",
                exitCode = result.ExitCode,
                output = result.Output
            });
        }

        [HttpPost("run-scheduled")]
        public async Task<IActionResult> RunScheduledTask()
        {
            var result = await _batchService.RunScheduledTask();

            if (result.ExitCode != 0)
            {
                return StatusCode(500, new
                {
                    message = "Scheduled task execution failed.",
                    exitCode = result.ExitCode,
                    output = result.Output,
                    error = result.Error
                });
            }

            return Ok(new
            {
                message = "Scheduled task triggered successfully.",
                exitCode = result.ExitCode,
                output = result.Output
            });
        }
    }
}