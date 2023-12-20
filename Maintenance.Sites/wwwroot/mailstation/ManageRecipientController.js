/* global NAP: false */
/* global sharedScriptManager: false */
/* global viewResources: false */

$(document).ready(function () {
    manageRecipientController.initPage();
});

var ManageRecipientController = function () {
    var self = this;
    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.getRecipientList();
        contextMenuController.bindContextMenuEvents();
    };

    self.cachedData = {
        searchParams: null
    };

    var cacheObject = {
        bodyData: null
    };

    var DOMElements = {
        emails: null
    };

    self.loadFilter = function () {
        self.renderDropdownStatus();
    };

    self.renderDropdownStatus = function () {
        var statuses = [{ Id: -1, Name: 'All' },
            { Id: 1, Name: 'Active' },
            { Id: 0, Name: 'Inactive'}];
        var options = '';
        var statusLength = statuses.length;
        for (var i = 0; i < statusLength; i++) {
            var status = statuses[i];
            options += '<option value="' + status.Id + '">' + status.Name + '</option>'
        }

        $("#status").empty().append(options);
    };
    self.bindEvents = function () {
        $("#typeCust").attr("disabled", null);
        $("#button-search").click(function () {
            self.getRecipientList();
        });

        $([$("#UserName"), $("#Emails")]).each(function () {
            $(this).keypress(function (e) {
                if (e.which == 13) {
                    self.getRecipientList();
                }
            });
        });
    };

    self.getRecipientList = function () {
        sharedScriptManager.blockUI();
        var searchParams = {
            Username: $("#UserName").val(),
            Emails: $("#Emails").val(),
            Status: $("#Status").val()
        };
        self.cachedData.searchParams = searchParams;
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/RecipientList",
            searchParams,
            self.renderRecipientList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderRecipientList = function (result) {
        //        result = self.dummyData();
        var recipientListLength = result.length;
        var bodyData = [];
        for (var i = 0; i < recipientListLength; i++) {
            var recipient = result[i];
            var rowColumns = [];
            var recipientIndex = ' recipient-index="' + recipient.RecipientId + '"';
            rowColumns.push({ Name: "ColId", Value: recipient.RecipientId });
            rowColumns.push({
                Name: "ColUsername",
                Value: viewResources.hasEditRight ?
                            ('<a class="link username" href="javascript:;"' + recipientIndex + '>' +
                               recipient.RecipientName +
                            '</a>') :
                             recipient.RecipientName
            });
            rowColumns.push({ Name: "ColNapUser", Value: recipient.IsNapUser ? '<div class="ico-tick"></div>' : '' });
            rowColumns.push({ Name: "ColEmail1", Value: recipient.Email1 });
            rowColumns.push({ Name: "ColEmail2", Value: recipient.Email2 });
            rowColumns.push({ Name: "ColEmail3", Value: recipient.Email3 });
            rowColumns.push({
                Name: "ColNoRelevantMail",
                Value: '<div class="ico-relevant-mail"' + recipientIndex + '>' + recipient.MailRelevant + '</div>'
            });
            rowColumns.push({ Name: "ColActive", Value: recipient.IsActive ? '<div class="ico-tick"></div>' : '' });
            if (viewResources.hasEditRight) {
                rowColumns.push({
                    Name: "ColAction",
                    Value: '<div class="action-wrp">\
                                    <span class="btn-action"' + recipientIndex + '>Action</span>\
                                    <div class="relative"></div>\
                                </div>'
                });
            }

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: viewResources.Id, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Username, Width: 300, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Nap_User, Width: 85, Name: "ColNapUser", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Email1, Width: 180, Name: "ColEmail1", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Email2, Width: 180, Name: "ColEmail2", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Email3, Width: 180, Name: "ColEmail3", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.No_RelevantMail, Width: 110, Name: "ColNoRelevantMail", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Active, Width: 50, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 50, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
        }

        var reRenderAfterPaged = function (page_index, itemsPerPage, jq) {
            gridOptions.pagerOption.itemsPerPage = itemsPerPage;
            gridOptions.pagerOption.currentPage = page_index;
            var startIndex = (page_index * itemsPerPage);
            var endIndex = ((page_index + 1) * itemsPerPage);
            var currentBodyRows = cacheObject.bodyData.slice(startIndex, endIndex);
            gridOptions.bodyRows = currentBodyRows;
            $("#recipient-list").fanexGrid(gridOptions);
            self.bindGridEvent();
            return false;
        };

        cacheObject.bodyData = bodyData;
        var startIndex = 0;
        var currentBodyRows = cacheObject.bodyData.slice(0, sharedScriptManager.cachedData.defaultItemsPerPage);
        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: currentBodyRows,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30,
            gridExpandHeight: 350,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: cacheObject.bodyData.length,
                pagerSelectCallBack: reRenderAfterPaged
            }
        };

        $("#recipient-list").removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
    };

    self.bindGridEvent = function () {
        var gridContainer = $("#recipient-list");
        $(".btn-action").click(function (event) {
            var $this = $(this);
            var position = $this.offset();
            contextMenuController.showContextMenu(position);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            contextMenuController.bindEvents({
                editEvent: function () {
                    self.editRecipient(recipientIndex);
                },
                deleteEvent: function () {
                    self.deleteRecipient(recipientIndex);
                }
            });

            event.stopPropagation();
        });

        gridContainer.find(".link.username").click(function () {
            var $this = $(this);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            self.editRecipient(recipientIndex);
        });

        gridContainer.find(".ico-relevant-mail").click(function () {
            var $this = $(this);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            if ($this.text() !== "0") {
                self.viewRelevantMail(recipientIndex);
            }
        });
    };

    self.editRecipient = function (recipientIndex) {
        window.location.href = window.rootUrl + "MailStation/MailRecipient/UpdateRecipientView?" +
            $.param({
                recipientId: recipientIndex
            });
    };

    self.deleteRecipient = function (recipientIndex) {
        NAP.headlineMsgBox.confirm('Are you sure you want to delete Recipient?', function () {
            sharedScriptManager.blockUI();
            NAP.headlineMsgBox.close();
            var deleteParams = {
                RecipientId: recipientIndex
            };

            var url = window.rootUrl + "MailStation/MailRecipient/DeleteRecipient";

            sharedScriptManager.callAjax(self,
            url,
            deleteParams,
            self.renderActionResult,
            null,
            sharedScriptManager.unblockUI);
        });
    };

    self.renderActionResult = function (result) {
        if (result.Code == 0) {
            result.Message = 'Recipient is deleted successfully.';
        }

        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.getRecipientList();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
    };

    self.viewRelevantMail = function (recipientIndex) {
        window.location.href = window.rootUrl + 'MailStation/MailRecipient/ViewRelevantMail?' + $.param({
            recipientId: recipientIndex
        });
    };
};

var manageRecipientController = new ManageRecipientController();
