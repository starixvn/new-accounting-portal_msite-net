/* File Created: March 6, 2014 */
/* global Validator */
/* jshint strict: false */
//'use strict';

(function ($) {
    $.widget('ui.combobox', {
        _create: function () {
            var input,
                self = this,
                select = this.element.hide(),
                selectAttributes = this.element.context.attributes,
                selected = select.children(':selected'),
                value = selected.val() ? selected.text() : '',
                wrapper = this.wrapper = $('<span>').addClass('ui-combobox').insertAfter(select);

            input = this.input = $('<input>')
                .appendTo(wrapper)
                .val(value)
                .addClass('ui-state-default ui-combobox-input')
                .autocomplete({
                    delay: 0,
                    minLength: 0,
                    source: function (request, response) {
                        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), 'i');

                        response(select.children('option').map(function () {
                            var text = $(this).text();

                            if (this.value && (!request.term || matcher.test(text))) {
                                return {
                                    label: text,
                                    value: text,
                                    option: this
                                };
                            }
                        }));
                    },
                    select: function (event, ui) {
                        ui.item.option.selected = true;
                        self._trigger('selected', event, {
                            item: ui.item.option
                        });
                        select.trigger('change');
                    },
                    change: function (event, ui) {
                        if (!ui.item) {
                            var txt = $(this),
                                value = txt.val(),
                                matcher = new RegExp('^' + $.ui.autocomplete.escapeRegex(value) + '$', 'i'),
                                valid = false;

                            //Validator.clearError(wrapper);
                            select.children('option').each(function () {
                                if ($(this).text().match(matcher)) {
                                    this.selected = valid = true;
                                    self._trigger('selected', event, {
                                        item: this
                                    });
                                    select.trigger('change');

                                    return false;
                                }
                            });

                            if (!valid) {
                                //Validator.showError(wrapper, value + ' didn\'t match any item.', 'w');
                                select.val('');
                                input.data('autocomplete').term = '';
                                return false;
                            }
                        }
                    }
                })
                .focusout(function () {
                    var txt = $(this);

                    txt.val($.trim(txt.val()));
                })
                .focus(function () {
                    $(this).select();
                })
                .on('paste', function () {
                     var txt = $(this);

                     txt.autocomplete('search');
                })
                .addClass('ui-widget ui-widget-content');

            input.data('autocomplete')._renderItem = function (ul, item) {
                return $('<li></li>')
                    .data('item.autocomplete', item)
                    .append('<a>' + item.label + '</a>')
                    .appendTo(ul);
            };

            input.data('autocomplete')._renderMenu = function (ul, items) {
                var self = this;

                $.each(items, function (index, item) {
                    self._renderItem(ul, item);
                });
                $(ul).removeClass('ui-corner-all');

                // Clear error when an item is selected.
                $(ul).children().click(function () {
                    //Validator.clearError(wrapper);
                });
            };

            this.a = $('<a>')
                .attr('tabIndex', -1)
                .attr('title', 'Show All Items')
                .appendTo(wrapper)
                .button({
                    icons: {
                        primary: 'ui-icon-triangle-1-s'
                    },
                    text: false
                })
                .removeClass('ui-corner-all')
                .addClass('ui-combobox-toggle') //.addClass('ui-corner-right ui-combobox-toggle')
                .click(function () {
                    // close if already visible
                    if (input.autocomplete('widget').is(':visible')) {
                        input.autocomplete('close');
                        return;
                    }

                    // work around a bug (likely same cause as #5265)
                    $(this).blur();

                    // pass empty string as value to search for, displaying all results
                    input.autocomplete('search', '');
                    input.focus();
                });

            $.each(selectAttributes, function () {
                if (this.name === 'class') {
                    input.removeClass('ui-state-default').addClass(this.value);
                } else if (this.name !== 'style') {
                    wrapper.attr(this.name, this.value);
                }
            });
        },

        refresh: function () {
            var selected = this.element.children(':selected'),
                value = selected.val() ? selected.text() : '';

            this.input.val(value);
        },

        disable: function () {
            this.element.prop('disabled', true);
            this.input.prop('disabled', true);
            this.input.autocomplete('disable');
            this.a.button('disable');
            //Validator.clearError(this.wrapper); // Clear all error.
        },

        enable: function () {
            this.element.prop('disabled', false);
            this.input.prop('disabled', false);
            this.input.autocomplete('enable');
            this.a.button('enable');
        },

        destroy: function () {
            this.wrapper.remove();
            this.element.show();
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);