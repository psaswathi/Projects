USE [Payroll]
GO
/****** Object:  Table [dbo].[EmployeeList]    Script Date: 26/06/2022 10:11:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCode] [int] NOT NULL,
	[EmployeeName] [varchar](50) NOT NULL,
	[JobCode] [int] NOT NULL,
	[JoiningDate] [date] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ExitDate] [date] NULL,
 CONSTRAINT [PK_EmployeeList] PRIMARY KEY CLUSTERED 
(
	[EmployeeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobTitleList]    Script Date: 26/06/2022 10:11:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobTitleList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[JobCode] [int] NOT NULL,
	[JobName] [varchar](50) NOT NULL,
	[JobDesc] [varchar](250) NOT NULL,
	[Salary] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_JobTitleList] PRIMARY KEY CLUSTERED 
(
	[JobCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EmployeeList] ON 

INSERT [dbo].[EmployeeList] ([ID], [EmployeeCode], [EmployeeName], [JobCode], [JoiningDate], [CreatedDate], [ModifiedDate], [ExitDate]) VALUES (1, 100001, N'Aswathi PS', 101, CAST(N'2022-06-02' AS Date), CAST(N'2022-06-26T13:44:17.923' AS DateTime), CAST(N'2022-06-26T13:51:52.330' AS DateTime), NULL)
INSERT [dbo].[EmployeeList] ([ID], [EmployeeCode], [EmployeeName], [JobCode], [JoiningDate], [CreatedDate], [ModifiedDate], [ExitDate]) VALUES (2, 100002, N'Priya Mathew', 102, CAST(N'2022-06-22' AS Date), CAST(N'2022-06-26T14:35:21.313' AS DateTime), CAST(N'2022-06-26T14:35:21.313' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[EmployeeList] OFF
GO
SET IDENTITY_INSERT [dbo].[JobTitleList] ON 

INSERT [dbo].[JobTitleList] ([ID], [JobCode], [JobName], [JobDesc], [Salary], [CreatedDate], [ModifiedDate]) VALUES (1, 101, N'Software Developer', N'Develop innovative Solutions', CAST(100.00 AS Decimal(18, 2)), CAST(N'2022-06-26T12:02:33.470' AS DateTime), CAST(N'2022-06-26T13:56:13.880' AS DateTime))
INSERT [dbo].[JobTitleList] ([ID], [JobCode], [JobName], [JobDesc], [Salary], [CreatedDate], [ModifiedDate]) VALUES (2, 102, N'Project Manager', N'Mange software project team', CAST(200.00 AS Decimal(18, 2)), CAST(N'2022-06-26T14:32:26.567' AS DateTime), CAST(N'2022-06-26T14:32:26.567' AS DateTime))
SET IDENTITY_INSERT [dbo].[JobTitleList] OFF
GO
ALTER TABLE [dbo].[EmployeeList]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeList] FOREIGN KEY([JobCode])
REFERENCES [dbo].[JobTitleList] ([JobCode])
GO
ALTER TABLE [dbo].[EmployeeList] CHECK CONSTRAINT [FK_EmployeeList]
GO
