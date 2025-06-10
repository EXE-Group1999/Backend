using EXE201.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly ChatBotService _chatBotService;

        public ChatBotController(ChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _chatBotService.GetChatResponseAsync(request.Message);
                return Ok(new ChatResponse { Response = response });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        public class ChatRequest
        {
            [Required(ErrorMessage = "Message is required")]
            [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
            public string Message { get; set; }
        }

        public class ChatResponse
        {
            public string Response { get; set; }
        }
    }
}
