using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace AspNetReactApp.Services;

public class EmailSender(ILogger<EmailSender> logger) : IEmailSender
{
    private readonly ILogger _logger = logger;

    private class IdentityMessage
    {
        /// <summary>
        /// Destination, i.e. To email, or SMS phone number
        /// </summary>
        public virtual required string Destination { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        public virtual required string Subject { get; set; }

        /// <summary>
        /// Message contents
        /// </summary>
        public virtual required string Body { get; set; }
    }

    public Task SendEmailAsync(string toEmail, string subject, string message)
    {
        Thread bgThread = new(new ParameterizedThreadStart(SendEmailAsync))
        {
            IsBackground = true
        };
        bgThread.Start(new IdentityMessage()
        {
            Destination = toEmail,
            Subject = subject,
            Body = message
        });
        return Task.CompletedTask;
    }

    private void SendEmailAsync(object? identityMsg)
    {
        // Check null email message
        if (identityMsg == null)
        {
            throw new Exception("Null email message");
        }
        // Send email using smtp
        IdentityMessage identityMessage = (IdentityMessage)identityMsg;
        var senderEmail = new MailAddress("bonsaigardenshop6@gmail.com", "Bonsai Garden");
        var receiverEmail = new MailAddress(identityMessage.Destination, "Receiver");
        var password = "akstlcgwjygfugua";
        var sub = identityMessage.Subject;
        var body = identityMessage.Body;
        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(senderEmail.Address, password)
        };
        using var mess = new MailMessage(senderEmail, receiverEmail)
        {
            Subject = sub,
            Body = body,
            IsBodyHtml = true
        };
        smtp.Send(mess);
        smtp.Dispose();
    }
}