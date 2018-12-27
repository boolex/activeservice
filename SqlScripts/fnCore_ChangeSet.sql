if (object_id(N'[dbo].[fnCore_ChangeSet]') is not null)
	drop function [dbo].[fnCore_ChangeSet]
go
print '[dbo].[fnCore_ChangeSet]'
go
create function [dbo].[fnCore_ChangeSet](
--declare 
	@now datetime
)returns varbinary(max)
begin
	declare @basedate datetime
set @basedate = '2000-01-01'

declare @vb varbinary(max)
set @vb = 0x

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[PUTimeEnd_Tracking]))

select
	@vb = @vb 
		+ convert(binary(1), 0x0)
		+ [dbo].[fnCore_DateTimeToBinary](TrackingDate)
		+ convert(binary(1), TrackingType) 
		+ convert(binary(8), convert(float(24), Amount)) 
		+ convert(binary(4), order_id) 
		+ [dbo].[fnCore_DateTimeToBinary](PUTime)
from 
	[dbo].[PUTimeEnd_Tracking]

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[Order_Tracking]))
select
	@vb = @vb 
		+ convert(binary(1), 0 | (case when EndTime is null then 1 else 0 end))
		+ [dbo].[fnCore_DateTimeToBinary](TrackingDate)
		+ convert(binary(1), TrackingType) 
		+ convert(binary(4), OperatorStation_Id) 
		+ convert(binary(4), order_id) 		
		+ [dbo].[fnCore_DateTimeToBinary](StartTime)
		+ [dbo].[fnCore_DateTimeToBinary](EndTime)
		+ convert(binary(8), convert(float(24), AmountPerUnit)) 
		+ convert(binary(8), convert(float(24), AmountPerPulseStart)) 
		+ convert(binary(8), convert(float(24), OpUnitTime))  
from 
	[dbo].[Order_Tracking]

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[OrderBatch_Tracking]))
select
	@vb = @vb 
		+ convert(binary(1), 0 | (case when EndTime is null then 1 else 0 end))
		+ [dbo].[fnCore_DateTimeToBinary](TrackingDate)
		+ convert(binary(1), TrackingType) 
		+ convert(binary(4), OrderBatch_ID) 
		+ convert(binary(4), order_id) 		
		+ [dbo].[fnCore_DateTimeToBinary](StartTime)
		+ [dbo].[fnCore_DateTimeToBinary](EndTime)  
from 
	[dbo].[OrderBatch_Tracking]

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[Downtimeoccasion_Tracking]))
select
	@vb = @vb 
	 +convert(binary(1), 0 | (case when EndTime is null then 1 else 0 end)| (case when LossType is null then 2 else 0 end))
	 +[dbo].[fnCore_DateTimeToBinary](TrackingDate)
	 +convert(binary(1), TrackingType) 
	 +convert(binary(4), DTOccasion_Id) 
	 +convert(binary(4), ProdPlace_Id) 	
	 +[dbo].[fnCore_DateTimeToBinary](BeginTime)
	 +[dbo].[fnCore_DateTimeToBinary](EndTime) 
	 +case when LossType is null then 0x  else convert(binary(4), LossType) end
 from 	
	[dbo].[Downtimeoccasion_Tracking]

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[CalendarHistory_Tracking]))
select
	@vb = @vb 
	 +convert(binary(1), 0)
	 +[dbo].[fnCore_DateTimeToBinary](TrackingDate)
	 +convert(binary(1), TrackingType) 
	 +convert(binary(4), OperatorStation_Id) 
	 +convert(binary(4), CalendarHistory_Id) 	
	 +convert(binary(4), ChangeType) 
	 +convert(binary(4), Calendar) 
	 +[dbo].[fnCore_DateTimeToBinary](PeriodStartTime)
	 +[dbo].[fnCore_DateTimeToBinary](ChangeDate) 	 
 from 	
	[dbo].[CalendarHistory_Tracking]

	return @vb
end