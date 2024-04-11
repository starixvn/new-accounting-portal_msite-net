$(document).ready(function () {
    messageController.initPage();
});

var MessageController = function () {
    var self = this;

    var DOMElements = {
        FromDate: null,
        ToDate: null,
        AppId: null,
        ChannelId: null,
        Status: null,
        BotId: null,
        SearchBtn: null,
        GridContainer: null
    };
    self.initPage = function () {
        self.cacheDOMElement();
        self.setValueDatePickerClientTime();
        self.initDateTimePicker();
        self.fetchData();
        self.initEvent();
    }

    self.cacheDOMElement = function () {
        DOMElements.FromDate = $('#FromDate');
        DOMElements.ToDate = $('#ToDate');
        DOMElements.AppId = $('#AppId');
        DOMElements.ChannelId = $('#Channel');
        DOMElements.Status = $('#Status');
        DOMElements.BotId = $('#Bot');
        DOMElements.SearchBtn = $('#button-search');
        DOMElements.GridContainer = $('#message-list');
    };

    self.setValueDatePickerClientTime = function () {
        var fromDate = new Date();
        var toDate = new Date();
        fromDate.setDate(fromDate.getDate() - 7);
        DOMElements.FromDate.val(fromDate.toDateString('MMM DD, YYYY'));
        DOMElements.ToDate.val(toDate.toDateString('MMM DD, YYYY'));
    }

    self.initEvent = function () {
        DOMElements.AppId.on("change", function (e) {
            self.reInitBotSelect();
        });

        DOMElements.ChannelId.on("change", function () {
            self.reInitBotSelect();
        });

        DOMElements.SearchBtn.on("click", function () {
            self.fetchData();
        })
    }

    self.reInitBotSelect = function () {
        sharedScriptManager.blockUI();
        var appId = DOMElements.AppId.val();
        var channelId = DOMElements.ChannelId.val();
        $.ajax({
            type: 'GET',
            url: "/Message/GetBotByAppId?appId=" + appId + "&channelId=" + channelId,
            success: function (bots) {
                var options = `<option value=''>All</option>`;
                if (bots.length > 0) {
                    for (var i = 0; i < bots.length; i++) {
                        options += `<option value=` + bots[i].BotId + `>` + bots[i].BotName + `</option>`;
                    }
                }
                DOMElements.BotId.html(options);
            },
            complete: function () {
                sharedScriptManager.unblockUI();
            }
        })
    }
    self.fetchData = function (pageIndex = 1, pageSize = 50) {
        sharedScriptManager.blockUI();
        var fromDate = self.convertToUtcDate(DOMElements.FromDate.val()),
            toDate = self.convertToUtcDate(moment(DOMElements.ToDate.val()).add(1, 'days'));
        $.ajax({
            type: 'POST',
            url: "/Message/GetMessage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                appId: DOMElements.AppId.val(),
                channelId: DOMElements.ChannelId.val(),
                botId: DOMElements.BotId.val(),
                status: DOMElements.Status.val(),
                fromDate: fromDate,
                toDate: toDate,
                pageIndex: pageIndex,
                pageSize: pageSize
            }),
            success: function (result) {
                self.renderGrid(result, pageIndex, pageSize);
            },
            complete: function () {
                sharedScriptManager.unblockUI();
            }
        })
    }

    self.convertToUtcDate = function (date) {
        date = moment(date);
        return moment.utc(date).format('MMM DD, YYYY hh:mm:ss A');
    }
    self.renderGrid = function (data, pageIndex, pageSize) {
        if (data == null) {
            return;
        }
        var totalItems = data.TotalItems,
            messages = data.Messages;
        var bodyData = [];
        for (var i = 0; i < messages.length; i++) {
            var dataRow = messages[i];
            var rowColumns = [];
            var status = `<div class='status ` + dataRow.Status.toLowerCase() + `'>` + dataRow.Status + `<div/>`;
            var retry = dataRow.Retry > 0 ? `<div class="view-retry-log" data-message-id="` + dataRow.MessageId + `">` + dataRow.Retry + `</div>` : dataRow.Retry;
            rowColumns.push({ Name: "No", Value: i + 1 + ((pageIndex - 1) * pageSize) });
            rowColumns.push({ Name: "AppName", Value: dataRow.AppName });
            rowColumns.push({ Name: "ChannelName", Value: dataRow.ChannelName });
            rowColumns.push({ Name: "BotName", Value: dataRow.BotName });
            rowColumns.push({ Name: "Content", Value: dataRow.IsHideContent ? `<i>Content hidden<i/>` : `<p class="overflow-text">` + dataRow.Content + `</p>` });
            rowColumns.push({ Name: "Status", Value: status });
            rowColumns.push({ Name: "ErrorMessage", Value: `<p class="overflow-text">` + dataRow.ErrorMessage + `</p>` });
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate !== null ? sharedScriptManager.toLocalDate(dataRow.CreatedDate) : '' });
            rowColumns.push({ Name: "UpdatedDate", Value: dataRow.UpdatedDate !== null ? sharedScriptManager.toLocalDate(dataRow.UpdatedDate) : '' });
            rowColumns.push({ Name: "Retry", Value: retry });
            rowColumns.push({ Name: "Log", Value: '<div class="icon-view-log" title="View log message ' + dataRow.MessageId + '" data-message-id="' + dataRow.MessageId + '"></div>' });

            if (viewResources.hasEditRight) {
                var resendBtn = dataRow.Status.toLowerCase() === 'failed' ? `<div class="resend-icon" title="Retry message ` + dataRow.MessageId + `" data-message-id="` + dataRow.MessageId + `"></div>` : '';
                rowColumns.push({ Name: "Action", Value: resendBtn });
            }
            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "App", Width: 150, Name: "AppName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Channel", Width: 100, Name: "ChannelName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bot", Width: 120, Name: "BotName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Content", Width: 400, Name: "Content", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Status", Width: 80, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Error Message", Width: 250, Name: "ErrorMessage", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Created Date (GTM " + sharedScriptManager.currentTimeZone() + ")", Width: 150, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Last Updated (GTM " + sharedScriptManager.currentTimeZone() + ")", Width: 150, Name: "UpdatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Retry time", Width: 60, Name: "Retry", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Log", Width: 60, Name: "Log", HeaderAlign: "center", CellAlign: "center" },
        ];

        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: "Action", Width: 60, Name: "Action", HeaderAlign: "center", CellAlign: "center" });
        }

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 270,
            headerColumnHeight: 29,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex - 1,
                itemsPerPageArray: [50, 100, 200, 500],
                showDetail: true,
                totalItem: totalItems,
                pagerSelectCallBack: function (pageIndex, pageSize, jq) {
                    self.fetchData(pageIndex + 1, pageSize);
                }
            }
        };

        DOMElements.GridContainer.removeClass("display-none").fanexGrid(gridOptions);
        DOMElements.GridContainer.fanexGrid('resizeGrid');
        self.bindGridEvent();
    }

    self.bindGridEvent = function () {
        DOMElements.GridContainer.find(".icon-view-log").click(function (event) {
            var msgId = parseInt($(this).attr("data-message-id"));
            popUpManager.openPopup(
                '/Message/MessageLog?' +
                $.param({
                    messageId: msgId
                }),
                "Logs for message " + msgId,
                1200,
                500,
                true,
                false,
                parent.window,
                3000,
                null,
                null,
                ['center', 80]);
        });

        DOMElements.GridContainer.find(".resend-icon").click(function () {
            var msgId = $(this).attr('data-message-id');
            NAP.confirm("Confirm Notification", "Are you sure you want to resend this message?", function () {
                sharedScriptManager.blockUI();
                $.ajax({
                    type: 'POST',
                    url: "/Message/RetryMessage",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: msgId,
                    success: function (result) {
                        var title = result.IsSuccessful ? 'Successfully!' : 'Error';
                        NAP.alert(title, result.Message, function () {
                            NAP.closeDialog();
                            self.fetchData();
                        })
                    },
                    complete: function () {
                        sharedScriptManager.unblockUI();
                    }
                })
                NAP.closeDialog();
            })
        });

        DOMElements.GridContainer.find(".view-retry-log").click(function () {
            var msgId = parseInt($(this).attr("data-message-id"));
            popUpManager.openPopup(
                '/Message/MessageRetryLog?' +
                $.param({
                    messageId: msgId
                }),
                "Retry logs for message " + msgId,
                500,
                400,
                true,
                false,
                parent.window,
                3000,
                null,
                null,
                ['center', 80]);
        });
    }

    self.initDateTimePicker = function () {
        DOMElements.FromDate.datepicker({
            defaultDate: "-1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "From",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        DOMElements.ToDate.datepicker({
            defaultDate: new Date(),
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#ToDate');
    }
}

var messageController = new MessageController();