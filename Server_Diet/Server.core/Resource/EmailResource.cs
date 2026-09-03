using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resource
{
    public class EmailResource
    {
        [Required(ErrorMessage = "שם הוא שדה חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם חייב להכיל בין 2 ל-100 תווים")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "אימייל הוא שדה חובה")]
        [EmailAddress(ErrorMessage = "פורמט אימייל לא תקין")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "נושא הוא שדה חובה")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "נושא חייב להיות בין 3 ל-200 תווים")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "תוכן ההודעה הוא שדה חובה")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "תוכן ההודעה קצר מדי")]
        public string Message { get; set; } = string.Empty;

    }
}
