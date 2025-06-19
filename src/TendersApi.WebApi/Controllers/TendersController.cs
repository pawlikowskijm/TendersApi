using MediatR;
using Microsoft.AspNetCore.Mvc;
using TendersApi.Application.Queries.GetTenders;

namespace TendersApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TendersController(ISender _sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTendersAsync([FromQuery] GetTendersQuery query, CancellationToken cancellationToken = default)
        {
            var response = await _sender.Send(query, cancellationToken);

            return Ok(response);
        }
    }
}
