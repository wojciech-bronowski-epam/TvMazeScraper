using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.ApiServices;

namespace TvMazeScraper.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showsService;

        public ShowsController(IShowService showsService)
        {
            _showsService = showsService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetShowsWithCastAsync([FromQuery] int pageId, [FromQuery] int pageSize)
        {
            if (pageId < 1 || pageSize < 1)
            {
                return BadRequest();
            }
            var data = await _showsService.GetShowsWithActorsAsync(pageId, pageSize);
            if (!data.Any())
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
