SELECT
	 [Order Key] = ISNULL(ROW_NUMBER() OVER(ORDER BY (SELECT NULL)),-1)
	,[City Key]
	,[Customer Key]
	,[Stock Item Key]
	,[Order Date Key]
	,[Picked Date Key]
	,[Salesperson Key]
	,[Picker Key]
	,[WWI Order ID]
	,[WWI Backorder ID]
	,[Description]
	,[Package]
	,[Quantity]
	,[Unit Price]
	,[Tax Rate]
	,[Total Excluding Tax]
	,[Tax Amount]
	,[Total Including Tax]
	,[Lineage Key]
INTO [Fact].[Order_Big_CCI]
FROM [Fact].[Order]
CROSS JOIN
(SELECT * FROM SYS.columns WHERE object_id < 50) tmp
