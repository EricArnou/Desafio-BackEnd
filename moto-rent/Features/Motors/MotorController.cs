// Controllers/MotorsController.cs
using Microsoft.AspNetCore.Mvc;
using moto_rent.Features.Motors;
using moto_rent.Services;
using moto_rent.Infraestructure.Exceptions;
using moto_rent.Features.Motors.DTOs;

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
            try
            {
                var motor = await _service.GetMotorByIdAsync(id);
                return Ok(motor);
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Request mal formada"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Moto não encontrado"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMotors()
        {
            var motors = await _service.GetAllMotorsAsync();
            return Ok(motors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotor([FromBody] MotorDto motor)
        {
            await _service.CreateMotorAsync(motor);
            return CreatedAtAction(nameof(GetMotor), new { id = motor.Id }, motor);
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdateMotor(string id, [FromBody] UpdateMotorDto motor)
        {

            try
            {
                await _service.UpdateMotorAsync(id, motor.LicensePlate);
                return Ok(new Message("Placa modificada com sucesso"));
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotor(string id)
        {
            try
            {
                await _service.DeleteMotorAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Dados inválidos"));
            }
        }
    }
}
