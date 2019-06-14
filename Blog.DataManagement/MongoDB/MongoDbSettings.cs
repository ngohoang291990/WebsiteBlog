namespace Blog.DataManagement.MongoDB
{
    public class MongoDbSettings : IAppSettingsSection
    {
        public string ConnectionString { get; set; }

        public string SslPfxFileBase64 { get; set; }

        public string SslPfxPassphrase { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }

        public bool IsSingleCollection { get; set; } = true;
    }
}