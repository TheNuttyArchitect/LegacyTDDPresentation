USE [Paycheck]
GO

/****** Object:  Table [dbo].[AvailableEmployeeDeduction]    Script Date: 8/4/2012 8:51:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AvailableEmployeeDeduction](
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[DeductionTypeId] [int] NOT NULL,
	[StartDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_AvailableEmployeeDeduction] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[DeductionTypeId] ASC,
	[StartDate] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AvailableEmployeeDeduction]  WITH CHECK ADD  CONSTRAINT [FK_AvailableEmployeeDeduction_AvailableEmployeeDeduction] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[AvailableEmployeeDeduction] CHECK CONSTRAINT [FK_AvailableEmployeeDeduction_AvailableEmployeeDeduction]
GO

ALTER TABLE [dbo].[AvailableEmployeeDeduction]  WITH CHECK ADD  CONSTRAINT [FK_AvailableEmployeeDeduction_DeductionType] FOREIGN KEY([DeductionTypeId])
REFERENCES [dbo].[DeductionType] ([TypeId])
GO

ALTER TABLE [dbo].[AvailableEmployeeDeduction] CHECK CONSTRAINT [FK_AvailableEmployeeDeduction_DeductionType]
GO

