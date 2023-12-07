USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[Acc_AppLog_MachineUpd]    Script Date: 9/12/2017 6:11:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Acc_AppLog_MachineUpd]
	@MachineId INT,
	@IsProduction BIT,
	@Timezone SMALLINT,
	@MachineIP NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AppLog_Machine] WITH(ROWLOCK, UPDLOCK)
	SET [IsProduction] = @IsProduction,
		[TimeZone] = @Timezone,
		[MachineIP] = @MachineIP
	WHERE [MachineId] = @MachineId
END


GO
GRANT	VIEW DEFINITION	ON	Acc_AppLog_MachineUpd	TO	vnteam
GRANT	EXECUTE	ON	Acc_AppLog_MachineUpd	TO	bodbAdminNet
GO