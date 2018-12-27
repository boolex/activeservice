if (object_id(N'[dbo].[fnCore_DateTimeToBinary]') is not null)
	drop function [dbo].[fnCore_DateTimeToBinary]
go
create function [dbo].[fnCore_DateTimeToBinary](
--declare 
	@datetime datetime
)returns varbinary(8)
begin
	if @datetime is null
		return 0x;
	
	declare @basedate datetime
	set @basedate = '2000-01-01'

	return 
		convert(binary(4), datediff(day, @basedate, @datetime))  
		+ convert(binary(4), datediff(ms, CONVERT(date, @datetime), @datetime)) 
end