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
            string sql = "update stock set count=count-@count where userid = @userid";
            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@count",System.Data.SqlDbType.Int,count),
             new SqlParameter("@userid",System.Data.SqlDbType.Int,userid)
            };
            return SQLHelper.ExecuteNonQuery(sql, paras) > 0;
        }
    }
}
