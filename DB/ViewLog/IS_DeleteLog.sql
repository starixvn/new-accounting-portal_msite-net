USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_DeleteLog]    Script Date: 9/12/2017 5:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IS_DeleteLog]
	@from DATETIME
AS
BEGIN
	declare  @mytable  table (LogID bigint )		
	INSERT INTO @mytable(LogID) 
		SELECT [AppLog_Log].LogID 	FROM [AppLog_Log] 
		WHERE [AppLog_Log].[Timestamp]<=@from

	DELETE [AppLog_Log] WHERE [AppLog_Log].LogID In
		(SELECT LogID FROM @mytable )

	DELETE FROM AppLog_CategoryLog WHERE AppLog_CategoryLog.LogID In
		(SELECT LogID FROM @mytable )
		
END



GO
GRANT	VIEW DEFINITION	ON	IS_DeleteLog	TO	vnteam
GRANT	EXECUTE	ON	IS_DeleteLog	TO	bodbAdminNet
GO
