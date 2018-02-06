namespace HandyManAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web;

    using HandyManAPI.Interfaces.Services;
    using HandyManAPI.Interfaces.Repositories;
    using HandyManAPI.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class EmailController : Controller
    {
        #region Private Properties

        private IEmailService _emailService;
        private IOrderRepository _orderRepository;
        private ILogger _logger;

        #endregion Private Properties

        #region Public Constructors

        public EmailController(IEmailService emailService, IOrderRepository orderRepository)
        {
            _emailService = emailService;
            _orderRepository = orderRepository;
            //_logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        // GET api/values
        [HttpGet]
        public bool SendEmail(string id)
        {
            //_logger.LogDebug("START Controller Send Email");
            bool response = false;
            List<MailMessage> mailMessages = new List<MailMessage>();

            try
            {
                var order = Task.Run(() => _orderRepository.GetById(Guid.Parse(id))).Result;

                var officeMailMessage = this.CreateEmail(true, order);
                mailMessages.Add(officeMailMessage);

                foreach (var mail in mailMessages)
                {
                    response = _emailService.SendEmail(mail);
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
            }

            // _logger.LogDebug("END Controller Send Email");
            return response;
        }

        #endregion Public Methods

        #region Private Methods

        private MailMessage CreateEmail(bool officeEmail, OrderRecord order)
        {
            string messageBody;
            
            //Read template file from the App_Data folder
            if (officeEmail)
            {
                using (var sr = new StreamReader(@".\Templates\officeEmailTemplate.txt"))
                {
                    messageBody = string.Format(
                        sr.ReadToEnd(),
                        order.FirstName,
                        order.LastName,
                        order.Address,
                        order.ContactNumber,
                        order.Email,
                        order.Category,
                        order.SubCategory,
                        order.Description);
                };
            }
            else
            {
                using (var sr = new StreamReader(@".\Templates\clientEmailemplate.txt"))
                {
                    messageBody = string.Empty;
                };

            }

            MailMessage mail = new MailMessage();
            if (order.ImgAttachments != null && order.ImgAttachments.Count > 0)
            {
                foreach (var imgAttachment in order.ImgAttachments)
                {
                    //save the data to a memory stream
                    MemoryStream ms = new MemoryStream(imgAttachment.ImageBlob);

                    //create the attachment from a stream. Be sure to name the data with a file and 
                    //media type that is respective of the data
                    mail.Attachments.Add(new Attachment(ms, imgAttachment.FileName + ".png", "image/png"));
                }
            }

            mail.To.Add("filip.ludma@gmail.com");
            mail.From = new MailAddress("filip.ludma@gmail.com");
            mail.Subject = "sub";
            mail.IsBodyHtml = true;
            mail.Body = messageBody;
            
            return mail;
        }

        #endregion Private Methods
    }
}