////////////////////////////
// http://adipalaz.com/experiments/jquery/expand_collapse_different_directions.html
// * When using this script, please keep the above url intact.
///////////////////////////
(function ($) {
    $.fn.addTrigger = function (options) {
        var defaults = {
            trigger1: 'Expand right',
            trigger2: 'Collapse left',
            ref: '.collapse',
            container: 'li.demo'
        };
        var o = $.extend({}, defaults, options);
        return this.each(function () {
            $('<p class="switch"><a href="#" title="expand/collapse">' + o.trigger2 + '</a></p>')
      .insertBefore(o.container + ':eq(' + $(o.container).index(this) + ') ' + o.ref);
            $(this).find('.switch a').click(function () {
                $(this).toggleClass("plus");
                ($(this).text() == o.trigger2) ? $(this).text(o.trigger1) : $(this).text(o.trigger2);
            });
        });
    };
    $.fn.marginLeft = function (speed, easing, callback) {
        var w = this.width() + parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight'));
        return this.animate({ marginLeft: parseInt(this.css('marginLeft')) < 0 ? 0 : -(w + 1) }, speed, easing, callback);
    };
    $.fn.marginRight = function (speed, easing, callback) {
        var w = this.width() + parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight'));
        return this.animate({ marginLeft: parseInt(this.css('marginLeft')) > 0 ? 0 : w + 1 }, speed, easing, callback);
    };
    $.fn.slideLeft = function (speed, easing, callback) {
        var w = this.width() + parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight'));
        return this.animate({ left: parseInt(this.css('left')) < 0 ? 0 : -(w + 1) }, speed, easing, callback);
    };
    $.fn.slideRight = function (speed, easing, callback) {
        var w = this.width() + parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight'));
        return this.animate({ left: parseInt(this.css('left')) > 0 ? 0 : w + 1 }, speed, easing, callback);
    };
    $.fn.partialWidth = function (speed, easing, callback) {
        var w = this.parent().width() + 1 + (parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight')));
        return this.animate({ width: parseInt(this.css('width')) > (w - 75) ? (w - 75) : w }, speed, easing, callback);
    };
    $.fn.slideDiag = function (speed, easing, callback) {
        var w = this.width() + parseInt(this.css('paddingLeft')) + parseInt(this.css('paddingRight')),
      h = parseInt(this.css('height'));
        return this.animate({ left: parseInt(this.css('left')) > 0 ? 0 : w + 1, bottom: parseInt(this.css('bottom')) < 0 ? 0 : -(h + 1) }, speed, easing, callback);
    };
    //http://www.learningjquery.com/2008/02/simple-effects-plugins:
    $.fn.slideFadeToggle = function (speed, easing, callback) {
        return this.animate({ opacity: 'toggle', height: 'toggle' }, speed, easing, callback);
    };
})(jQuery);

////////////////////////////
$(function () {
    $('li.demo').each(function (index) {
        var $thisDemo = $('li.demo:eq(' + index + ')');
        switch (index) {
            //override some default options in demos 1, 2, 6, 8, 9, 10: 
            case 0: $thisDemo.addTrigger({ trigger2: 'Collapse down', trigger1: 'Expand up' });
                break;
            case 1: $thisDemo.addTrigger({ trigger2: 'Collapse down', trigger1: 'Expand up' });
                break;
            case 5: $thisDemo.addTrigger({ trigger2: 'Collapse right', trigger1: 'Expand left' });
                break;
            case 7: $thisDemo.addTrigger({ trigger2: 'Collapse right', trigger1: 'Expand left' });
                break;
            case 8: $thisDemo.addTrigger({ trigger2: 'Collapse', trigger1: 'Expand' });
                break;
            case 9: $thisDemo.addTrigger({ trigger2: 'Collapse', trigger1: 'Expand' });
                break;
            //the rest of the demos use all the default options: 
            default: $thisDemo.addTrigger();
        };
    });

    //example 1
    $('li.demo:eq(0) .switch').click(function () {
        $(this).next().slideToggle('slow', 'linear');
        return false;
    });

    //example 2
    $('li.demo:eq(1) .switch').click(function () {
        $(this).next().slideFadeToggle('slow', 'linear');
        return false;
    });

    //example 3
    $('li.demo:eq(2) .switch').click(function () {
        $(this).next().animate({ width: 'toggle' }, 'slow', 'linear');
        return false;
    });

    //example 4
    $('li.demo:eq(3) .switch').click(function () {
        $(this).next().animate({ width: 'toggle', opacity: 'toggle' }, 'slow', 'linear');
        return false;
    });

    //example 5
    $('li.demo:eq(4) .switch').click(function () {
        $(this).next().marginLeft('slow', 'linear');
        return false;
    });

    //example 6
    $('li.demo:eq(5) .switch').click(function () {
        $(this).next().marginRight('slow', 'linear');
        return false;
    });

    //example 7
    $('li.demo:eq(6) .switch').click(function () {
        $(this).next().slideLeft('slow', 'linear');
        return false;
    });

    //example 8
    $('li.demo:eq(7) .switch').click(function () {
        $(this).next().slideRight('slow', 'linear');
        return false;
    });

    //example 9
    $('li.demo:eq(8) .collapse').css('width', '17em');
    $('li.demo:eq(8) .switch').click(function () {
        $(this).next().partialWidth('slow', 'linear');
        return false;
    });

    //example 10
    $('li.demo:eq(9) .switch').click(function () {
        $(this).next().slideDiag('slow', 'linear');
        return false;
    });
});