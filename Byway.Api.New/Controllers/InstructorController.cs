using Byway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Api.New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : Controller
    {

        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> CreateInstructor([FromForm] Byway.Application.DTOs.CreateInstructorDto instructorDto)
        {
            var createdInstructor = await _instructorService.CreateInstructorAsync(instructorDto);
            return Ok(createdInstructor);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, [FromForm] Byway.Application.DTOs.UpdateInstructorDto instructorDto)
        {
            var updatedInstructor = await _instructorService.UpdateInstructorAsync(id, instructorDto);

            return Ok(updatedInstructor);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetInstructorsCount()
        {
            var count = await _instructorService.GetInstructorsCountAsync();
            return Ok(count);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllInstructors(
     [FromQuery] string? searchTerm, 
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await _instructorService.GetAllInstructorsAsync(searchTerm, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            await _instructorService.DeleteInstructorAsync(id);

            return NoContent();
        }
    }
}

