use oeecoretest
go

select * from Calendarhistory with(nolock)  where ChangeDate > '2018-01-01'
select * from DowntimeOccasion  with(nolock)  where (EndTime > '2018-01-01' or endtime is null)
select OperatorStation_Id, StartTime, EndTime, Active, Completed from Orders with(nolock)  where EndTime > '2018-01-01' or EndTime is null
select * from OrderBatch with(nolock)  where EndTime > '2018-01-01'
select * from PUTimeEnd with(nolock) where PUTime > '2018-01-01' and order_id in(select o.Order_Id from Orders o where o.EndTime >= '2018-01-01' or o.endtime is null)
select * from PUTimeStart with(nolock)  where PUTime > '2018-01-01' and order_id in(select o.Order_Id from Orders o where o.EndTime > '2018-01-01' or o.endtime is null)


delete Calendarhistory where ChangeDate > '2018-01-01'
delete DowntimeOccasion where dtoccasion_Id in (select d.dtoccasion_Id from downtimeoccasion d with(nolock) where  d.endtime > '2018-01-01')
delete DowntimeOccasion where dtoccasion_Id in (select d.dtoccasion_Id from downtimeoccasion d with(nolock) where  d.endtime is null)
delete OrderBatch where Order_Id in (select o.Order_Id from Orders o where o.EndTime > '2018-01-01' or o.endtime is null)
delete Orders where EndTime > '2018-01-01'or EndTime is null
delete PUTimeEnd where PUTime > '2018-01-01' and order_id in(select o.Order_Id from Orders o where o.EndTime > '2018-01-01' or o.endtime is null)
delete PUTimeStart where PUTime > '2018-01-01' and order_id in(select o.Order_Id from Orders o where o.EndTime > '2018-01-01' or o.endtime is null)

delete PUTimeEnd_Tracking
delete OrderBatch_Tracking
delete Order_Tracking
delete DowntimeOccasion_Tracking
delete CalendarHistory_Tracking

select * from DowntimeOccasion_Tracking


