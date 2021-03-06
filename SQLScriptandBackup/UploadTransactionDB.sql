/****** Please first create db ******/
Create Database [UploadTransactionsDB]

/****** create table ******/
USE [UploadTransactionsDB]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 1/23/2022 4:01:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[CurrencyCode] [varchar](3) NOT NULL,
	[CurrencyName] [varchar](50) NULL,
 CONSTRAINT [PK_CurrencyCode] PRIMARY KEY CLUSTERED 
(
	[CurrencyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileRecord]    Script Date: 1/23/2022 4:01:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileRecord](
	[FileId] [varchar](10) NOT NULL,
	[FileName] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUser] [varchar](50) NULL,
	[CreatedIPAddress] [varchar](50) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvalidTransaction_Log]    Script Date: 1/23/2022 4:01:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvalidTransaction_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [varchar](50) NULL,
	[Amount] [float] NULL,
	[Currency] [varchar](3) NULL,
	[TransactionDate] [datetime] NULL,
	[Status] [varchar](10) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedIPAddress] [varchar](50) NULL,
 CONSTRAINT [PK_InvalidTransaction_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logging]    Script Date: 1/23/2022 4:01:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logging](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Thread] [varchar](255) NULL,
	[Level] [varchar](10) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](255) NULL,
	[Exception] [varchar](max) NULL,
 CONSTRAINT [PK_Logging] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionData]    Script Date: 1/23/2022 4:01:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionData](
	[FileId] [varchar](10) NOT NULL,
	[TransactionId] [varchar](50) NOT NULL,
	[Amount] [float] NULL,
	[CurrencyCode] [varchar](3) NULL,
	[TransactionDate] [datetime] NULL,
	[Status] [varchar](8) NULL,
 CONSTRAINT [PK_TransactionDetail] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'AUD', N'Australian Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'BDT', N'Bangladeshi Taka')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'CAD', N'Canadian Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'CHF', N'Swiss Franc')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'CNY', N'Yuan Renminbi')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'DKK', N'Danish Krone')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'EUR', N'Euro')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'GBP', N'Pound Sterling')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'HKD', N'Hong Kong Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'HTG', N'Gourde')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'IDR', N'Indonesian Rupiah')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'JPY', N'Yen')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'KHR', N'Riel')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'KRW', N'Korean Won')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'LAK', N'Kip')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'MMK', N'Myanmar Kyat')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'MYR', N'Malaysian Ringgit')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'NOK', N'Norwegian Krone')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'NZD', N'New Zealand Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'PHP', N'Philippine Peso')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'RUB', N'Russian Ruble')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'SEK', N'Swedish Krona')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'SGD', N'Singapore Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'THB', N' Baht')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'TWD', N'Taiwan Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'USD', N'US Dollar')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'VND', N'Viet Num Dong')
INSERT [dbo].[Currency] ([CurrencyCode], [CurrencyName]) VALUES (N'YER', N'Yemeni Rial')
GO
