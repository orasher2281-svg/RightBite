using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resource
{
    public class DailyNutritionSummaryResource
    {

        // מזהה המשתמש והתאריך של הסיכום
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        // יעדים יומיים
        public int DailyCalories { get; set; }
        public double TargetProtein { get; set; }
        public double TargetCarbs { get; set; }
        public double TargetFat { get; set; }

        // צריכה בפועל
        public double CurrentCalories { get; set; }
        public double CurrentProtein { get; set; }
        public double CurrentCarbs { get; set; }
        public double CurrentFat { get; set; }
        
    }
}
