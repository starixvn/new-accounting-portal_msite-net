ALTER PROCEDURE Letstalk_Record_GroupByTargetSelect 
     @TargetName NVARCHAR(50), 
     @FromDate   DATETIME, 
     @ToDate     DATETIME
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
           [Group]
    FROM   Letstalk_Record(nolock)
    WHERE  TargetName = CASE
                            WHEN @TargetName = 'All'
                            THEN TargetName
                            ELSE @TargetName
                        END
           AND CreatedDate BETWEEN @FromDate AND @ToDate
    ORDER BY [Group];
END;
GO

GRANT EXECUTE ON dbo.Letstalk_Record_GroupByTargetSelect TO [bodbAdminNet];
GO

GRANT VIEW DEFINITION ON dbo.Letstalk_Record_GroupByTargetSelect TO [vnteam];