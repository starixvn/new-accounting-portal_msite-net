$(document).ready(function () {
    editLetstalkMessageController.initPage();
});

var EditLetstalkMessageController = function () {
    var self = this;

    var DOMElements = {
        DivScheduledTime: null,
        ConfigSendTime: null,
        LetstalkGroup: null,
        Save: null,
        Cancel: null,
        ChkScheduledTime: null,
        Subject: null,
        Message: null,
        NotificationID: null
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.initData();
        self.initDatePicker();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.DivScheduledTime = $('#divScheduledTime');
        DOMElements.ConfigSendTime = $('#ConfigSendTime');
        DOMElements.LetstalkGroup = $('#LetstalkGroup');
        DOMElements.Save = $('#btnSave');
        DOMElements.Cancel = $('#btnCancel');
        DOMElements.ChkScheduledTime = $('#chkScheduledTime');
        DOMElements.Subject = $('#Subject');
        DOMElements.Message = $('#Message');
        DOMElements.NotificationID = $("#NotificationID");
    };

    self.initData = function () {
        var data = JSON.parse($("#notificationModel").val());
        DOMElements.Subject.val(data.Title);
        DOMElements.Message.val(data.Content);
        DOMElements.LetstalkGroup.val(data.RecipientID);
        DOMElements.ChkScheduledTime.attr("checked", true);
        DOMElements.DivScheduledTime.show();
        //DOMElements.ConfigSendTime.val(data.ConfigSendTime);
    }

    self.initDatePicker = function () {
        // var time = moment(new Date()).zone("-04:00");
        DOMElements.ConfigSendTime.datetimepicker({
            dateFormat: "M dd,yy",
            timeFormat: 'HH:mm',
            controlType: 'select',
            oneLine: true,
            timezone: "-0400"
        });
    };

    self.bindEvents = function () {
        DOMElements.Save.click(function () {
            self.editNotification();
        });

        DOMElements.Cancel.click(function () {
            NAP.confirm("Cancel Notification", "Are you sure you want to leave this page?", function () {
                window.location.replace("/ManageLetstalkMessage/Index");
            })
        });

        DOMElements.ChkScheduledTime.change(function () {
            if ($(this).is(":checked")) {
                DOMElements.DivScheduledTime.show();
            } else {
                DOMElements.DivScheduledTime.hide();
            }
        });
    }

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.LetstalkGroup.val() == '') {
            $('#LetstalkGroup').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Please select at least one Letstalk Group!'], false);
            return false;
        }

        if (DOMElements.Subject.val() === '') {
            DOMElements.Subject.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Subject cannot be empty.'], false);
            return false;
        }

        if (DOMElements.Message.val() === '') {
            DOMElements.Message.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Message cannot be empty.'], false);
            return false;
        }

        var isBefore = moment(DOMElements.ConfigSendTime.val()).isBefore(moment(new Date()).zone("-04:00").format("MMM DD,YYYY HH:mm"));
        if (DOMElements.ChkScheduledTime.is(":checked") && isBefore) {
            $('#ConfigSendTime').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['The scheduled date time should not be in the past!'], false);
            return false;
        }

        return true;
    };

    self.editNotification = function () {
        if (self.validateForm()) {
            NAP.modal("Edit Notification", "Are you sure you want to save the information above?", function () {
                sharedScriptManager.blockUI();
                var data = new FormData();
                var editNotificationParams = {
                    ConfigSendTime: DOMElements.ChkScheduledTime.is(":checked") ? DOMElements.ConfigSendTime.val() : null,
                    Title: DOMElements.Subject.val().trim(),
                    Content: DOMElements.Message.val().trim(),
                    LstRecipientID: DOMElements.LetstalkGroup.val(),
                    NotificationID: DOMElements.NotificationID.val()
                };
                data.append("editNotificationCriteria", JSON.stringify(editNotificationParams));
                $.ajax({
                    type: 'POST',
                    url: "/ManageLetstalkMessage/EditNotification",
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result.IsSuccessful) {
                            result.Message = viewResources.UpdateMessageSuccessMsg;
                            sharedScriptManager.showNonQueryResult(result, "/ManageLetstalkMessage/Index", 6000);
                        } else {
                            NAP.confirm("Information", result.Message, function () { window.location.replace("/ManageLetstalkMessage/Index"); });
                        }

                    },
                    complete: function () {
                        sharedScriptManager.unblockUI();
                    }
                })
            })
        }
        
    }

};

var editLetstalkMessageController = new EditLetstalkMessageController();