using Microsoft.AspNetCore.Mvc;

namespace Altura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenderController : ControllerBase
    {        
        private readonly ILogger<TenderController> _logger;

        public TenderController(ILogger<TenderController> logger)
        {
            _logger = logger;
        }                

        [HttpPost(Name = "CreateBoard")]
        public async Task<ActionResult> Post(CancellationToken cancellationToken)
        {
            try
            {
                var trelloIntegration = new TrelloIntegration();

                var tenders = trelloIntegration.GetTenders();

                if (tenders.Any() is false)
                {
                    return NotFound("Tenders not found");
                }

                await trelloIntegration.GetCard(tenders, cancellationToken);                
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
