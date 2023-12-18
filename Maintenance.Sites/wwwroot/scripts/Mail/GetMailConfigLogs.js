/* global viewResources: false */

$(document).ready(function () {
    mailConfigLogController.initPage();
});

var MailConfigLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMailConfigLog(mailConfigLog);
    };

    self.renderMailConfigLog = function (result) {
        var data = result.length;
        var bodyData = [];

        for (var i = 0; i < data; i++) {
            var log = result[i];

            bodyData.push({
                Columns: [{ Name: "SymbolNo", Value: i + 1 },
                    { Name: "Name", Value: log.AppName },
                    { Name: "Description", Value: log.Description },
                    { Name: "SmtpServers", Value: log.SettingName },
                    { Name: "MailFrom", Value: log.MailFrom },
                    { Name: "Encrypt", Value: log.IsEncrypt },
                    { Name: "LogTime", Value: log.LogTime },
                    { Name: "Username", Value: log.Username }]
            });
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.SymbolNo, Width: 50, Name: "SymbolNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Name, Width: 100, Name: "Name", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Description, Width: 150, Name: "Description", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.SmtpServers, Width: 200, Name: "SmtpServers", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.MailFrom, Width: 180, Name: "MailFrom", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Encrypt, Width: 50, Name: "Encrypt", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.LogTime, Width: 100, Name: "LogTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
                { HeaderText: viewResources.Username, Width: 50, Name: "Username", HeaderAlign: "center", CellAlign: "center" }],
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
        $("#mail-config-log").removeClass("display-none").fanexGrid(gridOptions);
    };
};

var mailConfigLogController = new MailConfigLogController();