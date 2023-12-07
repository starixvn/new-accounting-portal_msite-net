USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[Acc_AppLog_MonitorLogUpd_xtest]    Script Date: 9/27/2017 3:42:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[Acc_AppLog_MonitorLogUpd]
	@LogIds				VARCHAR(MAX),
	@MonitoringUserId	INT,
	@IsMonitored		BIT
AS

/*
	Created: 20130909@Edward
	Task: Update monitor log
	DB: bodb_CustomerLog

	Revisions:
*/

BEGIN
	SET NOCOUNT ON;

	UPDATE al
	SET al.IsMonitored = @IsMonitored,
		al.MonitoringUserId = @MonitoringUserId
	FROM dbo.AppLog_Log AS al WITH (ROWLOCK, UPDLOCK)
		INNER JOIN dbo.AppLog_CategoryLog AS acl WITH (NOLOCK) ON al.LogID = acl.LogID
		INNER JOIN dbo.AppLog_Log AS a WITH (NOLOCK)
			ON al.Message = a.Message
			AND al.Severity = a.Severity
			AND al.MachineId = a.MachineId
			AND DATEADD(HOUR,DATEDIFF(HOUR, 0, al.Timestamp), 0) = DATEADD(HOUR, DATEDIFF(HOUR, 0, a.Timestamp), 0)
		INNER JOIN dbo.AppLog_CategoryLog AS ac WITH (NOLOCK)
			ON a.LogID = ac.LogID AND acl.CategoryID = ac.CategoryID
	WHERE al.LogId IN (SELECT val FROM dbo.StrSplit(@LogIds,','))
END



GO
GRANT	VIEW DEFINITION	ON	Acc_AppLog_MonitorLogUpd	TO	vnteam
GRANT	EXECUTE	ON	Acc_AppLog_MonitorLogUpd	TO	bodbAdminNet
GO
