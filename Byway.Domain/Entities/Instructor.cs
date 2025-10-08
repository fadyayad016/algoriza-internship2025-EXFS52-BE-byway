using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class Instructor: BaseEntity
    {
        public string Name { get; set; }

        public JobTitle JobTitle { get; set; }
        public string Description { get; set; }

        public int Rating { get; set; }
        public string ImagePathUrl { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

    }
}
