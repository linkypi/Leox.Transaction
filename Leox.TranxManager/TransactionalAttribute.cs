using Leox.Aop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leox.TranxManager
{
    public class TransactionalAttribute:MethodAspect
    {
        public override void OnStart(MethodAspectArgs args)
        {
            string id = Guid.NewGuid().ToString().Replace("-", "");
            SetArgs(args, id);
            this.ExceptionStrategy = Aop.ExceptionStrategy.UnThrow;
            Manager.NewTransaction(id);
        }

        /// <summary>
        /// 将连接Id放在参数第一位
        /// </summary>
        /// <param name="args"></param>
        /// <param name="id"></param>
        private void SetArgs(MethodAspectArgs args, string id)
        {
            if (args.Argument == null) 
            { args.Argument = new object[] { id }; }
            else
            {
                var temps = new object[args.Argument.Length + 1];
                temps[0] = id;
                int index = 1;
                foreach (var item in args.Argument)
                {
                    temps[index++] = item;
                }
                args.Argument = temps;
            }
        }

        public override void OnEnd(MethodAspectArgs args)
        {
            if (args != null && args.Argument != null && args.Argument.Length > 0)
                Manager.Commit(args.Argument[0].ToString());
        }

        public override void OnException(MethodAspectArgs args)
        {
            if (args != null && args.Argument != null && args.Argument.Length > 0)
                Manager.RollBack(args.Argument[0].ToString());
        }
    }
}
