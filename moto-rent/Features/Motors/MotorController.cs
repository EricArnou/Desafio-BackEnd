using Microsoft.AspNetCore.Mvc;
using moto_rent.Services;
using moto_rent.Infraestructure.Exceptions;
using moto_rent.Features.Motors.DTOs;

namespace moto_rent.Controllers
{
    [ApiController]
    [Route("/motos")]
    public class MotorsController : ControllerBase
    {
        private readonly MotorService _service;
        private readonly ILogger<MotorsController> _logger;

        public MotorsController(MotorService service, ILogger<MotorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorMotorById(string id)
        {
            try
            {
                var motor = await _service.GetMotorByIdAsync(id);
                return Ok(motor);
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid request for motor with id {Id}", id);
                return BadRequest(new Message("Request mal formada"));
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError("Motor not found with id {Id}", id);
                return NotFound(new Message("Moto não encontrada"));
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
            try
            {
                await _service.CreateMotorAsync(motor);
                return Created();
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid data for motor creation: {Motor}", motor);
                return BadRequest(new Message("Dados inválidos"));
            }
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdateMotor(string id, [FromBody] UpdateMotorDto motor)
        {

            try
            {
                await _service.UpdateMotorAsync(id, motor.placa);
                return Ok(new Message("Placa modificada com sucesso"));
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid data for motor update: {Motor}", motor);
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError("Motor not found with id {Id}", id);
                return NotFound(new Message("Moto não encontrada"));
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotor(string id)
        {
            try
            {
                await _service.DeleteMotorAsync(id);
                return Ok();
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid data for motor deletion: {Id}", id);
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError("Motor not found with id {Id}", id);
                return NotFound(new Message("Moto não encontrada"));
            }
        }
    }
}
