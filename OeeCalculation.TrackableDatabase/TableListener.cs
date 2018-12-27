using Production.Abstract;
using System;
using TableDependency.SqlClient.Base.EventArgs;

namespace OeeCalculation.TrackableDatabase
{
    public class TableListener<T> : IListener<T> where T : class, new()
    {
        public event System.Action<T> Change;
        private readonly SqlServerListener sqlServerListener;
        private readonly string table;
        public TableListener(SqlServerListener listener, string table = null)
        {
            this.table = table;
            this.sqlServerListener = listener;
        }
        public void Listen(
            Action<T> inserted,
            Action<T> updated,
            Action<T> deleted)
        {
            sqlServerListener.Listen<T>(
                handler: new Action<object, RecordChangedEventArgs<T>>((o, e) =>
                {
                    switch (e.ChangeType)
                    {
                        case TableDependency.SqlClient.Base.Enums.ChangeType.Delete:
                            deleted(e.Entity);
                            break;
                        case TableDependency.SqlClient.Base.Enums.ChangeType.Update:
                            updated(e.Entity);
                            break;
                        case TableDependency.SqlClient.Base.Enums.ChangeType.Insert:
                            inserted(e.Entity);
                            break;
                        default:
                            break;
                    }
                }),
                table: table);
        }
    }
}
