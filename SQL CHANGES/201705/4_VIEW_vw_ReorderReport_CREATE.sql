
/****** Object:  View [dbo].[vw_ReorderProducts]    Script Date: 5/10/2017 12:35:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_ReorderProducts]
AS


SELECT A.*,
	SUM(b.[Total Number of Units Sold in Past 30 days]) AS TotalNumberOfUnitsSoldInPast30Days,
	CAST((SUM(b.[Total Number of Units Sold in Past 30 days]) / 30.00) AS DECIMAL(18,2)) AS DailyUnitsSoldRateForPast30Days,
	op.[Item Title]  [Product Title],
	isnull(datediff(day ,a.[Date product was last stocked],a.[Date Last Sale]),0) AS 'TotalNumberOfDaysFromLastStockToLastSaleDate',
	CASE WHEN isnull(datediff(day ,a.[Date product was last stocked],a.[Date Last Sale]),0) = 0 
	       THEN 0
		   ELSE CAST(CAST(A.[Total Number of Units Sold between Last Stock and Last sale date]  AS DECIMAL(18,2)) / isnull(datediff(day ,a.[Date product was last stocked],a.[Date Last Sale]),0) AS DECIMAL(18,2))  
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
				CASE WHEN  isnull(p.[Customs Description],'')='' THEN NULL ELSE CONVERT(DATE,CONVERT(DATETIME,p.[Customs Description],101))END  AS 'Date product was last stocked',  
				q.[Qty on Hand] 'Quantity on Hand',
				x.[Date] 'Date Last Sale',
				ROW_NUMBER() OVER ( PARTITION BY SUBSTRING( p. [Teapplix SKU],0,30) ORDER BY CONVERT(DATE,x.[Date],111) DESC,p. [Teapplix SKU] ASC) r
			FROM [PeopleSource].[dbo].[reorder_products] P			 
			LEFT JOIN [PeopleSource].[dbo].quantity_report q
				ON q.[Teapplix SKU]=P.[Teapplix SKU]
			LEFT JOIN (	
				SELECT item_sku,[date] 
				FROM( 
					SELECT 	ROW_NUMBER() OVER ( PARTITION BY SUBSTRING([item_sku],0,30) ORDER BY CONVERT(DATE,[date],111) DESC) r , SUBSTRING([item_sku],0,30) AS [item_sku] ,	 CONVERT(DATE,[date],111) [date]  
					FROM  [PeopleSource].[dbo].[order_report]  
					WHERE [item_sku]<>'' AND [item_sku] LIKE '%AAA-M1%'	AND order_source NOT IN ('ret','rma') 
			  	) o 	
				WHERE r = 1  AND CONVERT(VARCHAR,CONVERT(DATETIME ,[date]),111) <= CONVERT(VARCHAR,CONVERT(DATE, GETDATE()) ,111)  --DateTime Conversion corrected here
			) x ON P.[Teapplix SKU]=x.[item_sku]	 
		) final1 WHERE  SKU LIKE '%AAA-M1%' AND r=1
	 )final
LEFT JOIN [PeopleSource].[dbo].order_report o ON o.item_SKU  LIKE '%'+SUBSTRING(final.SKU,0,30) +'%' AND  [item_sku] <>'' AND order_source NOT IN ('ret','rma')
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
		
)  B ON b.SKU LIKE '%'+A.SKU+'%'
INNER JOIN [PeopleSource].[dbo].reorder_products op ON op.[Teapplix SKU] =SUBSTRING(A.SKU,0,30)
GROUP BY A.SKU,[Date Last Sale],[Date product was last stocked],[Quantity on Hand],a.[Total Number of Units Sold between Last Stock and Last sale date],op.[Item Title]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ReorderProducts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ReorderProducts'
GO


