USE [Paycheck]
GO

/****** Object:  Table [dbo].[TaxRate]    Script Date: 8/5/2012 11:34:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaxRate](
	[TaxRateId] [uniqueidentifier] NOT NULL,
	[TaxTypeId] [int] NOT NULL,
	[Rate] [decimal](4, 3) NOT NULL,
	[StartDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NOT NULL,
	[MinimumSalary] [decimal](9, 2) NULL,
	[MaximumSalary] [decimal](9, 2) NULL,
 CONSTRAINT [PK_TaxRate] PRIMARY KEY CLUSTERED 
(
	[TaxRateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TaxRate] ADD  CONSTRAINT [DF_TaxRate_TaxRateId]  DEFAULT (newid()) FOR [TaxRateId]
GO

ALTER TABLE [dbo].[TaxRate]  WITH CHECK ADD  CONSTRAINT [FK_TaxRate_TaxType] FOREIGN KEY([TaxTypeId])
REFERENCES [dbo].[TaxType] ([TaxTypeId])
GO

ALTER TABLE [dbo].[TaxRate] CHECK CONSTRAINT [FK_TaxRate_TaxType]
GO

