USE [PeopleSource]
GO

/****** Object:  View [dbo].[vw_ReorderProducts]    Script Date: 8/25/2017 9:33:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER VIEW [dbo].[vw_ReorderProducts]
AS
SELECT 
	A.[SKU],
	A.[Date Last Sale],
	A.[Date product was last stocked] AS StockDate,
	A.[Quantity on Hand],
	A.[Total Number of Units Sold between Last Stock and Last sale date] AS TotalNumberOfUnitsSoldBetweenLastReStockAndLastSaleDate,
    ISNULL(SUM(b.[Total Number of Units Sold in Past 30 days]),0) AS TotalNumberOfUnitsSoldInPast30Days,
	ISNULL(CAST((SUM(b.[Total Number of Units Sold in Past 30 days]) / 30.00) AS DECIMAL(18,2)),0) AS DailyUnitsSoldRateForPast30Days,
	op.[Item Title]  [Product Title],
	ISNULL(DATEDIFF(DAY ,a.[Date product was last stocked],a.[Date Last Sale]),0) AS 'TotalNumberOfDaysFromLastStockToLastSaleDate',
	CASE WHEN ISNULL(DATEDIFF(DAY ,a.[Date product was last stocked],a.[Date Last Sale]),0) = 0 
	       THEN 0
		   ELSE CAST(CAST(A.[Total Number of Units Sold between Last Stock and Last sale date]  AS DECIMAL(18,2)) / NULLIF(ISNULL(DATEDIFF(DAY ,a.[Date product was last stocked],a.[Date Last Sale]) + 1,0),0) AS DECIMAL(18,2))  
	END AS 'DailyUnitsSoldRateFromLastRestockToLastSaleDate'
		
FROM 
(
	SELECT final.SKU, final.[Date Last Sale],final.[Date product was last stocked], final.[Quantity on Hand],
		ISNULL(SUM(CAST(o.quantity AS INT)),0) AS 'Total Number of Units Sold between Last Stock and Last sale date'
	FROM
	(
		SELECT * FROM 
		(
			SELECT  p. [Teapplix SKU] as SKU, 
				CASE WHEN  isnull(p.[Customs Description],'')='' 
						 THEN NULL 
					 WHEN  ISDATE(p.[Customs Description]) = 1
						 THEN CONVERT(DATE,CONVERT(DATETIME,p.[Customs Description],101))
					 ELSE NULL END  AS 'Date product was last stocked', 
				q.[Qty on Hand] 'Quantity on Hand',
				x.[Date] 'Date Last Sale',
				ROW_NUMBER() OVER ( PARTITION BY p.[Teapplix SKU] ORDER BY CONVERT(DATE,x.[Date],111) DESC,p. [Teapplix SKU] ASC) r
			FROM [PeopleSource].[dbo].[reorder_products] P			 
			LEFT JOIN [PeopleSource].[dbo].quantity_report q
				ON q.[Teapplix SKU]=P.[Teapplix SKU]
			LEFT JOIN (	
				SELECT item_sku,[date] 
				FROM( 
					SELECT 	ROW_NUMBER() OVER ( PARTITION BY [item_sku] ORDER BY CONVERT(DATE,[date],111) DESC) r , 
					--SUBSTRING([item_sku],0,30) AS [item_sku] ,
					item_sku,	 
					CONVERT(DATE,[date],111) [date]  
					FROM  [PeopleSource].[dbo].[order_report]  
					WHERE [item_sku]<>'' AND [item_sku] LIKE '%AAA-M1%'	AND order_source NOT IN ('ret','rma') 
			  	) o 	
				WHERE r = 1  AND 
				CONVERT(VARCHAR,CONVERT(DATETIME ,[date]),111) <= CONVERT(VARCHAR,CONVERT(DATE, GETDATE()) ,111)  --DateTime Conversion corrected here
			) x ON P.[Teapplix SKU]=x.[item_sku]	 
		) final1 WHERE  SKU LIKE '%AAA-M1%' AND r=1
	 )final
LEFT JOIN [PeopleSource].[dbo].order_report o ON o.item_SKU = final.SKU --LIKE '%'+SUBSTRING(final.SKU,0,30) +'%' 
	--LIKE '%'+ final.SKU +'%' 
	AND  [item_sku] <>'' AND order_source NOT IN ('ret','rma')
	AND (
	(o.[date]  >= final. [Date product was last stocked] AND o.[date]  < DATEADD(day,1, final.[Date Last Sale])) 
	OR (final. [Date product was last stocked] IS NULL AND  final.[Date Last Sale] IS NULL)
	OR (final. [Date product was last stocked] IS NULL AND o.[date]  < DATEADD(day,1, final.[Date Last Sale]))
	OR ( final.[Date Last Sale] IS NULL AND o.[date]  >= final. [Date product was last stocked])
	)
	group by SKU,[Date Last Sale],[Date product was last stocked],[Quantity on Hand]
	) A
LEFT JOIN 
(
SELECT [item_sku] SKU ,(cast(quantity AS INT)) AS 'Total Number of Units Sold in Past 30 days' 
FROM  [PeopleSource].[dbo].[order_report]
WHERE [item_sku]<>''
	AND order_source NOT IN ('ret','rma') 
	AND  CONVERT(DATE,[date],111) BETWEEN CONVERT(DATE,DATEADD(DAY,-30,GETDATE()) ,111)  
	AND CONVERT(VARCHAR,CONVERT(DATE, GETDATE()) ,111)  --DateTime Conversion corrected here
		
)  B ON b.SKU = A.SKU --LIKE '%'+A.SKU+'%'
INNER JOIN [PeopleSource].[dbo].reorder_products op ON op.[Teapplix SKU] = A.SKU
GROUP BY A.SKU,[Date Last Sale],[Date product was last stocked],[Quantity on Hand],a.[Total Number of Units Sold between Last Stock and Last sale date],op.[Item Title]



GO


