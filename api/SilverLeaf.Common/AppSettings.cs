using System;

namespace SilverLeaf.Common
{
    public class AppSettings
    {
        public class ConnectionString
        {
            public virtual string IdentityServer { get; set; }

            public virtual string MSSQL { get; set; }

            public virtual string ElasticSearch { get; set; }

            public virtual string GeneralApi { get; set; }

            public virtual string GeoDb { get; set; } = "https://wft-geo-db.p.rapidapi.com";
        }

        public class ElasticIndex
        {
            public virtual string Center { get; set; }

            public virtual string Chat { get; set; }

            public virtual string Class { get; set; }

            public virtual string Course { get; set; }

            public virtual string Feedback { get; set; }

            public virtual string Fry { get; set; }

            public virtual string ComprehensionScreener { get; set; }

            public virtual string ComprehensionScreenerResult { get; set; }

            public virtual string OralScreener { get; set; }

            public virtual string OralScreenerResult { get; set; }

            public virtual string PhonicsScreener { get; set; }

            public virtual string PhonicsScreenerResult { get; set; }

            public virtual string Room { get; set; }

            public virtual string Student { get; set; }

            public virtual string Teacher { get; set; }

            public virtual string PhonicsScreenerSkill { get; set; }

            public virtual string PhonicsSkill { get; set; }
        }

        public class Api
        {
            public virtual TimeSpan? General { get; set; }
        }

        public class Cache
        {
            public virtual TimeSpan? Default { get; set; } = new TimeSpan(1, 0, 0, 0);
            public virtual TimeSpan? Day => new TimeSpan(1, 0, 0, 0);
            public virtual TimeSpan? StaticThirdParty { get; set; } = new TimeSpan(30, 0, 0, 0);
        }

        public class Timer
        {
            public virtual Api Apis { get; set; }

            public virtual Cache Caches { get; set; }
        }

        public class ApiKey
        {
            public virtual string GeoDb { get; set; }
        }

        public virtual ElasticIndex ElasticIndexes { get; set; }

        public virtual ConnectionString ConnectionStrings { get; set; }

        public virtual Timer Timers { get; set; }

        public virtual ApiKey ApiKeys { get; set; }

        public virtual string Suite { get; set; }

        public virtual string Name { get; set; }

        public virtual string Version { get; set; }

        public virtual string Environment { get; set; }

        public virtual string Url { get; set; }

        public virtual string StaticFilePath { get; set; }

        public virtual string StaticFileAlias { get; set; }

        public virtual string EntityAssemblyName => $"SilverLeaf.Entities";


        #region Identity Server
        public virtual string DefaultScheme { get; set; }
        public virtual string DefaultChallengeScheme { get; set; }
        #endregion

    }
}
