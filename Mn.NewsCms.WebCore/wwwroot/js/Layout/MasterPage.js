

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