using Core.Resource;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        // הזרקת השירות דרך ה-Constructor
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] EmailResource resource)
        {
            // 1. ה-ModelState נבדק אוטומטית בזכות [ApiController]
            // אם הנתונים ב-resource לא תקינים (למשל אימייל לא חוקי), 
            // הקוד הזה לא ירוץ והלקוח יקבל 400 Bad Request אוטומטית.

            try
            {
                // 2. קריאה לשירות שליחת המייל
                await _emailService.SendContactFormEmailAsync(
                    resource.Name,
                    resource.Email,
                    resource.Subject,
                    resource.Message
                );

                // 3. החזרת הצלחה ללקוח
                return Ok(new { message = "ההודעה נשלחה בהצלחה!" });
            }
            catch (Exception ex)
            {
                // במקום ההודעה הגנרית, נחזיר את ה-ex.Message
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
