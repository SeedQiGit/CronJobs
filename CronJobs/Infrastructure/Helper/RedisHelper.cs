using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Infrastructure.Helper
{
    /// <summary>
    /// 直接单例ConnectionMultiplexer就可以，没必要用这个类的。
    /// ConnectionMultiplexer对象是StackExchange.Redis最中枢的对象。这个类的实例需要被整个应用程序域共享和重用的，你不要在每个操作中不停的创建该对象的实例,所以使用单例来创建和存放这个对象是必须的。
    ///ConnectionMultiplexer 单例。GetDatabase() 返回的db对象是很轻量级别的，不需要被缓存起来，每次用每次拿即可。
    /// </summary>
    public class RedisHelper : IDisposable
    {
        private readonly string _connectionString; 
        private readonly string _instanceName; 
        private readonly int _defaultDb;
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

        public RedisHelper(string connectionString, string instanceName, int defaultDb = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDb = defaultDb;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }       
        public IDatabase GetDatabase(int defaultDb=0)
        {
            if (defaultDb==0)
            {
                return GetConnect().GetDatabase(_defaultDb);
            }
            else
            {
                return GetConnect().GetDatabase(defaultDb);
            }
        }

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }

        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }
}
