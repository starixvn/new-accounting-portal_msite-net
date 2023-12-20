/* global NAP: false */
/* global sharedScriptManager: false */
/* global viewResources: false */

$(document).ready(function () {
    createRecipientController.initPage();
});

var CreateRecipientController = function () {
    var self = this;

    var DOMElements = {
        UserName: null,
        Email1: null,
        Email2: null,
        Email3: null,
        Note: null,
        Status: null,
        IsNAPUser: null,
        NAPUserID: null,
        BtnSave: null,
        BtnCancel: null,
        BtnLoading: null
    };

    self.initPage = function () {
        self.cachedDOMElements();
        self.bindEvents();
    };

    self.cachedDOMElements = function () {
        DOMElements.UserName = $('#UserName');
        DOMElements.Email1 = $('#Email1');
        DOMElements.Email2 = $('#Email2');
        DOMElements.Email3 = $('#Email3');
        DOMElements.Note = $('#Note');
        DOMElements.Status = $('#Status');
        DOMElements.IsNAPUser = $('#IsNAPUser');
        DOMElements.NAPUserID = $('#NAPUserID');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
        DOMElements.BtnLoading = $('#btn-loading');
    }

    self.bindEvents = function () {
        DOMElements.BtnCancel.click(function () {
            NAP.headlineMsgBox.confirm(viewResources.Confirm_CancelInsertRecipient, function () {
                NAP.redirect('MailStation/MailRecipient/Index', 0);
            });
        });

        DOMElements.BtnSave.click(function () {
            self.saveRecipient();
        });
        DOMElements.UserName.blur(function () {
            self.checkRecipientInfo();
        });
    };

    self.saveRecipient = function () {
        if (self.validateRecipientInfo()) {
            var recipientInfo = self.getParams();
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(
                self,
                window.rootUrl + "MailStation/MailRecipient/CreateRecipient",
                recipientInfo,
                function (result) {
                    if (result.IsSuccessful) {
                        result.Message = viewResources.CreateRecipientSuccessMsg;
                    }

                    sharedScriptManager.showNonQueryResult(result, "MailStation/MailRecipient/Index", 6000);
                },
                null,
                sharedScriptManager.unblockUI
            );
        }
    };

    self.getParams = function () {
        var recipient = {
            Username: DOMElements.UserName.val(),
            Email1: DOMElements.Email1.val(),
            Email2: DOMElements.Email2.val(),
            Email3: DOMElements.Email3.val(),
            Note: DOMElements.Note.val(),
            IsNAPUser: DOMElements.IsNAPUser.val(),
            NAPUserID: DOMElements.NAPUserID.val(),
            Status: DOMElements.Status.is(":checked")
        };

        return recipient;
    };

    self.validateRecipientInfo = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        var email1 = DOMElements.Email1.val();
        var email2 = DOMElements.Email2.val();
        var email3 = DOMElements.Email3.val();

        if (($.trim(email1) === $.trim(email2) && email1 !== '' && email2 !== '')
            || ($.trim(email1) === $.trim(email3) && email1 !== '' && email3 !== '')
            || ($.trim(email2) === $.trim(email3) && email2 !== '' && email3 !== '')) {
            DOMElements.Email1.addClass('input-validation-error');
            DOMElements.Email2.addClass('input-validation-error');
            DOMElements.Email3.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Email address is reduntdant, please check again!']);
            return false;
        }

        if (email1 !== undefined && email1 !== '' && !sharedScriptManager.isValidEmail(email1)) {
            DOMElements.Email1.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }

        if (email2 !== undefined && email2 !== '' && !sharedScriptManager.isValidEmail(email2)) {
            DOMElements.Email2.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }

        if (email3 !== undefined && email3 !== '' && !sharedScriptManager.isValidEmail(email3)) {
            DOMElements.Email3.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }

        return true;
    };

    self.checkRecipientInfo = function () {
        var checkParams = {
            Username: DOMElements.UserName.val()
        };

        if (checkParams.Username !== '') {
            DOMElements.BtnLoading.removeClass("display-none");
            sharedScriptManager.callAjax(
                self,
                window.rootUrl + "MailStation/MailRecipient/CheckRecipient",
                checkParams,
                self.renderCheckRecipientInfoResult,
                null,
                null
            );
        }
    };

    self.renderCheckRecipientInfoResult = function (result) {
        DOMElements.BtnLoading.addClass("display-none");
        self.copyRecipientInfo(result);
    };

    self.copyRecipientInfo = function (result) {
        if (!result) {
            DOMElements.IsNAPUser.val(false);
            DOMElements.NAPUserID.val(0);
            return;
        }

        DOMElements.IsNAPUser.val(true);
        DOMElements.NAPUserID.val(result.Id);
        DOMElements.Email1.val(result.Email);
    };
};

var createRecipientController = new CreateRecipientController();
