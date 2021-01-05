using GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GrainImplementation
{
    public class EmailSenderGrain : Orleans.Grain, IEmailSender
    {
        public Task<string> SendEmailAsync(string emailMessage, string emailTitle="Default Title", string to="", string toName="")
        {

            var fromAddress = new MailAddress("PSSCPescaru@gmail.com", "PSSC Name");
            var toAddress = new MailAddress(to, toName);
            const string fromPassword = "12345Pescaru";
             string subject = emailTitle;
             string body = emailMessage;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
               
                
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            return Task.Run(() => string.Empty);
        }
    }
}
