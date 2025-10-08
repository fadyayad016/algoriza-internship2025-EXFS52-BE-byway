using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class ShoppingCart: BaseEntity
    {

        // Foreign Key for AppUser (One-to-One relationship)
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}
