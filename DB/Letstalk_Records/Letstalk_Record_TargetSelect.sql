ALTER PROCEDURE Letstalk_Record_TargetSelect 
     @FromDate DATETIME, 
     @ToDate   DATETIME
AS

/*	Creator: 20171129@Robert
	Task: mSite - Support Letstalk log and resend message function [#73400]
	DB: DBMA/bodb_Maintenance
	
	Revisions: 
		 - 20171129@Robert: Initialize		 
		
*/

BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT 
           TargetName
    FROM   Letstalk_Record(nolock)
    WHERE  CreatedDate BETWEEN @FromDate AND @ToDate
    ORDER BY TargetName;
END;
GO

GRANT EXECUTE ON dbo.Letstalk_Record_TargetSelect TO [bodbAdminNet];
GO

GRANT VIEW DEFINITION ON dbo.Letstalk_Record_TargetSelect TO [vnteam];