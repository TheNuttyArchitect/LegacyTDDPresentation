USE [Paycheck]
GO

/****** Object:  Table [dbo].[DeductionRate]    Script Date: 7/20/2012 10:51:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeductionRate](
	[DeductionRateId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DeductionTypeId] [int] NOT NULL,
	[Amount] [decimal](7, 2) NOT NULL,
	[StartDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DeductionRate] ADD  CONSTRAINT [DF_DeductionRate_DeductionRateId]  DEFAULT (newid()) FOR [DeductionRateId]
GO

ALTER TABLE [dbo].[DeductionRate]  WITH CHECK ADD  CONSTRAINT [FK_DeductionRate_DeductionType] FOREIGN KEY([DeductionTypeId])
REFERENCES [dbo].[DeductionType] ([TypeId])
GO

ALTER TABLE [dbo].[DeductionRate] CHECK CONSTRAINT [FK_DeductionRate_DeductionType]
GO

