using Leox.Aop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Leox.TranxManager
{
    public class TransactionalAttribute:MethodAspect
    {
        private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;
        public IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
            set
            {
                _isolationLevel = value;
            }
        }

        public override void OnStart(MethodAspectArgs args)
        {
            Console.WriteLine("tranx start.");
            string id = Guid.NewGuid().ToString().Replace("-", "");
            this.ExceptionStrategy = Aop.ExceptionStrategy.UnThrow;

            Manager.BeginTransaction(id, IsolationLevel);
        }

        public override void OnEnd(MethodAspectArgs args)
        {
            Console.WriteLine("tranx end.");
        }

        public override void OnException(MethodAspectArgs args)
        {
            Console.WriteLine("tranx exception." + args.Exception.Message);
            Manager.RollBack();
        }

        public override void OnSuccess(MethodAspectArgs args)
        {
            Console.WriteLine("tranx success.");
            Manager.Commit();
        }
    }
}
