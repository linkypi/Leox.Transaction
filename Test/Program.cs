using Leox.TranxManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        [Transactional]
        public static void TakeOrder()
        {
            UserDao userDao = new UserDao();
            decimal price = 88.5M;
            int count = 120;
            decimal total = price * count;
            userDao.DecreaseBalance(1, total);
            userDao.IncreaseBalance(2, total);

            StockDao stockDao = new StockDao();
            stockDao.Decrease(2,count);
        }
    }
}
