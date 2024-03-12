$(document).ready(function () {
    createApplicationSettingController.initPage();
});

var CreateApplicationSettingController = function () {
    var self = this;

    var DOMElements = {
        ApplicationName: null,
        MicrosoftAppId: null,
        MicrosoftAppPssword: null,
        ShowPassword: null,
        Description: null,
        Create: null,
        Cancel: null
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.ApplicationName = $('#ApplicationName');
        DOMElements.MicrosoftAppId = $('#MicrosoftAppId');
        DOMElements.MicrosoftAppPassword = $('#MicrosoftAppPassword');
        DOMElements.Description = $('#Description');
        DOMElements.Cancel = $('#btnCancel');
        DOMElements.Create = $('#btnCreate');
        DOMElements.ShowPassword = $('#ShowPassword');
    };

    self.bindEvents = function () {
        DOMElements.Create.click(function () {
            self.insertApplicationSetting();
        });

        DOMElements.Cancel.click(function () {
            NAP.confirm("Cancel Notification", "Are you sure you want to leave this page?", function () {
                window.location.replace("/ApplicationSetting/Index");
            })
        })
    }
    self.resetForm = function () {
        DOMElements.ApplicationName.val('');
        DOMElements.MicrosoftAppId.val('');
        DOMElements.MicrosoftAppPassword.val('');
        DOMElements.Description.val('');
    }
    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.ApplicationName.val() === '') {
            $('#ApplicationName').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Application Name cannot be empty'], false);
            return false;
        }

        if (DOMElements.MicrosoftAppId.val() === '') {
            DOMElements.MicrosoftAppId.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Microsoft App Id cannot be empty.'], false);
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

    self.insertApplicationSetting = function () {
        if (self.validateForm()) {
            sharedScriptManager.blockUI();
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "/ApplicationSetting/Create",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    applicationName: DOMElements.ApplicationName.val().trim(),
                    microsoftAppId: DOMElements.MicrosoftAppId.val().trim(),
                    microsoftAppPassword: DOMElements.MicrosoftAppPassword.val(),
                    description: DOMElements.Description.val()
                }),
                success: function (result) {
                    if (result.IsSuccessful) {
                        result.Message = 'Create application setting successfully!';
                        self.resetForm();
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

var createApplicationSettingController = new CreateApplicationSettingController();