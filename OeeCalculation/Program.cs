using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Production.Abstract;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace Production
{
    public class DowntimeOccasion
    {
        public int DTOccasion_Id { get; set; }
    }
    public class Program
    {      
      
        static void Main(string[] args)
        {
              string _con = @"Server=axkhm01\sql2008r2;Initial Catalog=OEECoreTest;User Id=sa;Password=0Soxxa0";
             var mapper = new ModelToTableMapper<DowntimeOccasion>();
            mapper.AddMapping(c => c.DTOccasion_Id, "DTOccasion_Id");
            

            // Here - as second parameter - we pass table name: 
            // this is necessary only if the model name is different from table name 
            // (in our case we have Customer vs Customers). 
            // If needed, you can also specifiy schema name.
            using (var dep = new SqlTableDependency<DowntimeOccasion>(_con, tableName: "DowntimeOccasion", mapper: mapper))
            {
                dep.OnChanged += Changed;
                dep.Start();

                Console.WriteLine("Press a key to exit");
                Console.ReadKey();

                dep.Stop();
            }
            //string connectionString = @"Server=axkhm01\sql2008r2;Initial Catalog=OEECoreTest;User Id=sa;Password=0Soxxa0";
            //string format = "yyyy-MM-dd HH:mm:ss";
            //for (var i = 1; i <= 6; i++)
            //{
            //    for (var startDate = DateTime.Parse("2015-01-01"); startDate <= DateTime.Parse("2017-01-01"); startDate = startDate.AddYears(1))
            //    {
            //        var toDate = startDate.AddYears(1);
            //        Console.WriteLine(string.Format("{3} Importing... ProdPlace: {0}, {1} - {2}", i, startDate.ToString(format), toDate.ToString(format), DateTime.Now.ToString(format)));
            //        LoadProdPlace(connectionString, i, startDate, toDate);
            //    }
            //}
            //Console.ReadLine();
            ////SqlBulkCopy Insert
        }

    public static void Changed(object sender, RecordChangedEventArgs<DowntimeOccasion> e)
    {
        var changedEntity = e.Entity;

        Console.WriteLine("DML operation: " + e.ChangeType);
        Console.WriteLine("ID: " + changedEntity.DTOccasion_Id);
        //Console.WriteLine("Name: " + changedEntity.Name);
        //Console.WriteLine("Surame: " + changedEntity.Surname);
    }
    public static void LoadToDb(string connectionString, IEnumerable<Intersection> intersections)
        {
            IDataReader reader = new IntersectionSet(intersections);

            using (var loader = new SqlBulkCopy(connectionString))
            {
                loader.DestinationTableName = "ProductionEventIntersection_CalendarHistory_Order_DTOccasion";
                loader.WriteToServer(reader);
            }
        }
        static List<ProductionEvent> GetEvents(DataTable table)
        {
            return table.AsEnumerable().Select(x => new ProductionEvent(
                prodplace: int.Parse(x["ProdPlace_Id"].ToString()),
                type: (ProductionEventType)int.Parse(x["Type"].ToString()),
                time: DateTime.Parse(x["Time"].ToString()),
                id: int.Parse(x["Id"].ToString())
                )).ToList();
        }
        static void LoadProdPlace(string connectionString, int prodplaceId, DateTime from, DateTime to)
        {
            var table = Load(connectionString, GetQuery(prodplaceId, from, to));
            var events = GetEvents(table);
            var intersections = new IntersectionContext(events);
            LoadToDb(connectionString, intersections.Intersections);
        }
        static string GetQuery(int prodplaceId, DateTime from, DateTime to)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            return string.Format("select ProdPlace_Id, Type, Time, Id from [dbo].[ProductionEvent] pe where pe.ProdPlace_Id = {0} and pe.Time between '{1}' and '{2}' order by pe.Time, pe.[Type] desc",
                prodplaceId.ToString(), from.ToString(format), to.ToString(format));
        }
        static DataTable Load(string connectionString, string query)
        {
            using (var connection = new SqlConnection(
                connectionString: connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    cmdText: query,
                    connection: connection
                    ))
                {
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        return tb;
                    }
                }
            }
        }
    }
}
