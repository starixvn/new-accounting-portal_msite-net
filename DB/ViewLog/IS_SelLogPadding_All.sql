USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogPadding_All]    Script Date: 9/12/2017 5:28:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[IS_SelLogPadding_All]
	@CategoryId				INT,
	@Severity				NVARCHAR(32),
	@MachineId				INT			= 0,
	@IsProduction			BIT			= NULL,
	@IsMonitored			BIT			= NULL,
	@ToGMT					SMALLINT	= 7,
	@From					DATETIME,
	@To						DATETIME,
	@SearchText				NVARCHAR(100)	= '',
	@RemovedLogCategories	NVARCHAR(500)	= '',
	@Page					INT,
	@Size					INT,
	@TotalRows				INT			= 0		OUTPUT
AS

/*
	- Created: 20130909@Edward
	- Task: Get log
	- DB: DBLOG.bodb_CustomerLog
	- Revisions:
		- 20150929@Kos: Modify to enhance performance
		- 20151009@Kos: Add machine ip
		- 20170915@Kos: Add removed log categories, search text [Redmine: #72546]

	- Param's Explanation :
 */

BEGIN
	SET NOCOUNT ON

	CREATE TABLE #TempTable (Id BIGINT, Num INT, CategoryID INT)
	CREATE CLUSTERED INDEX IDX_CategoryID ON #TempTable (CategoryID)

	CREATE TABLE #RemovedLogCategoryTable (CategoryId INT)

	IF @RemovedLogCategories IS NOT NULL AND @RemovedLogCategories <> ''
	BEGIN
		INSERT INTO #RemovedLogCategoryTable
		SELECT s.CategoryID
		FROM dbo.AppLog_Category AS s WITH (NOLOCK)
			INNER JOIN (SELECT val FROM dbo.StrSplit(@RemovedLogCategories, ',')) AS t ON s.CategoryName LIKE '%' + t.val + '%'
	END

	DECLARE @tsql			NVARCHAR(MAX),
		@ParamDefinition	NVARCHAR(500)

	SET @tsql = N'INSERT INTO #TempTable(Id, Num, CategoryID)
				SELECT MAX(al.LogID)
					,COUNT(*)
					,acl.CategoryID
				FROM dbo.AppLog_Machine AS m WITH(NOLOCK)
					INNER JOIN dbo.AppLog_Log AS al WITH (NOLOCK) ON al.MachineId = m.MachineId
					INNER JOIN dbo.AppLog_CategoryLog AS acl WITH (NOLOCK) ON al.LogID = acl.LogID
				WHERE al.Timestamp BETWEEN @From AND @To '

	IF @CategoryId <> 0
		SET @tsql = @tsql + N' AND acl.CategoryID = @Categoryid'

	IF @Severity <> 'ALL'
		SET @tsql = @tsql + N' AND al.Severity = @Severity'

	IF @MachineId <> 0
		SET @tsql = @tsql + N' AND al.MachineId = @MachineId'

	IF @IsMonitored IS NOT NULL
		SET @tsql = @tsql + N' AND al.IsMonitored = @IsMonitored'

	IF @IsProduction IS NOT NULL
		SET @tsql = @tsql + N' AND m.IsProduction = @IsProduction'

	IF @SearchText IS NOT NULL AND @SearchText <> ''
		SET @tsql = @tsql + N' AND al.FormattedMessage LIKE ''%' + @SearchText + '%'''

	IF EXISTS (SELECT 1 FROM #RemovedLogCategoryTable)
		SET @tsql = @tsql + N' AND acl.CategoryId NOT IN (SELECT CategoryId FROM #RemovedLogCategoryTable)'

	SET @tsql = @tsql + N' GROUP BY 
							al.Message,
							al.Severity,
							al.MachineId,
							acl.CategoryID,
							DATEADD(HOUR, DATEDIFF(HOUR, 0, al.Timestamp), 0)
							OPTION (recompile) '
 
	SET @ParamDefinition =  N'@From DATETIME, @To DATETIME, @Categoryid INT, @Severity NVARCHAR(32), @MachineId INT, @IsMonitored BIT, @IsProduction BIT';

	EXECUTE sp_executesql @tsql, @ParamDefinition,
						@From = @From,
						@To = @To,
						@CategoryId = @Categoryid,
						@Severity = @Severity,
						@MachineId = @MachineId,
						@IsMonitored = @IsMonitored,
						@IsProduction = @IsProduction;
 
	SELECT @TotalRows = COUNT(*)
	FROM #TempTable

	SELECT TOP(@Size) *
	FROM
	(
		SELECT
			DATEADD(Hour, @ToGMT, a.Timestamp) AS Timestamp
			,a.EventID As C_EventID
			,a.LogID
			,a.Priority 
			,a.ProcessID 
			,a.ProcessName
			,a.Win32ThreadId
			,a.ThreadName
			,a.EventID
			,a.Severity
			,a.Title
			,a.AppDomainName
			,a.Message
			,a.FormattedMessage
			,ci.username AS MonitoringUser
			,a.IsMonitored
			,temp.Num AS NumMessage
			,c.CategoryID
			,c.CategoryName
			,m.MachineId
			,m.MachineName
			,m.MachineIP
			,m.IsProduction
			,ROW_NUMBER() OVER (ORDER BY a.Timestamp DESC) AS num
		FROM dbo.AppLog_Category AS c WITH (NOLOCK)
			INNER JOIN #TempTable AS temp ON c.CategoryID = temp.CategoryID
			INNER JOIN dbo.AppLog_Log AS a WITH (NOLOCK) ON a.LogID = temp.Id
			INNER JOIN dbo.AppLog_Machine AS m WITH (NOLOCK) ON a.MachineId = m.MachineId
			LEFT JOIN bodb02.dbo.CustInfo AS ci WITH (NOLOCK) ON ci.custid = a.MonitoringUserID
	) AS alias
	WHERE num > (@Page - 1) * @Size

	DROP TABLE #TempTable
	DROP TABLE #RemovedLogCategoryTable
END



GO
GRANT	VIEW DEFINITION	ON	IS_SelLogPadding_All	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogPadding_All	TO	bodbAdminNet
GO
