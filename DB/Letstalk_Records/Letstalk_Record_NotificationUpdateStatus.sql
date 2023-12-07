USE [bodb_Maintenance];
GO

/****** Object:  StoredProcedure [dbo].[Letstalk_Record_NotificationUpdateStatus]    Script Date: 4/24/2017 1:54:15 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[Letstalk_Record_NotificationUpdateStatus] @Id           BIGINT,
                                                                 @IsSuccess    BIT,
                                                                 @ErrorCode    INT           = 0 OUTPUT,
                                                                 @ErrorMessage NVARCHAR(200) = '' OUTPUT
AS

/*	Creator: 20170329@Robert
	Task: mSite - Support Letstalk log and resend message function [#73400]
	DB: DBMA/bodb_Maintenance
	
	Revisions: 
		 - 20170329@Robert: Initialize
		
*/

     BEGIN
         SET NOCOUNT ON;
         UPDATE Letstalk_Record WITH(ROWLOCK, UPDLOCK)
           SET
               SendSuccess = CASE
                                 WHEN @IsSuccess = 1
                                 THEN SendSuccess + 1
                                 ELSE SendSuccess
                             END,
               SendFail = CASE
                              WHEN @IsSuccess = 0
                              THEN SendFail + 1
                              ELSE SendFail
                          END,
               LastSentDate = GETDATE()
         WHERE id = @Id;
         IF @@ERROR <> 0
             BEGIN
                 SELECT @ErrorCode = 101,
                        @ErrorMessage = 'Update does not successfully.';
             END;
     END;
GO
GRANT EXECUTE ON [dbo].[Letstalk_Record_NotificationUpdateStatus] TO [bodbAdminNet];
GO
GRANT VIEW DEFINITION ON [dbo].[Letstalk_Record_NotificationUpdateStatus] TO [vnteam];