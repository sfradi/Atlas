USE [ATLASQUANTUMTEST]
GO
/****** Object:  Table [dbo].[ACCOUNT_HISTORY]    Script Date: 07/23/2019 02:33:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCOUNT_HISTORY](
	[TransitionId] [uniqueidentifier] NULL,
	[AccountId] [uniqueidentifier] NULL,
	[Amount] [decimal](10, 2) NULL,
	[Type] [nvarchar](1) NULL,
	[ReceivedFrom] [uniqueidentifier] NULL,
	[OccurDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 07/23/2019 02:33:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCOUNT](
	[AccountId] [uniqueidentifier] NULL,
	[PersonName] [nvarchar](512) NULL,
	[Amount] [decimal](10, 2) NULL,
	[Ballance] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
