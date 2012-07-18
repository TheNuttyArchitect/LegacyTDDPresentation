USE [Paycheck]
GO

/****** Object:  Table [dbo].[AnnualSalary]    Script Date: 7/17/2012 9:35:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AnnualSalary](
	[SalaryId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](9, 2) NOT NULL,
	[BeginDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NULL,
 CONSTRAINT [PK_Salary] PRIMARY KEY CLUSTERED 
(
	[SalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AnnualSalary] ADD  CONSTRAINT [DF_Salary_SalaryId]  DEFAULT (newid()) FOR [SalaryId]
GO

ALTER TABLE [dbo].[AnnualSalary]  WITH CHECK ADD  CONSTRAINT [FK_AnnualSalary_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[AnnualSalary] CHECK CONSTRAINT [FK_AnnualSalary_Employee]
GO

