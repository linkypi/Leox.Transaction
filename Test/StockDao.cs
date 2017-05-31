using Leox.TranxManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class StockDao
    {
        public bool Decrease(int userid, int count)
        {
            string sql = "update stock set count=count-@count where user_id = @userid";
            var countParam = new SqlParameter("@count", System.Data.SqlDbType.Int, 32);
            var useridParam = new SqlParameter("@userid", System.Data.SqlDbType.Int, 32);
            countParam.Value = count;
            useridParam.Value = userid;
            SqlParameter[] paras = new SqlParameter[]{
                useridParam,countParam
            };
            return SQLHelper.ExecuteNonQuery(sql, paras) > 0;
        }
    }
}
