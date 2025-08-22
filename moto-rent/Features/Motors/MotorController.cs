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

        public MotorsController(MotorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Consulta uma moto pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Detalhes da moto
        /// </returns>
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
                return BadRequest(new Message("Request mal formada"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Moto não encontrada"));
            }
        }

        /// <summary>
        /// Consulta todas as motos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMotors()
        {
            var motors = await _service.GetAllMotorsAsync();
            return Ok(motors);
        }

        /// <summary>
        /// Cadastra uma nova moto
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
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
                return BadRequest(new Message("Dados inválidos"));
            }
        }

        /// <summary>
        /// Modifica a placa de uma moto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="motor"></param>
        /// <returns></returns>
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
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Moto não encontrada"));
            }

        }

        /// <summary>
        /// Deleta uma moto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return BadRequest(new Message("Dados inválidos"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Moto não encontrada"));
            }
        }
    }
}
