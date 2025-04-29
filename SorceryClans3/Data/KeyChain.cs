namespace SorceryClans3.Data
{
    /// <summary>
    /// In-app settings not needed to be abstracted to user secrets
    /// </summary>
    public static class KeyChain
    {
        public static string AppName
        {
            get
            {
                return "Sorcery Clans";
            }
        }
        public static string AppCode
        {
            get
            {
                return "SorceryClans";
            }
        }
        public static string Container
        {
            get
            {
                return "scfilestorage";
            }
        }
        public static string OwnerEmail
        {
            get
            {
                return "prions48@gmail.com";
            }
        }
    }
    public enum Environ
    {
        MainApp
    }

}