/* global viewResources: false */

$(document).ready(function () {
    msgLog.initPage();
});

var MessageRetryLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMessageRetryLog(messageRetryLog);
    };

    self.renderMessageRetryLog = function (logs) {
        if (logs == null) {
            return;
        }

        var messageRetryLogLength = logs.length;
        var bodyData = [];

        for (var i = 0; i < messageRetryLogLength; i++) {
            var log = logs[i];

            bodyData.push({
                Columns: [
                    { Name: "No", Value: i + 1 },
                    { Name: "RetryDate", Value: sharedScriptManager.toLocalDate(log.RetryDate) },
                    { Name: "RetryById", Value: log.RetryBy },
                    { Name: "RetryByUsername", Value: log.RetryByUsername },
                ]
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Retry Date (GMT " + sharedScriptManager.currentTimeZone() + ")", Width: 150, Name: "RetryDate", HeaderAlign: "center", CellAlign: "left", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Retry By", Width: 60, Name: "RetryById", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 150, Name: "RetryByUsername", HeaderAlign: "center", CellAlign: "center" }
        ];

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30,
            gridHeight: 450,
            gridExpandHeight: 20
        };
        $("#message-retry-log").removeClass("display-none").fanexGrid(gridOptions);
        $("#message-retry-log").fanexGrid('resizeGrid');
    };
};

var msgLog = new MessageRetryLogController();