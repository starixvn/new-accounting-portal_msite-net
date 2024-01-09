/* global viewResources: false */

$(document).ready(function () {
    mailActionLogController.initPage();
});

var MailActionLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMailLog(mailLog);
    };

    self.renderMailLog = function (result) {
        var mailLogLength = result.length;
        var bodyData = [];

        for (var i = 0; i < mailLogLength; i++) {
            var log = result[i];

            bodyData.push({
                Columns: [{ Name: "ColId", Value: log.MailLogId },                 
                    { Name: "ColMessage", Value: log.Message },
                    { Name: "ColLogTime", Value: log.LogTime }]
            });
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.Symbol_No, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Message, Width: 380, Name: "ColMessage", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: viewResources.Log_Time, Width: 160, Name: "ColLogTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" }],
            showTitle: false,
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30,
            gridHeight: 450,
            gridExpandHeight: 20
        };
        $("#mail-log").removeClass("display-none").fanexGrid(gridOptions);
    };
};

var mailActionLogController = new MailActionLogController();





