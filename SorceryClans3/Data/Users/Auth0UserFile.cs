using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SorceryClans3.Data.Users
{
    public class Auth0UserFile
    {
        [Key] public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string DisplayName { get; set; }
        public string BlobName { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public bool ImageFile { get; set; }
        public Auth0UserFile()
        {
            ID = Guid.NewGuid();
            CreatedTimeStamp = DateTime.Now;
        }
        public string DownloadFile
        {
            get
            {
                return UserID.ToString() + "/" + BlobName;
            }
        }
    }
}