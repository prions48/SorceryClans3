namespace SorceryClans3.Data.Files
{
    public interface IFileSettings
    {
        string GetConnString();
    }
    public class FileSettings : IFileSettings
    {
        public IConfiguration _config;
        public FileSettings(IConfiguration configuration)
        {
            _config=configuration;
        }
         
        public string GetConnString()
        {
            return _config.GetSection("ConnectionStrings:Blob").Value;
        }
    }
}