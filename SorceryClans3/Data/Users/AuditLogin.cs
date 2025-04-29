using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Users
{
    public class AuditLogin
	{
		[Key] public Guid ID { get; set; }
		public Guid UserID { get; set; }
		public DateTime TimeStamp { get; set; }
		public string AppName { get; set; }
		public AuditLogin()
		{
			ID = Guid.NewGuid();
			AppName = KeyChain.AppCode;
			TimeStamp = DateTime.Now;
		}
	}
}