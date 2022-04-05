using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pinkJB_core.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string userId { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
