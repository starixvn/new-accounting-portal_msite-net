(function ($, undefined) {
    $.extend($.datepicker, {
        _dateRange: [],
        _dateValue: null,
        _isOldData: false,
        _updateSetting: function (target, name, value) {
            var settings, date, minDate, maxDate,
				inst = this._getInst(target),
				isDateRange = this._get(inst, "isDateRange");

            if (arguments.length === 2 && typeof name === "string") {
                return (name === "defaults" ? $.extend({}, $.datepicker._defaults) :
					(inst ? (name === "all" ? $.extend({}, inst.settings) :
					this._get(inst, name)) : null));
            }

            settings = name || {};
            if (typeof name === "string") {
                settings = {};
                settings[name] = value;
            }

            if (inst) {
                if ((this._curInst === inst) && (!isDateRange)) {
                    this._hideDatepicker();
                }

                date = this._getDateDatepicker(target, true);
                minDate = this._getMinMaxDate(inst, "min");
                maxDate = this._getMinMaxDate(inst, "max");
                extendRemove(inst.settings, settings);
                // reformat the old minDate/maxDate values if dateFormat changes and a new minDate/maxDate isn't provided
                if (minDate !== null && settings.dateFormat !== undefined && settings.minDate === undefined) {
                    inst.settings.minDate = this._formatDate(inst, minDate);
                }
                if (maxDate !== null && settings.dateFormat !== undefined && settings.maxDate === undefined) {
                    inst.settings.maxDate = this._formatDate(inst, maxDate);
                }
                if ("disabled" in settings) {
                    if (settings.disabled) {
                        this._disableDatepicker(target);
                    } else {
                        this._enableDatepicker(target);
                    }
                }
                this._attachments($(target), inst);
                this._autoSize(inst);

                if (!isDateRange) {
                    this._setDate(inst, date);
                }

                this._updateAlternate(inst);
                this._updateDatepicker(inst);
            }
        },
        _convertStr2Date: function (inst, date) {
            var dateFormat = this._get(inst, "dateFormat"),
				settings = this._getFormatConfig(inst),
				result = null;

            try {
                result = this.parseDate(dateFormat, date, settings);
            } catch (event) {
            }
            return result;
        },
        _convertDate2Str: function (inst, date) {
            var dateFormat = this._get(inst, "dateFormat"),
				settings = this._getFormatConfig(inst),
				result = null;

            try {
                result = this.formatDate(dateFormat, date, settings);
            } catch (event) {
            }
            return result;
        },
        _selectDate: function (id, dateStr) {
            var onSelect,
				target = $(id),
				inst = this._getInst(target[0]),
				isDateRange = this._get(inst, "isDateRange"); //Get custom option

            dateStr = (dateStr != null ? dateStr : this._formatDate(inst));

            //Add Min, Max date
            if (isDateRange) {
                if (this._isOldData) {
                    this._dateRange = [];
                    this._isOldData = false;
                }

                var minMax = (this._dateRange.length < 1) ? 'min' : 'max';
                this._dateRange.push(this._convertStr2Date(inst, dateStr));
                this._updateSetting(target[0], minMax + 'Date', dateStr);

                if (minMax === 'max') {
                    dateStr = this._dateValue + ' - ' + dateStr;
                    this._isOldData = true;
                }
                this._dateValue = dateStr;
            }

            if (inst.input) {
                inst.input.val(dateStr);
            }
            this._updateAlternate(inst);

            onSelect = this._get(inst, "onSelect");
            if (onSelect) {
                onSelect.apply((inst.input ? inst.input[0] : null), [dateStr, inst]);  // trigger custom callback
            } else if (inst.input) {
                inst.input.trigger("change"); // fire the change event
            }

            //Prevent close popup		
            if (isDateRange && (this._dateRange.length < 2)) {
                return false;
            }

            if (inst.inline) {
                this._updateDatepicker(inst);
            } else {
                this._hideDatepicker();
                this._lastInput = inst.input[0];
                if (typeof (inst.input[0]) !== "object") {
                    inst.input.focus(); // restore focus
                }
                this._lastInput = null;
            }

            //Update min, max date values
            if (isDateRange) {
                this._updateSetting(target[0], 'minDate', null);
                this._updateSetting(target[0], 'maxDate', null);
                this._updateSetting(target[0], 'dateFrom', this._convertDate2Str(inst, this._dateRange[0]));
                this._updateSetting(target[0], 'dateTo', this._convertDate2Str(inst, this._dateRange[1]));
            }
        },
		_initDatepicker: function(id) {
		    var target = $(id),
			    inst = this._getInst(target[0]),
			    isDateRange = this._get(inst, "isDateRange");
			
			if (isDateRange) {
				var dfDateFrom = this._get(inst, "dateFrom");
				var dfDateTo = this._get(inst, "dateTo");
				this._dateRange = [];
				this._selectDate(id, this._convertDate2Str(inst, dfDateFrom));
				this._selectDate(id, this._convertDate2Str(inst, dfDateTo));
			}
		}
    });
    $.extend($.datepicker._defaults, {
        beforeShowDay: function (date) {
            if ($.datepicker._dateRange.length > 0) {
                if (($.datepicker._dateRange[0] <= date) && ($.datepicker._dateRange[1] >= date)) {
                    return [true, 'ui-state-highlight ui-state-active'];
                }
            }
            return [true];
        }
    });

    function extendRemove(target, props) {
        $.extend(target, props);
        for (var name in props) {
            if (props[name] == null) {
                target[name] = props[name];
            }
        }
        return target;
    }

})(jQuery);