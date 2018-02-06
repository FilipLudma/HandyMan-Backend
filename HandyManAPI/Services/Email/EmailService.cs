namespace HandyManAPI.Services.Email
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Gmail.v1;
    using Google.Apis.Gmail.v1.Data;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net.Mail;
    using HandyManAPI.Interfaces.Services;
    using MimeKit;
    using System.Net;
    using Org.BouncyCastle.Utilities.Encoders;
    using Microsoft.Extensions.Logging;

    public class EmailService : IEmailService
    {
        #region Private Properties

        private readonly ILogger _logger;

        private string _senderEmail = "filip.ludma@gmail.com";

        #endregion Private Properties

        #region Public Constructors

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        #endregion Public Constructor

        #region Public Methods

        public bool SendEmail(MailMessage mail)
        {
            _logger.LogDebug("START Send Email");

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     ("filip.ludma@gmail.com", "bam1bac2ik4"); // ***use valid credentials***
                smtp.Port = 587;

                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            _logger.LogDebug("END Send Email");
            return true;
        }

        #endregion Public Methods
    }

}