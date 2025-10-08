using Byway.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Interfaces
{
    public interface ICourseService
    {
       
        Task<PaginatedResult<CourseDto>> GetAllCoursesAsync(CourseQueryParams queryParams);

        
        Task<CourseDto> GetCourseByIdAsync(int id);

       
        Task<CourseDto> CreateCourseAsync(AddCourseDto courseDto);

       
        Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto courseDto);

       
        Task<bool> DeleteCourseAsync(int id);



    }
}
