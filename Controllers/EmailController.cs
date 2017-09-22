namespace WebAPI.Controllers
{
    using WebAPI.Interfaces.Services;

    public class EmailController : IEmailService
    {
        private IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void EmailTest()
        {
            
        }
    }
}