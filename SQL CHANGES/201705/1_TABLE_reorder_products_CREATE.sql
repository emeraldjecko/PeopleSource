

/****** Object:  Table [dbo].[reorder_products]    Script Date: 5/15/2017 7:20:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[reorder_products](
	[Item Type] [nvarchar](500) NULL,
	[Variation Type] [nvarchar](500) NULL,
	[Teapplix SKU] [nvarchar](500) NULL,
	[Root Teapplix SKU] [nvarchar](500) NULL,
	[Item Title] [nvarchar](500) NULL,
	[Category] [nvarchar](500) NULL,
	[Jet Category Id] [nvarchar](500) NULL,
	[Walmart Category Id] [nvarchar](500) NULL,
	[Walmart Tax Code] [nvarchar](500) NULL,
	[Supplier] [nvarchar](500) NULL,
	[Supplier SKU] [nvarchar](500) NULL,
	[Asin] [nvarchar](500) NULL,
	[UPC] [nvarchar](500) NULL,
	[Location] [nvarchar](500) NULL,
	[Xref3] [nvarchar](500) NULL,
	[Image Small] [nvarchar](500) NULL,
	[Image Large] [nvarchar](500) NULL,
	[Cost] [nvarchar](500) NULL,
	[Default Price] [nvarchar](500) NULL,
	[Customs Value] [nvarchar](500) NULL,
	[Customs Description] [nvarchar](500) NULL,
	[Localized Customs Description] [nvarchar](500) NULL,
	[Harmonized Tariff Code] [nvarchar](500) NULL,
	[Sales Rep] [nvarchar](500) NULL,
	[Commission Rate] [nvarchar](500) NULL,
	[Date] [nvarchar](500) NULL,
	[Dangerous Goods Type] [nvarchar](500) NULL,
	[Country of Origin] [nvarchar](500) NULL,
	[Brand] [nvarchar](500) NULL,
	[Model] [nvarchar](500) NULL,
	[Manufacturer] [nvarchar](500) NULL,
	[Manufacturer Part Number] [nvarchar](500) NULL,
	[Active] [nvarchar](500) NULL
) ON [PRIMARY]

GO


