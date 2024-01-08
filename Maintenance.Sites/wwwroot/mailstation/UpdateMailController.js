/* global sharedScriptManager: false */
/* global viewResources: false */

/// <reference path="../../../../../scripts/ShareScripts/SharedScriptManager.js" />
$(document).ready(function () {
    updateMailController.initPage();
});

var UpdateMailController = function () {
    var self = this;

    var Enums = {
        TriggerType: {
            Function: '0',
            Schedule: '1'
        }
    };

    var magicSuggestOptions = {
        maxSelection: null,
        highlight: false,
        allowFreeEntries: false,
        data: "/MailStation/MailManage/SuggestMail",
    };

    self.initPage = function () {
        self.init();
        self.bindEvents();
    };

    var DOMElements = {
        MailGroupId: null,
        MailId: null,
        MailName: null,
        SiteId: null,
        TriggerType: null,
        RepeatType: null,
        CronExpression: null,
        NexCron: null,
        StartOn: null,
        EndOn: null,
        Attempt: null,
        Interval: null,
        MailFrom: null,
        MailTo: null,
        MailCc: null,
        MailBcc: null,
        Description: null,
        Owners: null,
        IsActive: null,
        BtnCancel: null,
        BtnSave: null
    };

    var initualValueForMagicSuggestion = function ($ms) {
        var valueArr = $ms.val();
        if (valueArr != undefined && valueArr.length > 0) {
            valueArr = $.map(valueArr.split(','), function (value) { return parseInt(value) });
        }
        else {
            valueArr = [];
        }

        $ms.attr('value', '');
        $ms = $ms.magicSuggest(magicSuggestOptions);
        $($ms).on('load', function () {
            if (this._dataSet === undefined) {
                this._dataSet = true;
                $ms.setValue(valueArr);
                $ms.setDataUrlParams({});
            }
        });

        return $ms;
    }

    self.init = function () {
        self.cacheDOMElement();
        $('select.combobox-autocomplete').each(function () {
            $(this).combobox();
        });

        DOMElements.MailTo = initualValueForMagicSuggestion(DOMElements.MailTo);
        DOMElements.MailCc = initualValueForMagicSuggestion(DOMElements.MailCc);
        DOMElements.MailBcc = initualValueForMagicSuggestion(DOMElements.MailBcc);
        DOMElements.TriggerType.change(self.triggerChange);
        DOMElements.TriggerType.change();

        // Is Schedule Mail
        if (DOMElements.TriggerType.val() === '1') {
            var cronResult = {
                RepeatType: DOMElements.RepeatType.val(),
                CronExpression: DOMElements.CronExpression.val(),
                StartOn: DOMElements.StartOn.val(),
                EndOn: DOMElements.EndOn.val(),
                Attempt: DOMElements.Attempt.val(),
                Interval: DOMElements.Interval.val()
            };

            DOMElements.NexCron.nexCron('setCronExpression', cronResult);
        }
    };

    self.cacheDOMElement = function () {
        DOMElements.MailGroupId = $('#MailGroupId');
        DOMElements.MailId = $('#MailId');
        DOMElements.MailName = $('#MailName');
        DOMElements.SiteId = $('#SiteId');
        DOMElements.TriggerType = $('#TriggerType');
        DOMElements.RepeatType = $('#RepeatType');
        DOMElements.NexCron = $('#nexCron');
        DOMElements.CronExpression = $('#CronExpression');
        DOMElements.StartOn = $('#StartOn');
        DOMElements.EndOn = $('#EndOn');
        DOMElements.Attempt = $('#Attempt');
        DOMElements.Interval = $('#Interval');
        DOMElements.MailFrom = $('#MailFrom');
        DOMElements.MailTo = $('#MailTo');
        DOMElements.MailCc = $('#MailCc');
        DOMElements.MailBcc = $('#MailBcc');
        DOMElements.Description = $('#Description');
        DOMElements.Owners = $('#Owners');
        DOMElements.IsActive = $('#IsActive');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
    };

    self.bindEvents = function () {
        $("#btn-add-group").click(function () {
            self.addGroup($(".ui-autocomplete-input").val());
        });

        $("#button-cancel").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.CancelUpdateMail, function () {
                NAP.redirect('/MailStation/MailManage/Index', 0);
            });
        });

        $("#button-save").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.ConfirmClearHistoryWhenUpdateMail, function () {
                if (self.validateForm()) {
                    sharedScriptManager.callAjax(
                        self,
                        "/MailStation/MailManage/UpdateMail",
                        self.getParams(),
                        function (result) {
                            if (result.IsSuccessful) {
                                result.Message = viewResources.UpdateMailSuccessMsg;
                            }

                            sharedScriptManager.showNonQueryResult(result, "/MailStation/MailManage/Index", 6000);
                        },
                        null,
                        sharedScriptManager.unblockUI
                    );
                }
            });
            
        });
    };

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.nexcron-validation-error').removeClass('nexcron-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if ($.trim(DOMElements.MailName.val()) === '') {
            DOMElements.MailName.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Mail Name cannot be empty.'], false);
            return false;
        }

        //        if (!sharedScriptManager.isValidEmail(DOMElements.MailFrom.val())) {
        //            DOMElements.MailFrom.addClass('input-validation-error');
        //            sharedScriptManager.createErrorSummary(['Mail From is not a valid email.']);
        //            return false;
        //        }

        if (DOMElements.MailTo.getValue().length === 0) {
            $(DOMElements.MailTo).addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Mail To cannot be empty.'], false);
            $('#MailTo').addClass('magic-suggest-error');
            $('input', '#MailTo').addClass('magic-suggest-error');
            return false;
        }

        if (DOMElements.TriggerType.val() === Enums.TriggerType.Schedule) {
            var cronExpression = DOMElements.NexCron.nexCron('getCronExpression');
            if (cronExpression.RepeatType === '2') {
                var repeaton = $('#nexcron-repeat-on-day').val().split(',');
                for (var i = 0; i < repeaton.length; i++) {
                    var day = repeaton[i];
                    if (parseInt(day) > 28) {
                        $('#nexcron-repeat-on-day').addClass('nexcron-validation-error');
                        sharedScriptManager.createValidationSummary(['Date should not be greater than 28.'], false);
                        return false;
                    }
                }
            }           
        }

        return true;
    };

    self.getParams = function () {
        var triggerType = DOMElements.TriggerType.val();
        if (triggerType === Enums.TriggerType.Function) {
            return {
                MailId: DOMElements.MailId.val(),
                MailName: DOMElements.MailName.val(),
                MailGroupId: DOMElements.MailGroupId.val(),
                SiteId: DOMElements.SiteId.val(),
                TriggerType: DOMElements.TriggerType.val(),
                MailFrom: DOMElements.MailFrom.val(),
                MailTo: DOMElements.MailTo.getValue().join(','),
                MailCc: DOMElements.MailCc.getValue().join(','),
                MailBcc: DOMElements.MailBcc.getValue().join(','),
                IsActive: DOMElements.IsActive.is(':checked'),
                Description: DOMElements.Description.val(),
                Owners: DOMElements.Owners.val()
            };
        }

        var cronResult = DOMElements.NexCron.nexCron('getCronExpression');
        return {
            MailId: DOMElements.MailId.val(),
            MailName: DOMElements.MailName.val(),
            MailGroupId: DOMElements.MailGroupId.val(),
            SiteId: DOMElements.SiteId.val(),
            TriggerType: DOMElements.TriggerType.val(),
            RepeatType: cronResult.RepeatType,
            CronExpression: cronResult.CronExpression,
            MailFrom: DOMElements.MailFrom.val(),
            MailTo: DOMElements.MailTo.getValue().join(','),
            MailCc: DOMElements.MailCc.getValue().join(','),
            MailBcc: DOMElements.MailBcc.getValue().join(','),
            IsActive: DOMElements.IsActive.is(":checked"),
            StartOn: cronResult.StartOn,
            EndOn: cronResult.EndOn,
            Attempt: cronResult.Attempt,
            Interval: cronResult.Interval,
            Description: DOMElements.Description.val(),
            Owners: DOMElements.Owners.val()
        };
    };

    self.addGroup = function (groupName) {
        sharedScriptManager.callAjax(
            self,
            "/MailStation/MailManage/AddMailGroup",
            { MailGroupName: groupName },
            function (result) {
                if (result.IsSuccessful) {
                    result.Message = viewResources.AddMailGroupSuccessMsg;
                    sharedScriptManager.showNonQueryResult(result, null, null, self.refreshMailGroup(groupName));
                }
                else {
                    sharedScriptManager.showNonQueryResult(result, null, null);
                }
            },
            null,
            sharedScriptManager.unblockUI
        );
    };

    self.refreshMailGroup = function (groupName) {
        sharedScriptManager.callAjax(self,
            "/MailStation/MailManage/MailGroups",
            { hasAllElement: false },
            function (result) {
                var mailGroupHtml = '';
                for (var i = 0; i < result.length; i++) {
                    mailGroupHtml += '<option value="' + result[i].Value + '"' + (groupName === result[i].Text ? ' selected' : '') + '>' + result[i].Text + '</option>';
                }

                DOMElements.MailGroupId.empty().append(mailGroupHtml);
            },
            null,
            sharedScriptManager.unblockUI);
    };

    self.triggerChange = function () {
        var triggerId = DOMElements.TriggerType.val();
        if (triggerId === Enums.TriggerType.Schedule) {
            $(".schedule-row").removeClass("display-none");
            DOMElements.NexCron.nexCron();
        } else {
            $(".schedule-row").addClass("display-none");
        }
    };
};

var updateMailController = new UpdateMailController(); 