using System.Configuration;
using System.Runtime.Caching;

namespace QuartzDemo
{
    public class SysCache
    {

        #region 私有属性
        private static SysCache _sysCache;
        private ObjectCache _cache = MemoryCache.Default;
        private static object _lock = new object();
        private static int _interval { get; set; } = 20;
        #endregion
        /// <summary>
        /// 全局单例
        /// </summary>
        public static SysCache Instance
        {
            get
            {
                if (_sysCache == null)
                {
                    lock (_lock)
                    {
                        if (_sysCache == null)
                        {
                            _sysCache = new SysCache();
                        }
                    }
                }
                return _sysCache;
            }
        }


        /// <summary>
        /// 防止司机连续派单
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public bool SetCache(string key,object value)
        {
            var _item = _cache.Get(key);
            if (_item != null)
            {
                return false;
            }
            _cache.Set(key, value, DateTime.Now.AddSeconds(_interval));
            return true;
        }



        /// <summary>
        /// 检查订单是否有派单历史
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsCacheKey(string key)
        {
            var _order = _cache.Get(key);
            if (_order != null)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 清除这个订单的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }
}
