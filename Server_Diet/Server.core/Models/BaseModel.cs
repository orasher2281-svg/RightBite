using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class BaseModel
    {
   
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // מוודא שהמספור יהיה אוטומטי בצד בסיס הנתונים
            public int Id { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

