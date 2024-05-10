using Altura.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Altura.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderProcessor _tenderProcessor;
        private readonly ILogger<TenderController> _logger;

        public TenderController(ITenderProcessor tenderProcessor, ILogger<TenderController> logger)
        {
            _tenderProcessor = tenderProcessor;
            _logger = logger;
        }

        [HttpPost(Name = "ExtractTendersFromCsv")]
        public async Task<ActionResult> Post(CancellationToken cancellationToken)
        {
            try
            {
                var hasTenders = await _tenderProcessor.ExtractTendersFromCsv(cancellationToken);

                if (hasTenders is false)
                {
                    return NotFound("Tenders not found");
                }
            }
            catch (TaskCanceledException tcex)
            {
                _logger.LogWarning("Task cancelled", tcex);

                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request closed");
            }
            catch (Exception ex)
            {
                _logger.LogError("Unknown error", ex);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error loading tenders");
            }

            return Ok();
        }
    }
}
