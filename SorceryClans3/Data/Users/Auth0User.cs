using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Users
{
    public class Auth0User
    {
        [Key] public string UserSAML { get; set; }
        public Guid UserID { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? CreatedTimeStamp { get; set; }
        public int UserType { get; set; }
        public DateTime? LastPaid { get; set; }
        public Auth0User()
        {
            //for EF
        }
        public Auth0User(string saml)
        {
            UserSAML = saml;
            UserID = Guid.NewGuid();
            CreatedTimeStamp = DateTime.UtcNow;
        }
    }
}