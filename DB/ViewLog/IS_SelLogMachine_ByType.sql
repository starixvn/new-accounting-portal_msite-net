USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogMachine_ByType]    Script Date: 9/12/2017 5:21:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jason.Ly
-- Create date: 2013-08-07
-- Description:	Get machines by machine type
--		- 20151009@Ricky: add machine ip
-- =============================================
ALTER PROCEDURE [dbo].[IS_SelLogMachine_ByType] 
	@MachineType BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [MachineId], [MachineName], [MachineIP], [IsProduction], [TimeZone]
	FROM [dbo].[AppLog_Machine] WITH(NOLOCK)
	WHERE (@MachineType IS NULL OR [IsProduction] = @MachineType)
	ORDER BY [MachineName]
END


GO
GRANT	VIEW DEFINITION	ON	IS_SelLogMachine_ByType	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogMachine_ByType	TO	bodbAdminNet
GO
