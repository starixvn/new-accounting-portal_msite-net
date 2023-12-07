USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[Acc_AppLog_MachineIns]    Script Date: 9/12/2017 6:10:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Acc_AppLog_MachineIns]
	@MachineName NVARCHAR(50),
	@IsProduction BIT,
	@Timezone SMALLINT,
	@MachineIP NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[AppLog_Machine]
			([MachineName]
			,[IsProduction]
			,[TimeZone]
			,[MachineIP])
	VALUES
		(@MachineName
		,@IsProduction
		,@Timezone
		,@MachineIP)

	RETURN @@IDENTITY
END



GO
GRANT	VIEW DEFINITION	ON	Acc_AppLog_MachineIns	TO	vnteam
GRANT	EXECUTE	ON	Acc_AppLog_MachineIns	TO	bodbAdminNet
GO
