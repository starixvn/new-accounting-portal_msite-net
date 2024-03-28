/* global viewResources: false */

$(document).ready(function () {
    msgLog.initPage();
});

var MessageLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMessageLog(messageLog);
    };

    self.renderMessageLog = function (messageLog) {
        if (messageLog == null) {
            return;
        }

        var messageLogLenghth = messageLog.length;
        var bodyData = [];

        for (var i = 0; i < messageLogLenghth; i++) {
            var log = messageLog[i];

            bodyData.push({
                Columns: [
                    { Name: "No", Value: i + 1 },
                    { Name: "ConversationBotId", Value: self.renderConversationId(log.ConversationBotId) },
                    { Name: "ConversationId", Value: self.renderConversationId(log.ConversationId) },
                    { Name: "StartSendTime", Value: sharedScriptManager.toLocalDate(log.StartSentTime) },
                    { Name: "EndSentTime", Value: sharedScriptManager.toLocalDate(log.EndSentTime) },
                    { Name: "Duration", Value: log.Duration + "ms" },
                    { Name: "Status", Value: log.IsSuccessful ? "success" : "failed" },
                    { Name: "ErrorMessage", Value: log.ErrorMessage },
                ]
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 45, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bot Conversation Id", Width: 200, Name: "ConversationBotId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Conversation Id", Width: 200, Name: "ConversationId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Start At (GMT " + sharedScriptManager.currentTimeZone() + ")", Width: 150, Name: "StartSendTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "End at (GMT " + sharedScriptManager.currentTimeZone() + ")", Width: 150, Name: "EndSentTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Duration", Width: 60, Name: "Duration", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 60, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Error Message", Width: 220, Name: "ErrorMessage", HeaderAlign: "center", CellAlign: "left" }
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
        $("#message-log").removeClass("display-none").fanexGrid(gridOptions);
        $("#message-log").fanexGrid('resizeGrid');
    };

    self.renderConversationId = function (conversationId) {
        return `<p class="overflow-text" title="` + conversationId + `">` + conversationId + `</p>`;
    }
};

var msgLog = new MessageLogController();