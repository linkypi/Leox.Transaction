using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Leox.TranxManager
{
    public class Manager:IDisposable
    {
        private static ConcurrentDictionary<string, Connectionx> _cache = new ConcurrentDictionary<string, Connectionx>();
        private static Timer _timer;
        static Manager() {
            //启动一个定时器每10分钟来检测所有连接的时长
            //如果超过10分钟则释放该连接
            _timer = new Timer();
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

        public static Connectionx this[int index]
        {
            get
            {
                if (_cache.ContainsKey(index.ToString()))
                    return _cache[index.ToString()];
                return null;
            }
        }

        public static void NewTransaction(string id)
        {
            var conn = new Connectionx(id);
            conn.BeginTransction();
            Add(id.ToString(), conn);
        }

        public static void Commit(string id) {
            if (_cache.ContainsKey(id)) {
                _cache[id].Commit();

                Connectionx connx = null;
                _cache.TryRemove(id, out connx);
            }
        }

        public static void RollBack(string id)
        {
            if (_cache.ContainsKey(id))
            {
                _cache[id].RollBack();

                Connectionx connx = null;
                _cache.TryRemove(id,out connx);
            }
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
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }
    }
}
