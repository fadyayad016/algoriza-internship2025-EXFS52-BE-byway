using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class Order: BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign Key for AppUser
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        // An order contains one or more items
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
