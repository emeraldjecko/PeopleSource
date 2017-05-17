/****** Object:  Table [dbo].[quantity_report]    Script Date: 5/15/2017 7:40:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[quantity_report](
	[Teapplix SKU] [nvarchar](500) NULL,
	[Column 1] [nvarchar](500) NULL,
	[Asin] [nvarchar](500) NULL,
	[UPC] [nvarchar](500) NULL,
	[Location] [nvarchar](500) NULL,
	[Xref3] [nvarchar](500) NULL,
	[Unit Cost] [nvarchar](500) NULL,
	[Total Cost] [nvarchar](500) NULL,
	[Qty on Hand] [nvarchar](500) NULL,
	[Qty to Ship] [nvarchar](500) NULL,
	[Qty Available] [nvarchar](500) NULL,
	[Qty Reserve] [nvarchar](500) NULL,
	[Item Title] [nvarchar](500) NULL
) ON [PRIMARY]

GO


