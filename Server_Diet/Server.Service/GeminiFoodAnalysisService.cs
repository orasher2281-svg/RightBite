using System;
using System.Text.Json; // זה ה-using הנכון
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Resource;
using Core.Services;
using Microsoft.Extensions.Configuration;
namespace Service
{
    public class GeminiFoodAnalysisService : IFoodAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiFoodAnalysisService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeminiSettings:ApiKey"] ?? throw new Exception("API Key missing");
        }

        public async Task<NutritionalInfoResource> AnalyzeAsync(string? foodName, string? userDescription, byte[]? imageBytes)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
            var parts = new List<object>();

            // בניית הבקשה בצורה חכמה בהתאם למה שהמשתמש הזין
            string prompt = "You are a professional nutritionist API. Analyze the provided inputs. \n";

            if (!string.IsNullOrWhiteSpace(foodName))
                prompt += $"Food Name provided: '{foodName}'. \n";
            else
                prompt += $"Identify the food from the image and provide a concise Name. \n";

            if (!string.IsNullOrWhiteSpace(userDescription))
                prompt += $"User's description: '{userDescription}'. Enhance it into a proper short description. \n";
            else
                prompt += $"Create a short, engaging description (1-2 sentences) of this food. \n";

            prompt += "Provide nutritional values per 100g. Return ONLY valid JSON with exactly these keys: Name, Description, Calories, Protein, Carbs, Fat. No markdown tags like ```json.";

            parts.Add(new { text = prompt });

            // הוספת התמונה למערך הבקשה, רק אם קיימת
            if (imageBytes != null && imageBytes.Length > 0)
            {
                parts.Add(new { inline_data = new { mime_type = "image/jpeg", data = Convert.ToBase64String(imageBytes) } });
            }

            var requestBody = new { contents = new[] { new { parts = parts.ToArray() } } };

            var response = await _httpClient.PostAsJsonAsync(url, requestBody);
            response.EnsureSuccessStatusCode();

            using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            string rawText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<NutritionalInfoResource>(rawText.Trim(), options);

            return result ?? throw new Exception("Failed to parse AI response.");
        }
    }
}