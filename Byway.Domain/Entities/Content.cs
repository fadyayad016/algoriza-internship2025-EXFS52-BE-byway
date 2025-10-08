using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class Content:BaseEntity
    {
        public string Name { get; set; }
        public int LecturesNumber { get; set; }
        public int TimeInMinutes { get; set; } 

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
