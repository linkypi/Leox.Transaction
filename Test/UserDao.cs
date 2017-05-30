using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Leox.TranxManager;

namespace Test
{
    public class UserDao
    {
        public bool DecreaseBalance(int userid, decimal balance)
        {
            return Update(userid,false,balance);
        }

        public bool IncreaseBalance(int userid, decimal balance)
        {
            return Update(userid, true, balance);
        }

        private bool Update(int userid, bool increase, decimal balance)
        {
            string sql = string.Format("update user set balance=balance{0}@balance where userid = @userid", increase ? "+" : "-");
            var balanceParam = new SqlParameter("@balance", SqlDbType.Decimal, 32);
            balanceParam.Value = balance;
            var useridParams = new SqlParameter("@userid", SqlDbType.Int, userid);
            useridParams.Value = userid;
            SqlParameter[] paras = new SqlParameter[]{
                  balanceParam , useridParams
            };
            return SQLHelper.ExecuteNonQuery(sql, paras) > 0;
        }
    }
}
