ALTER PROCEDURE Letstalk_Record_NotificationInsert @TargetName   NVARCHAR(50),
                                                   @Group        NVARCHAR(50),
                                                   @Subject      NVARCHAR(250),
                                                   @Content      NVARCHAR(MAX),
                                                   @Attachments  NVARCHAR(250) = NULL,
                                                   @InlineImages NVARCHAR(250) = NULL,
                                                   @IsMessage    BIT,
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
         DECLARE @CreatedDate DATETIME, @SendSuccess TINYINT, @SendFail TINYINT;
         SELECT @CreatedDate = GETDATE(),
                @SendSuccess = CASE
                                   WHEN @IsSuccess = 1
                                   THEN 1
                                   ELSE 0
                               END,
                @SendFail = CASE
                                WHEN @IsSuccess = 0
                                THEN 1
                                ELSE 0
                            END;
         INSERT INTO Letstalk_Record
         (TargetName,
          [Group],
          [Subject],
          [Content],
          Attachments,
          InlineImages,
          CreatedDate,
          SendSuccess,
          SendFail,
          IsMessage
         )
         VALUES
         (@TargetName,
          @Group,
          @Subject,
          @Content,
          @Attachments,
          @InlineImages,
          @CreatedDate,
          @SendSuccess,
          @SendFail,
          @IsMessage
         );
         IF @@ERROR <> 0
             BEGIN
                 SELECT @ErrorCode = 101,
                        @ErrorMessage = 'Insert does not successfully.';
             END;
     END;
GO
GRANT EXECUTE ON [dbo].[Letstalk_Record_NotificationInsert] TO [bodbAdminNet];
GO
GRANT VIEW DEFINITION ON [dbo].[Letstalk_Record_NotificationInsert] TO [vnteam];