using StackExchange.Redis;

namespace PulseGamingMVC.Helpers
{
    public class HelperCacheMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                string cnn = "cacheredisrazvan.redis.cache.windows.net:6380,password=XyHXP8H4gv8McqW56xeyoQPERgGmAV46tAzCaF6YMFs=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cnn);
            });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
