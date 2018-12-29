using System.Data.SqlClient;
using Production.Abstract;
namespace OeeCalculation.TrackableDatabase
{
    public class SoxxaTrackableDatabase : ITrackableDatabase
    {
        private readonly string connectionString;
        public SoxxaTrackableDatabase(
            string connectionString
            )
        {
            this.connectionString = connectionString;
        }
        public event Updated Updated;
        //https://github.com/christiandelbianco/monitor-table-change-with-sqltabledependency/blob/master/TableDependency.SqlClient/SqlTableDependency.cs
        public async void Track()
        {
            /*
            string conversationHandle = "cnvHndl1";
            int timeOutWatchDog = 1000;
            int unqueueMessageNumber = 1;
            int timeOut = 10;
            string _schemaName = "dbo";
            string _dataBaseObjectsNamingConvention = "tableName?";
            var waitforSqlScript =
               $"BEGIN CONVERSATION TIMER ('{conversationHandle}') TIMEOUT = " + timeOutWatchDog + ";";// +
              // $"WAITFOR (RECEIVE TOP({unqueueMessageNumber}) [message_type_name], [message_body] FROM [{_schemaName}].[{_dataBaseObjectsNamingConvention}_Receiver]), TIMEOUT {timeOut * 1000};";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                while (true)
                {
                    using (var sqlCommand = new SqlCommand(waitforSqlScript, sqlConnection))
                    {
                        sqlCommand.CommandTimeout = 0;

                        using (var sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (sqlDataReader.Read())
                            {
                                byte[] data = (byte[])sqlDataReader[1];
                                // if (message.MessageType == SqlMessageTypes.ErrorType) throw new QueueContainingErrorMessageException();
                                //messagesBag.AddMessage(message);
                                //this.WriteTraceMessage(TraceLevel.Verbose, $"Received message type = {message.MessageType}.");
                            }
                        }
                    }
                }
             
            }
               */
        }
    }
}
