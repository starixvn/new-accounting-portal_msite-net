/* global NAP: false */
/* global sharedScriptManager: false */
/* global viewResources: false */

$(document).ready(function () {
    mailReportController.initPage();
});

var MailReportController = function () {
    var self = this;
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
        },
        TriggerType: {
            FunctionMail: 0,
            ScheduleMail: 1
        }
    };
    self.gridOptions = {};

    self.cachedData = {
        searchParams: null,
        bodyRows: null
    };

    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.getMailTaskStatusCounter();
        self.getMailReport();
    };

    self.loadFilter = function () {
        self.renderDropdownRefresh();
        self.initDatePicker();
        countdownController.initPage();
        var cookie = $.cookie('MailReport');
        if (cookie) {
            $('#MailName').val(cookie.MailName);
            $('#SiteId').val(cookie.SiteId);
            $('#FromDate').datepicker('setDate', new Date(cookie.FromDate));
            $('#ToDate').datepicker('setDate', new Date(cookie.ToDate));
            $('#Status').val(cookie.Status);
            $('#TriggerType').val(cookie.TriggerType);
        }
    };
    self.renderDropdownRefresh = function () {
        var countDowns = [{ Id: 0, Value: viewResources.Not_Refresh },
            { Id: 1, Value: 30 },
            { Id: 2, Value: 60 },
            { Id: 3, Value: 90 },
            { Id: 4, Value: 120 },
            { Id: 5, Value: 150 },
            { Id: 6, Value: 180 }];
        var options = '';
        var countDownsLength = countDowns.length;
        for (var i = 0; i < countDownsLength; i++) {
            var countDown = countDowns[i];
            var displayName = countDown.Value === viewResources.Not_Refresh ?
                countDown.Value :
                countDown.Value + viewResources.Symbol_Second;
            options += '<div class="countDownOption' + (i === 0 ? ' countDownOptionCheck' : '') +
                '" value="' + countDown.Value + '">' +
                    displayName +
                '</div>';
        }

        $(".countDownOptionContainer").empty().append(options);
    };

    self.initDatePicker = function () {
        $("#FromDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "From",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        $("#ToDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#ToDate');
    };

    self.bindEvents = function () {
        countdownController.setRefreshEvent(self.refreshMailReport);

        $("#button-search").click(function () {
            self.getMailReport();
        });

        $("#MailName").keypress(function (e) {
            if (e.which == 13) {
                self.getMailReport();
            }
        });

        $("#dashboard").on("click", ".status", function () {
            var statusId = $(this).data("status-id"),
                currentDate = $("#dashboard").data("current-date");

            $("#MailName").val('');
            $("#SiteId").val(-1);
            $("#TriggerType").val(-1);
            $("#Status").val(statusId);
            $("#FromDate").val(currentDate);
            $("#ToDate").val(currentDate);

            self.getMailReport();
        });
    };
    self.refreshMailReport = function () {
        self.getMailTaskStatusCounter();

        sharedScriptManager.blockUI();
        sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/MailReport",
            self.cachedData.searchParams,
            self.renderMailReport,
            null,
            sharedScriptManager.unblockUI);
    };
    self.getMailReport = function () {
        sharedScriptManager.blockUI();
        var searchParams = {
            MailName: $("#MailName").val(),
            SiteId: $("#SiteId").val(),
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Status: $("#Status").val(),
            TriggerType: $("#TriggerType").val()
        };
        self.cachedData.searchParams = searchParams;
        $.cookie('MailReport', searchParams);
        sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/MailReport",
            searchParams,
            self.renderMailReport,
            null,
            sharedScriptManager.unblockUI);
    };

    self.getMailTaskStatusCounter = function () {
        sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/GetMailTaskStatusCounter",
            null,
            function (result) {
                if (result) {
                    var $dashboard = $("#dashboard");

                    $dashboard.data("current-date", moment(result.CurrentDate).format("MMM DD,YYYY"));
                    $dashboard.find(".missing-count").text(result.MissingItemCount);
                    $dashboard.find(".failed-count").text(result.FailedItemCount);
                    $dashboard.find(".running-count").text(result.RunningItemCount);
                    $dashboard.find(".succeeded-count").text(result.SucceededItemCount);
                    $dashboard.find(".sent-count").text(result.SentItemCount);
                }
            },
            null,
            null);
    };

    self.renderMailReport = function (result) {
        var mailReportLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailReportLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailIndex = ' mail-index="' + mail.MSiteMailId + '"';
            rowColumns.push({ Name: "ColId", Value: mail.MailTaskId });
            rowColumns.push({
                Name: "ColMailName",
                Value: mail.MSiteMailId == 0 ? mail.MailName : '<a class="link mail-name" href="javascript:;"' + mailIndex + '>' +
                           mail.MailName +
                       '</a>',
                Data: mail.MailName
            });

            rowColumns.push({ Name: "ColSite", Value: mail.SiteName, Data: mail.SiteName });
            rowColumns.push({ Name: "ColTrigger", Value: self.getTriggerIcon(mail) });
            rowColumns.push({ Name: "ColScheduledTime", Value: mail.ScheduledTime });
            rowColumns.push({ Name: "ColLastSent", Value: mail.LastSent });
            rowColumns.push({ Name: "ColStatus", Value: self.getStatusDisplay(mail, i), Data: self.getStatusName(mail.Status) });
            if (viewResources.hasEditRight) {
                rowColumns.push({ Name: "ColAction", Value: self.getActionColumn(mail, i) });
            }

            rowColumns.push({ Name: "ColLog", Value: self.getLogColumn(mail, i) });

            bodyData.push({
                Columns: rowColumns
            });
        }
        self.cachedData.bodyRows = bodyData;
        var headerColumns = [];
        headerColumns.push({ HeaderText: viewResources.Id, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Mail_Name, Width: 340, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left", Sort: true });
        headerColumns.push({ HeaderText: viewResources.Site, Width: 150, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", Sort: true });
        headerColumns.push({ HeaderText: viewResources.Trigger, Width: 90, Name: "ColTrigger", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Scheduled_Time, Width: 150, Name: "ColScheduledTime", HeaderAlign: "center", CellAlign: "center", Sort: true, FormatDateTime: "MMM DD, YYYY hh:mm:ss A" });
        headerColumns.push({ HeaderText: viewResources.Last_Run, Width: 150, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center", Sort: true, FormatDateTime: "MMM DD, YYYY hh:mm:ss A" });
        headerColumns.push({ HeaderText: viewResources.Status, Width: 120, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center", Sort: true });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 120, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
        }

        headerColumns.push({ HeaderText: viewResources.Log, Width: 120, Name: "ColLog", HeaderAlign: "center", CellAlign: "center" });
        self.gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: self.cachedData.bodyRows.slice(0, sharedScriptManager.cachedData.defaultItemsPerPage),
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 350,
            headerColumnHeight: 29,
            showPager: true,
            timeZone: viewResources.TimeZone,
            customPager: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: bodyData.length,
                pagerSelectCallBack: function (page_index, itemsPerPage, jq) {
                    self.gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                    self.gridOptions.pagerOption.currentPage = page_index;
                    var startIndex = (page_index * itemsPerPage);
                    var endIndex = ((page_index + 1) * itemsPerPage);
                    var currentBodyRows = self.cachedData.bodyRows.slice(startIndex, endIndex);
                    self.gridOptions.bodyRows = currentBodyRows;
                    $("#mail-report").fanexGrid(self.gridOptions);
                    self.bindGridEvent();
                    return false;
                }
            },
            sortOption: {
                customGridSort: function (rows, sortColumnName, sortDirection) {
                    rows = self.customSortColumn(rows, sortColumnName, sortDirection);

                    return rows;
                },
                afterGridSorted: function (bodyRows) {
                    self.bindGridEvent();

                    return bodyRows;
                }
            }
        };

        $("#mail-report").removeClass("display-none").fanexGrid(self.gridOptions);
        self.bindGridEvent();
    };

    self.customSortColumn = function (rows, sortColumnName, sortDirection) {
        var result = rows;

        if (rows && rows.length > 0) {
            var index = -1;

            var cols = $.grep(rows[0].Columns, function (item) {
                return item.Name === sortColumnName;
            });

            if (cols.length > 0) {
                index = rows[0].Columns.indexOf(cols[0]);
            }

            if (index !== -1) {
                var sortedData = self.cachedData.bodyRows;

                switch (sortColumnName) {
                    case "ColMailName":
                    case "ColSite":
                    case "ColStatus":
                        sortedData.sort(function (row1, row2) {
                            var value1 = row1.Columns[index].Data,
                                value2 = row2.Columns[index].Data,
                                comparison = value1.localeCompare(value2);

                            return comparison;
                        });
                        break;

                    case "ColLastSent":
                    case "ColScheduledTime":
                        sortedData.sort(function (row1, row2) {
                            var value1 = row1.Columns[index].Value,
                                value2 = row2.Columns[index].Value,
                                comparison = 0;

                            if (value1 && value2) {
                                var date1 = moment(value1),
                                     date2 = moment(value2);

                                comparison = date1 - date2;
                            } else if (!value1 && value2) {
                                comparison = -1;
                            } else if (value1 && !value2) {
                                comparison = 1;
                            } else {
                                comparison = 0;
                            }

                            return comparison;
                        });
                        break;
                }

                if (sortDirection === 'desc') {
                    sortedData.reverse();
                }

                self.gridOptions.pagerOption.currentPage = 0;
                var startIndex = (self.gridOptions.pagerOption.currentPage * self.gridOptions.pagerOption.itemsPerPage);
                var endIndex = ((self.gridOptions.pagerOption.currentPage + 1) * self.gridOptions.pagerOption.itemsPerPage);

                result = sortedData.slice(startIndex, endIndex);
                self.gridOptions.bodyRows = result;
                $("#mail-report").fanexGrid(self.gridOptions);
            }
        }

        return result;
    };

    self.getTriggerIcon = function (mail) {
        switch (mail.TriggerType) {
            case 0:
                return '<div class="trigger-type trigger-function"></div>';
            case 1:
                return '<div class="trigger-type trigger-schedule"></div>';
            default:
                return '';
        }
    };

    self.getStatusDisplay = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
        switch (mail.Status) {
            case self.Enums.TaskStatus.Waiting:
                return '<div class="column-status waiting-status"' + mailIndex + '>' + viewResources.Waiting.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Running:
                return '<div class="column-status running-status">' +
                            '<span class="status">' + viewResources.Running.toUpperCase() + '</span>' +
                            (mail.Attempt > 0 ? ' <span>(' + mail.Attempt + ')</span>' : '') +
                        '</div>';
            case self.Enums.TaskStatus.DataNotReady:
                return '<div class="column-status data-not-ready-status"' + mailIndex + '>' + viewResources.Data_not_ready.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Sent:
                return '<div class="column-status sent-status"' + mailIndex + '>' + viewResources.Sent.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Failed:
                return '<div class="column-status failed-status"' + mailIndex + '>' + viewResources.Failed.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Abort:
                return '<div class="column-status abort-status"' + mailIndex + '>' + viewResources.Abort + '</div>';
            case self.Enums.TaskStatus.Succeeded:
                return '<div class="column-status sent-status"' + mailIndex + '>' + viewResources.Succeeded.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Missing:
                return '<div class="column-status missing-status"' + mailIndex + '>' + viewResources.Missing.toUpperCase() + '</div>';
        }
    };

    self.getStatusName = function (status) {
        var statusName = "";

        switch (status) {
            case self.Enums.TaskStatus.Waiting:
                statusName = viewResources.Waiting.toUpperCase();
                break;
            case self.Enums.TaskStatus.Running:
                statusName = viewResources.Running.toUpperCase();
                break;
            case self.Enums.TaskStatus.DataNotReady:
                statusName = viewResources.Data_not_ready.toUpperCase();
                break;
            case self.Enums.TaskStatus.Sent:
                statusName = viewResources.Sent.toUpperCase();
                break;
            case self.Enums.TaskStatus.Failed:
                statusName = viewResources.Failed.toUpperCase();
                break;
            case self.Enums.TaskStatus.Abort:
                statusName = viewResources.Abort;
                break;
            case self.Enums.TaskStatus.Succeeded:
                statusName = viewResources.Succeeded.toUpperCase();
                break;
            case self.Enums.TaskStatus.Missing:
                statusName = viewResources.Missing.toUpperCase();
                break;
        }

        return statusName;
    };

    self.getActionColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';

        if (mail.TriggerType === self.Enums.TriggerType.ScheduleMail) {
            switch (mail.Status) {
                case self.Enums.TaskStatus.Running:
                    return '<input type="submit" value="Abort" class="button-abort"' + mailIndex + '>';
                case self.Enums.TaskStatus.DataNotReady:
                case self.Enums.TaskStatus.Sent:
                case self.Enums.TaskStatus.Failed:
                case self.Enums.TaskStatus.Abort:
                    if (mail.IsUnableToRetry == 0) {
                        return '<input type="submit" value="Retry" class="button-retry"' + mailIndex + '>';
                    }

                    return '';
                case self.Enums.TaskStatus.Missing:
                    return '<input type="submit" value="Retry" class="button-retry-missing-task"'
                        + 'mailId="' + mail.MailId + '"'
                        + 'siteId="' + mail.SiteId + '"'
                        + 'scheduledTime="' + mail.ScheduledTime + '"'
                        + ' >';
            }
        } else if (mail.Status === self.Enums.TaskStatus.Running) {
            return '<input type="submit" value="Abort" class="button-abort"' + mailIndex + '>';
        }
    };

    self.getLogColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
        switch (mail.Status) {
            case self.Enums.TaskStatus.Abort:
            case self.Enums.TaskStatus.Succeeded:
            case self.Enums.TaskStatus.Running:
            case self.Enums.TaskStatus.DataNotReady:
            case self.Enums.TaskStatus.Sent:
            case self.Enums.TaskStatus.Failed:
                return '<div class="icon-view-log"' + mailIndex + '></div>';
        }
    };

    self.bindGridEvent = function () {
        var gridContainer = $("#mail-report");
        gridContainer.find(".mail-name").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.viewMailDetail(mailIndex);
        });

        $(".button-retry").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.retrySendMail(mailIndex, function () {
                setTimeout(function () {
                    NAP.headlineMsgBox.close();
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        });

        $(".button-retry-missing-task").click(function (event) {
            var $this = $(this);
            var mailId = parseInt($this.attr("mailId"));
            var siteId = parseInt($this.attr("siteId"));
            var scheduledTime = moment($this.attr("scheduledTime")).format();
            //var scheduledTime = $this.attr("scheduledTime");
            self.retryMissingTask(mailId, siteId, scheduledTime, function () {
                setTimeout(function () {
                    NAP.headlineMsgBox.close();
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        });

        $(".button-abort").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.abortTask(mailIndex, function () {
                setTimeout(function () {
                    NAP.headlineMsgBox.close();
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        });

        gridContainer.find(".icon-view-log").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.viewMailLog(mailIndex);
        });
    };

    self.abortTask = function (mailIndex, callback) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_AbortTask, function () {
            sharedScriptManager.blockUI();
            var abortParams = {
                TaskId: mailIndex
            };
            sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/AbortTask",
            abortParams,
            function (result) {
                result = {
                    Code: 0,
                    Message: 'Job has been aborted.'
                };
                sharedScriptManager.renderActionResult(result, callback);
            },
            null,
            sharedScriptManager.unblockUI);
        });
    };

    self.retrySendMail = function (mailIndex, afterRetryEvent) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_RetrySendMail, function () {
            sharedScriptManager.blockUI();
            var retryParams = {
                TaskId: mailIndex
            };
            sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/RetrySendMail",
            retryParams,
            function (result) {
                result = {
                    Code: 0,
                    Message: 'Job has been retried.'
                };
                sharedScriptManager.renderActionResult(result, afterRetryEvent);
            },
            null,
            sharedScriptManager.unblockUI);
        });
    };

    self.retryMissingTask = function (mailId, siteId, scheduledTime, afterRetryEvent) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_RetryMissingTask, function () {
            sharedScriptManager.blockUI();
            var retryParams = {
                MailId: mailId,
                SiteId: siteId,
                ScheduledTime: scheduledTime
            };
            sharedScriptManager.callAjax(self,
            "/MailStation/MailReport/RetryMissingTask",
            retryParams,
            function (result) {
                result = {
                    Code: 0,
                    Message: 'Job has been retried.'
                };
                sharedScriptManager.renderActionResult(result, afterRetryEvent);
            },
            null,
            sharedScriptManager.unblockUI);
        });
    };

    self.renderActionResult = function (result) {
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            }
        );
    };

    self.viewMailDetail = function (mailIndex) {
        popUpManager.openPopup(
            '/MailStation/MailReport/MailDetail?' +
            $.param({
                mailId: mailIndex
            }),
            viewResources.Details,
            $(window).width() - 200,
            $(window).height() - 200,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 80]);
    };

    self.viewMailLog = function (mailIndex) {
        popUpManager.openPopup(
            '/MailStation/MailReport/MailLog?' +
            $.param({
                mailId: mailIndex
            }),
            viewResources.Log_details,
            $(window).width() - 200,
            $(window).height() - 200,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 80]);
    };
};

var mailReportController = new MailReportController();