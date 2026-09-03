using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resource
{
    public class UserMealResource
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int FoodId { get; set; }
        public double Quantity { get; set; }
        public string? MealType { get; set; }
        public DateTime? MealDate { get; set; } = null;
        public FoodResource? Food { get; set; }
    }
}
