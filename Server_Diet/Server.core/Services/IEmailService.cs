using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IEmailService
    {
        Task SendContactFormEmailAsync(string name, string fromEmail, string subject, string message);
    }
}
