USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogMachine_ById]    Script Date: 9/12/2017 5:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jason.Ly
-- Create date: 2013-08-08
-- Description:	Get machine by machine id
--		- 20151009@Ricky: add machine ip
-- =============================================
ALTER PROCEDURE [dbo].[IS_SelLogMachine_ById] 
	@MachineId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [MachineId], [MachineName], [IsProduction], [TimeZone], [MachineIP]
	FROM [dbo].[AppLog_Machine] WITH(NOLOCK)
	WHERE ([MachineId] = @MachineId)
END




GO
GRANT	VIEW DEFINITION	ON	IS_SelLogMachine_ById	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogMachine_ById	TO	bodbAdminNet
GO
