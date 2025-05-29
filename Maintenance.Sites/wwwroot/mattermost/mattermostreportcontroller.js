/* global NAP: false */
/* global sharedScriptManager: false */

$(document).ready(function () {
    mattermostReportController.initPage();
});

var MattermostReportController = function () {
    var self = this;

    self.cachedData = {
        getReportParams: null,
        resendParams: null,
        bodyRows: null
    };

    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.getReport();
    };

    self.loadFilter = function () {
        self.initDatePicker();

        var cookie = $.cookie('MailReport');
        if (cookie) {
            $('#FromDate').datepicker('setDate', new Date(cookie.FromDate));
            $('#ToDate').datepicker('setDate', new Date(cookie.ToDate));
        }
        self.loadTarget();
        self.loadGroupsByTarget();
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

    self.getReport = function () {
        sharedScriptManager.blockUI();
        var getReportParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            TargetName: $('#Target').val(),
            Group: $('#Group').val(),
            Status: parseInt($('#Status').val())
        };
        self.cachedData.getReportParams = getReportParams;
        $.cookie('MailReport', getReportParams);
        sharedScriptManager.callAjax(self,
            "/MattermostReport/GetReport",
            getReportParams,
            self.renderReport,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderReport = function (result) {
        if (result == null) {
            return;
        }

        var reportLength = result.length;
        var bodyData = [];
        for (var i = 0; i < reportLength; i++) {
            var dataRow = result[i];
            var rowColumns = [];

            var email = "<div title='Email' class='email'></div>";
            var letstalk = "<div title='Mattermost' class='mattermost'></div>";
            var resend = "<div class='resend'></div>";
            var linkResend = "<a title='Resend' href='#' class='resendMattermost' data-mattermost-id='" + dataRow.Id + "' data-mattermost-ismessage='" + dataRow.IsMessage + "'>" + resend + "</a>";
            var chkResend = '<input type="checkbox" class="chkResend" data-mattermost-id="' + dataRow.Id + '" data-mattermost-ismessage="' + dataRow.IsMessage + '">';

            rowColumns.push({ Name: "ResendMultiple", Value: chkResend });
            rowColumns.push({ Name: "TargetName", Value: dataRow.TargetName });
            rowColumns.push({ Name: "Subject", Value: dataRow.Subject });
            rowColumns.push({ Name: "Content", Value: dataRow.Content });
            rowColumns.push({ Name: "Group", Value: dataRow.Group });
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate });
            rowColumns.push({ Name: "LatestSendDate", Value: dataRow.LastSentDate });
            rowColumns.push({ Name: "SendSuccess", Value: dataRow.SendSuccess });
            rowColumns.push({ Name: "SendFail", Value: dataRow.SendFail });
            rowColumns.push({ Name: "IsMessage", Value: dataRow.IsMessage ? letstalk : email });
            rowColumns.push({ Name: "Resend", Value: linkResend });

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
            { HeaderText: "<input class='chkResend-all' type='checkbox'>", Width: 40, Name: "ResendMultiple", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Target Name", Width: 125, Name: "TargetName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Subject", Width: 195, Name: "Subject", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Content", Width: 500, Name: "Content", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Group", Width: 680, Name: "Group", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date<br/> (GMT +7)", Width: 70, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "Latest Send Date<br/> (GMT +7)", Width: 70, Name: "LatestSendDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
            { HeaderText: "<div class='success'></div>", Width: 30, Name: "SendSuccess", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "<div class='fail'></div>", Width: 30, Name: "SendFail", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "<div class='type'></div>", Width: 30, Name: "IsMessage", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "<div class='resend'></div>", Width: 30, Name: "Resend", HeaderAlign: "center", CellAlign: "center" }
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
                    var $checkboxAll = $(".freegrid .chkResend-all");
                    var isCheckAll = $(".freegrid .chkResend:checked").length === $(".freegrid .chkResend").length;
                    $checkboxAll.prop("checked", isCheckAll);
                }
            }
        };

        $("#mattermost-report").removeClass("display-none").fanexGrid(gridOptions);
        $(".btnResend").removeClass("display-none");
    };

    self.bindEvents = function () {
        $("#button-search").click(function () {
            self.getReport();
        });

        $('.resendMattermost').live('click', function () {
            var letstalkId = $(this).attr("data-mattermost-id");
            var isMessage = $(this).attr("data-mattermost-ismessage") == 'true';
            var messageConfirm = isMessage ? "Resend this message ?" : "Resend this email";
            if (confirm(messageConfirm)) {
                self.reSend(letstalkId, isMessage);
            }
        });

        $('.hasDatepicker').change(function () {
            self.loadTarget();
            self.loadGroupsByTarget();
        });

        $('#Target').change(function () {
            self.loadGroupsByTarget();
        });

        $("#btnResendMulti").click(function () {
            self.resendMultiple();
        });

        $(".freegrid").on("click", ".chkResend-all", function () {
            var isCheckAll = $(this).prop("checked");
            $(".freegrid .chkResend").prop("checked", isCheckAll);
        });

        $(".freegrid").on("click", ".chkResend", function () {
            var isChecked = $(this).prop("checked"),
                $checkboxAll = $(".freegrid .chkResend-all");

            if (!isChecked) {
                $checkboxAll.prop("checked", false);
            } else {
                var isCheckAll = $(".freegrid .chkResend:checked").length === $(".freegrid .chkResend").length;

                $checkboxAll.prop("checked", isCheckAll);
            }
        });
    };

    self.reSend = function (id, isMessage) {
        var resendParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Id: id
        };

        sharedScriptManager.blockUI();
        var url = isMessage ? "/MattermostReport/ResendMessage" : "/MattermostReport/ReSendMail";

        self.cachedData.resendParams = resendParams;
        sharedScriptManager.callAjax(self,
            url,
            resendParams,
            self.renderResend,
            null,
            sharedScriptManager.unblockUI);
    };

    self.resendMultiple = function () {
        var data = $(".freegrid .chkResend:checked").map(function () { return { Id: parseInt($(this).data("mattermost-id")), IsMessage: $(this).data("mattermost-ismessage") } }).get();
        if (data.length > 0) {
            if (confirm("Resend the chosen mattermost ?")) {
                var resendParams = {
                    FromDate: $("#FromDate").val(),
                    ToDate: $("#ToDate").val(),
                    Data: data
                };

                sharedScriptManager.blockUI();
                var url = "/MattermostReport/ResendMultiple";

                self.cachedData.resendParams = resendParams;
                sharedScriptManager.callAjax(self,
                    url,
                    resendParams,
                    self.renderResendMultiple,
                    null,
                    sharedScriptManager.unblockUI);
            }
        } else {
            NAP.headlineMsgBox.showInfo("Please choose at least one mattermost to resend!", { closeBtn: false });
        }

    };

    self.renderResend = function (result) {
        if (result) {
            NAP.headlineMsgBox.showInfo("Resend Success !", { closeBtn: false });
        }
        else {
            NAP.headlineMsgBox.showInfo("Resend Failed Lestalk!", { closeBtn: false });
        }

        setTimeout(function () {
            self.getReport();
        }, 3000);

        window.location.href = window.location.href;
    };

    self.renderResendMultiple = function (result) {
        if (result.length == 0) {
            NAP.headlineMsgBox.showInfo("Resend Success !", { closeBtn: false });
        }
        else {
            NAP.headlineMsgBox.showInfo("Resend " + result.length + " Failed Mattermost!", { closeBtn: false });
        }

        setTimeout(function () {
            self.getReport();
        }, 3000);
    };

    self.loadTarget = function () {
        sharedScriptManager.blockUI();
        var getReportParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
        };
        self.cachedData.getReportParams = getReportParams;
        $.cookie('MailReport', getReportParams);
        sharedScriptManager.callAjax(self,
            "/MattermostReport/GetTargets",
            getReportParams,
            self.renderTarget,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderTarget = function (result) {
        var targetOptions = [];
        targetOptions.push("<option value='All'>All</option>");
        if (result !== null) {
            for (var i = 0; i < result.length; i++) {
                targetOptions.push("<option value='" + result[i].TargetName + "'>" + result[i].TargetName + "</option>");
            };
        }

        $('#Target').html(targetOptions.join());
    };

    self.loadGroupsByTarget = function () {
        sharedScriptManager.blockUI();
        var getReportParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            TargetName: $('#Target').val()
        };
        self.cachedData.getReportParams = getReportParams;
        $.cookie('MailReport', getReportParams);
        sharedScriptManager.callAjax(self,
            "/MattermostReport/GetGroupsByTarget",
            getReportParams,
            self.renderGroupsByTarget,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderGroupsByTarget = function (result) {
        var groups = [];
        groups.push("<option value='All'>All</option>");
        if (result !== null) {
            for (var i = 0; i < result.length; i++) {
                groups.push("<option value='" + result[i].Group + "'>" + result[i].Group + "</option>");
            };
        }

        $('#Group').html(groups.join());
    };
};

var mattermostReportController = new MattermostReportController();