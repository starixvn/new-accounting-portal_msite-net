/* global NAP: false */
/* global sharedScriptManager: false */
/* global viewResources: false */

$(document).ready(function () {
    updateMailGroupController.initPage();
});

var UpdateMailGroupController = function () {
    var self = this;

    self.initPage = function () {
        self.init();
        self.bindEvents();
    };

    var DOMElements = {
        MailGroupId: null,
        MailGroupName: null,
        Description: null,
        BtnCancel: null,
        BtnSave: null
    };

    self.init = function () {
        self.cacheDOMElement();
    };

    self.cacheDOMElement = function () {
        DOMElements.MailGroupId = $('#MailGroupId');
        DOMElements.MailGroupName = $('#MailGroupName');
        DOMElements.Description = $('#Description');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
    };

    self.bindEvents = function () {
        $("#button-cancel").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.CancelUpdateMailGroup, function () {
                NAP.redirect('MailStation/MailGroupManage/Index', 0);
            });
        });

        $("#button-save").click(function () {
            if (self.validateForm()) {
                sharedScriptManager.callAjax(
                    self,
                    window.rootUrl + "MailStation/MailGroupManage/UpdateMailGroup",
                    self.getParams(),
                    function (result) {
                        if (result.IsSuccessful) {
                            result.Message = viewResources.UpdateMailGroupSuccessMsg;
                        }

                        sharedScriptManager.showNonQueryResult(result, "MailStation/MailGroupManage/Index", 6000);
                    },
                    null,
                    sharedScriptManager.unblockUI
                );
            }
        });
    };

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        if (DOMElements.MailGroupName.val() === '') {
            DOMElements.MailGroupName.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Group Name cannot be empty.'], false);
            return false;
        }

        return true;
    };

    self.getParams = function () {
        return {
            MailGroupId: DOMElements.MailGroupId.val(),
            MailGroupName: DOMElements.MailGroupName.val(),
            Description: DOMElements.Description.val()
        };
    };
};

var updateMailGroupController = new UpdateMailGroupController(); 