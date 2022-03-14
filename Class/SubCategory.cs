using System.ComponentModel.DataAnnotations;

namespace BuchhaltungPrivat.Class
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        public string? SubCategoryName { get; set; }


        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
