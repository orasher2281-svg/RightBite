using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Food:BaseModel
    {
       
           

            [Required(ErrorMessage = "שם המאכל הוא שדה חובה")]
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [StringLength(255)]
            public string? Description { get; set; }

            /// <summary>
            /// נתיב לתמונה - המשתמש יכול להעלות תמונה והשרת ישמור את הנתיב שלה
            /// </summary>
            [StringLength(255)]
            public string? ImageUrl { get; set; }

            // ערכים תזונתיים - השרת יחשב או ישלוף אותם
            [Required]
            [Range(0, double.MaxValue)]
            public double Calories { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public double Protein { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public double Carbs { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public double Fat { get; set; }
            public virtual ICollection<UserMeal> UserMeals { get; set; } = new List<UserMeal>();

    }
}
