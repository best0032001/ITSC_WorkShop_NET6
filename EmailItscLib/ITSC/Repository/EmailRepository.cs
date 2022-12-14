using EmailItscLib.ITSC.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailItscLib.ITSC.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private IConfiguration _configuration;
        private Boolean IsActive;
        private String _mailServer;
        private Int32 _mailPort;
        private String _mailSender;
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            IsActive = Convert.ToBoolean(_configuration["mailActive"]);
            _mailServer = _configuration["mailServer"];
            _mailPort = Convert.ToInt32(_configuration["mailPort"]);
            _mailSender = _configuration["mailSender"];

        }
        public async Task SendEmailAsync(String nameSender, string email_To, string subject, string message, List<IFormFile> Attachment)
        {
            if (IsActive)
            {

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(nameSender, _mailSender));
                emailMessage.To.Add(new MailboxAddress(" ", email_To));
                emailMessage.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = message };
                if (Attachment != null)
                {
                    foreach (IFormFile formFile in Attachment)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            builder.Attachments.Add(formFile.FileName, memoryStream.ToArray());
                        }
                    }
                }
                emailMessage.Body = builder.ToMessageBody();
                await Task.Run(() =>
                {
                    using (var client = new SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(_mailServer, _mailPort, SecureSocketOptions.Auto);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Send(emailMessage);
                        client.Disconnect(true);
                    }
                });
            }
        }

        public async Task SendEmailAsync(string nameSender, string reply, string email_To, string subject, string message, List<IFormFile> Attachment)
        {
            if (IsActive)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(nameSender, _mailSender));
                emailMessage.ReplyTo.Add(new MailboxAddress("Reply-to", reply));
                emailMessage.To.Add(new MailboxAddress(" ", email_To));
                emailMessage.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = message };
                if (Attachment != null)
                {
                    foreach (IFormFile formFile in Attachment)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            builder.Attachments.Add(formFile.FileName, memoryStream.ToArray());
                        }
                    }
                }
                emailMessage.Body = builder.ToMessageBody();
                await Task.Run(() =>
                {
                    using (var client = new SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(_mailServer, _mailPort, SecureSocketOptions.Auto);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Send(emailMessage);
                        client.Disconnect(true);
                    }
                });
            }
        }
    }
}
