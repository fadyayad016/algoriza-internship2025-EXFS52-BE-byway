using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
    public class CartItem: BaseEntity
    {
        // Foreign Key for ShoppingCart
        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        // Foreign Key for Course
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }




       

    }
}
