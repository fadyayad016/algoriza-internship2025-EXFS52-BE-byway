using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Api.New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {


        private  readonly ICourseService _courseService;    


        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] CourseQueryParams queryParams)
        {
            var paginatedResult = await _courseService.GetAllCoursesAsync(queryParams);
            return Ok(paginatedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            return Ok(course);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] AddCourseDto addCourseDto)
        {
            var createdCourse = await _courseService.CreateCourseAsync(addCourseDto);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id }, createdCourse);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] UpdateCourseDto updateCourseDto)
        {
            var updatedCourse = await _courseService.UpdateCourseAsync(id, updateCourseDto);
            return Ok(updatedCourse);
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            return NoContent();
        }



    }
}
