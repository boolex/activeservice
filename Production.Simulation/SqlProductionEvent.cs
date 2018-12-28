namespace Production.Simulation
{
    public class SqlProductionEvent
    {
        private readonly ProductionEvent e;
        private readonly SqlCreateCommand command;
        private string dateFormat = "yyyy-MM-dd HH:mm:ss";
        public SqlProductionEvent(
            ProductionEvent e,
            SqlCreateCommand command)
        {
            this.e = e;
            this.command = command;
        }
        public void Create()
        {
            command.Execute(ProductionEventCreateStatement);
        }
        private string ProductionEventCreateStatement
        {
            get
            {
                if (e.Start && e.Type == ProductionEventType.Shift)
                {
                    return string.Format(
                        @"insert into [dbo].[CalendarHistory](
                            OperatorStation_Id, Calendar, ChangeType, ChangeDate,  PeriodStartTime, Updated, ServerDownTime
                        )
                        values(
                            {0}, 4, {1}, '{2}', '{3}', GETDATE(), 0
                        )",
                        e.OperatorStationId,
                        e.Id,
                        e.On.ToString(dateFormat),
                        e.On.ToString(dateFormat)
                    );
                }
                else if (!e.Start && e.Type == ProductionEventType.Shift)
                {
                    return string.Format(
                        @"update [dbo].[CalendarHistory]
                          set
                            Calendar = 1,
                            ChangeDate = '{1}' 
                        where OperatorStation_Id = {0} and Calendar = 4",
                        e.OperatorStationId,
                        e.On.ToString(dateFormat)
                    );
                }
                else if (e.Start && e.Type == ProductionEventType.Unit)
                {
                    return string.Format(
                        @"insert into PUTimeStart(Order_Id, Amount, PUTime)
                        select
	                        Order_Id = o.[Order_Id],
	                        Amount = {1},
	                        PUTime = '{2}'
                        from
                            [dbo].[Orders] o where o.[OperatorStation_Id] = {0} and o.[Active] = 1 and o.[Completed] = 0
                        ",
                        e.OperatorStationId,
                        1,
                        e.On.ToString(dateFormat)
                   );
                }
                else if (!e.Start && e.Type == ProductionEventType.Unit)
                {
                    return string.Format(
                        @"insert into PUTimeEnd(Order_Id, Amount, PUTime)
                        select
	                        Order_Id = o.[Order_Id],
	                        Amount = {1},
	                        PUTime = '{2}'
                        from
                            [dbo].[Orders] o where o.[OperatorStation_Id] = {0} and o.[Active] = 1 and o.[Completed] = 0
                        ",
                        e.OperatorStationId,
                        1,
                        e.On.ToString(dateFormat)
                   );
                }
                else if (e.Start && e.Type == ProductionEventType.Stop)
                {
                    return string.Format(
                        @"insert into [dbo].[DowntimeOccasion](ProdPlace_Id, BeginTime, Exported)
                        select
	                        ProdPlace_Id = {0},
	                        BeginTime = '{1}',
	                        Exported = 0",
                        e.OperatorStationId,
                        e.On.ToString(dateFormat)
                   );
                }
                else if (!e.Start && e.Type == ProductionEventType.Stop)
                {
                    return string.Format(
                        @"Update [dbo].[DowntimeOccasion]
                        set
	                        EndTime ='{1}'
                        where 
                            ProdPlace_Id = {0} and EndTime is null",
                        e.OperatorStationId,
                        e.On.ToString(dateFormat)
                   );
                }
                else if (e.Start && e.Type == ProductionEventType.Order)
                {
                    return string.Format(
                        @"insert into [dbo].[Orders](OperatorStation_Id, StartTime, Completed, Active, AmountPerUnit, OpUnitTime)
                        select
	                        OperatorStation_Id = {0},
	                        StartTime = '{1}',
	                        Completed = 0,
	                        Active = 1,
                            1,1
                        go
                        insert into [dbo].[OrderBatch](Order_Id, StartTime, BatchNumber, User_Id)
                        select
	                        Order_Id = ident_current('[dbo].[Orders]'),
	                        StartTime = '{1}',
	                        BatchNumber = 1,
	                        User_Id = 1
                        ",
                        e.OperatorStationId,
                        e.On.ToString(dateFormat)
                   );
                }
                else if (!e.Start && e.Type == ProductionEventType.Order)
                {
                    return string.Format(
                        @" Update [dbo].[OrderBatch]
                        set EndTime = '{1}'
                        where EndTime is null and Order_Id in (select o.Order_Id from [dbo].[Orders] o where o.OperatorStation_Id = {0} and o.Active = 1 and o.Completed = 0 and o.EndTime is null)
                 
                        update [dbo].[Orders]
                        set
	                        EndTime = '{1}',
                            Active = 0,
                            Completed = 1
                        where OperatorStation_Id = {0} and Active = 1 and Completed = 0 and EndTime is null",
                        e.OperatorStationId,
                        e.On.ToString(dateFormat)
                   );
                }
                throw new System.NotImplementedException();
            }
        }
    }
}
