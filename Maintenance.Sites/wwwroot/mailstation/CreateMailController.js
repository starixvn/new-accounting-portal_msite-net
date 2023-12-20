$(document).ready(function () {
    insertMailController.initPage();
});

var InsertMailController = function () {
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
        data: window.rootUrl + "MailStation/MailManage/SuggestMail"
    };

    self.initPage = function () {
        self.init();
        self.bindEvents();
    };

    var DOMElements = {
        MailGroupId: null,
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
    };

    self.cacheDOMElement = function () {
        DOMElements.MailGroupId = $('#MailGroupId');
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

        DOMElements.TriggerType.change(self.triggerChange);

        $("#button-cancel").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.CancelCreateMail, function () {
                NAP.redirect('MailStation/MailManage/Index', 0);
            });
        });

        $("#button-save").click(function () {
            var params = self.getParams();
            if (self.validateForm(params)) {
                sharedScriptManager.callAjax(
                    self,
                    window.rootUrl + "MailStation/MailManage/CreateMail",
                    params,
                    function (result) {
                        if (result.IsSuccessful) {
                            result.Message = viewResources.CreateMailSuccessMsg;
                        }

                        sharedScriptManager.showNonQueryResult(result, "MailStation/MailManage/Index", 6000);
                    },
                    null,
                    sharedScriptManager.unblockUI
                );
            }
        });
    };

    self.validateForm = function (reportParams) {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.nexcron-validation-error').removeClass('nexcron-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.MailName.val() === '') {
            DOMElements.MailName.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Mail Name cannot be empty.'], false);
            return false;
        }

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
            //            else if (cronExpression.RepeatType === '1') {
            //                // Weekly
            //                if ($('[id^="nexcron-weekly-"]:checked').length === 0) {
            //                    sharedScriptManager.createValidationSummary(['At least one day must be selected.'], false);
            //                    return false;
            //                }
            //            }
        }

        return true;
    };

    self.getParams = function () {
        var triggerType = DOMElements.TriggerType.val();
        if (triggerType === Enums.TriggerType.Function) {
            return {
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
            IsActive: DOMElements.IsActive.is(':checked'),
            StartOn: cronResult.StartOn,
            EndOn: cronResult.EndOn,
            Attempt: cronResult.Attempt,
            Interval: cronResult.Interval,
            Description: DOMElements.Description.val(),
            Owners: DOMElements.Owners.val()
        };
    };

    self.addGroup = function (groupName) {
        var groupName = $('span[id="MailGroupId"]>input').val();
        if (groupName === undefined || groupName === null || groupName === '') {
            sharedScriptManager.createValidationSummary(['The group name cannot be empty, please try again!'], false);
        }
        else {
            sharedScriptManager.callAjax(
                self,
                window.rootUrl + "MailStation/MailManage/AddMailGroup",
                { MailGroupName: groupName },
                function (result) {
                    if (!result.IsSuccessful) {
                        result.Message = 'The group exists, please try again!';
                        sharedScriptManager.showNonQueryResult(result, null, null);
                    }
                    else {
                        result.Message = 'Group is added successfully.';
                        sharedScriptManager.showNonQueryResult(result, null, null);
                        self.refreshMailGroup(groupName);
                    }
                },
                null,
                sharedScriptManager.unblockUI
            );
        }
    };

    self.refreshMailGroup = function (selectedGroupName) {
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailGroups",
            { hasAllElement: false },
            function (result) {
                var mailGroupHtml = '';
                for (var i = 0; i < result.length; i++) {
                    mailGroupHtml += '<option value="' + result[i].Value + '" ' +
                    (result[i].Text === selectedGroupName ? 'selected' : '') +
                    ' >' + result[i].Text + '</option>';
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

var insertMailController = new InsertMailController();