using System.Data;
using System.Data.SqlClient;
namespace Production.Test1.MsSql
{
    public interface IRequest
    {
        DataSet Query(string query);
        DataSet Proc(string name, SqlParameter[] parameters = null);
    }
    public class DbRequest : IRequest
    {
        private readonly string connectionString;
        public DbRequest(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public DataSet Query(string query)
        {
            using (var c = new SqlConnection(connectionString))
            {
                c.Open();
                SqlCommand command = new SqlCommand(query, c);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataSet set = new DataSet();
                adapter.Fill(set);
                return set;
            }
        }
        public DataSet Proc(string name, SqlParameter[] parameters = null)
        {
            using (var c = new SqlConnection(connectionString))
            {
                c.Open();
                SqlCommand command = new SqlCommand(name, c);
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataSet set = new DataSet();
                adapter.Fill(set);
                return set;
            }
        }
    }
}
