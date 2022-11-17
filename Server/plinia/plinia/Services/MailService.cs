namespace plinia.Services
{
    public class MailService
    {
        private IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        internal Task SendMessage(string email, string v, string activationLink)
        {
            throw new NotImplementedException();
        }
    }
}
