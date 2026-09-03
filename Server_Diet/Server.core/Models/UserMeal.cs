using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public  class UserMeal:BaseModel
    {
        
            [Required]
            public int UserId { get; set; }

            [Required]
            public int FoodId { get; set; }

            [Required]
            [Range(0.01, 1000, ErrorMessage = "הכמות חייבת להיות גדולה מ-0")]
            public double Quantity { get; set; }

          
            public string? MealType { get; set; } 

            [Required]
            [DataType(DataType.Date)]
            public DateTime MealDate { get; set; }

            // קשרי גומלין (Navigation Properties)
            [ForeignKey("UserId")]
            public virtual User? User { get; set; }

            [ForeignKey("FoodId")]
            public virtual Food? Food { get; set; }
        
    }
}
