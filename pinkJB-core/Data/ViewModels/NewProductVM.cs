using System.ComponentModel.DataAnnotations;

namespace pinkJB_core.Data.ViewModels
{
    public class NewProductVM
    {
        public int Id { get; set; }
       [Required(ErrorMessage ="Product name is required")]
        [Display(Name = "Product name")] 
        public string ProductName { get; set; }
       
        [Display(Name = "Product description")]
        public string ProductDescription { get; set; }
        
        
        [Required(ErrorMessage = "Product price is required")]
        [Display(Name = "Product price in $")]
        public double ProductPrice { get; set; }
        
        
        [Required(ErrorMessage = "Product image is required")]
        [Display(Name = "Product image")]
        public string ProductImage { get; set; }
        
        [Required(ErrorMessage = "Amount of products is required")]
        [Display(Name = "Amount left (number)")]
        public int amountLeft { get; set; }
       
        [Required(ErrorMessage = "Product material is required")]
        [Display(Name = "Product material")]
        public string ProductMaterial { get; set; }

    }
}
