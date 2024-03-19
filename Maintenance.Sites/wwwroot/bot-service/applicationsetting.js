/* global NAP: false */
/* global sharedScriptManager: false */
$(document).ready(function () {
    applicationManagement.initPage();
});

var ApplicationManagementController = function () {
    var self = this;
    self.initPage = function () {
        self.getData();
        self.bindEvents();
    };

    self.bindEvents = function () {
        $("#button-search").on("click", function () {
            self.getData();
        });
    }
    self.getData = function () {
        sharedScriptManager.blockUI();
        $.ajax({
            type: 'GET',
            url: "/ApplicationSetting/GetApplicationSetting?name=" + $("#Name").val() + '&isActive=' + $("#Active").val(),
            success: function (result) {
                self.renderGrid(result);
            },
            complete: function () {
                sharedScriptManager.unblockUI();
            }
        })
    };

    self.renderGrid = function (result) {
        if (result == null) {
            return;
        }
        var bodyData = [];

        for (var i = 0; i < result.length; i++) {
            var dataRow = result[i];
            var bots = dataRow.Bots;
            var mergedRowNumber = bots.length > 0 ? bots.length : 1;
            var rowColumns = [];
            var active = dataRow.IsActive === true ? '<div class="ico-tick"></div>' : '';
            rowColumns.push({ Name: "AppId", Value: dataRow.AppId, MergeRowNumber: mergedRowNumber });
            rowColumns.push({ Name: "AppName", Value: dataRow.AppName, MergeRowNumber: mergedRowNumber });
            rowColumns.push({ Name: "BotName", Value: bots.length > 0 ? bots[0].BotName : ''});
            rowColumns.push({ Name: "ChannelName", Value: bots.length > 0 ? bots[0].ChannelName : '' });
            rowColumns.push({ Name: "MicrosoftAppId", Value: dataRow.MicrosoftAppId, MergeRowNumber: mergedRowNumber });
            rowColumns.push({ Name: "Description", Value: dataRow.Description, MergeRowNumber: mergedRowNumber });
            rowColumns.push({ Name: "Active", Value: active, MergeRowNumber: mergedRowNumber });
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate, MergeRowNumber: mergedRowNumber });
            if (viewResources.hasEditRight) {
                rowColumns.push({
                    Name: "Action",
                    Value: `<a href="/ApplicationSetting/Update?appId=` + dataRow.AppId + `" ><div class="edit-icon"></div></a>`,
                    MergeRowNumber: mergedRowNumber
                });
            } 

            bodyData.push({
                Columns: rowColumns
            });
            if (mergedRowNumber > 1) {
                for (var j = 1; j < mergedRowNumber; j++) {
                    bodyData.push({
                        Columns: [
                            { Name: "BotName", Value: bots[j].BotName },
                            { Name: "ChannelName", Value: bots[j].ChannelName }
                        ]
                    });
                }
            }

            
        }

        var headerColumns = [
            { HeaderText: "App ID", Width: 50, Name: "AppId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "App Name", Width: 120, Name: "AppName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Bot Name", Width: 100, Name: "BotName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Channel", Width: 60, Name: "ChannelName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Microsoft App ID", Width: 250, Name: "MicrosoftAppId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Description", Width: 250, Name: "Description", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Active", Width: 70, Name: "Active", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 200, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" }

        ];
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" })
        }
        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 350,
            headerColumnHeight: 29,
            showPager: false
        };

        $('#application-setting').removeClass("display-none").fanexGrid(gridOptions);
    }
}

var applicationManagement = new ApplicationManagementController();