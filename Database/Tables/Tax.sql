USE [Paycheck]
GO

/****** Object:  Table [dbo].[Tax]    Script Date: 7/17/2012 9:36:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Tax](
	[TaxId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[PaymentId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](6, 2) NOT NULL,
	[Type] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Tax] PRIMARY KEY CLUSTERED 
(
	[TaxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Tax] ADD  CONSTRAINT [DF_Tax_TaxId]  DEFAULT (newid()) FOR [TaxId]
GO

ALTER TABLE [dbo].[Tax] ADD  CONSTRAINT [DF_Tax_Type]  DEFAULT ('Unknown') FOR [Type]
GO

ALTER TABLE [dbo].[Tax]  WITH CHECK ADD  CONSTRAINT [FK_Tax_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([PaymentId])
GO

ALTER TABLE [dbo].[Tax] CHECK CONSTRAINT [FK_Tax_Payment]
GO

