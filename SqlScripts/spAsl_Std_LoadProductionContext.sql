if (object_id(N'[dbo].[spAsl_Std_LoadProductionContext]') is not null)
   drop proc [dbo].[spAsl_Std_LoadProductionContext]
go

print '[dbo].[spAsl_Std_LoadProductionContext]'
go
create proc [dbo].[spAsl_Std_LoadProductionContext]
--declare
	   @os_ids varchar(max) = '0',
	   @from datetime = null,
	   @now datetime = null	   
as
begin
	if @now is null
		set @now = getdate()

	if @from is null
		set @from = dateadd(day, -1, @now)

	--select @os_ids = '0', @from = '2017-11-02', @now = getdate()
	declare @OperatorStation table(Operatorstation_Id int)
	insert into @OperatorStation
	select
		os.[OperatorStation_Id]
	from
		[dbo].[OperatorStation] os with(nolock)
	where
		os.[OperatorStation_Id] in (select x.field from [dbo].[fnAsl_Std_Split](@os_ids, ',') x)
		or @os_ids = '0'
	--0
	select 
		os.[Operatorstation_Id]
	from 
		@OperatorStation os;
	--1
	select
		ch.[OperatorStation_Id],
		ch.[CalendarHistory_Id],
		ch.[ChangeType],
		ch.[PeriodStartTime],
		ch.[ChangeDate],
		ch.[Calendar]
	from
		@OperatorStation os
	join
		[dbo].[CalendarHistory] ch with(nolock)
		on ch.[OperatorStation_Id] = os.[Operatorstation_Id]
		and Ch.[ChangeDate] > @from
	--2
	select 
		p.[OperatorStation_Id],
		p.[ProdPlace_Id]
	from 
		[dbo].[ProdPlace] p with(nolock)
	where
		p.[OperatorStation_Id] in (select OperatorStation_ID from @OperatorStation)
	--3
	select
		dto.[ProdPlace_Id],
		dto.[DTOccasion_Id],
		dto.[BeginTime],
		dto.[EndTime],
		LossType = 2
	from
		[dbo].[DowntimeOccasion] dto with(nolock)
	where
		(dto.[EndTime] > @from 
		or dto.[EndTime] is null)
		and dto.[BeginTime] < @now
		and dto.[ProdPlace_Id] in (select p.[ProdPlace_Id] from [dbo].[ProdPlace] p where p.[OperatorStation_Id] in (select OperatorStation_Id from @OperatorStation))
	--4
	declare @orders table(order_id int)
	insert into @orders(order_Id)
	select
		o.[Order_Id]
	from
		[dbo].[Orders] o with(nolock)
	where
		o.[OperatorStation_Id] in (select Operatorstation_Id from @OperatorStation)
		and (o.[EndTime] is null or o.[EndTime] > @from)
		and o.[StartTime] < @now

	select 
		o.[OperatorStation_Id],
		o.[Order_Id],
		o.[StartTime],
		o.[EndTime],
		o.[Active],
		o.[Completed]
	from	
		[dbo].[Orders] o with(nolock)
	where
		o.[Order_ID] in (select Order_Id from @orders)
	--5
	select
		ob.[Order_id],
		ob.[OrderBatch_Id],
		ob.[StartTime],
		ob.[EndTime]
	from	
		[dbo].[OrderBatch] ob with(nolock)
	where
		ob.[Order_Id] in (select Order_Id from @orders)
		and (ob.[EndTime] is null or ob.[EndTime] > @from)
		and ob.[StartTime] < @now
	--6
	select
		pu.[Order_Id],
		pu.[Amount],
		pu.[PUTime]
	from	
		[dbo].[PUTimeEnd] pu with(nolock)
	where
		pu.[Order_Id] in (select Order_Id from @orders)
		and pu.[PUTime] >= @from
		and pu.[PUTime] <= @now
	--7
	select
		pu.[Order_Id],
		pu.[Amount],
		pu.[PUTime]
	from	
		[dbo].[PUTimeStart] pu with(nolock)
	where
		pu.[Order_Id] in (select Order_Id from @orders)
		and pu.[PUTime] >= @from
		and pu.[PUTime] <= @now
	--8
	select
		pu.[Order_Id],
		pu.[Amount],
		pu.[PUTime]
	from	
		[dbo].[PUTimeScrapped] pu with(nolock)
	where
		pu.[Order_Id] in (select Order_Id from @orders)
		and pu.[PUTime] >= @from
		and pu.[PUTime] <= @now
end
/*
declare 
	@operatorStation int,
	@from datetime
select 
	@operatorStation = 1,
	@from = '2017-11-16'

select
	ch.[OperatorStation_Id],
	ch.[CalendarHistory_Id],
	ch.[ChangeType],
	ch.[PeriodStartTime],
	ch.[ChangeDate],
	ch.[Calendar],
	ch.[Updated]
from
	[dbo].[CalendarHistory] ch with(nolock)
where
	ch.[OperatorStation_Id] = @operatorStation
	and Ch.[ChangeDate] > @from
*/