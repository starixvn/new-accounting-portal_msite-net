/* global NAP: false */
/* global sharedScriptManager: false */
/* global viewResources: false */

$(document).ready(function () {
    MailManageController.initPage();
});

var MailManageController = function () {
    var self = this;
    var DOMElements = {
        MailGroupName: null,
        gridContainer: null,
        btnSearch: null
    };

    var cacheObject = {
        bodyData: null
    };

    self.reportParams = {
        MailGroupName: null
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.bindEvents();
        contextMenuController.bindContextMenuEvents();
        DOMElements.btnSearch.click();
    };

    self.cacheDOMElements = function () {
        DOMElements.MailGroupName = $("#MailGroupName");
        DOMElements.gridContainer = $("#mail-group-list");
        DOMElements.btnSearch = $('#button-search');
    };

    self.bindEvents = function () {
        DOMElements.btnSearch.click(function () {
            self.getMailGroupList();
        });

        $([DOMElements.MailGroupName]).each(function () {
            $(this).keypress(function (e) {
                if (e.which == 13) {
                    self.getMailGroupList();
                }
            });
        });
    };

    self.getMailGroupList = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        self.reportParams = {
            MailGroupName: DOMElements.MailGroupName.val()
        };

        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailGroupManage/MailGroupList",
            self.reportParams,
            self.renderMailGroupList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderMailGroupList = function (result) {
        var mailListLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailListLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailGroupIndex = ' groupid="' + mail.MailGroupId + '"';
            rowColumns.push({ Name: "ID", Value: mail.MailGroupId });
            rowColumns.push({
                Name: "Group",
                Value: parseInt(mail.MailGroupId) === 1 ? mail.MailGroupName : '<a title="' + mail.MailGroupName + '" class="link group-mail-name" href="javascript:;"' + mailGroupIndex + '>' +
                                mail.MailGroupName +
                            '</a>'
            });
            rowColumns.push({ Name: "Description", Value: mail.Description });
            rowColumns.push({ Name: "NoRelevantMail", Value: '<div class="ico-relevant-mail"' + mailGroupIndex + '>' + mail.MailRelevantCount + '</div>' });
            var actionElement = '<div class="action-wrp">\
                                    <span class="btn-action"' + mailGroupIndex + '>Action</span>\
                                    <div class="relative"></div>\
                                </div>';
            rowColumns.push({
                Name: "Action",
                Value: mail.MailGroupId !== 1 ? actionElement : ''
            });

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: "ID", Width: 55, Name: "ID", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Group", Width: 400, Name: "Group", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Description", Width: 550, Name: "Description", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: "No.Relevant Mail", Width: 150, Name: "NoRelevantMail", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" });

        var reRenderAfterPaged = function (page_index, itemsPerPage, jq) {
            gridOptions.pagerOption.itemsPerPage = itemsPerPage;
            gridOptions.pagerOption.currentPage = page_index;
            var startIndex = (page_index * itemsPerPage);
            var endIndex = ((page_index + 1) * itemsPerPage);
            var currentBodyRows = cacheObject.bodyData.slice(startIndex, endIndex);
            gridOptions.bodyRows = currentBodyRows;
            DOMElements.gridContainer.fanexGrid(gridOptions);
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
            showPager: true,
            customPager: true,
            gridExpandHeight: 350,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: cacheObject.bodyData.length,
                pagerSelectCallBack: reRenderAfterPaged
            }
        };

        DOMElements.gridContainer.removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
        DOMElements.gridContainer.fanexGrid('resizeGrid');
    };

    self.bindGridEvent = function () {
        $(".btn-action").click(function (event) {
            var $this = $(this);
            var position = $this.offset();
            var groupIndex = parseInt($this.attr("groupid"));
            if (groupIndex !== 1) {
                contextMenuController.showContextMenu(position);
                contextMenuController.bindEvents({
                    editEvent: function () {
                        self.editMailGroup(groupIndex);
                    },
                    deleteEvent: function () {
                        self.deleteMailGroup(groupIndex);
                    }
                });
            }
            event.stopPropagation();
        });

        $('.ico-relevant-mail').click(function () {
            var $this = $(this);
            var groupid = parseInt($this.attr("groupid"));
            window.location.href = window.rootUrl + 'MailStation/MailGroupManage/ViewRelevantMail?' + $.param({
                mailGroupID: groupid
            });
        });

        DOMElements.gridContainer.find(".link.group-mail-name").click(function () {
            var $this = $(this);
            var groupid = parseInt($this.attr("groupid"));
            self.editMailGroup(groupid);
        });
    };

    self.deleteMailGroup = function (mailGroupId) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_DeleteMailGroup, function () {
            sharedScriptManager.blockUI();
            NAP.headlineMsgBox.close();
            var deleteParams = {
                MailGroupId: mailGroupId
            };
            sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/MailGroupManage/DeleteMailGroup",
                deleteParams,
                self.renderActionResult,
                null,
                null);
        });
    };

    self.editMailGroup = function (mailGroupId) {
        window.location.href = window.rootUrl + "MailStation/MailGroupManage/UpdateMailGroupView?" +
            $.param({
                MailGroupId: mailGroupId
            });
    };

    self.renderActionResult = function (result) {
        var result = {
            Code: 0,
            Message: 'Mail Group is deleted successfully.'
        };
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.getMailGroupList();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
    };
};

var MailManageController = new MailManageController();