using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdOrder = await _orderService.CreateAsync(dto);

            if (createdOrder == null)
                return StatusCode(500, "An error occurred while creating the order.");

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }


        /// <summary>
        /// Get a paginated list of orders.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<OrderDto>>> GetAll([FromQuery] OrderQueryParameters parameters)
        {
            var result = await _orderService.GetAllAsync(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific order by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
                return NotFound($"No orders found for user with ID {userId}");

            return Ok(orders);
        }
        /// <summary>
        /// Update the status of an existing order.
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status must not be empty.");

            try
            {
                await _orderService.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
