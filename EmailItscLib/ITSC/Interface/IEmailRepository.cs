using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailItscLib.ITSC.Interface
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string nameSender, string email_To, string subject, string message, List<IFormFile> Attachment);
        Task SendEmailAsync(string nameSender, string reply, string email_To, string subject, string message, List<IFormFile> Attachment);
    }
}
