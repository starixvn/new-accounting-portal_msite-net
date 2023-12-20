$(document).ready(function () {
    editRecipientController.initPage();
});

var EditRecipientController = function () {
    var self = this;

    self.initPage = function () {
        self.cacheDOMElements();
        self.bindEvents();
    };

    var DOMElements = {
        RecipientID: null,
        RecipientName: null,
        EmailID1: null,
        EmailID2: null,
        EmailID3: null,
        Email1: null,
        Email2: null,
        Email3: null,
        Note: null,
        IsActive: null,
        BtnSave: null,
        BtnCancel: null
    };

    self.cacheDOMElements = function () {
        DOMElements.RecipientID = $('#RecipientId');
        DOMElements.RecipientName = $('#RecipientName');
        DOMElements.EmailID1 = $('#EmailId1');
        DOMElements.EmailID2 = $('#EmailId2');
        DOMElements.EmailID3 = $('#EmailId3');
        DOMElements.Email1 = $('#Email1');
        DOMElements.Email2 = $('#Email2');
        DOMElements.Email3 = $('#Email3');
        DOMElements.Note = $('#Note');
        DOMElements.IsActive = $('#IsActive');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
    }

    self.bindEvents = function () {
        DOMElements.BtnCancel.click(function () {
            NAP.headlineMsgBox.confirm(viewResources.Confirm_CancelEditRecipient, function () {
                NAP.redirect('MailStation/MailRecipient/Index', 0);
            });
        });

        DOMElements.BtnSave.click(function () {
            self.saveRecipient();
        });
    };

    self.saveRecipient = function () {
        if (self.validateRecipientInfo()) {
            sharedScriptManager.blockUI();
            var recipientInfo = self.getRecipientInfo();
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/UpdateRecipient",
            recipientInfo,
            function (result) {
                if (result.IsSuccessful) {
                    result.Message = viewResources.UpdateRecipientSuccessMsg;
                }

                sharedScriptManager.showNonQueryResult(result, "MailStation/MailRecipient/Index", 6000);
            },
            null,
            sharedScriptManager.unblockUI);
        }
    };

    self.getRecipientInfo = function () {
        var recipient = {
            RecipientId: DOMElements.RecipientID.val(),
            Username: DOMElements.RecipientName.val(),
            EmailID1: DOMElements.EmailID1.val(),
            EmailID2: DOMElements.EmailID2.val(),
            EmailID3: DOMElements.EmailID3.val(),
            Email1: DOMElements.Email1.val(),
            Email2: DOMElements.Email2.val(),
            Email3: DOMElements.Email3.val(),
            Note: DOMElements.Note.val(),
            Status: DOMElements.IsActive.is(':checked')
        };

        return recipient;
    };
    self.validateRecipientInfo = function (recipientInfo) {
        sharedScriptManager.hideValidateSummary();
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

        if (email1 !== '' && !sharedScriptManager.isValidEmail(email1)) {
            DOMElements.Email1.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }

        if (email2 !== '' && !sharedScriptManager.isValidEmail(email2)) {
            DOMElements.Email2.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }
        if (email3 !== '' && !sharedScriptManager.isValidEmail(email3)) {
            DOMElements.Email3.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Invalid email, please check again!']);
            return false;
        }

        return true;
    };
};

var editRecipientController = new EditRecipientController();
