
/****** Object:  Table [dbo].[order_report]    Script Date: 5/15/2017 7:41:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[order_report](
	[order_source] [nvarchar](500) NULL,
	[account] [nvarchar](500) NULL,
	[txn_id] [nvarchar](500) NULL,
	[txn_id2] [nvarchar](500) NULL,
	[txn_seq] [nvarchar](500) NULL,
	[date] [nvarchar](500) NULL,
	[status] [nvarchar](500) NULL,
	[payment_type] [nvarchar](500) NULL,
	[payment_auth_info] [nvarchar](500) NULL,
	[name] [nvarchar](500) NULL,
	[payer_email] [nvarchar](500) NULL,
	[contact_phone] [nvarchar](500) NULL,
	[address_country] [nvarchar](500) NULL,
	[address_state] [nvarchar](500) NULL,
	[address_zip] [nvarchar](500) NULL,
	[address_city] [nvarchar](500) NULL,
	[address_street] [nvarchar](500) NULL,
	[address_street2] [nvarchar](500) NULL,
	[total] [nvarchar](500) NULL,
	[shipping] [nvarchar](500) NULL,
	[tax] [nvarchar](500) NULL,
	[discount] [nvarchar](500) NULL,
	[fee] [nvarchar](500) NULL,
	[ship_date] [nvarchar](500) NULL,
	[carrier] [nvarchar](500) NULL,
	[method] [nvarchar](500) NULL,
	[weight] [nvarchar](500) NULL,
	[tracking] [nvarchar](500) NULL,
	[postage] [nvarchar](500) NULL,
	[postage_account] [nvarchar](500) NULL,
	[num_order_lines] [nvarchar](500) NULL,
	[line_number] [nvarchar](500) NULL,
	[item_name] [nvarchar](500) NULL,
	[item_number] [nvarchar](500) NULL,
	[item_sku] [nvarchar](500) NULL,
	[location] [nvarchar](500) NULL,
	[xref3] [nvarchar](500) NULL,
	[quantity] [nvarchar](500) NULL,
	[subtotal] [nvarchar](500) NULL,
	[item_description] [nvarchar](500) NULL,
	[queue_id] [nvarchar](500) NULL
) ON [PRIMARY]

GO


