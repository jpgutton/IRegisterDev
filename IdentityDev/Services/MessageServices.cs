using SendGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using Twilio;

namespace IdentityDev.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly ILogger<AuthMessageSender> _logger;

        private readonly AuthMessageSMSSenderOptions _smsSettings;

        public AuthMessageSender(ILogger<AuthMessageSender> logger, IOptions<AuthMessageSenderOptions> optionsAccessor, IOptions<AuthMessageSMSSenderOptions> smsSettings)
        {
            _logger = logger;
            Options = optionsAccessor.Value;
            _smsSettings = smsSettings.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        //public AuthMessageSMSSenderOptions OptionsSMS { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            //var myMessage = new SendGrid.SendGridMessage();
            //myMessage.AddTo(email);
            //myMessage.From = new System.Net.Mail.MailAddress("info@digitalkailash.com", "Info DK");
            //myMessage.Subject = subject;
            //myMessage.Text = message;
            //myMessage.Html = message;
            //var credentials = new System.Net.NetworkCredential(
            //    Options.SendGridUser,
            //    Options.SendGridKey);
            //// Create a Web transport for sending email.
            //var transportWeb = new SendGrid.Web(credentials);
            //return transportWeb.DeliverAsync(myMessage);


            var myMessage = new SendGridMessage();
            myMessage.From = new MailAddress("info@digitalkailash.com", "Info DK");
            myMessage.Subject = subject;
            myMessage.AddTo(email);
            myMessage.Html = message;
            myMessage.Text = message;
            var client = new Web("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");
            //client.DeliverAsync(myMessage).Wait();
            return client.DeliverAsync(myMessage);





            // Plug in your email service here to send an email.
            //_logger.LogInformation("Email: {email}, Subject: {subject}, Message: {message}", email, subject, message);
            //return Task.FromResult(0);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_smsSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_smsSettings.Sid}:{_smsSettings.Token}")));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("To",$"+{number}"),
                    new KeyValuePair<string, string>("From", _smsSettings.From),
                    new KeyValuePair<string, string>("Body", message)
                    });

                await client.PostAsync(_smsSettings.RequestUri, content).ConfigureAwait(false);
            }
            //Plug in your SMS service here to send a text message.
            _logger.LogInformation("SMS: {number}, Message: {message}", number, message);
            //return Task.FromResult(0);
        }
    }
}

