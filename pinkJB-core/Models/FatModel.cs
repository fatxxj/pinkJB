using System.ComponentModel.DataAnnotations;

namespace pinkJB_core.Models
{
    public class FatModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
