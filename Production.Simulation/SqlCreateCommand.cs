using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Production.Simulation
{
    public class SqlCreateCommand : IDisposable
    {
        private readonly string connectionString;
        public SqlCreateCommand(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private SqlConnection connection;
        private SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                return connection;
            }
        }
        public DataTable Execute(string query)
        {
            using (var command = new SqlCommand(query, Connection))
            {
                SqlDataReader dr = command.ExecuteReader();
                var tb = new DataTable();
                tb.Load(dr);
                return tb;
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }
}
