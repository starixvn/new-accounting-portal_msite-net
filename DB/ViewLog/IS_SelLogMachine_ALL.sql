USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogMachine_ALL]    Script Date: 9/12/2017 5:16:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jason.Ly
-- Create date: 2013-08-07
-- Description:	Get all machine names
-- =============================================
ALTER PROCEDURE [dbo].[IS_SelLogMachine_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [MachineId], [MachineName], [IsProduction], [TimeZone]
	FROM [dbo].[AppLog_Machine] WITH(NOLOCK)
	ORDER BY [MachineName]
END



GO
GRANT	VIEW DEFINITION	ON	IS_SelLogMachine_ALL	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogMachine_ALL	TO	bodbAdminNet
GO
