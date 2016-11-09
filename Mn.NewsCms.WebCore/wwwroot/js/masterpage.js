$(document).ready(function () {

    AuthenticateObject.Authentication();

    var dtDate2 = LastSiteUpdate.replace(/-/g, '/')
    var nDifference = Math.abs(new Date() - new Date(dtDate2));
    var one_day = 1000 * 60 * 60 * 24;
    var one_Hour = 1000 * 60 * 60;
    var one_Min = 1000 * 60;
    var perDay = (nDifference / one_day);
    var perHour = (nDifference / one_Hour);
    var perMin = (nDifference / one_Min) + 1;

    //$("#lastSiteUpdate").html(perDay.toFixed() == 0
    //    ? (perHour.toFixed() == 0 ? perMin.toFixed() + " دقیقه پیش" : perHour.toFixed() + " ساعت پیش")
    //    : perDay.toFixed() + " روز پیش");

    if ($("#content").length > 0) {
        try {
            $("#content").keyup(function (event) {
                SetTextDir();
            });
        } catch (e) {

        }
    }

    //----new autocompelete-----
    try {
        $('#content').autocomplete({
            serviceUrl: '/feed/autocompleteasync',
            minChars: 3,
            deferRequestBy: 100,
            transformResult: function (response) {
                return {
                    suggestions: $.map($.parseJSON(response), function (dataItem) {
                        return { value: dataItem.value, data: dataItem.data }
                    })
                }
            },
            onSelect: function (suggestion) {
                $("#form-search").submit();
            }

        });
    }
    catch (ex) { }

    //----Top Mneu hover-----
    $('ul.nav li.dropdown').hover(function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn();
    }, function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut();
    });

    // TREE MENU
    if ($("#treeMenu").length > 0) {
        $('#treeMenu ul li:has("div")').find('span:first').addClass('closed');
        $('#treeMenu ul li:has("div")').find('div').hide();
        $('#treeMenu li:has("div")').find('span:first').click(function () {
            $(this).parent('li').find('span:first').toggleClass('opened');
            $(this).parent('li').find('div:first').slideToggle();

        });
    }

    //----Redirect To Main Site-----    
    //if (window.top !== window.self) {
    //    try {
    //        if (window.top.location.host)
    //        { /* will throw */ }
    //        else {
    //            defrm(); /* chrome */
    //        }
    //    } catch (ex) {
    //        defrm(); /* everyone else */
    //    }
    //}

    //$(".ItemTracker").click(function () {
    //    IncreaseVisitCount($(this).attr("data-FeedItemId"), $(this).html());
    //});

    generateTopMenu();

    $('#HeaderMenu>ul>li a').click(function (e) {
        //e.preventDefault()
        $("#HeaderMenu>ul>li.active").removeClass("active");
        $(this).parent().addClass("active");

        $(".tab-content .tab-pane").removeClass("active");
        var name = $(this).attr("name");
        $(".tab-content #tabs-" + name).addClass("active");
        //$(".tab-content>div").hide();      
        //$(".tab-content #tabs-" + name).show();
        if ($(this).attr("data-type") == "webpart")
            loadRemoteWp(this, name);
        else if ($(this).attr("data-type") == "page")
            loadPage(this, name);
    })


    //setTimeout(preLoadTabs, 3000);
    //setTimeout(function () { location.reload() }, 50000);
    //-----last item visited----
    setInterval(function () { $("#ticker li:first").slideUp(function () { $(this).appendTo($("#ticker")).slideDown(); }); }, 2500);

});

function preLoadTabs() {
    var tabslink = $('#HeaderMenu>ul>li a');
    for (var i = 0; i < tabslink.length; i++) {
        var name = $(tabslink[i]).attr("name");
        if ($(tabslink[i]).attr("data-type") == "webpart")
            loadRemoteWp(tabslink[i], name);
        else if ($(tabslink[i]).attr("data-type") == "page")
            loadPage(tabslink[i], name);
    }
}

//--------search box------------       
function log(message) {
    $("<div/>").text(message).prependTo("#log");
    $("#log").scrollTop(0);
}

function defrm() {
    document.write = '';
    window.top.location = window.self.location;
    setTimeout(function () {
        document.body.innerHTML = '';
    }, 0);
    window.self.onload = function (evt) {
        document.body.innerHTML = '';
    };
}

function BtnSearch() {
    var query = $('#content').val();
    query = $.trim(query);
    // Here you could use AJAX instead of window.location if you wish  
    if (query.length > 3)
        window.location = '/Key/' + encodeURIComponent(query) + '/';
    return false;
}
function SetTextDir() {
    if ($('#content').val().length > 1) {
        if ((($('#content').val().charCodeAt(0) > 0x600) && ($('#content').val().charCodeAt(0) < 0x6FF)) || ($('#content').val() == "") &&
                (($('#content').val().charCodeAt(1) > 0x600) && ($('#content').val().charCodeAt(1) < 0x6FF)) || ($('#content').val() == "")) {
            $('#content').css("direction", "rtl");
            $('#content').css("text-align", "right");
        }
        else {
            $('#content').css("direction", "ltr");
            $('#content').css("text-align", "left");
        }
    }
}

//------Flow Unflow------
function FlowMe(sender, EntityCode, Content) {
    if (typeof EntityCode == "undefined") { EntityCode = this.EntityCode; }
    if (typeof Content == "undefined") { Content = this.Content; }
    var queri;
    setLoadingInput(sender);
    if (EntityCode == null && EntityCode != "") {
        if (location.href.indexOf('/Cat/') > 0 || location.href.indexOf('/cat/') > 0) {
            queri = "CatsUsersAdding?CatCode";
        }
        else if (location.href.indexOf('/Tag/') > 0 || location.href.indexOf('/tag/') > 0) {
            queri = "TagsUsersAdding?TagContent";
        }
        else if (location.href.indexOf('/Site/') > 0 || location.href.indexOf('/site/') > 0) {
            queri = "SitesUsersAdding?SiteURL";
        }
    }
    else {
        EntityCode = EntityCode.toLowerCase();
        if (EntityCode == "cat") {
            queri = "CatsUsersAdding?CatCode";
        }
        else if (EntityCode == "tag") {
            queri = "TagsUsersAdding?TagContent";
        }
        else if (EntityCode == "site") {
            queri = "SitesUsersAdding?SiteURL";
        }
    }
    $.ajax({
        url: baseLocation + "/user/" + queri + "=" + Content,
        type: 'GET',
        async: false,
        success: function (result) {
            if (result) {
                alert("این مورد با موفقیت  به دنباله های شما افزوده شد");
                $(sender).hide();
                return true;
            }
            else {
                alert("خطا رخ داد،لطفا مجدد سعی کنید");
                return false;
            }
        },
        error: function (result) {
            alert("error");
            return false;
        }
    });
    addGAEvent("TZSocial", "FlowMe", Content);
    return true;
}
function UnFlow(sender, EntityCode, Content) {
    if (typeof EntityCode == "undefined") { EntityCode = this.EntityCode; }
    if (typeof Content == "undefined") { Content = this.Content; }

    var queri;
    setLoadingInput(sender);
    if (EntityCode == null && EntityCode != "") {
        if (location.href.indexOf('/Cat/') > 0 || location.href.indexOf('/cat/') > 0) {
            queri = "CatsUsersDeleting?CatCode";
        }
        else if (location.href.indexOf('/Tag/') > 0 || location.href.indexOf('/tag/') > 0) {
            queri = "TagsUsersDeleting?TagContent";
        }
        else if (location.href.indexOf('/Site/') > 0 || location.href.indexOf('/site/') > 0) {
            queri = "SitesUsersDeleting?SiteURL";
        }
    }
    else {
        EntityCode = EntityCode.toLowerCase();
        if (EntityCode == "cat") {
            queri = "CatsUsersDeleting?CatCode";
        }
        else if (EntityCode == "tag") {
            queri = "TagsUsersDeleting?TagContent";
        }
        else if (EntityCode == "site") {
            queri = "SitesUsersDeleting?SiteURL";
        }
    }
    $.ajax({
        url: baseLocation + "/user/" + queri + "=" + Content,
        type: 'GET',
        async: false,
        success: function (result) {
            if (result) {
                alert("این مورد از لیست دنباله های شما حذف شد");
                $(sender).hide();
                $(sender).prev().hide();
                return true;
            }
            else {
                alert("خطا رخ داد،لطفا مجدد سعی کنید");
                return false;
            }
        },
        error: function (result) {
            alert("error");
            return false;
        }
    });

    addGAEvent("TZSocial", "UnFlowMe", Content);
    return true;
}

function setLoading(DivId) {
    $(DivId).html("<div style='margin:auto;text-align:center'><br /><br /><img src='http://" + location.host + "/Images/facebook-loading.gif' /><br /></div>");
}
function setLoadingHorizontal(DivId) {
    $(DivId).html("<img style='margin:auto;' src='http://" + location.host + "/Images/loadingAnimation.gif' />");
}
function setLoadingInput(InputId) {
    $(InputId).val("منتظر باشید...");
    $(InputId).attr("disabled", "disabled");
}

function IncreaseVisitCount(Itemstr, Titlep) {
    var arr = Itemstr.split(':');
    var FeedItemIdp = arr[0];
    var EntityCodep = arr[1];
    if (arr[2] == null || arr[2] == "")
        arr[2] = typeof EntityRef == "undefined" ? "0" : EntityRef;
    var EntityRefp = parseInt(arr[2]);
    var pubDate = new Date(Date.parse(arr[3].replace('_', ":")));

    addGAEvent("VisitItem", EntityCodep + ":" + EntityRefp, FeedItemIdp);

    if (EntityCodep == null || EntityCodep == "")
        EntityCodep = typeof EntityCode == "undefined" ? "null" : EntityCode;

    var Iurl = '/Items/IncreaseVisitCount';//?FeedItemId=' + FeedItemId + '&EntityCode=' + EntityCode + '&EntityRef=' + EntityRef + '&Title=' + Title.substring(0, 199);
    Iurl = Iurl.trim();
    $.ajax({
        url: Iurl,
        type: 'POST',
        data: JSON.stringify({ FeedItemId: FeedItemIdp, EntityCode: EntityCodep, EntityRef: EntityRefp, Title: Titlep, pubDate: pubDate }),
        contentType: "application/json; charset=utf-8"
    });

}

function isUrlExists(url) {

    var baseAddress;
    if (location.href.indexOf('/site/') > 0)
        baseAddress = location.href.substring(0, location.href.indexOf('/site/'));
    else if (location.href.indexOf('/Site/') > 0)
        baseAddress = location.href.substring(0, location.href.indexOf('/Site/'));
    else
        baseAddress = location.hostname;

    $.ajax({
        url: baseAddress + '/toolbox/IsWebReady?url=' + url,
        type: 'GET',
        success: function (result) {
            if (result) {
                // $("#DFaceBookLike").show();
                $(".DRater").css("height", 80);
            }
            else {
                $("#DFaceBookLike").hide();
            }
        }
    });
}

function resizeIframe(obj) {
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
}

function loadMostVisitedItems(entityCode, entityRef) {
    entityRef = entityRef == "" || entityRef == null ? 0 : entityRef;
    var EntityRefId = parseInt(entityRef);
    if ($('#DMostVisitedContent').html().length < 50) {
        setLoading("#DMostVisitedContent");
        entityCode = entityCode == null || entityCode == "" ? "Any" : entityCode;
        $('#DMostVisitedContent').load('/Items/MostVisitedItems/?EntityCode=' + entityCode + '&EntityRef=' + EntityRefId + '&PageSize=' + 20, function (html) {
            $('#DMostVisitedContent').html(html);

        });
        addGAEvent('Tab', "LoadMostVisitedItems", entityCode + ":" + entityRef);
    }

}

function loadRemoteWp(sender, entityCode) {
    var wpDivId = '#DWp_' + sender.name;
    if ($(wpDivId).html().length > 50)
        return;
    setLoading(wpDivId);
    entityCode = entityCode == null || entityCode == "" ? "Any" : entityCode;
    $(wpDivId).load('/RemotewebPart/wpframe/?wpcode=' + entityCode, function (html) {
        $(wpDivId).html(html);
        addGAEvent("Tab", "LoadRemoteWP", entityCode);
    });

}
function loadPage(sender, entityCode) {
    var wpDivId = '#Dp_' + sender.name;
    if ($(wpDivId).html().length > 50)
        return;
    setLoading(wpDivId);
    entityCode = entityCode == null || entityCode == "" ? "Any" : entityCode;
    $(wpDivId).load('/Page/' + entityCode + '/true', function (html) {
        $(wpDivId).html(html);
        addGAEvent("Tab", "LoadPage", entityCode);
    });

}

//--------Tabs Top Menu-----
function tbShiftLeft() {
    $("DRightShifter").show();
    $("#uTabsHeader li")[0].style.marginRight = "-100px";
}
function tbShiftRight() {

}

//-------------Google Events-------
function addGAEvent(eventCat, Label, Value) {
    try {
        _gaq.push(['_trackEvent', eventCat, Label, Value]);
    } catch (e) {
        console.log("GA error ", e);
    }
}

//------------Load Ads Menu-------
function loadVerticalMenu() {
    var targetUrl = baseLocation + "/Ads/VerticalMenu";
    $('#DAdsMain').load(targetUrl, function (html) {
        $('#DAdsMain').html(html);
    });
}
function loadVerticalMenuBottom() {
    var targetUrl = baseLocation + "/Ads/VerticalMenuBottom";
    $('#DAdsMain').load(targetUrl, function (html) {
        $('#DAdsMain').html(html);
    });
}

//----Share Content-----
function funSocialAjaxi(itemid, SocialNetwork, gotolink, refLocation) {
    addGAEvent("ShareInSocialNetwork", SocialNetwork, itemid);
    $(this).load("/SNShare/Log/?FeedItemId=" + itemid + "&SocialNetwork=" + SocialNetwork, function (html) {

    });
    if (typeof refLocation == "undefined")
        window.open(gotolink + parent.location.href);
    else
        window.open(gotolink + baseLocation + refLocation);
}


/*----Mobile Device-----*/
function showRightMenu(sender) {
    document.getElementById("DRightMenu").style.display = "block";
    document.getElementById("DRightMenu").style.width = "100%";
    sender.style.display = "none";
}

//---Make top menu---
function generateTopMenu() {
    //----Menu----
    $('#TopMenu.cssmenu').prepend('<div id="indicatorContainer"><div id="pIndicator"><div id="cIndicator"></div></div></div>');
    var activeElement = $('#TopMenu.cssmenu>ul>li:first');

    $('#TopMenu.cssmenu>ul>li').each(function () {
        if ($(this).hasClass('active')) {
            activeElement = $(this);
        }
    });


    var posLeft = activeElement.position().left;
    var elementWidth = activeElement.width();
    posLeft = posLeft + elementWidth / 2 - 6;
    if (activeElement.hasClass('has-sub')) {
        posLeft -= 6;
    }

    $('#TopMenu.cssmenu #pIndicator').css('left', posLeft);
    var element, leftPos, indicator = $('#cssmenu pIndicator');

    $("#TopMenu.cssmenu>ul>li").hover(function () {
        element = $(this);
        var w = element.width();
        if ($(this).hasClass('has-sub')) {
            leftPos = element.position().left + w / 2 - 12;
        }
        else {
            leftPos = element.position().left + w / 2 - 6;
        }

        $('#TopMenu.cssmenu #pIndicator').css('left', leftPos);
    }
    , function () {
        $('#TopMenu.cssmenu #pIndicator').css('left', posLeft);
    });


    $('#TopMenu.cssmenu>ul>.has-sub>ul').append('<div class="submenuArrow"></div>');
    $('#TopMenu.cssmenu>ul').children('.has-sub').each(function () {
        var posLeftArrow = $(this).width();
        posLeftArrow /= 2;
        posLeftArrow += 22;
        $(this).find('.submenuArrow').css('right', posLeftArrow);

    });

    $('#TopMenu.cssmenu>ul').prepend('<li id="menu-button"><a>نمایش منو</a><span class="btn-lg glyphicon glyphicon-align-justify"></span></li>');
    $("#TopMenu.cssmenu #menu-button").click(function () {
        if ($(this).parent().hasClass('open')) {
            $(this).parent().removeClass('open');
        }
        else {
            $(this).parent().addClass('open');
        }
    });
}

//--generate ajaxlink----
function generateAjaxLink() {
    var elements = $("a.ajaxlink");
    for (var i = 0; i < elements.length; i++) {
        $(elements[i]).attr("data-link", $(elements[i]).attr("href"));
        $(elements[i]).attr("href", "##");
    }
}
/**
*  Ajax Autocomplete for jQuery, version %version%
*  (c) 2015 Tomas Kirda
*
*  Ajax Autocomplete for jQuery is freely distributable under the terms of an MIT-style license.
*  For details, see the web site: https://github.com/devbridge/jQuery-Autocomplete
*/

/*jslint  browser: true, white: true, plusplus: true, vars: true */
/*global define, window, document, jQuery, exports, require */

// Expose plugin as an AMD module if AMD loader is present:
(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object' && typeof require === 'function') {
        // Browserify
        factory(require('jquery'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {
    'use strict';

    var
        utils = (function () {
            return {
                escapeRegExChars: function (value) {
                    return value.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
                },
                createNode: function (containerClass) {
                    var div = document.createElement('div');
                    div.className = containerClass;
                    div.style.position = 'absolute';
                    div.style.display = 'none';
                    return div;
                }
            };
        }()),

        keys = {
            ESC: 27,
            TAB: 9,
            RETURN: 13,
            LEFT: 37,
            UP: 38,
            RIGHT: 39,
            DOWN: 40
        };

    function Autocomplete(el, options) {
        var noop = function () { },
            that = this,
            defaults = {
                ajaxSettings: {},
                autoSelectFirst: false,
                appendTo: document.body,
                serviceUrl: null,
                lookup: null,
                onSelect: null,
                width: 'auto',
                minChars: 1,
                maxHeight: 300,
                deferRequestBy: 0,
                params: {},
                formatResult: Autocomplete.formatResult,
                delimiter: null,
                zIndex: 9999,
                type: 'GET',
                noCache: false,
                onSearchStart: noop,
                onSearchComplete: noop,
                onSearchError: noop,
                preserveInput: false,
                containerClass: 'autocomplete-suggestions',
                tabDisabled: false,
                dataType: 'text',
                currentRequest: null,
                triggerSelectOnValidInput: true,
                preventBadQueries: true,
                lookupFilter: function (suggestion, originalQuery, queryLowerCase) {
                    return suggestion.value.toLowerCase().indexOf(queryLowerCase) !== -1;
                },
                paramName: 'query',
                transformResult: function (response) {
                    return typeof response === 'string' ? $.parseJSON(response) : response;
                },
                showNoSuggestionNotice: false,
                noSuggestionNotice: 'No results',
                orientation: 'bottom',
                forceFixPosition: false
            };

        // Shared variables:
        that.element = el;
        that.el = $(el);
        that.suggestions = [];
        that.badQueries = [];
        that.selectedIndex = -1;
        that.currentValue = that.element.value;
        that.intervalId = 0;
        that.cachedResponse = {};
        that.onChangeInterval = null;
        that.onChange = null;
        that.isLocal = false;
        that.suggestionsContainer = null;
        that.noSuggestionsContainer = null;
        that.options = $.extend({}, defaults, options);
        that.classes = {
            selected: 'autocomplete-selected',
            suggestion: 'autocomplete-suggestion'
        };
        that.hint = null;
        that.hintValue = '';
        that.selection = null;

        // Initialize and set options:
        that.initialize();
        that.setOptions(options);
    }

    Autocomplete.utils = utils;

    $.Autocomplete = Autocomplete;

    Autocomplete.formatResult = function (suggestion, currentValue) {
        var htmlSafeString = suggestion.value
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');

        var pattern = '(' + utils.escapeRegExChars(currentValue) + ')';

        return htmlSafeString;//.replace(new RegExp(pattern, 'gi'), '<strong>$1<\/strong>');
    };

    Autocomplete.prototype = {

        killerFn: null,

        initialize: function () {
            var that = this,
                suggestionSelector = '.' + that.classes.suggestion,
                selected = that.classes.selected,
                options = that.options,
                container;

            // Remove autocomplete attribute to prevent native suggestions:
            that.element.setAttribute('autocomplete', 'off');

            that.killerFn = function (e) {
                if ($(e.target).closest('.' + that.options.containerClass).length === 0) {
                    that.killSuggestions();
                    that.disableKillerFn();
                }
            };

            // html() deals with many types: htmlString or Element or Array or jQuery
            that.noSuggestionsContainer = $('<div class="autocomplete-no-suggestion"></div>')
                                          .html(this.options.noSuggestionNotice).get(0);

            that.suggestionsContainer = Autocomplete.utils.createNode(options.containerClass);

            container = $(that.suggestionsContainer);

            container.appendTo(options.appendTo);

            // Only set width if it was provided:
            if (options.width !== 'auto') {
                container.width(options.width);
            }

            // Listen for mouse over event on suggestions list:
            container.on('mouseover.autocomplete', suggestionSelector, function () {
                that.activate($(this).data('index'));
            });

            // Deselect active element when mouse leaves suggestions container:
            container.on('mouseout.autocomplete', function () {
                that.selectedIndex = -1;
                container.children('.' + selected).removeClass(selected);
            });

            // Listen for click event on suggestions list:
            container.on('click.autocomplete', suggestionSelector, function () {
                that.select($(this).data('index'));
            });

            that.fixPositionCapture = function () {
                if (that.visible) {
                    that.fixPosition();
                }
            };

            $(window).on('resize.autocomplete', that.fixPositionCapture);

            that.el.on('keydown.autocomplete', function (e) { that.onKeyPress(e); });
            that.el.on('keyup.autocomplete', function (e) { that.onKeyUp(e); });
            that.el.on('blur.autocomplete', function () { that.onBlur(); });
            that.el.on('focus.autocomplete', function () { that.onFocus(); });
            that.el.on('change.autocomplete', function (e) { that.onKeyUp(e); });
            that.el.on('input.autocomplete', function (e) { that.onKeyUp(e); });
        },

        onFocus: function () {
            var that = this;
            that.fixPosition();
            if (that.options.minChars <= that.el.val().length) {
                that.onValueChange();
            }
        },

        onBlur: function () {
            this.enableKillerFn();
        },

        abortAjax: function () {
            var that = this;
            if (that.currentRequest) {
                that.currentRequest.abort();
                that.currentRequest = null;
            }
        },

        setOptions: function (suppliedOptions) {
            var that = this,
                options = that.options;

            $.extend(options, suppliedOptions);

            that.isLocal = $.isArray(options.lookup);

            if (that.isLocal) {
                options.lookup = that.verifySuggestionsFormat(options.lookup);
            }

            options.orientation = that.validateOrientation(options.orientation, 'bottom');

            // Adjust height, width and z-index:
            $(that.suggestionsContainer).css({
                'max-height': options.maxHeight + 'px',
                'width': options.width + 'px',
                'z-index': options.zIndex
            });
        },


        clearCache: function () {
            this.cachedResponse = {};
            this.badQueries = [];
        },

        clear: function () {
            this.clearCache();
            this.currentValue = '';
            this.suggestions = [];
        },

        disable: function () {
            var that = this;
            that.disabled = true;
            clearInterval(that.onChangeInterval);
            that.abortAjax();
        },

        enable: function () {
            this.disabled = false;
        },

        fixPosition: function () {
            // Use only when container has already its content

            var that = this,
                $container = $(that.suggestionsContainer),
                containerParent = $container.parent().get(0);
            // Fix position automatically when appended to body.
            // In other cases force parameter must be given.
            if (containerParent !== document.body && !that.options.forceFixPosition) {
                return;
            }

            // Choose orientation
            var orientation = that.options.orientation,
                containerHeight = $container.outerHeight(),
                height = that.el.outerHeight(),
                offset = that.el.offset(),
                styles = { 'top': offset.top, 'left': offset.left };

            if (orientation === 'auto') {
                var viewPortHeight = $(window).height(),
                    scrollTop = $(window).scrollTop(),
                    topOverflow = -scrollTop + offset.top - containerHeight,
                    bottomOverflow = scrollTop + viewPortHeight - (offset.top + height + containerHeight);

                orientation = (Math.max(topOverflow, bottomOverflow) === topOverflow) ? 'top' : 'bottom';
            }

            if (orientation === 'top') {
                styles.top += -containerHeight;
            } else {
                styles.top += height;
            }

            // If container is not positioned to body,
            // correct its position using offset parent offset
            if (containerParent !== document.body) {
                var opacity = $container.css('opacity'),
                    parentOffsetDiff;

                if (!that.visible) {
                    $container.css('opacity', 0).show();
                }

                parentOffsetDiff = $container.offsetParent().offset();
                styles.top -= parentOffsetDiff.top;
                styles.left -= parentOffsetDiff.left;

                if (!that.visible) {
                    $container.css('opacity', opacity).hide();
                }
            }

            // -2px to account for suggestions border.
            if (that.options.width === 'auto') {
                styles.width = (that.el.outerWidth() - 2) + 'px';
            }

            $container.css(styles);
        },

        enableKillerFn: function () {
            var that = this;
            $(document).on('click.autocomplete', that.killerFn);
        },

        disableKillerFn: function () {
            var that = this;
            $(document).off('click.autocomplete', that.killerFn);
        },

        killSuggestions: function () {
            var that = this;
            that.stopKillSuggestions();
            that.intervalId = window.setInterval(function () {
                that.hide();
                that.stopKillSuggestions();
            }, 50);
        },

        stopKillSuggestions: function () {
            window.clearInterval(this.intervalId);
        },

        isCursorAtEnd: function () {
            var that = this,
                valLength = that.el.val().length,
                selectionStart = that.element.selectionStart,
                range;

            if (typeof selectionStart === 'number') {
                return selectionStart === valLength;
            }
            if (document.selection) {
                range = document.selection.createRange();
                range.moveStart('character', -valLength);
                return valLength === range.text.length;
            }
            return true;
        },

        onKeyPress: function (e) {
            var that = this;

            // If suggestions are hidden and user presses arrow down, display suggestions:
            if (!that.disabled && !that.visible && e.which === keys.DOWN && that.currentValue) {
                that.suggest();
                return;
            }

            if (that.disabled || !that.visible) {
                return;
            }

            switch (e.which) {
                case keys.ESC:
                    that.el.val(that.currentValue);
                    that.hide();
                    break;
                case keys.RIGHT:
                    if (that.hint && that.options.onHint && that.isCursorAtEnd()) {
                        that.selectHint();
                        break;
                    }
                    return;
                case keys.TAB:
                    if (that.hint && that.options.onHint) {
                        that.selectHint();
                        return;
                    }
                    if (that.selectedIndex === -1) {
                        that.hide();
                        return;
                    }
                    that.select(that.selectedIndex);
                    if (that.options.tabDisabled === false) {
                        return;
                    }
                    break;
                case keys.RETURN:
                    if (that.selectedIndex === -1) {
                        that.hide();
                        return;
                    }
                    that.select(that.selectedIndex);
                    break;
                case keys.UP:
                    that.moveUp();
                    break;
                case keys.DOWN:
                    that.moveDown();
                    break;
                default:
                    return;
            }

            // Cancel event if function did not return:
            e.stopImmediatePropagation();
            e.preventDefault();
        },

        onKeyUp: function (e) {
            var that = this;

            if (that.disabled) {
                return;
            }

            switch (e.which) {
                case keys.UP:
                case keys.DOWN:
                    return;
            }

            clearInterval(that.onChangeInterval);

            if (that.currentValue !== that.el.val()) {
                that.findBestHint();
                if (that.options.deferRequestBy > 0) {
                    // Defer lookup in case when value changes very quickly:
                    that.onChangeInterval = setInterval(function () {
                        that.onValueChange();
                    }, that.options.deferRequestBy);
                } else {
                    that.onValueChange();
                }
            }
        },

        onValueChange: function () {
            var that = this,
                options = that.options,
                value = that.el.val(),
                query = that.getQuery(value),
                index;

            if (that.selection && that.currentValue !== query) {
                that.selection = null;
                (options.onInvalidateSelection || $.noop).call(that.element);
            }

            clearInterval(that.onChangeInterval);
            that.currentValue = value;
            that.selectedIndex = -1;

            // Check existing suggestion for the match before proceeding:
            if (options.triggerSelectOnValidInput) {
                index = that.findSuggestionIndex(query);
                if (index !== -1) {
                    that.select(index);
                    return;
                }
            }

            if (query.length < options.minChars) {
                that.hide();
            } else {
                that.getSuggestions(query);
            }
        },

        findSuggestionIndex: function (query) {
            var that = this,
                index = -1,
                queryLowerCase = query.toLowerCase();

            $.each(that.suggestions, function (i, suggestion) {
                if (suggestion.value.toLowerCase() === queryLowerCase) {
                    index = i;
                    return false;
                }
            });

            return index;
        },

        getQuery: function (value) {
            var delimiter = this.options.delimiter,
                parts;

            if (!delimiter) {
                return value;
            }
            parts = value.split(delimiter);
            return $.trim(parts[parts.length - 1]);
        },

        getSuggestionsLocal: function (query) {
            var that = this,
                options = that.options,
                queryLowerCase = query.toLowerCase(),
                filter = options.lookupFilter,
                limit = parseInt(options.lookupLimit, 10),
                data;

            data = {
                suggestions: $.grep(options.lookup, function (suggestion) {
                    return filter(suggestion, query, queryLowerCase);
                })
            };

            if (limit && data.suggestions.length > limit) {
                data.suggestions = data.suggestions.slice(0, limit);
            }

            return data;
        },

        getSuggestions: function (q) {
            var response,
                that = this,
                options = that.options,
                serviceUrl = options.serviceUrl,
                params,
                cacheKey,
                ajaxSettings;

            options.params[options.paramName] = q;
            params = options.ignoreParams ? null : options.params;

            if (options.onSearchStart.call(that.element, options.params) === false) {
                return;
            }

            if ($.isFunction(options.lookup)) {
                options.lookup(q, function (data) {
                    that.suggestions = data.suggestions;
                    that.suggest();
                    options.onSearchComplete.call(that.element, q, data.suggestions);
                });
                return;
            }

            if (that.isLocal) {
                response = that.getSuggestionsLocal(q);
            } else {
                if ($.isFunction(serviceUrl)) {
                    serviceUrl = serviceUrl.call(that.element, q);
                }
                cacheKey = serviceUrl + '?' + $.param(params || {});
                response = that.cachedResponse[cacheKey];
            }

            if (response && $.isArray(response.suggestions)) {
                that.suggestions = response.suggestions;
                that.suggest();
                options.onSearchComplete.call(that.element, q, response.suggestions);
            } else if (!that.isBadQuery(q)) {
                that.abortAjax();

                ajaxSettings = {
                    url: serviceUrl,
                    data: params,
                    type: options.type,
                    dataType: options.dataType
                };

                $.extend(ajaxSettings, options.ajaxSettings);

                that.currentRequest = $.ajax(ajaxSettings).done(function (data) {
                    var result;
                    that.currentRequest = null;
                    result = options.transformResult(data);
                    that.processResponse(result, q, cacheKey);
                    options.onSearchComplete.call(that.element, q, result.suggestions);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    options.onSearchError.call(that.element, q, jqXHR, textStatus, errorThrown);
                });
            } else {
                options.onSearchComplete.call(that.element, q, []);
            }
        },

        isBadQuery: function (q) {
            if (!this.options.preventBadQueries) {
                return false;
            }

            var badQueries = this.badQueries,
                i = badQueries.length;

            while (i--) {
                if (q.indexOf(badQueries[i]) === 0) {
                    return true;
                }
            }

            return false;
        },

        hide: function () {
            var that = this,
                container = $(that.suggestionsContainer);

            if ($.isFunction(that.options.onHide) && that.visible) {
                that.options.onHide.call(that.element, container);
            }

            that.visible = false;
            that.selectedIndex = -1;
            clearInterval(that.onChangeInterval);
            $(that.suggestionsContainer).hide();
            that.signalHint(null);
        },

        suggest: function () {
            if (this.suggestions.length === 0) {
                if (this.options.showNoSuggestionNotice) {
                    this.noSuggestions();
                } else {
                    this.hide();
                }
                return;
            }

            var that = this,
                options = that.options,
                groupBy = options.groupBy,
                formatResult = options.formatResult,
                value = that.getQuery(that.currentValue),
                className = that.classes.suggestion,
                classSelected = that.classes.selected,
                container = $(that.suggestionsContainer),
                noSuggestionsContainer = $(that.noSuggestionsContainer),
                beforeRender = options.beforeRender,
                html = '',
                category,
                formatGroup = function (suggestion, index) {
                    var currentCategory = suggestion.data[groupBy];

                    if (category === currentCategory) {
                        return '';
                    }

                    category = currentCategory;

                    return '<div class="autocomplete-group"><strong>' + category + '</strong></div>';
                },
                index;

            if (options.triggerSelectOnValidInput) {
                index = that.findSuggestionIndex(value);
                if (index !== -1) {
                    that.select(index);
                    return;
                }
            }

            // Build suggestions inner HTML:
            $.each(that.suggestions, function (i, suggestion) {
                if (groupBy) {
                    html += formatGroup(suggestion, value, i);
                }

                html += '<div class="' + className + '" data-index="' + i + '">' + formatResult(suggestion, value) + '</div>';
            });

            this.adjustContainerWidth();

            noSuggestionsContainer.detach();
            container.html(html);

            if ($.isFunction(beforeRender)) {
                beforeRender.call(that.element, container);
            }

            that.fixPosition();
            container.show();

            // Select first value by default:
            if (options.autoSelectFirst) {
                that.selectedIndex = 0;
                container.scrollTop(0);
                container.children('.' + className).first().addClass(classSelected);
            }

            that.visible = true;
            that.findBestHint();
        },

        noSuggestions: function () {
            var that = this,
                container = $(that.suggestionsContainer),
                noSuggestionsContainer = $(that.noSuggestionsContainer);

            this.adjustContainerWidth();

            // Some explicit steps. Be careful here as it easy to get
            // noSuggestionsContainer removed from DOM if not detached properly.
            noSuggestionsContainer.detach();
            container.empty(); // clean suggestions if any
            container.append(noSuggestionsContainer);

            that.fixPosition();

            container.show();
            that.visible = true;
        },

        adjustContainerWidth: function () {
            var that = this,
                options = that.options,
                width,
                container = $(that.suggestionsContainer);

            // If width is auto, adjust width before displaying suggestions,
            // because if instance was created before input had width, it will be zero.
            // Also it adjusts if input width has changed.
            // -2px to account for suggestions border.
            if (options.width === 'auto') {
                width = that.el.outerWidth() - 2;
                container.width(width > 0 ? width : 300);
            }
        },

        findBestHint: function () {
            var that = this,
                value = that.el.val().toLowerCase(),
                bestMatch = null;

            if (!value) {
                return;
            }

            $.each(that.suggestions, function (i, suggestion) {
                var foundMatch = suggestion.value.toLowerCase().indexOf(value) === 0;
                if (foundMatch) {
                    bestMatch = suggestion;
                }
                return !foundMatch;
            });

            that.signalHint(bestMatch);
        },

        signalHint: function (suggestion) {
            var hintValue = '',
                that = this;
            if (suggestion) {
                hintValue = that.currentValue + suggestion.value.substr(that.currentValue.length);
            }
            if (that.hintValue !== hintValue) {
                that.hintValue = hintValue;
                that.hint = suggestion;
                (this.options.onHint || $.noop)(hintValue);
            }
        },

        verifySuggestionsFormat: function (suggestions) {
            // If suggestions is string array, convert them to supported format:
            if (suggestions.length && typeof suggestions[0] === 'string') {
                return $.map(suggestions, function (value) {
                    return { value: value, data: null };
                });
            }

            return suggestions;
        },

        validateOrientation: function (orientation, fallback) {
            orientation = $.trim(orientation || '').toLowerCase();

            if ($.inArray(orientation, ['auto', 'bottom', 'top']) === -1) {
                orientation = fallback;
            }

            return orientation;
        },

        processResponse: function (result, originalQuery, cacheKey) {
            var that = this,
                options = that.options;

            result.suggestions = that.verifySuggestionsFormat(result.suggestions);

            // Cache results if cache is not disabled:
            if (!options.noCache) {
                that.cachedResponse[cacheKey] = result;
                if (options.preventBadQueries && result.suggestions.length === 0) {
                    that.badQueries.push(originalQuery);
                }
            }

            // Return if originalQuery is not matching current query:
            if (originalQuery !== that.getQuery(that.currentValue)) {
                return;
            }

            that.suggestions = result.suggestions;
            that.suggest();
        },

        activate: function (index) {
            var that = this,
                activeItem,
                selected = that.classes.selected,
                container = $(that.suggestionsContainer),
                children = container.find('.' + that.classes.suggestion);

            container.find('.' + selected).removeClass(selected);

            that.selectedIndex = index;

            if (that.selectedIndex !== -1 && children.length > that.selectedIndex) {
                activeItem = children.get(that.selectedIndex);
                $(activeItem).addClass(selected);
                return activeItem;
            }

            return null;
        },

        selectHint: function () {
            var that = this,
                i = $.inArray(that.hint, that.suggestions);

            that.select(i);
        },

        select: function (i) {
            var that = this;
            that.hide();
            that.onSelect(i);
        },

        moveUp: function () {
            var that = this;

            if (that.selectedIndex === -1) {
                return;
            }

            if (that.selectedIndex === 0) {
                $(that.suggestionsContainer).children().first().removeClass(that.classes.selected);
                that.selectedIndex = -1;
                that.el.val(that.currentValue);
                that.findBestHint();
                return;
            }

            that.adjustScroll(that.selectedIndex - 1);
        },

        moveDown: function () {
            var that = this;

            if (that.selectedIndex === (that.suggestions.length - 1)) {
                return;
            }

            that.adjustScroll(that.selectedIndex + 1);
        },

        adjustScroll: function (index) {
            var that = this,
                activeItem = that.activate(index);

            if (!activeItem) {
                return;
            }

            var offsetTop,
                upperBound,
                lowerBound,
                heightDelta = $(activeItem).outerHeight();

            offsetTop = activeItem.offsetTop;
            upperBound = $(that.suggestionsContainer).scrollTop();
            lowerBound = upperBound + that.options.maxHeight - heightDelta;

            if (offsetTop < upperBound) {
                $(that.suggestionsContainer).scrollTop(offsetTop);
            } else if (offsetTop > lowerBound) {
                $(that.suggestionsContainer).scrollTop(offsetTop - that.options.maxHeight + heightDelta);
            }

            if (!that.options.preserveInput) {
                that.el.val(that.getValue(that.suggestions[index].value));
            }
            that.signalHint(null);
        },

        onSelect: function (index) {
            var that = this,
                onSelectCallback = that.options.onSelect,
                suggestion = that.suggestions[index];

            that.currentValue = that.getValue(suggestion.value);

            if (that.currentValue !== that.el.val() && !that.options.preserveInput) {
                that.el.val(that.currentValue);
            }

            that.signalHint(null);
            that.suggestions = [];
            that.selection = suggestion;

            if ($.isFunction(onSelectCallback)) {
                onSelectCallback.call(that.element, suggestion);
            }
        },

        getValue: function (value) {
            var that = this,
                delimiter = that.options.delimiter,
                currentValue,
                parts;

            if (!delimiter) {
                return value;
            }

            currentValue = that.currentValue;
            parts = currentValue.split(delimiter);

            if (parts.length === 1) {
                return value;
            }

            return currentValue.substr(0, currentValue.length - parts[parts.length - 1].length) + value;
        },

        dispose: function () {
            var that = this;
            that.el.off('.autocomplete').removeData('autocomplete');
            that.disableKillerFn();
            $(window).off('resize.autocomplete', that.fixPositionCapture);
            $(that.suggestionsContainer).remove();
        }
    };

    // Create chainable jQuery plugin:
    $.fn.autocomplete = $.fn.devbridgeAutocomplete = function (options, args) {
        var dataKey = 'autocomplete';
        // If function invoked without argument return
        // instance of the first matched element:
        if (arguments.length === 0) {
            return this.first().data(dataKey);
        }

        return this.each(function () {
            var inputElement = $(this),
                instance = inputElement.data(dataKey);

            if (typeof options === 'string') {
                if (instance && typeof instance[options] === 'function') {
                    instance[options](args);
                }
            } else {
                // If instance already exists, destroy it:
                if (instance && instance.dispose) {
                    instance.dispose();
                }
                instance = new Autocomplete(this, options);
                inputElement.data(dataKey, instance);
            }
        });
    };
}));