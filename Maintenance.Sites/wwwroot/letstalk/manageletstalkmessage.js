$(document).ready(function () {
    manageLetstalkMessageController.initPage();
});

var ManageLetstalkMessageController = function () {
    var self = this;

    var DOMElements = {
        FromDate: null,
        ToDate: null,
        Submit: null,
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.initDatePicker();
        self.bindEvents();
        self.GetNotificationList();
    };

    self.cacheDOMElement = function () {
        DOMElements.FromDate = $('#FromDate');
        DOMElements.ToDate = $('#ToDate');
        DOMElements.Submit = $('#btnSubmit');
    };

    self.initDatePicker = function () {
        DOMElements.FromDate.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        DOMElements.ToDate.datepicker({
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

    self.getFiles = function () {
        var input = document.getElementById('myFile');
        var output = document.getElementById('fileList');
        output.innerHTML = '<div>';
        for (var i = 0; i < input.files.length; ++i) {
            output.innerHTML += '<span class="mr-5">' + input.files.item(i).name + ', </span>';
        }
        output.innerHTML += '</div>';
    }

    self.loadData = function (data) {
        var reportLength = data.length;
        var bodyData = [];
        for (var i = 0; i < reportLength; i++) {
            var dataRow = data[i];
            var rowColumns = [];
            var actionResend = "<div class='pointer-cursor resend' onclick='manageLetstalkMessageController.resendNotification(" + dataRow.NotificationID + "," + dataRow.RecipientID + ")'>Resend</div>";
            var editIcon = dataRow.IsEdit == 1 ? "<div class='inline-flex'><div class='edit-icon pointer-cursor mr-5' onclick='manageLetstalkMessageController.editNotification(" + dataRow.NotificationID + "," + dataRow.RecipientID + ")'></div>" : "";
            var deleteIcon = dataRow.IsDelete == 1 ? "<div class='delete-icon pointer-cursor' onclick='manageLetstalkMessageController.deleteNotification(" + dataRow.NotificationID + ")'></div></div >" : "";
            rowColumns.push({ Name: "No", Value: i + 1 });
            rowColumns.push({ Name: "Subject", Value: dataRow.Title });
            rowColumns.push({ Name: "Message", Value: dataRow.Content });
            rowColumns.push({ Name: "LetstalkGroup", Value: dataRow.ConversationID + " - " + dataRow.RecipientName });
            //rowColumns.push({ Name: "AttachesFile", Value: dataRow.NumberAttachment + " file(s) | <a href='#' class='link-color'>Download All</a>" });
            rowColumns.push({ Name: "SendType", Value: dataRow.NotificationType == 1 ? "Instant" : "Scheduled" });
            rowColumns.push({ Name: "SendTime", Value: dataRow.ConfigSendTimeString });
            rowColumns.push({ Name: "LastSentTime", Value: dataRow.LastSentTimeString });
            rowColumns.push({ Name: "Status", Value: dataRow.Status.toLowerCase().trim() == "failed" ? "<div class='red-color upper-case'>" + dataRow.Status + "</div>" : "<div class='upper-case'>" + dataRow.Status + "</div>"});
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedTimeString });
            rowColumns.push({ Name: "Creator", Value: dataRow.Creator });
            rowColumns.push({ Name: "LatestUpdateTime", Value: dataRow.LastUpdTimeString });
            rowColumns.push({ Name: "LatestEditor", Value: dataRow.LatestEditor });
            rowColumns.push({ Name: "Resend", Value: self.isResend(dataRow.Status.toLowerCase().trim())  ? actionResend : "" });
            rowColumns.push({ Name: "Action", Value: editIcon + deleteIcon });
            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Subject", Width: 150, Name: "Subject", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Message", Width: 250, Name: "Message", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Letstalk Group", Width: 200, Name: "LetstalkGroup", HeaderAlign: "center", CellAlign: "left" },
            //{ HeaderText: "Attaches file(s)", Width: 150, Name: "AttachesFile", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Send Type", Width: 80, Name: "SendType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Scheduled Time", Width: 120, Name: "SendTime", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Last Run", Width: 120, Name: "LastSentTime", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Status", Width: 80, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "right"},
            { HeaderText: "Creator", Width: 100, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Modified", Width: 120, Name: "LatestUpdateTime", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Modified By", Width: 100, Name: "LatestEditor", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Resend", Width: 100, Name: "Resend", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
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
                showDetail: true
            }
        };

        $("#manage-letstalk-message").removeClass("display-none").fanexGrid(gridOptions);
    };

    self.bindEvents = function () {
        DOMElements.Submit.click(function () {
            self.GetNotificationList();
        });
    }

    self.isResend = function (status) {
        if (status == "failed" || status == "sent") {
            return true;
        }
        return false;
    }

    self.GetNotificationList = function () {
        var params = {
            FromDate: DOMElements.FromDate.val(),
            ToDate: DOMElements.ToDate.val() + " 23:59:59.997",
            RecipientID: $('#LetstalkGroup').val(),
            Statusid: $('#Status').val(),
            NotificationID: null
        };
        sharedScriptManager.blockUI();
        sharedScriptManager.callAjax(self,
            "/ManageLetstalkMessage/GetNotificationList",
            params,
            function (result) {
                self.loadData(result);
            },
            null,
            sharedScriptManager.unblockUI);
    }

    self.deleteNotification = function (notificationId) {
        NAP.confirm("Delete Notification", "Are you sure you want to delete this message?", function () {
            var params = {
                NotificationID: notificationId
            };
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
                "/ManageLetstalkMessage/DeleteNotification",
                params,
                function (result) {
                    if (result.IsSuccessful) {
                        NAP.alert("Information", result.Message, function () { NAP.closeDialog(); self.GetNotificationList(); });
                    } else {
                        NAP.alert("Information", result.Message, null);
                    }
                },
                null,
                sharedScriptManager.unblockUI);
        })   
    }

    self.resendNotification = function (notificationId, recipientId) {
        NAP.confirm("Resend Notification", "Are you sure you want to resend this message?", function () {
            var params = {
                NotificationID: notificationId,
                RecipientID: recipientId,
            };
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
                "/ManageLetstalkMessage/ResendNotification",
                params,
                function (result) {
                    if (result.IsSuccessful) {
                        NAP.alert("Information", result.Message, function () { NAP.closeDialog(); self.GetNotificationList(); });
                    } else {
                        NAP.alert("Information", result.Message, null);
                    }
                },
                null,
                sharedScriptManager.unblockUI);
        })
    }

    self.editNotification = function (notificationId, recipientId) {
        location.href = "/ManageLetstalkMessage/EditLetstalkMessage?notificationID=" + notificationId +"&recipientID=" + recipientId +"";
    }

};

var manageLetstalkMessageController = new ManageLetstalkMessageController();