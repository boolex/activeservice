if (object_id(N'[dbo].[spCore_Send_ChangeSet]') is not null)
drop proc [dbo].[spCore_Send_ChangeSet]
go

print '[dbo].[spCore_Send_ChangeSet]'
go

create proc [dbo].[spCore_Send_ChangeSet](
	@now datetime = null
)
as 
begin
select [dbo].[fnCore_ChangeSet](getdate())
end

