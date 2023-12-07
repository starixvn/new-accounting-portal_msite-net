USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogCategory_ALL]    Script Date: 9/12/2017 5:04:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Stored Procedure dbo.IS_SelLogCategory_ALL    Script Date: 3/8/2012 4:58:15 PM ******/
ALTER PROCEDURE [dbo].[IS_SelLogCategory_ALL]
AS
BEGIN
	SELECT * FROM AppLog_category ORDER BY [CategoryName]
END


GO
GRANT	VIEW DEFINITION	ON	IS_SelLogCategory_ALL	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogCategory_ALL	TO	bodbAdminNet
GO
