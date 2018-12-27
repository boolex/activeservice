delete PUTimeEnd where Order_Id = 1056479

insert into PUTimeEnd(Order_Id, AMount, PUTime)
select
	Order_Id = 1056479,
	AMount = 4,
	PUTime = '2015-01-01 00:06:15.000'
from
	PUTimeEnd where Order_Id = 1056478


delete [dbo].[OrderBatch] where OrderBatch_Id > = 3000000
delete [dbo].[Orders] where Order_Id > = 3000000
set identity_insert [dbo].[Orders] on
insert into [dbo].[Orders](OperatorStation_Id, Order_Id, StartTime, EndTime, AmountPerUnit, AmountPerPulseStart, OpUnitTime, Completed, Active)
select
	OperatorStation_Id = 1,
	Order_Id = o.[order_Id] + 1000000,
	StartTime = o.[StartTime],
	EndTime = o.[EndTime],
	AmountPerUnit = 2.3,
	AmountPerPulseStart = 1.3,
	OpUnitTime = 3.44444,
	Completed = o.[Completed],
	Active = o.[Active]
from 
	[dbo].[orders] o
where 
	o.[order_Id] >= 2000000
	and o.[order_Id] <= 2000010
set identity_insert [dbo].[Orders] off



set identity_insert [dbo].[OrderBatch] on
insert into [dbo].[OrderBatch](OrderBatch_id, Order_Id, StartTime, EndTime, BatchNumber, User_Id)
select
	OrderBatch_Id = o.[order_Id] + 1000000,
	Order_Id = o.[order_Id] + 1000000,
	StartTime = o.[StartTime],
	EndTime = o.[EndTime],
	BatchNumber = 1,
	User_Id = 1
from 
	[dbo].[orders] o
where 
	o.[order_Id] >= 2000000
	and o.[order_Id] <= 2000010
set identity_insert [dbo].[OrderBatch] off

delete downtimeoccasion where DTOccasion_Id >= 31500000 and DTOccasion_Id <= 31500030
set identity_insert [dbo].DowntimeOccasion on
insert into dbo.DowntimeOccasion(ProdPlace_Id, DTOccasion_Id, BeginTime, EndTime, LossType)
select 
	ProdPlace_Id,
	DTOccasion_Id +  30000000,
	BeginTime,
	EndTime,
	LossType
from 
	downtimeoccasion where DTOccasion_Id >= 1500000 and DTOccasion_Id <= 1500030
set identity_insert [dbo].DowntimeOccasion off
/*
delete [dbo].[PUTimeEnd_Tracking]
delete  [dbo].[Order_Tracking]
*/

delete CalendarHistory where CalendarHistory_Id >= 50001 and CalendarHistory_Id <= 50011
set identity_insert [dbo].CalendarHistory on
insert into dbo.CalendarHistory(OperatorStation_Id, CalendarHistory_Id, ChangeType, Calendar, PeriodStartTime, ChangeDate, Updated, ServerDowntime)
select 
	OperatorStation_Id,
	CalendarHistory_Id +  10000,
	ChangeType,
	Calendar,
	PeriodStartTime,
	dateadd(second, 1,ChangeDate ),
	getdate(),
	ServerDowntime = 0
from CalendarHistory where CalendarHistory_Id >= 40001 and CalendarHistory_Id <= 40011
set identity_insert [dbo].CalendarHistory off



select * from [dbo].[PUTimeEnd_Tracking]
select * from  [dbo].[Order_Tracking]
select * from [dbo].[OrderBatch_Tracking]