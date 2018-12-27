using System;
using System.Collections.Generic;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Abstracts;
using TableDependency.SqlClient.Base.EventArgs;
namespace OeeCalculation.TrackableDatabase
{
    public class SqlServerListener : IDisposable
    {
        private readonly string connection;
        public SqlServerListener(string connection)
        {
            this.connection = connection;
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            dependencies.ForEach(x => { if (x != null) { x.Stop(); x.Dispose(); } });
        }
        private readonly List<ITableDependency> dependencies = new List<ITableDependency>();
        public void Listen<T>(Action<object, RecordChangedEventArgs<T>> handler, string table = null) where T : class, new()
        {
            try
            {
                var dep = new SqlTableDependency<T>(connection, table);
                dependencies.Add(dep);
                dep.OnError += OnError;
                dep.OnChanged += (o, e) => handler(o, e);
                dep.Start();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private void OnError(object sender, ErrorEventArgs e)
        {
            throw e.Error;
        }
    }
}
