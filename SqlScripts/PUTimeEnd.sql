use OEECoreTest
go
 select datalength(convert(float,4))
set statistics time on
go
insert into PUTimeEnd(Order_Id, AMount, PUTime)
select
	Order_Id = 1056479,
	AMount = 4,
	PUTime = '2015-01-01 00:06:15.000'
from
	PUTimeEnd where Order_Id = 1056478
set statistics time off
go
delete [PUTimeEnd_Tracking]
select * from [PUTimeEnd_Tracking]

drop trigger [dbo].[PUTimeEnd_Tracking_Inserted] 
drop table  [dbo].[PUTimeEnd_Tracking]
create table [dbo].[PUTimeEnd_Tracking](TrackingDate datetime, TrackingType tinyint, Amount float null, Order_ID int not null, PUTime datetime not null)

create trigger [dbo].[PUTimeEnd_Tracking_Inserted] 
on [dbo].[PUTimeEnd]
after insert
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
end


set statistics time on
go
declare @vb varbinary(max)
set @vb = 0x

set @vb = @vb +  convert(binary(4), (select count(1) from [dbo].[PUTimeEnd_Tracking]))

select
top(200)
	@vb = @vb + convert(binary(8), TrackingDate) + convert(binary(1), TrackingType) + convert(binary(8), Amount) + convert(binary(4), order_id) + convert(binary(8), PUTime)
from 
	[dbo].[PUTimeEnd_Tracking]


select DATALENGTH(@vb)
select @vb
set statistics time off
go


select * from [dbo].[PUTimeEnd_Tracking]




DECLARE @q UNIQUEIDENTIFIER = '01234567-89ab-cdef-0123-456789abcdef';
SELECT @q;
SELECT CONVERT(VARBINARY, getdate());


