using pinkJB_core.Data.Base;
using pinkJB_core.Enums;
using System.ComponentModel.DataAnnotations;

namespace pinkJB_core.Models
{
    public class Product :IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int amountLeft { get; set; }
        

        public string ProductMaterial { get; set; }

    }
}
