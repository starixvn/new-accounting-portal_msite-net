ALTER PROCEDURE Letstalk_Record_NotificationSelect 
     @FromDate   DATETIME, 
     @ToDate     DATETIME, 
     @TargetName NVARCHAR(50) = 'All', 
     @Group      NVARCHAR(50) = 'All'
AS

/*	Creator: 20170329@Robert
	Task: mSite - Support Letstalk log and resend message function [#73400]
	DB: DBMA/bodb_Maintenance
	
	Revisions: 
		 - 20170329@Robert: Initialize
		 - 20171129@Robert: Select log by filter Targetname or Gtoup
		
*/

BEGIN
    SET NOCOUNT ON;
    SELECT Id, 
           TargetName, 
           Subject, 
           Content, 
           [Group], 
           Attachments, 
           InlineImages, 
           CreatedDate, 
           LastSentDate, 
           SendSuccess, 
           SendFail, 
           IsMessage
    FROM   Letstalk_Record(nolock)
    WHERE  CreatedDate BETWEEN @FromDate AND @ToDate
           AND TargetName = CASE
                                WHEN @TargetName = 'All'
                                THEN TargetName
                                ELSE @TargetName
                            END
           AND [Group] = CASE
                             WHEN @Group = 'All'
                             THEN [Group]
                             ELSE @Group
                         END
    ORDER BY id DESC;
END;
GO

GRANT EXECUTE ON dbo.Letstalk_Record_NotificationSelect TO [bodbAdminNet];
GO

GRANT VIEW DEFINITION ON dbo.Letstalk_Record_NotificationSelect TO [vnteam];