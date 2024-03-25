$(document).ready(function () {
    updateApplicationSettingController.initPage();
});

var UpdateApplicationSettingController = function () {
    var self = this;

    var DOMElements = {
        AppId: null,
        ApplicationName: null,
        MicrosoftAppId: null,
        MicrosoftAppPassword: null,
        Description: null,
        Update: null,
        Cancel: null,
        Active: null,
        HiddenContent: null,
        ShowPwd: null,
        ChangePwd: null,
    };

    var isChangedPassword = false;

    self.initPage = function () {
        self.cacheDOMElement();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.AppId = $('#AppId');
        DOMElements.ShowPwd = $('.show-pwd');
        DOMElements.ChangePwd = $('.change-pwd');
        DOMElements.ApplicationName = $('#ApplicationName');
        DOMElements.MicrosoftAppPassword = $('#MicrosoftAppPassword');
        DOMElements.Description = $('#Description');
        DOMElements.Cancel = $('#btnCancel');
        DOMElements.Active = $('#Active');
        DOMElements.HiddenContent = $('#HiddenContent');
        DOMElements.Update = $('#btnUpdate');
    };

    self.bindEvents = function () {
        DOMElements.Update.click(function () {
            self.updateApplicationSetting();
        });

        DOMElements.Cancel.click(function () {
            NAP.confirm("Cancel Notification", "Are you sure you want to leave this page?", function () {
                window.location.replace("/ApplicationSetting/Index");
            })
        });

        DOMElements.ShowPwd.click(function () {
            var inputType = DOMElements.MicrosoftAppPassword.prop("type");
            if (inputType === "password") {
                DOMElements.MicrosoftAppPassword.prop("type", "text");
                $(this).html('Hide');
            } else {
                DOMElements.MicrosoftAppPassword.prop("type", "password");
                $(this).html('Show');
            }
        });

        DOMElements.ChangePwd.click(function () {
            $(this).hide();
            DOMElements.ShowPwd.show();
            DOMElements.MicrosoftAppPassword.val('');
            DOMElements.MicrosoftAppPassword.prop("disabled", false).prop("readonly", false); 
            isChangedPassword = true;
        });
    }
    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.ApplicationName.val() === '') {
            $('#ApplicationName').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Application Name cannot be empty'], false);
            return false;
        }

        if (DOMElements.MicrosoftAppPassword.val() === '') {
            DOMElements.MicrosoftAppPassword.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Microsoft App Password cannot be empty.'], false);
            return false;
        }
        sharedScriptManager.hideValidateSummary();
        return true;
    };

    self.updateApplicationSetting = function () {
        if (self.validateForm()) {
            sharedScriptManager.blockUI();
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "/ApplicationSetting/Update",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    appId: parseInt(DOMElements.AppId.val()),
                    appName: DOMElements.ApplicationName.val().trim(),
                    microsoftAppPassword: DOMElements.MicrosoftAppPassword.val(),
                    active: DOMElements.Active.is(':checked'),
                    description: DOMElements.Description.val(),
                    hiddenContent: DOMElements.HiddenContent.is(':checked'),
                    isChangedPassword: isChangedPassword
                }),
                success: function (result) {
                    if (result.IsSuccessful) {
                        result.Message = 'Update application setting successfully!';
                    }
                    sharedScriptManager.showNonQueryResult(result, "/ApplicationSetting/Index", 3000);
                },
                complete: function () {
                    sharedScriptManager.unblockUI();
                }
            })
        }
    }
};

var updateApplicationSettingController = new UpdateApplicationSettingController();