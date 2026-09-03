using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User:BaseModel
    {
           

            [Required, StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required, EmailAddress, StringLength(100)]
            public string Email { get; set; } = string.Empty;

            [Required, StringLength(100)]
            public string Password { get; set; } = string.Empty;

            [Required]
            public double Weight { get; set; }

            [Required]
            public double Height { get; set; }

            [Required]
            public int Age { get; set; }

            // עכשיו השדות משתמשים בטיפוסים שיצרנו
            [Required]
            public UserGender Gender { get; set; }

            [Required]
            public UserGoal Goal { get; set; }

            [Required]
            public int DailyCalories { get; set; }

            public double TargetProtein { get; set; } = 0;
            public double TargetCarbs { get; set; } = 0;
            public double TargetFat { get; set; } = 0;
        public virtual ICollection<UserMeal> UserMeals { get; set; } = new List<UserMeal>();
    }
    }

