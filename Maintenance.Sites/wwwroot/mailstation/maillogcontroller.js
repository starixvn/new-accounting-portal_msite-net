/* global viewResources: false */

$(document).ready(function () {
    mailLogController.initPage();
});

var MailLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMailLog(mailLog);
    };

    self.Enums = {
        TaskStatus: {
            All: -1,
            Waiting: 0,
            Running: 1,
            DataNotReady: 2,
            Failed: 3,
            Sent: 4,
            Abort: 6,
            Succeeded: 7,
            Missing: 8
        }
    };

    self.getStatusDisplay = function (statusid) {
        switch (statusid) {
            case self.Enums.TaskStatus.Waiting:
                return '<div class="column-status waiting-status">' + viewResources.Waiting.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Running:
                return '<div class="column-status running-status">' +
                            '<span class="status">' + viewResources.Running.toUpperCase() + '</span>' +
                        '</div>';
            case self.Enums.TaskStatus.DataNotReady:
                return '<div class="column-status data-not-ready-status">' + viewResources.Data_not_ready.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Succeeded:
                return '<div class="column-status sent-status">' + viewResources.Succeeded.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Sent:
                return '<div class="column-status sent-status">' + viewResources.Sent.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Failed:
                return '<div class="column-status failed-status">' + viewResources.Failed.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Abort:
                return '<div class="column-status failed-status">' + viewResources.Aborted.toUpperCase() + '</div>';
        }
    };

    self.renderMailLog = function (result) {
        // result = self.dummyData();
        var mailLogLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailLogLength; i++) {
            var log = result[i];

            bodyData.push({
                Columns: [{ Name: "ColId", Value: log.LogId },
                    { Name: "ColStatus", Value: self.getStatusDisplay(log.Status) },
                    { Name: "ColMessage", Value: log.ExceptionMessage },
                    { Name: "ColLogTime", Value: log.LogTime },
                    { Name: "ColStackTrace", Value: log.StackTrace }]
            });
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.Symbol_No, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Status, Width: 100, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Message, Width: 580, Name: "ColMessage", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: viewResources.Log_Time, Width: 135, Name: "ColLogTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Stack_Trace, Width: 430, Name: "ColStackTrace", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true }],
            showTitle: false,
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30
        };
        $("#mail-log").removeClass("display-none").fanexGrid(gridOptions);
    };
    self.dummyData = function () {
        var result = [];
        result.push({
            Id: 1,
            Status: "Running",
            Message: 'Mail To can not be null or empty',
            LogTime: '11/3/2014 2:00:12 AM',
            StackTrace: 'asdsad sadasd '
        });
        result.push({
            Id: 2,
            Status: "Running",
            Message: 'Mail To can not be null or empty',
            LogTime: '11/3/2014 2:00:12 AM',
            StackTrace: 'asdsad sadasd '
        });

        for (var i = 0; i < 5; i++) {
            result.push(result[0]);
        }

        return result;
    };
};

var mailLogController = new MailLogController();





