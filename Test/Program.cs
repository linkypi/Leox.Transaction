using Leox.Aop;
using Leox.TranxManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" --- start --- ");
            try
            {
                Console.ReadKey();
                TakeOrder();
            }
            catch (Exception ex)
            {
                Console.WriteLine("take error: " + ex.Message + ". stack trace : " + ex.StackTrace);
            }
           
            Console.WriteLine(" --- end --- ");
            Console.Read();
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
            //throw new Exception("stock is not enough.");
        }
         //[Transactional]
        public static void TakeOrder127()
        {
            Type arg_22_0 = typeof(Program);
            string arg_22_1 = "TakeOrder";
            Type[] types = new Type[0];
            MemberInfo method = arg_22_0.GetMethod(arg_22_1, types);
            MethodAspectArgs methodAspectArgs = new MethodAspectArgs();
            MAList mAList = new MAList();
            TransactionalAttribute item = method.GetCustomAttributes(typeof(TransactionalAttribute), false)[0] as TransactionalAttribute;
            mAList.Add(item);
            foreach (MethodAspect current in mAList)
            {
                current.OnStart(methodAspectArgs);
            }
            try
            {
                Program._TakeOrder_33();
                foreach (MethodAspect current in mAList)
                {
                    current.OnSuccess(methodAspectArgs);
                }
            }
            catch (Exception ex)
            {
                methodAspectArgs.Exception = ex;
                foreach (MethodAspect current in mAList)
                {
                    current.OnException(methodAspectArgs);
                }
                switch ((int)mAList[0].ExceptionStrategy)
                {
                    case 1:
                        throw ex;
                    case 2:
                        throw;
                }
            }
            foreach (MethodAspect current in mAList)
            {
                current.OnEnd(methodAspectArgs);
            }
        }

        public static void _TakeOrder_33()
        {
            UserDao userDao = new UserDao();
            decimal price = 88.5M;
            int count = 120;
            decimal total = price * count;
            userDao.DecreaseBalance(1, total);
            userDao.IncreaseBalance(2, total);

            StockDao stockDao = new StockDao();
            stockDao.Decrease(2, count);
        }

        // Test.Program
        public static void TakeOrder2()
        {
            Type arg_22_0 = typeof(Program);
            string arg_22_1 = "TakeOrder";
            Type[] types = new Type[0];
            MemberInfo method = arg_22_0.GetMethod(arg_22_1, types);
            MethodAspectArgs methodAspectArgs = new MethodAspectArgs();
            MAList mAList = new MAList();
            TransactionalAttribute item = method.GetCustomAttributes(typeof(TransactionalAttribute), false)[0] as TransactionalAttribute;
            mAList.Add(item);
            foreach (MethodAspect current in mAList)
            {
                current.OnStart(methodAspectArgs);
            }
            try
            {
                Program._TakeOrder_33();
                foreach (MethodAspect current in mAList)
                {
                    current.OnSuccess(methodAspectArgs);
                }
            }
            catch (Exception ex)
            {
                methodAspectArgs.Exception = ex;
                foreach (MethodAspect current in mAList)
                {
                    current.OnException(methodAspectArgs);
                }
                switch ((int)mAList[0].ExceptionStrategy)
                {
                    case 1:
                        throw ex;
                    case 2:
                        throw;
                }
            }
            foreach (MethodAspect current in mAList)
            {
                current.OnEnd(methodAspectArgs);
            }
        }

    }
}
