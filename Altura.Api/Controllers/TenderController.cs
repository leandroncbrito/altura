using Altura.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Altura.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderProcessor _tenderExtractor;
        private readonly ILogger<TenderController> _logger;

        public TenderController(ITenderProcessor tenderParser, ILogger<TenderController> logger)
        {
            _tenderExtractor = tenderParser;
            _logger = logger;
        }

        [HttpPost(Name = "CreateBoard")]
        public async Task<ActionResult> Post(CancellationToken cancellationToken)
        {
            try
            {
                var hasTenders = await _tenderExtractor.ExtractTendersFromCsv(cancellationToken);

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
                _logger.LogError("Trello error", ex);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error loading tenders");
            }

            return Ok();
        }
    }
}
