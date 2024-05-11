using Altura.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Altura.Api.Controllers
{
    [Route("api/tenders")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderService _tenderService;
        private readonly ILogger<TenderController> _logger;

        public TenderController(ITenderService tenderService, ILogger<TenderController> logger)
        {
            _tenderService = tenderService;
            _logger = logger;
        }

        [HttpPost("parse")]
        public async Task<ActionResult> Post(CancellationToken cancellationToken)
        {
            try
            {
                var hasTenders = await _tenderService.ExtractTendersFromCsv(cancellationToken);

                if (hasTenders is false)
                {
                    return NotFound("Tenders not found");
                }
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning("Task cancelled", ex);

                return StatusCode(StatusCodes.Status499ClientClosedRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("General exception", ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok();
        }
    }
}
