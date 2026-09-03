using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Resource
{
    public class UserResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public UserGender Gender { get; set; }
        public UserGoal Goal { get; set; }
        public int? DailyCalories { get; set; }
        public double? TargetProtein { get; set; }
        public double? TargetCarbs { get; set; }
        public double? TargetFat { get; set; }
        

    }
}
