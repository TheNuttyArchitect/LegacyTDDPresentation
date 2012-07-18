USE [Paycheck]
GO

/****** Object:  Table [dbo].[Payment]    Script Date: 7/17/2012 9:35:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Payment](
	[PaymentId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[Paydate] [date] NOT NULL,
	[TotalAmount] [decimal](7, 2) NOT NULL,
	[TotalDeductions] [decimal](6, 2) NOT NULL,
	[TotalTaxes] [decimal](6, 2) NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [DF_Payment_TotalDeductions]  DEFAULT ((0)) FOR [TotalDeductions]
GO

ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [DF_Payment_TotalTaxes]  DEFAULT ((0)) FOR [TotalTaxes]
GO

ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Employee]
GO

