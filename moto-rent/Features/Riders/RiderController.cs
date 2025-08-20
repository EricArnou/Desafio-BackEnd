using Microsoft.AspNetCore.Mvc;
using moto_rent.Features.Riders.DTOs;
using moto_rent.Features.Riders.Services;
using moto_rent.Infraestructure.Exceptions;

namespace moto_rent.Features.Riders.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class RiderController : ControllerBase
    {
        private readonly RiderService _service;

        public RiderController(RiderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRider([FromBody] RiderDto dto)
        {
            try
            {
                var rider = await _service.CreateRiderAsync(dto);

                return Created();
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }
        }
    }
}
