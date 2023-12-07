USE [bodb_Maintenance]
GO

/****** Object:  Index [IDX_TargetName_Group]    Script Date: 11/29/2017 10:44:57 AM ******/
CREATE NONCLUSTERED INDEX [IDX_TargetName_Group] ON [dbo].[Letstalk_Record]
(
	[TargetName] ASC,
	[Group] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


