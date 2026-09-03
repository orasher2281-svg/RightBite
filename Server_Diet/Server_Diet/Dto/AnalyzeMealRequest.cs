using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Core.Resource
{
    public class AnalyzeMealRequest
    {
       
            public string? FoodName { get; set; }
            public string? Description { get; set; }
            public IFormFile? File { get; set; }
 
    }
}
