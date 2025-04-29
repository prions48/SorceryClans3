namespace SorceryClans3.Data.Email
{
    public interface IEmailSettings
    {
        string GetConnString();
        string GetSender();
    }
    public class EmailSettings : IEmailSettings
    {
        public IConfiguration _config;
        public EmailSettings(IConfiguration configuration)
        {
            _config=configuration;
        }
         
        public string GetConnString()
        {
            return _config.GetSection("ConnectionStrings:Email").Value;
        }
        public string GetSender()
        {
            return _config.GetSection("EmailSettings:Sender").Value;
        }
    }
}