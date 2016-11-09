var arrHots;


var clickCounter = 0;
var cats = ["fun", "news", "music", "job", "football", "download"];
jQuery.browser = {};


$(document).ready(function () {
    setBrowser();
    resizeToWindow();

    //setTimeout(function () { $(".Banner").slideToggle("slow"); }, 4000);
    //setTimeout(function () { $(".DRater").slideToggle('slow'); }, 7000);
    setTimeout(function () { $("#DLastVisitedAbs").slideToggle('slow'); }, 5000);
    setTimeout(function () { $("#DFramContiner").css("background-image", ""); }, 8000);        

    //-----check facebook-------    
    //try {
    //    var url = "https://graph.facebook.com/tazeyab/feed?limit=2&callback=?";
    //    $.getJSON(url, function (json) {
    //        if (json.error.message.indexOf("access token") > 0) {
    //            // $("#DFaceBookLike").show();                
    //            $("#DFaceBookLike").css("visibility", "visible");
    //            //$(".DRater").css("height", 80);
    //        }
    //    });
    //}
    //catch (ex) { }
    //----hot subject-------
    arrHots = $("#DHotSubject a");
    showHotSubject(0);

    //----Menu----
    $('#cssmenu').prepend('<div id="indicatorContainer"><div id="pIndicator"><div id="cIndicator"></div></div></div>');
    var activeElement = $('#cssmenu>ul>li:first');

    $('#cssmenu>ul>li').each(function () {
        if ($(this).hasClass('active')) {
            activeElement = $(this);
        }
    });
  
    //var posLeft = activeElement.position().left;
    //var elementWidth = activeElement.width();
    //posLeft = posLeft + elementWidth / 2 - 6;
    //if (activeElement.hasClass('has-sub')) {
    //    posLeft -= 6;
    //}
   
    //$('#cssmenu #pIndicator').css('left', posLeft);
    //var element, leftPos, indicator = $('#cssmenu pIndicator');   
    //$("#cssmenu>ul>li").hover(function () {
    //    element = $(this);
    //    var w = element.width();
    //    if ($(this).hasClass('has-sub')) {
    //        leftPos = element.position().left + w / 2 - 12;
    //    }
    //    else {
    //        leftPos = element.position().left + w / 2 - 6;
    //    }

    //    $('#cssmenu #pIndicator').css('left', leftPos);
    //}
    //, function () {
    //    $('#cssmenu #pIndicator').css('left', posLeft);
    //});


    //$('#cssmenu>ul>.has-sub>ul').append('<div class="submenuArrow"></div>');
    //$('#cssmenu>ul').children('.has-sub').each(function () {
    //    var posLeftArrow = $(this).width();
    //    posLeftArrow /= 2;
    //    posLeftArrow += 22;
    //    $(this).find('.submenuArrow').css('right', posLeftArrow);

    //});

    //$('#cssmenu>ul').prepend('<li id="menu-button"><a>نمایش منو</a></li>');
    //$("#menu-button").click(function () {
    //    if ($(this).parent().hasClass('open')) {
    //        $(this).parent().removeClass('open');
    //    }
    //    else {
    //        $(this).parent().addClass('open');
    //    }
    //});

    $("#TopMenu #home-logo").removeClass("home-logo-hidden");

});

function setBrowser() {
    jQuery.browser.mozilla = /mozilla/.test(navigator.userAgent.toLowerCase()) && !/webkit    /.test(navigator.userAgent.toLowerCase());
    jQuery.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
    jQuery.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
    jQuery.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
}

function errorAlert() {
    console.log("5050");
}

function showPopup(sender) {
    try {
        if (clickCounter == 0) {
            $(sender).hide();
            //var num = Math.floor((Math.random() * 6) + 0);
            //openNewTab("/cat/" + cats[num]);
            if (document.referrer.indexOf("tazeyab.com") < 0)
                openNewTab("http://www.tazeyab.com");
            clickCounter++;
        }
    }
    catch (ex) {
        $(sender).hide();
        clickCounter++;
    }
}


function openNewTab(pagePath) {
    var isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
    if (isChrome) {
        var a = document.createElement("a");
        a.href = pagePath;
        var evt = document.createEvent("MouseEvents");
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    }
    else {
        var myWindow = window.open(pagePath);
    }
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
function resizeToWindow() {
    var windowHeight = $(window).height();

    if ($.browser.msie && parseInt($.browser.version, 10) < 9)
        $('.resize-to-window').height(windowHeight - 4 - $("#DBanner").height());
    else
        $('.resize-to-window').height(windowHeight);
}

function showHotSubject(thisId) {
    $("#DHotSubject a").hide('slow');
    $(arrHots[thisId]).show('slow');
    if (thisId < arrHots.length - 1) {
        var calll = "showHotSubject(" + (thisId + 1) + ")";
        setTimeout(calll, 8000);
    }
    else if (thisId == arrHots.length - 1) {

        var calll = "showHotSubject(0)";
        setTimeout(calll, 8000);
    }

}
function sleep(ms) {
    var dt = new Date();
    dt.setTime(dt.getTime() + ms);
    while (new Date().getTime() < dt.getTime());
}
$(window).resize(function () {
    resizeToWindow();
});
function funSocial(itemid, SocialNetwork, gotolink) {
    addGAEvent("ShareInSocialNetwork", SocialNetwork, itemid);
    window.open("/SNShare/?Item=" + itemid + "&SocialNetwork=" + SocialNetwork + "&GotoLink=" + gotolink + parent.location.href)
    //parent.location.href = gotolink + parent.location.href
}

function CloseWin(gotoLink) {
    parent.location.href = gotoLink;
    //alert(top.location.href);
}



