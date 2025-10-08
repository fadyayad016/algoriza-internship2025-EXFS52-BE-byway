using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs
{
    public class InstructorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int JobTitle { get; set; } 
        public string Description { get; set; }
        public int Rating { get; set; }
        public string ImagePathUrl { get; set; }
    }

    public class CreateInstructorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public JobTitle JobTitle { get; set; }
        public int Rating { get; set; }
        public IFormFile Image { get; set; }
    }



    public class UpdateInstructorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public JobTitle JobTitle { get; set; }
        public int Rating { get; set; }
        public IFormFile? Image { get; set; } 
    }


    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
