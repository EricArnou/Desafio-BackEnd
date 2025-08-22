using Microsoft.AspNetCore.Mvc;
using moto_rent.Features.Rentals.Services;
using moto_rent.Infraestructure.Exceptions;
using moto_rent.Features.Riders.DTOs;

namespace moto_rent.Features.Rentals.Controllers
{
    [ApiController]
    [Route("/locacao")]
    public class RentalController : ControllerBase
    {
        private readonly RentalService _rentalService;

        public RentalController(RentalService rentalService)
        {
            _rentalService = rentalService;
        }

        /// <summary>
        /// Consulta uma locação pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(string id)
        {
            try
            {
                var rental = await _rentalService.GetRentalByIdAsync(id);
                return Ok(rental);
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }

            catch (KeyNotFoundException)
            {
                return NotFound(new Message("Locação não encontrada"));
            }
        }

        /// <summary>
        /// Cria uma nova locação
        /// </summary>
        /// <param name="rental"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalDto rental)
        {
            try
            {
                await _rentalService.CreateRentalAsync(rental);
                return Created();
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }
        }

        /// <summary>
        /// Informa a data de devolução de uma locação existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rental"></param>
        /// <returns></returns>

        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> UpdateRental(string id, [FromBody] UpdateRentalDto rental)
        {
            try
            {
                await _rentalService.UpdateRentalAsync(id, rental);
                return Ok(new Message("Data de devolução informada com sucesso"));
            }
            catch (ArgumentException)
            {
                return BadRequest(new Message("Dados inválidos"));
            }
        }
    }
}
