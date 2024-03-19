/* global viewResources: false */

$(document).ready(function () {
    msgLog.initPage();
});

var MessageLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMessageLog(messageLog);
    };

    self.renderMessageLog = function (result) {
        var messageLogLenghth = result.length;
        var bodyData = [];

        for (var i = 0; i < messageLogLenghth; i++) {
            var log = result[i];

            bodyData.push({
                Columns: [
                    { Name: "No", Value: i + 1 },
                    { Name: "ConversationBotId", Value: self.renderConversationId(log.ConversationBotId) },
                    { Name: "ConversationId", Value: self.renderConversationId(log.ConversationId) },
                    { Name: "StartSendTime", Value: log.StartSentTime },
                    { Name: "EndSentTime", Value: log.EndSentTime },
                    { Name: "Duration", Value: log.Duration + "ms" },
                    { Name: "Status", Value: log.IsSuccessful ? "success" : "failed" },
                    { Name: "ErrorMessage", Value: log.ErrorMessage },
                ]
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 42, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "From", Width: 150, Name: "ConversationBotId", HeaderAlign: "center", CellAlign: "left"},
            { HeaderText: "To", Width: 180, Name: "ConversationId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Start At", Width: 150, Name: "StartSendTime", HeaderAlign: "center", CellAlign: "left", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "End at", Width: 150, Name: "EndSentTime", HeaderAlign: "center", CellAlign: "left", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Duration", Width: 50, Name: "Duration", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 60, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Error Message", Width: 200, Name: "ErrorMessage", HeaderAlign: "center", CellAlign: "left" }
        ];

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            numberOfFrozenColumn: 8,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30,
            gridHeight: 450,
            gridExpandHeight: 20
        };
        $("#message-log").removeClass("display-none").fanexGrid(gridOptions);
        $("#message-log").fanexGrid('resizeGrid');
    };

    self.renderConversationId = function (conversationId) {
        return `<div class="overflow" title="` + conversationId + `">` + conversationId + `</div>`;
    }
};

var msgLog = new MessageLogController();