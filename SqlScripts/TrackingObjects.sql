--PUTimeEnd
if (object_id(N'[dbo].[PUTimeEnd_Tracking_Changed]') is not null)
  drop trigger [dbo].[PUTimeEnd_Tracking_Changed] 
go
if (object_id(N'[dbo].[PUTimeEnd_Tracking]') is not null)
  drop table [dbo].[PUTimeEnd_Tracking]
go
create table [dbo].[PUTimeEnd_Tracking](TrackingDate datetime, TrackingType tinyint, Amount float null, Order_ID int not null, PUTime datetime not null)
go
create trigger [dbo].[PUTimeEnd_Tracking_Changed] 
on [dbo].[PUTimeEnd]
after INSERT, DELETE
as
begin
	declare @now datetime
	set @now = getdate()

	insert into [dbo].[PUTimeEnd_Tracking](TrackingDate, TrackingType, Amount, Order_ID, PUTime)
	select
		TrackingDate = @now,
		TrackingType = 1,
		i.[Amount],
		i.[Order_ID],
		i.[PUTime]
	from 
		inserted i

	--insert into [dbo].[PUTimeEnd_Tracking](TrackingDate, TrackingType, Amount, Order_ID, PUTime)
	--select
	--	TrackingDate = @now,
	--	TrackingType = 0,
	--	d.[Amount],
	--	d.[Order_ID],
	--	d.[PUTime]
	--from 
	--	deleted d
end
go
--Order
if (object_id(N'[dbo].[Order_Tracking_Changed]') is not null)
  drop trigger [dbo].[Order_Tracking_Changed] 
go
if (object_id(N'[dbo].[Order_Tracking]') is not null)
  drop table [dbo].[Order_Tracking]
go
create table [dbo].[Order_Tracking](
	TrackingDate datetime, 
	TrackingType tinyint, 
	Order_ID int not null, 
	OperatorStation_Id int not null,
	StartTime datetime not null,
	EndTime datetime null,
	AmountPerUnit float not null,
	AmountPerPulseStart float not null,
	OpUnitTime float not null)
go
create trigger [dbo].[Order_Tracking_Changed] 
on [dbo].[Orders]
after INSERT, DELETE
as
begin
	declare @now datetime
	set @now = getdate()

	insert into [dbo].[Order_Tracking](TrackingDate, TrackingType, Order_Id, OperatorStation_Id, StartTime, EndTime, AmountPerUnit, AmountPerPulseStart, OpUnitTime)
	select
		TrackingDate = @now,
		TrackingType = 1,
		i.[Order_Id],
		i.[OperatorStation_Id],
		i.[StartTime],
		i.[EndTime],
		i.[AmountPerUnit],
		i.[AmountPerPulseStart],
		i.[OpUnitTime]
	from 
		inserted i

	--insert into [dbo].[Order_Tracking](TrackingDate, TrackingType, Order_Id, OperatorStation_Id, StartTime, EndTime, AmountPerUnit, AmountPerPulseStart, OpUnitTime)
	--select
	--	TrackingDate = @now,
	--	TrackingType = 0,
	--	d.[Order_Id],
	--	d.[OperatorStation_Id],
	--	d.[StartTime],
	--	d.[EndTime],
	--	d.[AmountPerUnit],
	--	d.[AmountPerPulseStart],
	--	d.[OpUnitTime]
	--from 
	--	deleted d
end
go
--OrderBatch
if (object_id(N'[dbo].[OrderBatch_Tracking_Changed]') is not null)
  drop trigger [dbo].[OrderBatch_Tracking_Changed] 
go
if (object_id(N'[dbo].[OrderBatch_Tracking]') is not null)
  drop table [dbo].[OrderBatch_Tracking]
go
create table [dbo].[OrderBatch_Tracking](
	TrackingDate datetime, 
	TrackingType tinyint, 
	OrderBatch_ID int not null, 
	Order_Id int not null,
	StartTime datetime not null,
	EndTime datetime null)
go
create trigger [dbo].[OrderBatch_Tracking_Changed] 
on [dbo].[OrderBatch]
after INSERT, DELETE
as
begin
	declare @now datetime
	set @now = getdate()

	insert into [dbo].[OrderBatch_Tracking](TrackingDate, TrackingType, OrderBatch_Id, Order_Id, StartTime, EndTime)
	select
		TrackingDate = @now,
		TrackingType = 1,
		i.[OrderBatch_Id],
		i.[Order_Id],
		i.[StartTime],
		i.[EndTime]
	from 
		inserted i

	--insert into [dbo].[OrderBatch_Tracking](TrackingDate, TrackingType, OrderBatch_Id, Order_Id, StartTime, EndTime)
	--select
	--	TrackingDate = @now,
	--	TrackingType = 0,
	--	d.[OrderBatch_Id],
	--	d.[Order_Id],
	--	d.[StartTime],
	--	d.[EndTime]
	--from 
	--	deleted d
end
go
--DowntimeOccasion
if (object_id(N'[dbo].[DowntimeOccasion_Tracking_Changed]') is not null)
  drop trigger [dbo].[DowntimeOccasion_Tracking_Changed] 
go
if (object_id(N'[dbo].[DowntimeOccasion_Tracking]') is not null)
  drop table [dbo].[DowntimeOccasion_Tracking]
go
create table [dbo].[DowntimeOccasion_Tracking](
	TrackingDate datetime, 
	TrackingType tinyint, 
	DTOccasion_Id int not null, 
	ProdPlace_Id int not null,
	BeginTime datetime not null,
	EndTime datetime null,
	LossType int null)
go
create trigger [dbo].[DowntimeOccasion_Tracking_Changed] 
on [dbo].[DowntimeOccasion]
after INSERT, DELETE
as
begin
	declare @now datetime
	set @now = getdate()

	insert into [dbo].[DowntimeOccasion_Tracking](TrackingDate, TrackingType, DTOccasion_Id, ProdPlace_Id, BeginTime, EndTime, LossType)
	select
		TrackingDate = @now,
		TrackingType = 1,
		i.[DTOccasion_Id],
		i.[ProdPlace_Id],
		i.[BeginTime],
		i.[EndTime],
		i.[LossType]
	from 
		inserted i

	--insert into [dbo].[DowntimeOccasion_Tracking](TrackingDate, TrackingType, DTOccasion_Id, ProdPlace_Id, BeginTime, EndTime, LossType)
	--select
	--	TrackingDate = @now,
	--	TrackingType = 0,
	--	d.[DTOccasion_Id],
	--	d.[ProdPlace_Id],
	--	d.[BeginTime],
	--	d.[EndTime],
	--	d.[LossType]
	--from 
	--	deleted d
end
go
--CalendarHistory
if (object_id(N'[dbo].[CalendarHistory_Tracking_Changed]') is not null)
  drop trigger [dbo].[CalendarHistory_Tracking_Changed] 
go
if (object_id(N'[dbo].[CalendarHistory_Tracking]') is not null)
  drop table [dbo].[CalendarHistory_Tracking]
go
create table [dbo].[CalendarHistory_Tracking](
	TrackingDate datetime, 
	TrackingType tinyint, 
	CalendarHistory_ID int not null, 
	OperatorStation_Id int not null,
	PeriodStartTime datetime not null,
	ChangeDate datetime null,
	ChangeType int not null,
	Calendar int not null)
go
create trigger [dbo].[CalendarHistory_Tracking_Changed] 
on [dbo].[CalendarHistory]
after INSERT
as
begin
	declare @now datetime
	set @now = getdate()

	insert into [dbo].[CalendarHistory_Tracking](TrackingDate, TrackingType, CalendarHistory_Id, OperatorStation_Id, PeriodStartTime, ChangeDate, ChangeType, Calendar)
	select
		TrackingDate = @now,
		TrackingType = 1,
		i.[CalendarHistory_Id],
		i.[OperatorStation_Id],
		i.[PeriodStartTime],
		i.[ChangeDate],
		i.[ChangeType],
		i.[Calendar]
	from 
		inserted i
end
go