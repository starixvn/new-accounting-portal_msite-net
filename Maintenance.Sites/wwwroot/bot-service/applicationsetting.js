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
        //self.getReport();
    };

    self.bindEvents = function () {
        $("#button-search").on("click", function () {
            self.getData();
        });
    }

    self.getData = function () {
        sharedScriptManager.blockUI();
        var getAppSettingParams = {
            name: $("#Name").val(),
            active: $("#Active").val()
        };
        sharedScriptManager.callAjax(self,
            "/ApplicationSetting/GetApplicationSetting",
            getAppSettingParams,
            self.renderGrid,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderGrid = function (result) {
        if (result == null) {
            return;
        }
        var bodyData = [];
        for (var i = 0; i < result.length; i++) {
            var dataRow = result[i];
            var rowColumns = [];
            var chkResend = '<input type="checkbox" class="chkResend" data-app-id="' + dataRow.AppId + '">';
            rowColumns.push({ Name: "AppId", Value: dataRow.AppId });
            rowColumns.push({ Name: "AppName", Value: dataRow.AppName });
            rowColumns.push({ Name: "MicrosoftAppId", Value: dataRow.MicrosoftAppId });
            rowColumns.push({ Name: "Description", Value: dataRow.Description });
            rowColumns.push({ Name: "Active", Value: chkResend });
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate });

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
            { HeaderText: "App ID", Width: 100, Name: "AppId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "App Name", Width: 185, Name: "AppName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Microsoft App Id", Width: 300, Name: "MicrosoftAppId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Description", Width: 400, Name: "Description", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Active", Width: 70, Name: "Active", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 200, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
        ];
        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 350,
            headerColumnHeight: 29,
            showPager: true,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 200, 500],
                showDetail: true,
                afterPagedFunction: function () {
                }
            }
        };

        $('#application-setting').removeClass("display-none").fanexGrid(gridOptions);
    }
}

var applicationManagement = new ApplicationManagementController();