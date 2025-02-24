using MailKit.Net.Smtp;
using Marqdouj.Aspire.MailKit.Client;
using MimeKit;
using System.Net.Mail;

namespace Marqdouj.MailDev.ApiService.Maps
{
    internal static class NewsletterMaps
    {
        public static void MapNewsletter(this WebApplication app)
        {
            app.MapPost("/newsletter/subscribe",
                async (MailKitClientFactory factory, string email) =>
                {
                    ISmtpClient client = await factory.GetSmtpClientAsync();

                    using var message = new MailMessage("newsletter@yourcompany.com", email)
                    {
                        Subject = "Welcome to our newsletter!",
                        Body = "Thank you for subscribing to our newsletter!"
                    };

                    await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
                });

            app.MapPost("/newsletter/unsubscribe",
                async (MailKitClientFactory factory, string email) =>
                {
                    ISmtpClient client = await factory.GetSmtpClientAsync();

                    using var message = new MailMessage("newsletter@yourcompany.com", email)
                    {
                        Subject = "You are unsubscribed from our newsletter!",
                        Body = "Sorry to see you go. We hope you will come back soon!"
                    };

                    await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
                });
        }
    }
}
