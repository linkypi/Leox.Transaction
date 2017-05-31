using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leox.TranxManager
{
    public class SQLHelper
    {
        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            var command = Manager.GetSqlCommand();
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            if (parameters != null)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteNonQuery();
        }

        public static object ExecuteScalar(string sql, SqlParameter[] parameters = null)
        {
            var command = Manager.GetSqlCommand();
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            if (parameters != null)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteScalar();
        }
    }
}
