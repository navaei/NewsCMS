/// <reference path="../Layout/MasterPage.js" />



var occureMore = false;
$(document).ready(function () {

    var fufcallback = function (data, textStatus, xhr) {
        if (data == "True" || data == true || data == "true") {
            $("#DUnFlow").show();
            $("#DFlow").hide();
        }
        else {
            $("#DUnFlow").hide();
            $("#DFlow").show();
        }
    }
    AuthenticateObject.AuthenticateCallBack = function (res) {
        if (res == true || res == true || res == "True" || res == "true") {
            $("#DFlowUnFlow").show();
            IsUserFlow(fufcallback);
        }
    }

    try {
        $("#HeaderMenu li").removeClass("active");
        $("#HeaderMenu li a[href*='" + location.pathname + "']").parent().addClass("active");
    } catch (e) { }

    //if (Toggle == 1) {
    //    if ($(".ItemToggle").length > 0) {
    //        $(".ItemToggle").toggle();
    //        var bg = $(".UpBtn").css("background-image");
    //        bg = bg.replace("up", "down");
    //        $(".UpBtn").css("background-image", bg);
    //    }
    //}
    //-----dropdown-----
    $("#SubCatsList").change(function () {
        location.href = '/cat/' + $(this).val();
    });

    //$('#content').val("");
    //----most visited tabs----
    $("#uTabsHeader").show();

    //setTimeout(function () { var tabs = $("#tabs").tabs().scrollabletab(); $("#uTabsHeader").show(); }, 1000);

    if (!occureMore) {
        $(window).scroll(function () {
            if ($(window).scrollTop() + 280 > $(document).height() - $(window).height()) {
                // ajax call get data from server and append to the div
                occureMore = true;
                try {
                    loadMoreItems();
                }
                catch (ex) { }
            }
        });
    }

    //setTimeout(function () { $(document).tooltip() }, 1000);

});
//loadMostVisitedItems
function showAdminScript(content, PagePath, title) {
    setLoading("#DialogScriptAdminContiner");
    //---------dialog---------   
    $("#DialogScriptAdminContiner").dialog({
        width: 600,
        modal: true,
        close: function (event, ui) {
            $("#DialogScriptAdminContiner").remove();
        }
    });
    var targetUrl = encodeURI('/Toolbox/Demo?content=' + content + '&PagePath=' + PagePath + "&title=" + title);
    $('#DialogScriptAdminContiner').load(targetUrl, function (html) {
        $('#DialogScriptAdminContiner').html(html);
    });

}
//-------LIKE++ --------
function LikeItem(sender, feedItemId, fullPath, Rate) {
    $.ajax({
        url: fullPath + '/Feed/LikeItem?feeditemid=' + feedItemId + '&Rate=' + Rate,
        type: 'GET',
        success: function (result) {
            $(sender).parent().parent().html("این مطلب تاکنون توسط " + result + " نفر پیشنهاد داده شده است");
        }
    });
}

function callService(feedidp, IdP) {
    $.ajax({
        type: "POST",
        url: "/Feed/DeleteFeedFromCat",
        data: JSON.stringify({ feedid: feedidp, Id: IdP }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            alert("delete....");
        },
        error: function (response) {
            alert("error..." + response.d);
        }
    });
}

function getParameterByName(name) {

    var match = RegExp('[?&]' + name + '=([^&]*)')
                    .exec(window.location.search);

    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));

}

function ToggleItem(sender) {

    $(sender).next().toggle('slow');
    var bg = $(sender).css("background-image");
    if (bg.indexOf("down") > 0)
        bg = bg.replace("down", "up");
    else
        bg = bg.replace("up", "down");
    $(sender).css("background-image", bg);

}

function IsUserFlow(cb) {
    $.ajax({
        url: FullPath + '/User/IsUserFlow?Content=' + Content + "&EntityCode=" + EntityCode,
        type: 'GET',
        async: false,
        success: cb
    });
}

function loadMoreItems() {
    if ($("#DMoreItems_" + PageIndex).html().length > 400)
        return;
    var target = $("#DMoreItems_" + PageIndex + " a.MoreBtn").attr("href");
    if (typeof target == "undefined")
        return;
    target = target.toLocaleLowerCase().replace("/q/", "/");
    target = target.replace(EntityCode.toLowerCase(), EntityCode.toLowerCase() + "/items");
    setLoading("#DMoreItems_" + PageIndex);
    $("#DMoreItems_" + PageIndex).load(encodeURI(target), function (html) {
        $("#DMoreItems_" + PageIndex).html(html);
        var dTitle = $("#DMoreItems_" + PageIndex + " div.alert")[0];
        if (dTitle != null)
            $(dTitle).hide();
        if (Toggle == 1) {
            if ($("#DMoreItems_" + PageIndex + " .ItemToggle").length > 0) {
                $("#DMoreItems_" + PageIndex + " .ItemToggle").toggle();
                var bg = $("#DMoreItems_" + PageIndex + " .UpBtn").css("background-image");
                bg = bg.replace("up", "down");
                $("#DMoreItems_" + PageIndex + " .UpBtn").css("background-image", bg);
            }
        }
        occureMore = false;
        PageIndex++;
        $("#DScrollTop").show();
        //$(".ItemTracker").click(function () {
        //    IncreaseVisitCount($(this).attr("data-FeedItemId"), $(this).html());
        //});
        textToWiki("DMoreItems_" + (PageIndex - 1));
        addGAEvent("MoreData", EntityCode + ":" + Content, PageIndex);

    });
}

function loadItemsPaging(pageIndex, target) {
    if (typeof pageIndex == "undefined" || typeof target == "undefined")
        return;

    $(".items-tab").hide();
    $("#tab-" + pageIndex).show();
    if ($(".items-tab-container").has("#tab-" + pageIndex).length > 0)
        return;
    $(".items-tab-container").append("<div class='items-tab' id='tab-" + pageIndex + "'></div>");
    setLoading("#tab-" + pageIndex);
    $.get(encodeURI(target), function (html) {
        $("#tab-" + pageIndex).html(html);
        var dTitle = $("#tab-" + pageIndex + " div.alert")[0];
        if (dTitle != null)
            $(dTitle).hide();
        if (Toggle == 1) {
            if ($("#tab-" + pageIndex + " .ItemToggle").length > 0) {
                $("#tab-" + pageIndex + " .ItemToggle").toggle();
                var bg = $("#tab-" + pageIndex + " .UpBtn").css("background-image");
                bg = bg.replace("up", "down");
                $("#tab-" + pageIndex + " .UpBtn").css("background-image", bg);
            }
        }
        occureMore = false;
        textToWiki("tab-" + (pageIndex - 1));
        addGAEvent("MoreData", EntityCode + ":" + Content, pageIndex);

    });
}

$(window).load(function () {
    textToWiki("DMainFeedItemContent");
});

function textToWiki(mianContentId) {
    var tags;
    try {
        if (localStorage["lastTagsUpdate"] != null)
            tags = localStorage['tagsList'];
    }
    catch (e) { }
    if (tags == null) {
        $.ajax({
            type: "GET",
            url: "/Home/tags",
            cache: true,
            async: false,
            success: function (response) {
                tags = response;
                try {
                    localStorage['tagsList'] = response;
                    localStorage["lastTagsUpdate"] = (new Date()).getFullYear() + '/' + (new Date().getMonth() + 1 < 10 ? '0' + ((new Date()).getMonth() + 1) : (new Date()).getMonth() + 1) + '/' + (new Date()).getDate();
                }
                catch (e) { }
            }
        });
    }
    tags = tags.split(':');
    var items = $("#" + mianContentId + " .ItemContent");
    for (var j = 0; j < items.length; j++) {
        var res = $(items[j]).html();
        for (var i = 0; i < tags.length; i++)
            res = res.replace(" " + tags[i], "<a href='/tag/" + encodeURI(tags[i]) + "'>" + tags[i] + "</a>")
        $(items[j]).html(res);
    }
}
