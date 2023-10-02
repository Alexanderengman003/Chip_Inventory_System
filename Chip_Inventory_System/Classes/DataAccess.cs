using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Chip_Inventory_System
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                var sqlConn = dbConnection as SqlConnection;
                if (sqlConn != null)
                {
                    sqlConn.Open(); // Open the connection synchronously
                }
                return dbConnection.Query<T>(sql, parameters);
            }
        }
    }
}
