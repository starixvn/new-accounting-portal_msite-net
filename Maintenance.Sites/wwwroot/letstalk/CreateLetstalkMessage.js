$(document).ready(function () {
    createLetstalkMessageController.initPage();
});

var CreateLetstalkMessageController = function () {
    var self = this;

    var DOMElements = {
        DivScheduledTime: null,
        ConfigSendTime: null,
        LetstalkGroup: null,
        Create: null,
        Cancel: null,
        ChkScheduledTime: null,
        Subject: null,
        Message: null
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.initScheduledTime();
        self.initDatePicker();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.DivScheduledTime = $('#divScheduledTime');
        DOMElements.ConfigSendTime = $('#ConfigSendTime');
        DOMElements.LetstalkGroup = $('#LetstalkGroup');
        DOMElements.Create = $('#btnCreate');
        DOMElements.Cancel = $('#btnCancel');
        DOMElements.Clear = $('#btnClear');
        DOMElements.ChkScheduledTime = $('#chkScheduledTime');
        DOMElements.Subject = $('#Subject');
        DOMElements.Message = $('#Message');
    };

    self.initScheduledTime = function () {
        DOMElements.DivScheduledTime.hide();
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

    //self.getFiles = function () {
    //    var input = document.getElementById('myFile');
    //    var output = document.getElementById('fileList');
    //    output.innerHTML = '<div>';
    //    for (var i = 0; i < input.files.length; ++i) {
    //        output.innerHTML += '<span class="mr-5">' + input.files.item(i).name + ', </span>';
    //    }
    //    output.innerHTML += '</div>';
    //}

    self.bindEvents = function () {
        DOMElements.Create.click(function () {
            self.insertNotification();
        });

        DOMElements.Clear.click(function () {
            self.resetForm();
        });

        DOMElements.ChkScheduledTime.change(function () {
            if ($(this).is(":checked")) {
                DOMElements.DivScheduledTime.show();
                DOMElements.Create.attr('value', 'CREATE')
            } else {
                DOMElements.DivScheduledTime.hide();
                DOMElements.Create.attr('value', 'SEND NOW')
            }
        });

        DOMElements.Cancel.click(function () {
            NAP.confirm("Cancel Notification", "Are you sure you want to leave this page?", function () {
                window.location.replace("/ManageLetstalkMessage/Index");
            })
        })
    }

    self.resetForm = function () {
        DOMElements.Subject.val('');
        DOMElements.Message.val('');
        DOMElements.LetstalkGroup.val('');
        DOMElements.ChkScheduledTime.attr("checked", false);
        DOMElements.DivScheduledTime.hide();
        DOMElements.ConfigSendTime.val(moment(new Date()).format("MMM DD,YYYY HH:mm"));
    }

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.LetstalkGroup.val() === '') {
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

    self.GetNotificationList = function () {
        var params = {
            FromDate: DOMElements.FromDate.val(),
            ToDate: DOMElements.ToDate.val(),
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

    self.insertNotification = function () {
        if (self.validateForm()) {
            sharedScriptManager.blockUI();
            var data = new FormData();
            //$.each($("input[type='file']")[0].files, function (i, file) {
            //    data.append('files', file);
            //});
            var insertNotificationParams = {
                ConfigSendTime: DOMElements.ChkScheduledTime.is(":checked") ? DOMElements.ConfigSendTime.val() : null,
                Title: DOMElements.Subject.val().trim(),
                Content: DOMElements.Message.val().trim(),
                LstRecipientID: DOMElements.LetstalkGroup.val()
            };
            data.append("insertNotificationCriteria", JSON.stringify(insertNotificationParams));
            $.ajax({
                type: 'POST',
                url: "/ManageLetstalkMessage/InsertNotification",
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    result.Message = viewResources.CreateMessageSuccessMsg;
                    sharedScriptManager.showNonQueryResult(result, "/ManageLetstalkMessage/Index", 6000);
                    self.resetForm();
                },
                complete: function () {
                    sharedScriptManager.unblockUI();
                }
            })
        }
    }

};

var createLetstalkMessageController = new CreateLetstalkMessageController();