using Byway.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // ✅ ADDED
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public string Level { get; set; }
        public string ImagePath { get; set; }
        public int TotalHours { get; set; }
        public string Certification { get; set; } // ✅ ADDED

        // IDs for the dropdowns
        public int InstructorId { get; set; } // ✅ ADDED
        public int CategoryId { get; set; }   // ✅ ADDED

        // Names for display purposes
        public string InstructorName { get; set; }
        public string CategoryName { get; set; }

        // Content for step 2
        public List<ContentDto> Contents { get; set; } // ✅ ADDED
    }


    public class ContentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LecturesNumber { get; set; }
        public int TimeInMinutes { get; set; }
    }


    public class AddContentDto
    {
      
        public string Name { get; set; }

        public int LecturesNumber { get; set; }

        public int TimeInMinutes { get; set; }
    }

    public class AddCourseDto
    {
        
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public double Rating { get; set; }

        public Level Level { get; set; } 

        public int TotalHours { get; set; }

        public string Certification { get; set; }

       
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }

        public ICollection<AddContentDto> Contents { get; set; }
        public IFormFile ImageFile { get; set; }

    }


    public class UpdateCourseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public Level Level { get; set; }
        public int TotalHours { get; set; }
        public string Certification { get; set; }
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public List<AddContentDto> Contents { get; set; }

        public IFormFile? ImageFile { get; set; }

    }

    public class PaginatedResult<T>
    {
        public int TotalCount { get; set; }
        public ICollection<T> Items { get; set; }
    }

    public class CourseQueryParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }

        public int? CategoryId { get; set; }

    }


   





}
