USE [Paycheck]
GO

/****** Object:  Table [dbo].[Deduction]    Script Date: 7/17/2012 9:35:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Deduction](
	[DeductionId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[PaymentId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](6, 2) NOT NULL,
	[Type] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Deduction] PRIMARY KEY CLUSTERED 
(
	[DeductionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Deduction] ADD  CONSTRAINT [DF_Deduction_DeductionId]  DEFAULT (newid()) FOR [DeductionId]
GO

ALTER TABLE [dbo].[Deduction] ADD  CONSTRAINT [DF_Deduction_Type]  DEFAULT ('Unknown') FOR [Type]
GO

ALTER TABLE [dbo].[Deduction]  WITH CHECK ADD  CONSTRAINT [FK_Deduction_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([PaymentId])
GO

ALTER TABLE [dbo].[Deduction] CHECK CONSTRAINT [FK_Deduction_Payment]
GO

