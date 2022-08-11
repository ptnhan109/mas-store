create procedure [mas].[sp_report_revenue]
	@startDate datetime = null,
	@endDate datetime = null,
	@categoryId uniqueidentifier = null,
	@userId uniqueidentifier = null,
	@customerId uniqueidentifier = null
as
begin
	declare @units table(id int, [name] nvarchar(20));
	select
		detail.Id as Id,
		detail.[Name] as [ProductName],
		detail.Quantity as Quantity,
		detail.CurrentImport as ImportPrice,
		detail.CurrentPrice as SellPrice,
		detail.Profit as Profit,
		detail.Discount as Discount,
		category.[Name] as Category,
		detail.UnitId as Unit,
		case when invoice.CustomerId is null then 'Khách lẻ' else cus.[name] end as CustomerName
	
	from InvoiceDetails detail
	inner join Products product on product.Id = detail.ProductId
	inner join Category category on category.Id = product.CategoryId
	inner join Invoices invoice on invoice.Id = detail.InvoiceId
	left join Customers cus on cus.Id = invoice.CustomerId
	where
		(@startDate is null or detail.CreatedAt >= @startDate) and
		(@endDate is null or detail.CreatedAt <= @endDate) and
		(@categoryId is null or category.Id = @categoryId) and
		(@userId is null or invoice.EmployeeId = @userId) and
		(@customerId is null or cus.Id = @customerId)
	
end