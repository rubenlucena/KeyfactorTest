using Keyfactor.Models;

namespace Keyfactor.Engine
{
    public interface ICache
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset timeOffset);
    }
    public class Cache : ICache
    {
        private readonly ICache _cache;

        public T GetData<T>(string key)
        {
            try
            {
                return _cache.GetData<T>(key);
            }
            catch
            {
                throw;
            }
        }


        public bool SetData<T>(string key, T value, DateTimeOffset timeOffset)
        {
            bool result = true;

            if(!string.IsNullOrEmpty(key))
            {
                try
                {
                    _cache.SetData(key, value, timeOffset);
                }
                catch
                {
                    throw;
                }
            }        
            return result;
        }
    }
}