/* File Created: November 26, 2014 */
/// <reference path="../jquery-1.8.0.min.js" />

(function ($) {
    methods =
    {
        init: function () {
            var nexCron = new NexCron(this);
            nexCron.RenderScheduleContainer();
            nexCron.RepeatTypeChange();
        },
        getCronExpression: function () {
            var nexCron = new NexCron(this);
            return nexCron.GetCron();
        },
        setCronExpression: function (cronObject) {
            var nexCron = new NexCron(this);
            return nexCron.SetCron(cronObject);
        }
    }

    $.fn.nexCron = function (m) {
        if (methods[m]) {
            return methods[m].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof m == "object" || !m) {
            return methods.init.apply(this, arguments);
        }
        else {
            alert('Unknow Method.');
        }
    }

    var NexCron = NexCron || {};

    NexCron = function (nexCron, options) {
        this.Options = options;

        this.$nexCron = $(nexCron);

        var url = '';
        var sc = document.getElementsByTagName("script");

        for (idx = 0; idx < sc.length; idx++) {
            s = sc.item(idx);

            if (s.src && s.src.match(/NexCron\.js$/))
            { url = s.src; break; }
        }

        this.CalendarImagePath = url.replace('/js/NexCron.js', '/images/calendar.gif');
        this.$nexCron.css('float', 'left');
    }

    NexCron.prototype.DOMElements = {
        ScheduleTypeContainer: null,
        RepeatType: null,
        RepeatDaily: null,
        RepeatWeeklyMonday: null,
        RepeatWeeklyTuesday: null,
        RepeatWeeklyWednesday: null,
        RepeatWeeklyThursday: null,
        RepeatWeeklyFriday: null,
        RepeatWeeklySaturday: null,
        RepeatWeeklySunday: null,
        RepeatMonthly: null,
        RepeatOnDaysOfMonth: null,
        RepeatHour: null,
        RepeatMinute: null,
        StartOn: null,
        EndOn: null,
        EndRadioName: null,
        Attempt: null,
        Interval: null
    }

    NexCron.prototype.OnlyNumericAndCommaInput = function (e) {
        // Allow: backspace, delete, tab, escape, enter and comma
        if ($.inArray(e.keyCode, [44, 8, 46, 9, 27, 13, 110, 188]) !== -1 ||
        // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }

        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    }

    NexCron.prototype.OnlyNumericInput = function (e, value) {
        // Allow: backspace, delete, tab, escape, enter and comma
        if ($.inArray(e.keyCode, [8, 46, 9, 27, 13, 110]) !== -1 ||
        // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }

        // Ensure that it is a number and stop the keypress
        if (value.length >= 4 || (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    }

    NexCron.prototype.RenderScheduleContainer = function () {
        var containerhtml = [];
        containerhtml.push('<div id="schedule-container" class="nexcron-schedule-container">');
        containerhtml.push('    <div class="nexcron-row">');
        containerhtml.push('        <div class="nexcron-lbl-field">');
        containerhtml.push('            <label for="nexcron-repeat-type" class="nexcron-bold-weight">');
        containerhtml.push('                Repeats');
        containerhtml.push('            </label>');
        containerhtml.push('        </div>');
        containerhtml.push('        <div class="nexcron-edit-field">');
        containerhtml.push('            <select id="nexcron-repeat-type" name="repeat-type">');
        containerhtml.push('                <option value="0">Daily</option>');
        containerhtml.push('                <option value="1">Weekly</option>');
        containerhtml.push('                <option value="2">Monthly</option>');
        containerhtml.push('            </select>');
        containerhtml.push('        </div>');
        containerhtml.push('    </div>');
        containerhtml.push('    <div id="schedule-type-container"></div>');
        containerhtml.push('</div>');
        this.$nexCron.html(containerhtml.join(''));
        this.DOMElements.ScheduleTypeContainer = this.$nexCron.find('#schedule-type-container');
        this.DOMElements.RepeatType = $('#nexcron-repeat-type');
        var self = this;
        this.DOMElements.RepeatType.change(function () {
            self.RepeatTypeChange();
        });
    }

    NexCron.prototype.RepeatTypeChange = function () {
        var self = this;
        var defaultCron = null;

        switch (self.DOMElements.RepeatType.val()) {
            case "0": // Daily
                {
                    defaultCron = {
                        RepeatType: '0',
                        CronExpression: '0 0 1 * * ?',
                        StartOn: new Date(),
                        EndOn: '',
                        Attempt: 1,
                        Interval: 30
                    };
                    self.RenderDailySchedule();
                    break;
                }
            case "1": // Weekly
                defaultCron = {
                    RepeatType: '1',
                    CronExpression: '0 0 1 ? * ',
                    StartOn: new Date(),
                    EndOn: '',
                    Attempt: 1,
                    Interval: 30
                };
                self.RenderWeeklySchedule();
                break;
            case "2": // Monthly
                defaultCron = {
                    RepeatType: '2',
                    CronExpression: '0 0 1  * ',
                    StartOn: new Date(),
                    EndOn: '',
                    Attempt: 1,
                    Interval: 30
                };
                self.RenderMonthlySchedule();
                break;
            default:
                break;
        }

        $('input:radio[name=' + self.DOMElements.EndRadioName + ']').change(function () {
            if (this.value == '1') {
                self.DOMElements.EndOn.datepicker('disable');
            }
            else if (this.value == '2') {
                self.DOMElements.EndOn.datepicker('enable');
            }
        });

        if (defaultCron !== null) {
            self.SetCron(defaultCron);
        }
    }

    NexCron.prototype.RenderDatePicker = function () {
        var startPicker = this.$nexCron.find("#nexcron-start-day-daily");
        startPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: this.CalendarImagePath,
            buttonText: "Start day",
            buttonImageOnly: true
        }).datepicker('init', '#nexcron-start-day-daily');

        var endPicker = this.$nexCron.find("#nexcron-end-day-daily");
        endPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: this.CalendarImagePath,
            buttonText: "End day",
            buttonImageOnly: true
        }).datepicker('init', '#nexcron-end-day-daily');

        $('.ui-datepicker-trigger').addClass('nexcron-calendar-image');
    }

    NexCron.prototype.RenderDailySchedule = function () {
        var dailyhtml = [];
        dailyhtml.push('<div class="nexcron-daily-schedule">');
        // Row 1
        dailyhtml.push('    <div class="nexcron-row">');
        dailyhtml.push('        <div class="nexcron-lbl-field">');
        dailyhtml.push('            <label for="nexcron-repeat-every-daily" class="nexcron-bold-weight">');
        dailyhtml.push('            Repeat every');
        dailyhtml.push('            </label>');
        dailyhtml.push('        </div>');
        dailyhtml.push('        <div class="nexcron-edit-field">');
        dailyhtml.push('            <select id="nexcron-repeat-day-daily" name="nexcron-repeat-day" class="nexcron-repeat-day">');
        for (var i = 1; i < 32; i++) {
            dailyhtml.push('            <option value="' + i + '">' + i + '</option>');
        }
        dailyhtml.push('            </select>');
        dailyhtml.push('        </div>');
        dailyhtml.push('        <div class="nexcron-lbl-field nexcron-day-time-separate-label">');
        dailyhtml.push('            <label>');
        dailyhtml.push('                days');
        dailyhtml.push('            </label>');
        dailyhtml.push('            <label for="nexcron-repeat-time">');
        dailyhtml.push('                 at');
        dailyhtml.push('            </label>');
        dailyhtml.push('        </div>');
        dailyhtml.push('        <div class="nexcron-edit-field">');
        dailyhtml.push('            <select id="nexcron-repeat-time" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 24; i++) {
            var number = ("0" + i).slice(-2);
            dailyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        dailyhtml.push('            </select> : ');
        dailyhtml.push('            <select id="nexcron-repeat-minute" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 60; i++) {
            var number = ("0" + i).slice(-2);
            dailyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        dailyhtml.push('            </select>');
        dailyhtml.push('        </div>');
        dailyhtml.push('    </div>');

        this.RenderFooterPart(dailyhtml);
        dailyhtml.push('</div>');
        this.DOMElements.ScheduleTypeContainer.html(dailyhtml.join(''));
        this.RenderDatePicker();
        var self = this;
        this.DOMElements.RepeatDaily = $('#nexcron-repeat-day-daily', this.$nexCron);
        this.DOMElements.RepeatHour = $('#nexcron-repeat-time', this.$nexCron);
        this.DOMElements.RepeatMinute = $('#nexcron-repeat-minute', this.$nexCron);
        this.DOMElements.StartOn = $('#nexcron-start-day-daily', this.$nexCron);
        this.DOMElements.EndOn = $('#nexcron-end-day-daily', this.$nexCron);
        this.DOMElements.EndRadioName = 'nexcron-is-end-limit-daily';
        this.DOMElements.Attempt = $('#nexcron-attempt-count-daily', this.$nexCron);
        this.DOMElements.Interval = $('#nexcron-interval-time', this.$nexCron);
        this.DOMElements.Interval.keydown(function (e) {
            self.OnlyNumericInput(e, $(this).val());
        });
    }

    NexCron.prototype.RenderWeeklySchedule = function () {
        var weeklyhtml = [];
        weeklyhtml.push('<div class="nexcron-weekly-schedule">');
        weeklyhtml.push('   <div class="nexcron-row">');
        weeklyhtml.push('      <div class="nexcron-lbl-field">');
        weeklyhtml.push('          <label class="nexcron-bold-weight">');
        weeklyhtml.push('              Repeat every');
        weeklyhtml.push('          </label>');
        weeklyhtml.push('      </div>');
        weeklyhtml.push('      <div class="nexcron-repeat-day-group ">');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-monday" class="nexcron-check-box" type="checkbox" value="1" name="repeat-day-checkbox" />S</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-tuesday" class="nexcron-check-box" type="checkbox" value="2" name="repeat-day-checkbox" />M</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-wednesday" class="nexcron-check-box" type="checkbox" value="3" name="repeat-day-checkbox" />T</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-thursday" class="nexcron-check-box" type="checkbox" value="4" name="repeat-day-checkbox" />W</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-friday" class="nexcron-check-box" type="checkbox" value="5" name="repeat-day-checkbox" />T</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-saturday" class="nexcron-check-box" type="checkbox" value="6" name="repeat-day-checkbox" />F</label>');
        weeklyhtml.push('          <label>');
        weeklyhtml.push('              <input id="nexcron-weekly-sunday" class="nexcron-check-box" type="checkbox" value="7" name="repeat-day-checkbox" />S</label>');
        weeklyhtml.push('      </div>');
        weeklyhtml.push('      <div class="nexcron-lbl-field nexcron-day-time-separate-label">');
        weeklyhtml.push('          <label for="nexcron-repeat-time">');
        weeklyhtml.push('              at');
        weeklyhtml.push('          </label>');
        weeklyhtml.push('      </div>');
        weeklyhtml.push('      <div class="nexcron-edit-field">');
        weeklyhtml.push('            <select id="nexcron-repeat-time" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 24; i++) {
            var number = ("0" + i).slice(-2);
            weeklyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        weeklyhtml.push('            </select> : ');
        weeklyhtml.push('            <select id="nexcron-repeat-minute" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 60; i++) {
            var number = ("0" + i).slice(-2);
            weeklyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        weeklyhtml.push('            </select>');
        weeklyhtml.push('      </div>');
        weeklyhtml.push('   </div>');

        this.RenderFooterPart(weeklyhtml);
        weeklyhtml.push('</div>');
        this.DOMElements.ScheduleTypeContainer.html(weeklyhtml.join(''));
        this.RenderDatePicker();
        var self = this;
        this.DOMElements.RepeatWeeklyMonday = $('#nexcron-weekly-monday', this.$nexCron);
        this.DOMElements.RepeatWeeklyTuesday = $('#nexcron-weekly-tuesday', this.$nexCron);
        this.DOMElements.RepeatWeeklyWednesday = $('#nexcron-weekly-wednesday', this.$nexCron);
        this.DOMElements.RepeatWeeklyThursday = $('#nexcron-weekly-thursday', this.$nexCron);
        this.DOMElements.RepeatWeeklyFriday = $('#nexcron-weekly-friday', this.$nexCron);
        this.DOMElements.RepeatWeeklySaturday = $('#nexcron-weekly-saturday', this.$nexCron);
        this.DOMElements.RepeatWeeklySunday = $('#nexcron-weekly-sunday', this.$nexCron);
        this.DOMElements.RepeatHour = $('#nexcron-repeat-time', this.$nexCron);
        this.DOMElements.RepeatMinute = $('#nexcron-repeat-minute', this.$nexCron);
        this.DOMElements.StartOn = $('#nexcron-start-day-daily', this.$nexCron);
        this.DOMElements.EndOn = $('#nexcron-end-day-daily', this.$nexCron);
        this.DOMElements.EndRadioName = 'nexcron-is-end-limit-daily';
        this.DOMElements.Attempt = $('#nexcron-attempt-count-daily', this.$nexCron);
        this.DOMElements.Interval = $('#nexcron-interval-time', this.$nexCron);
        this.DOMElements.Interval.keydown(function (e) {
            self.OnlyNumericInput(e, $(this).val());
        });
    }

    NexCron.prototype.RenderMonthlySchedule = function () {
        var monthlyhtml = [];
        monthlyhtml.push('<div class="nexcron-row">');
        monthlyhtml.push('  <div class="nexcron-lbl-field">');
        monthlyhtml.push('      <label for="nexcron-repeat-day-monthly" class="nexcron-bold-weight">');
        monthlyhtml.push('          Repeat every');
        monthlyhtml.push('      </label>');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('  <div class="nexcron-edit-field">');
        monthlyhtml.push('      <select id="nexcron-repeat-day-monthly" name="repeat-day" class="nexcron-repeat-day">');
        for (var i = 1; i <= 12; i++) {
            monthlyhtml.push('      <option value="' + i + '">' + i + '</option>');
        }
        monthlyhtml.push('      </select>');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('  <div class="nexcron-lbl-field nexcron-day-time-separate-label">');
        monthlyhtml.push('      <label>');
        monthlyhtml.push('          months');
        monthlyhtml.push('      </label>');
        monthlyhtml.push('      <label for="nexcron-repeat-time">');
        monthlyhtml.push('          at');
        monthlyhtml.push('      </label>');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('  <div class="nexcron-edit-field">');
        monthlyhtml.push('            <select id="nexcron-repeat-time" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 24; i++) {
            var number = ("0" + i).slice(-2);
            monthlyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        monthlyhtml.push('            </select> : ');
        monthlyhtml.push('            <select id="nexcron-repeat-minute" type="text" value="" name="nexcron-repeat-time" >');
        for (var i = 0; i < 60; i++) {
            var number = ("0" + i).slice(-2);
            monthlyhtml.push('            <option value=' + i + '>' + number + '</option>');
        }
        monthlyhtml.push('            </select>');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('</div>');
        monthlyhtml.push('<div class="nexcron-row">');
        monthlyhtml.push('  <div class="nexcron-lbl-field">');
        monthlyhtml.push('      <label for="nexcron-repeat-on-day" class="nexcron-bold-weight">');
        monthlyhtml.push('          Repeat on:');
        monthlyhtml.push('      </label>');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('  <div class="nexcron-edit-field">');
        monthlyhtml.push('      <input id="nexcron-repeat-on-day" type="text" value="30" name="nexcron-repeat-on-day" />');
        monthlyhtml.push('  </div>');
        monthlyhtml.push('  <div class="nexcron-lbl-field">');
        monthlyhtml.push('      <label>');
        monthlyhtml.push('          of each Month');
        monthlyhtml.push('      </label>');
        monthlyhtml.push('    </div>');
        monthlyhtml.push('</div>');

        this.RenderFooterPart(monthlyhtml);
        monthlyhtml.push('</div>');
        this.DOMElements.ScheduleTypeContainer.html(monthlyhtml.join(''));
        this.RenderDatePicker();
        var self = this;
        $('#nexcron-repeat-on-day').keydown(function (e) {
            self.OnlyNumericAndCommaInput(e);
        });

        this.DOMElements.RepeatMonthly = $('#nexcron-repeat-day-monthly', this.$nexCron);
        this.DOMElements.RepeatHour = $('#nexcron-repeat-time', this.$nexCron);
        this.DOMElements.RepeatMinute = $('#nexcron-repeat-minute', this.$nexCron);
        this.DOMElements.RepeatOnDaysOfMonth = $('#nexcron-repeat-on-day', this.$nexCron);
        this.DOMElements.StartOn = $('#nexcron-start-day-daily', this.$nexCron);
        this.DOMElements.EndOn = $('#nexcron-end-day-daily', this.$nexCron);
        this.DOMElements.EndRadioName = 'nexcron-is-end-limit-daily';
        this.DOMElements.Attempt = $('#nexcron-attempt-count-daily', this.$nexCron);
        this.DOMElements.Interval = $('#nexcron-interval-time', this.$nexCron);
        this.DOMElements.Interval.keydown(function (e) {
            self.OnlyNumericInput(e, $(this).val());
        });
    }

    NexCron.prototype.RenderFooterPart = function (htmlArray) {
        // Row 2
        htmlArray.push('    <div class="nexcron-row">');
        htmlArray.push('        <div class="nexcron-lbl-field">');
        htmlArray.push('            <label for="nexcron-start-day-daily" class="nexcron-bold-weight">');
        htmlArray.push('                Start on:');
        htmlArray.push('            </label>');
        htmlArray.push('        </div>');
        htmlArray.push('        <div class="nexcron-edit-field">');
        htmlArray.push('            <input id="nexcron-start-day-daily" type="text" value="" />');
        htmlArray.push('        </div>');
        htmlArray.push('    </div>');

        // Row 3
        htmlArray.push('    <div class="nexcron-row">');
        htmlArray.push('        <div class="nexcron-lbl-field">');
        htmlArray.push('            <label class="nexcron-bold-weight">');
        htmlArray.push('                Ends:');
        htmlArray.push('            </label>');
        htmlArray.push('        </div>');
        htmlArray.push('        <div class="nexcron-edit-field">');
        htmlArray.push('            <label>');
        htmlArray.push('                <input type="radio" value="1" name="nexcron-is-end-limit-daily" />Never</label>');
        htmlArray.push('        </div>');
        htmlArray.push('    </div>');

        // Row 4
        htmlArray.push('    <div class="nexcron-row">');
        htmlArray.push('        <div class="nexcron-lbl-field"></div>');
        htmlArray.push('            <div class="nexcron-edit-field">');
        htmlArray.push('                <label><input type="radio" value="2" name="nexcron-is-end-limit-daily" checked="checked" />On</label>');
        htmlArray.push('                <input id="nexcron-end-day-daily" type="text" value="" />');
        htmlArray.push('            </div>');
        htmlArray.push('        </div>');
        htmlArray.push('    </div>');

        // Row 5
        htmlArray.push('    <div class="nexcron-row">');
        htmlArray.push('        <div class="nexcron-lbl-field">');
        htmlArray.push('            <label for="attempt-count-daily" class="nexcron-bold-weight">');
        htmlArray.push('                Attempt');
        htmlArray.push('            </label>');
        htmlArray.push('        </div>');
        htmlArray.push('        <div class="nexcron-edit-field">');
        htmlArray.push('            <select id="nexcron-attempt-count-daily" name="nexcron-attempt-count" class="nexcron-attempt-count">');
        for (var i = 1; i <= 10; i++) {
            htmlArray.push('            <option value="' + i + '">' + i + '</option>');
        }

        htmlArray.push('            </select>');

        htmlArray.push('            <label class="nexcron-label-time">');
        htmlArray.push('                time(s)');
        htmlArray.push('            </label>');
        htmlArray.push('        </div>');
        htmlArray.push('        <div class="nexcron-lbl-field nexcron-separate-label-group">');

        htmlArray.push('            <label>');
        htmlArray.push('                Interval');
        htmlArray.push('            </label>');
        htmlArray.push('        </div>');
        htmlArray.push('        <div class="nexcron-edit-field">');
        htmlArray.push('            <input id="nexcron-interval-time" type="text" value="30" name="nexcron-interval-time" />   minute(s)');
        htmlArray.push('        </div>');
        htmlArray.push('    </div>');
    }

    NexCron.prototype.GetCron = function () {
        var cronResult = {
            RepeatType: this.DOMElements.RepeatType.val(),
            CronExpression: '',
            StartOn: null,
            EndOn: null,
            Attempt: null,
            Interval: null
        };

        var cronExpression = [];
        cronExpression.push('0');
        cronExpression.push(this.DOMElements.RepeatMinute.val().toString());
        cronExpression.push(this.DOMElements.RepeatHour.val().toString());

        switch (this.DOMElements.RepeatType.val()) {
            case "0": // Daily
                if (this.DOMElements.RepeatDaily.val() === "1") {
                    cronExpression.push("*");
                }
                else {
                    cronExpression.push("1/" + this.DOMElements.RepeatDaily.val());
                }

                cronExpression.push('*');
                cronExpression.push('?');
                break;
            case "1": // Weekly
                cronExpression.push('?');
                cronExpression.push('*');
                var dayOfWeek = '';
                dayOfWeek += this.DOMElements.RepeatWeeklyMonday.is(':checked') ? '1' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklyTuesday.is(':checked') ? ',2' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklyWednesday.is(':checked') ? ',3' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklyThursday.is(':checked') ? ',4' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklyFriday.is(':checked') ? ',5' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklySaturday.is(':checked') ? ',6' : '';
                dayOfWeek += this.DOMElements.RepeatWeeklySunday.is(':checked') ? ',7' : '';
                dayOfWeek = dayOfWeek.replace(/^,|,$/g, '');
                if (dayOfWeek.length === 13 || dayOfWeek.length === 0) {
                    // All Days of Week is selected
                    dayOfWeek = '*';
                }

                cronExpression.push(dayOfWeek);
                break;
            case "2": // Monthly
                var repeatOnDaysOfMonth = this.DOMElements.RepeatOnDaysOfMonth.val();
                if (repeatOnDaysOfMonth !== undefined && repeatOnDaysOfMonth !== '') {
                    cronExpression.push(repeatOnDaysOfMonth);
                }
                else {
                    cronExpression.push('*');
                }

                var repeatMonthly = this.DOMElements.RepeatMonthly.val();
                if (repeatMonthly !== undefined && repeatMonthly !== '1') {
                    cronExpression.push('1/' + repeatMonthly);
                }
                else {
                    cronExpression.push('*');
                }

                cronExpression.push('?');
                break;
            default:
                break;
        }

        cronResult.CronExpression = cronExpression.join(' ');
        cronResult.StartOn = this.DOMElements.StartOn.val();
        if ($('input[name=' + this.DOMElements.EndRadioName + ']:checked', this.$nexCron).val() === '1') {
            // Never
            cronResult.EndOn = '';
        }
        else {
            // End on some day
            cronResult.EndOn = this.DOMElements.EndOn.val();
        }

        cronResult.Attempt = this.DOMElements.Attempt.val();
        cronResult.Interval = this.DOMElements.Interval.val();
        return cronResult;
    }

    NexCron.prototype.SetCron = function (cronObject) {
        if (this.DOMElements.RepeatType.val() !== cronObject.RepeatType) {
            this.DOMElements.RepeatType.val(cronObject.RepeatType);
            this.DOMElements.RepeatType.change();
        }

        var cronExpression = cronObject.CronExpression.split(' ');
        if (cronObject.StartOn !== undefined && cronObject.StartOn !== '') {
            this.DOMElements.StartOn.datepicker('setDate', new Date(cronObject.StartOn));
        }

        if (cronObject.EndOn !== undefined && cronObject.EndOn !== '') {
            $('input[name="nexcron-is-end-limit-daily"][value="2"]').prop('checked', true);
            this.DOMElements.EndOn.datepicker('setDate', new Date(cronObject.EndOn));
        } else {
            $('input[name="nexcron-is-end-limit-daily"][value="1"]').prop('checked', true);
        }

        this.DOMElements.RepeatMinute.val(cronExpression[1]);
        this.DOMElements.RepeatHour.val(cronExpression[2]);
        this.DOMElements.Attempt.val(cronObject.Attempt);
        this.DOMElements.Interval.val(cronObject.Interval);
        switch (cronObject.RepeatType) {
            case "0":
                // Daily
                if (cronExpression[3] === '*') {
                    this.DOMElements.RepeatDaily.val(1);
                }
                else {
                    var repeatDaily = cronExpression[3].split('/')[1];
                    this.DOMElements.RepeatDaily.val(repeatDaily);
                }

                break;
            case "1":
                // Weekly
                if (cronExpression[5] === '*') {
                    this.DOMElements.RepeatWeeklyMonday.prop('checked', true);
                    this.DOMElements.RepeatWeeklyTuesday.prop('checked', true);
                    this.DOMElements.RepeatWeeklyWednesday.prop('checked', true);
                    this.DOMElements.RepeatWeeklyThursday.prop('checked', true);
                    this.DOMElements.RepeatWeeklyFriday.prop('checked', true);
                    this.DOMElements.RepeatWeeklySaturday.prop('checked', true);
                    this.DOMElements.RepeatWeeklySunday.prop('checked', true);
                }
                else {
                    var repeatDaily = cronExpression[5].split(',');
                    this.DOMElements.RepeatWeeklyMonday.prop('checked', $.inArray('1', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklyTuesday.prop('checked', $.inArray('2', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklyWednesday.prop('checked', $.inArray('3', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklyThursday.prop('checked', $.inArray('4', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklyFriday.prop('checked', $.inArray('5', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklySaturday.prop('checked', $.inArray('6', repeatDaily) !== -1);
                    this.DOMElements.RepeatWeeklySunday.prop('checked', $.inArray('7', repeatDaily) !== -1);
                }

                break;
            case "2":
                // Monthly
                if (cronExpression[4] === '*') {
                    this.DOMElements.RepeatMonthly.val(1);
                }
                else {
                    var repeatDaily = cronExpression[4].split('/')[1];
                    this.DOMElements.RepeatMonthly.val(repeatDaily);
                }

                this.DOMElements.RepeatOnDaysOfMonth.val(cronExpression[3] === '*' ? '' : cronExpression[3]);
            default:
                break;
        }
    }

})(jQuery);
