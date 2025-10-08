using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class OrderItem: BaseEntity
    {
        public decimal Price { get; set; }

        // Foreign Key for Order
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        // Foreign Key for Course
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }

}
