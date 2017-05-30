using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Leox.TranxManager
{
    public class Manager:IDisposable
    {
        private static ConcurrentDictionary<string, Connectionx> _cache = new ConcurrentDictionary<string, Connectionx>();
        private static ThreadLocal<string> _threadLocal;
        private static System.Timers.Timer _timer;

        static Manager() {

            _threadLocal = new ThreadLocal<string>();

            //启动一个定时器每10分钟来检测所有连接的时长
            //如果超过10分钟则释放该连接
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 1000 * 60 * 10;
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            foreach (var item in _cache)
            {
                var timeSpan = now - item.Value.CreateTime;
                if (timeSpan.Minutes >= 10) {
                    Remove(item.Key);
                }
            }
        }

        public Connectionx this[int index]
        {
            get
            {
                if (_cache.ContainsKey(index.ToString()))
                    return _cache[index.ToString()];
                return null;
            }
        }

        public static bool NewTransaction(string id, IsolationLevel isolationLevel)
        {
            var conn = new Connectionx(id, isolationLevel);
            if (conn.BeginTransction())
            {
                Add(id.ToString(), conn);
                _threadLocal.Value = id;
                return true;
            }

            conn.Dispose();
            return false;
        }

        public static SqlCommand GetSqlCommand() {

            var id = _threadLocal.Value;
            if (!_cache.ContainsKey(id))
                throw new Exception("内部错误: 连接已丢失.");
            return _cache[id].SqlCommand;
        }

        public static void Commit(string id)
        {
            if (!_cache.ContainsKey(id))
                throw new Exception("内部错误: 连接已丢失.");

            _cache[id].Commit();

            Remove(id);
        }

        public static void RollBack(string id)
        {
            if (!_cache.ContainsKey(id))
                throw new Exception("内部错误: 连接已丢失.");

            _cache[id].RollBack();

            Remove(id);
        }

        private static bool Add(string id, Connectionx connx)
        {
            if (_cache.ContainsKey(id)) return false;

            _cache.AddOrUpdate(id, connx, null);
            return true;
        }

        public static bool Remove(string id)
        {
            Connectionx connx;
            var result = _cache.TryRemove(id,out connx);
            if (result)
            {
                connx.Dispose();
            }
            return result;
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }
    }
}
