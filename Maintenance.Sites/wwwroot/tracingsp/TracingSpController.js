/* global NAP: false */
/* global sharedScriptManager: false */

$(document).ready(function () {
    tracingSPController.initPage();
    
    $("#errorDialog").dialog({
        modal: false,
        draggable: false,
        resizable: false,
        position: ['center', 'top'],
        show: 'blind',
        hide: 'blind',
        width: 400,
        dialogClass: 'main-dialog-class',
        autoOpen: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close"); //closing on Ok click
            }
        }
    });
});

var TracingSPController = function () {
    var self = this;
    self.params = {
        Site: null,
        TraceTimer: null,
        TextDataLike: null,
        TextDataLike2: null,
        TextDataLike3: null,
        TraceStartEvents: null,
        Mode: null,
        Server: null
    };

    self.initPage = function () {
        self.bindingEvent();
        self.validate();
    };

    self.bindingEvent = function () {
        $("#button-search").click(function () {
            if ($("#TextDataLike").val() == '' && $("#TextDataLike2").val() == '' && $("#TextDataLike3").val() == '')
            {
                $("#errorDialog").html("- Text Like cannot be empty.");
                $("#errorDialog").dialog("open");                
                return;
            }
          
            self.getData();
        });
    };

    self.validate = function () {
        $('#TraceTimer').keypress(function (event) {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#TraceTimer").keyup(function () {
            var $this = $(this);
            if ($this.val() > 120) {
                $this.val('120');
            }

        });
    };

    self.getData = function () {

        self.params = {
            Site: $("#Site").val(),
            Server: $("#Server").val(),
            TraceTimer: $("#TraceTimer").val(),
            TextDataLike: $("#TextDataLike").val(),
            TextDataLike2: $("#TextDataLike2").val(),
            TextDataLike3: $("#TextDataLike3").val(),
            TraceStartEvents: $("#TraceStartEvents").is(':checked')
        };

        $.ajax({
            url: window.rootUrl + "ProfilerSP/TracingSp/GetTracingSpData",
            data: self.params,
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                sharedScriptManager.blockUI();
            },
            complete: function () {
                sharedScriptManager.unblockUI();
            },
            success: function (result) {
                if (result.HasError == 0)
                {
                    self.renderData(result.Data);
                }
                else
                {
                    $("#errorDialog").html(result.ErrorMsg);
                    $("#errorDialog").dialog("open");
                }
            },
            error: function (xhr, status, err) {
                NAP.handleAjaxError(xhr, status, err);
            }
        });
    };

    self.renderData = function (result) {
        var bodyData = [];
        for (var i = 0; i < result.length; i++) {
            var record = result[i];

            var rowColumns = [];

            rowColumns.push({ Name: "ColEvent", Value: record.Event });
            rowColumns.push({ Name: "ColSPID", Value: record.SPID });
            rowColumns.push({ Name: "ColTextData", Value: record.TextData });
            rowColumns.push({ Name: "ColLoginName", Value: record.LoginName });
            rowColumns.push({ Name: "ColDatabaseName", Value: record.DatabaseName });
            rowColumns.push({ Name: "ColReads", Value: record.Reads });
            rowColumns.push({ Name: "ColWrites", Value: record.Writes });
            rowColumns.push({ Name: "ColCPU", Value: record.CPU });
            rowColumns.push({ Name: "ColDuration", Value: record.Duration });            
            rowColumns.push({ Name: "ColStartTime", Value: record.StartTime });            
            if (record.EndTime != '/Date(-62135596800000)/' || record.EndTime != '/Date(-62135582400000)/') {
                rowColumns.push({ Name: "ColEndTime", Value: record.EndTime });
            }
            else {
                rowColumns.push({ Name: "ColEndTime", Value: "N/A" });
            }

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: "Event", Width: 120, Name: "ColEvent", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "SPID", Width: 50, Name: "ColSPID", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "TextData", Width: 750, Name: "ColTextData", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Login Name", Width: 120, Name: "ColLoginName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Db Name", Width: 110, Name: "ColDatabaseName", HeaderAlign: "center", CellAlign: "left" });        
        headerColumns.push({ HeaderText: "Reads", Width: 70, Name: "ColReads", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Writes", Width: 70, Name: "ColWrites", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "CPU", Width: 70, Name: "ColCPU", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Duration(ms)", Width: 70, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Start Time", Width: 150, Name: "ColStartTime", HeaderAlign: "center", CellAlign: "right", FormatDateTime: "MM/DD/YYYY h:mm:ss A" });
        headerColumns.push({ HeaderText: "End Time", Width: 150, Name: "ColEndTime", HeaderAlign: "center", CellAlign: "right", FormatDateTime: "MM/DD/YYYY h:mm:ss A" });

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 350,
            headerColumnHeight: 29,
            showPager: true,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 200, 500],
                showDetail: true
            }
        };

        $("#tracingsp-report").removeClass("display-none").fanexGrid(gridOptions);
    };
};


var tracingSPController = new TracingSPController();