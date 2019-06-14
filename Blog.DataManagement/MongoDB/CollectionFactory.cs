using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.DataManagement.MongoDB
{
    public class CollectionFactory : ICollectionFactory, ISingletonDependency, IDependency
    {
        private readonly MongoDbSettings _settings;
        private IMongoDatabase _database;

        public IMongoDatabase Database
        {
            get
            {
                if (this._database != null)
                    return this._database;
                this._database = this.CreateClient().GetDatabase(this._settings.DatabaseName, (MongoDatabaseSettings)null);
                return this._database;
            }
        }

        public CollectionFactory(IOptions<MongoDbSettings> optionsAccessor)
        {
            this._settings = optionsAccessor.Value;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return this.Database.GetCollection<T>(!this._settings.IsSingleCollection || string.IsNullOrEmpty(this._settings.CollectionName) ? collectionName : this._settings.CollectionName, (MongoCollectionSettings)null);
        }

        private MongoClient CreateClient()
        {
            MongoClientSettings settings=MongoClientSettings.FromUrl(new MongoUrl(this._settings.ConnectionString));
            if (!string.IsNullOrEmpty(this._settings.SslPfxFileBase64) &&
                !string.IsNullOrEmpty(this._settings.SslPfxPassphrase))
            {
                X509Certificate2 x509Certificate2 = new X509Certificate2(Convert.FromBase64String(this._settings.SslPfxFileBase64), this._settings.SslPfxPassphrase);
                MongoClientSettings mongoClientSettings = settings;
                SslSettings sslSettings1 = new SslSettings();
                //sslSettings1.set_ClientCertificates((IEnumerable<X509Certificate>)new X509Certificate2[1]
                //{
                //    x509Certificate2
                //});
                sslSettings1.CheckCertificateRevocation = false;
                SslSettings sslSettings2 = sslSettings1;
                mongoClientSettings.SslSettings = sslSettings2;
                settings.UseSsl = true;
                settings.VerifySslCertificate = false;
            }
            return new MongoClient(settings);
        }
    }
}