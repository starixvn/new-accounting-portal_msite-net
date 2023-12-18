/* global NAP: false */

"use strict";

var MailList = (function ($) {

    return {
        generateData: function (pageSize, index) {
            var appId = $('#ApplicationID').val();
            var from = $('#From').val();
            var to = $('#To').val();
            var status = $('#MailStatus').val();
            index++;

            $.ajax({
                url: NAP.resolveClientUrl("Mail/MailListPagi"),
                data: { status: status, applicationID: appId, from: from, to: to, page: index, size: pageSize },
                type: 'POST',
                dataType: 'JSON',
                cache: false,
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    var data = result.ListMail;
                    var bodyData = [];
                    var count = 1,
                        $resendMultiContainer = $(".grid-toolbox .btnResend"),
                        checkboxAll = '<input class="mail-checkbox-all" type="checkbox">';

                    if (result.HasResend === 0) {
                        $resendMultiContainer.hide();
                        checkboxAll = "";
                    } else {
                        $resendMultiContainer.show();
                    }

                    $.each(data, function (key, value) {

                        var classFail = "";
                        var linkSendFail = "0";
                        if (value.SentFail > 0) {
                            classFail = "mailfail";
                            linkSendFail = "<a href='#' class='errormail' data-mailid='" + value.MailID + "'>" + value.SentFail + "</a>";
                        }

                        var linkResend = "<a href='#' class='resendmail " + classFail + "' data-mailid='" + value.MailID + "'>Resend</a>";
                        var checkbox = '<input class="mail-checkbox" type="checkbox" data-mailid="' + value.MailID + '">';
                        if (result.HasResend === 0) {
                            linkResend = "";
                            checkbox = "";
                        }

                        var linkViewDetail = value.Subject;
                        if (result.HasViewDetail === 1) {
                            linkViewDetail = "<a href='#' class='emailMessage' data-mailid='" + value.MailID + "'>" + value.Subject + "</a>";
                        }

                        bodyData.push({
                            Columns:
                        [
                            { Name: "Checkbox", Value: checkbox },
                            { Name: "No", Value: pageSize * (index - 1) + count++ },
                            { Name: "Subject", Value: linkViewDetail },
                            { Name: "ApplicationName", Value: '<span title="' + value.Config.Setting.SettingName + '">' + value.Config.AppName + '</span>' },
                            { Name: "MailTo", Value: value.MailTo.split(',').join('<br/>') },
                            { Name: "CreatedDate", Value: value.MailDate },
                            { Name: "LatestSendDate", Value: value.MailUpdatedDate },
                            { Name: "SendSuccess", Value: value.SentSuccess },
                            { Name: "SendFail", Value: linkSendFail },
                            { Name: "Resend", Value: linkResend }
                        ],
                            CssClass: value.SentSuccess === 0 ? "mailfail" : ""
                        });
                    });

                    var gridOption = {
                        columns: [
                            { HeaderText: checkboxAll, Width: 35, Name: "Checkbox", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "#", Width: 35, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Subject", Width: 405, Name: "Subject", HeaderAlign: "center", CellAlign: "left" },
                            { HeaderText: "Application", Width: 125, Name: "ApplicationName", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Mail To", Width: 200, Name: "MailTo", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                            { HeaderText: "Created Date<br/> (GMT -4)", Width: 130, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Latest Send Date<br/> (GMT -4)", Width: 130, Name: "LatestSendDate", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Success", Width: 50, Name: "SendSuccess", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Fail", Width: 40, Name: "SendFail", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "Resend", Width: 70, Name: "Resend", HeaderAlign: "center", CellAlign: "center" }
                        ],
                        bodyRows: bodyData,
                        showPager: true,
                        customPager: true,
                        pagerOption: {
                            itemsPerPage: pageSize,
                            itemsPerPageArray: [50, 100, 500],
                            currentPage: index - 1,
                            totalItem: result.TotalRows,
                            startPerPage: 50,
                            pagerSelectCallBack: MailList.reRenderAfterPaged
                        },
                        gridExpandHeight: 260
                    };

                    $("#gridId").fanexGrid(gridOption);
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },

        reRenderAfterPaged: function (pageIndex, itemsPerPage) {
            MailList.generateData(itemsPerPage, pageIndex);
        },

        resend: function (mailId) {
            $.ajax({
                url: NAP.resolveClientUrl("Mail/ResendMail"),
                data: { mailId: mailId },
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    if (result) {
                        NAP.headlineMsgBox.showInfo("Resend Success !", { closeBtn: false });
                    }
                    else {
                        NAP.headlineMsgBox.showInfo("Resend Failed !", { closeBtn: false });
                    }

                    MailList.generateData(50, 0);
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },

        resendMultipleMails: function () {
            var mailIds = $(".freegrid .mail-checkbox:checked").map(function () { return $(this).data("mailid"); }).get();

            if (mailIds && mailIds.length > 0) {
                if (confirm("Resend the chosen emails ?")) {
                    $.ajax({
                        url: NAP.resolveClientUrl("Mail/ResendMultipleMails"),
                        data: $.param({ mailIds: mailIds }, true),
                        type: 'POST',
                        cache: false,
                        beforeSend: function () {
                            NAP.loadingBox.show();
                        },
                        complete: function () {
                            NAP.loadingBox.close();
                        },
                        success: function (result) {
                            NAP.headlineMsgBox.showInfo("Resent the chosen mails. Please check their statuses.", { closeBtn: false });

                            MailList.generateData(50, 0);
                        },
                        error: function (xhr, status, err) {
                            NAP.handleAjaxError(xhr, status, err);
                        }
                    });
                }
            } else {
                NAP.headlineMsgBox.showInfo("Please choose at least one mail to resend!", { closeBtn: false });
            }
        },

        getMessage: function (mailId) {
            $.ajax({
                url: NAP.resolveClientUrl("Mail/GetMessage"),
                data: { mailId: mailId },
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    if (result.length > 0) {
                        NAP.popup("Details", result);
                    }
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },

        getError: function (mailId) {
            $.ajax({
                url: NAP.resolveClientUrl("Mail/GetError"),
                data: { mailId: mailId },
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    if (result.length > 0) {
                        NAP.popup("Details", result);
                    }
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        }
    };

}(jQuery));

$(document).ready(function () {

    $('.resendmail').live('click', function () {
        if (confirm("Resend this email ?")) {
            var mailId = $(this).attr("data-mailid");
            MailList.resend(mailId);
        }
    });

    $('.emailMessage').live('click', function () {
        var mailId = $(this).attr("data-mailid");
        MailList.getMessage(mailId);
    });

    $('.errormail').live('click', function () {
        var mailId = $(this).attr("data-mailid");
        MailList.getError(mailId);
    });

    MailList.generateData(50, 0);

    $('#submit').live('click', function () {
        MailList.generateData(50, 0);
    });

    $("#btnResendMulti").click(function () {
        MailList.resendMultipleMails();
    });

    $(".freegrid").on("click", ".mail-checkbox-all", function () {
        var isCheckAll = $(this).prop("checked");

        $(".freegrid .mail-checkbox").prop("checked", isCheckAll);
    });

    $(".freegrid").on("click", ".mail-checkbox", function () {
        var isChecked = $(this).prop("checked"),
            $checkboxAll = $(".freegrid .mail-checkbox-all");

        if (!isChecked) {
            $checkboxAll.prop("checked", false);
        } else {
            var isCheckAll = $(".freegrid .mail-checkbox:checked").length === $(".freegrid .mail-checkbox").length;

            $checkboxAll.prop("checked", isCheckAll);
        }
    });
});