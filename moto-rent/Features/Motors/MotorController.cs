// Controllers/MotorsController.cs
using Microsoft.AspNetCore.Mvc;
using moto_rent.Features.Motors;
using moto_rent.Services;

namespace moto_rent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorsController : ControllerBase
    {
        private readonly MotorService _service;

        public MotorsController(MotorService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotor(string id)
        {
            var motor = await _service.GetMotorByIdAsync(id);
            return motor == null ? NotFound() : Ok(motor);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMotors()
        {
            var motors = await _service.GetAllMotorsAsync();
            return Ok(motors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotor([FromBody] Motor motor)
        {
            await _service.CreateMotorAsync(motor);
            return CreatedAtAction(nameof(GetMotor), new { id = motor.Id }, motor);
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdateMotor(string id, [FromBody] string licensePlate)
        {
            await _service.UpdateMotorAsync(id, licensePlate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotor(string id)
        {
            await _service.DeleteMotorAsync(id);
            return NoContent();
        }
    }
}
