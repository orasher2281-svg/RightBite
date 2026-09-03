using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Resource;

namespace Core.Services
{
    public interface IFoodAnalysisService
    {
        // שלושת הפרמטרים יכולים להיות null, אבל הקונטרולר יוודא שלפחות שם או תמונה הגיעו
        Task<NutritionalInfoResource> AnalyzeAsync(string? foodName, string? description, byte[]? imageBytes);
    }
}
