using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pinkJB_core.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        
        public string userId { get; set; }
        [ForeignKey(nameof(userId))]
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
