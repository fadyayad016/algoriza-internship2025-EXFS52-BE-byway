using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Byway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // <-- الحماية مفعلة وجاهزة
        public async Task<IActionResult> CreateInstructor(CreateInstructorDto instructorDto)
        {
            var createdInstructor = await _instructorService.CreateInstructorAsync(instructorDto);
            return Ok(createdInstructor);
        }
    }
}
