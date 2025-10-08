using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseCount { get; set; }

    }

    public class CreateOrUpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }

}
