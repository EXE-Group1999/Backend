using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FurnitureController : ControllerBase
    {
        private readonly IFurnitureService _furnitureService;

        public FurnitureController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        // POST: api/furniture
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFurnitureDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdFurniture = await _furnitureService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdFurniture.Id }, createdFurniture);
        }

        // GET: api/furniture
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FurnitureQueryParameters parameters)
        {
            var paginatedResult = await _furnitureService.GetAllAsync(parameters);
            return Ok(paginatedResult);
        }

        // GET: api/furniture/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var furniture = await _furnitureService.GetByIdAsync(id);
                return Ok(furniture);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/furniture/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateFurnitureDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _furnitureService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/furniture/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _furnitureService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
