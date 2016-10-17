var EntityCode = "Home";
$(document).ready(function () {

    try {     
        $('.carousel').carousel({
            toggle: false,
        });

        $("* [data-toggle='tooltip']").tooltip({
            html: true,
            'selector': '',
            'placement': 'top',
            'container': 'body'
        });
    }
    catch (ex) { }

    //----tag ajax-----
    $("a.ajaxlink div").click(function () {
        loadPopUpItems($(this).parents('a'));
        return false;
    });
    $("a.ajaxlink").click(function () {
        loadPopUpItems(this);
        return false;
    });

    //--load more---
    $(".btn-moreItems").click(function () {
        loadMoreItems(this);
        return false;
    });

});

function ToggleItemHide() {
    $(".FeedDescSpan").hide();
}
function FullShow(sender) {
    $(sender).parent().animate({
        "marginTop": "-100px"
    }, 1500);
    $(sender).next().show();
    $(sender).hide();
}
function catClose(sender) {
    $(sender).parent().parent().parent().hide('slow');
}


function loadMoreItems(sender) {
    var pageIndex = parseInt($(sender).attr("data-pageIndex"));
    var pageSize = 12;
    var content = $(sender).attr("data-content");
    var type = $(sender).attr("data-type");
    var target = "/api/items/?content=" + content + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&type=" + type;
    $(sender).attr("disabled", "disabled");
    var panelBody = $(sender).closest(".panel-body");
    if ($(sender).html().length > 450)
        return;
    setLoading(panelBody);

    $.getJSON(target, function (data) {
        var items = [];
        jdata = data;
        $.each(data, function (key, val) {
            var bgUrl = 'http://www.google.com/s2/favicons?domain=' + val.SiteUrl;
            var newItem = "<div  id='" + key + "'>";
            newItem += "<div class='pull-right' style='background-image: url(" + bgUrl + ");width:18px;height:18px'></div>";
            newItem += "<div class='pull-right'>" + val.PersianDate.replace('[', '<').replace('[', '<').replace(']', '>').replace(']', '>') + "</div>";
            newItem += "<a target='_blank' class='ItemTracker' data-toggle='tooltip' title='<div class=title-panel><div class=panel-heading>" + val.SiteTitle + "</div><div class=panel-body>" + val.Description + "</div></div>' href='/site/" + val.SiteUrl + "/" + val.FeedItemId + "' >" + val.Title + "</a>";
            newItem += "</div>";
            items.push(newItem);
        });
        var btnMore = '<a data-content="' + content + '" data-pageindex="' + (pageIndex + 1) + '" data-type="' + type + '" onclick="loadMoreItems(this)" class="btn-moreItems btn btn-primary">مطالب بیشتر...</a>';
        $(panelBody).html(items.join("") + btnMore);

        //-----set tooltip----
        $(panelBody).find("a[data-toggle='tooltip']").tooltip({
            html: true,
            'selector': '',
            'placement': 'top',
            'container': 'body'
        });
    });
    addGAEvent("MoreData", type + ":" + content, pageIndex);
}

function loadPopUpItems(sender) {
    var pageSize = 15;
    var pageIndex = 0;
    if ($(sender).attr("data-pageIndex") != null)
        pageIndex = parseInt($(sender).attr("data-pageIndex"));
    var content = $(sender).attr("data-content");
    var type = "tag";
    if ($(sender).attr("data-type") != null)
        type = $(sender).attr("data-type");
    var target = "/api/items/?content=" + content + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&type=" + type;
    $(sender).attr("disabled", "disabled");
    //if ($(sender).html().length > 450)
    //    return;
    $("#shareIndexModal #indexModalTitle").html(content);
    setLoading("#shareIndexModal .modal-body");
    $('#shareIndexModal').modal({
        show: true
    });
    $.getJSON(encodeURI(target), function (data) {
        var items = [];
        jdata = data;
        $.each(data, function (key, val) {
            var item = "<div class='FeedItemSite DItem'> <a target='_blank' class='FeedTitle h2 ItemTracker' href='/site/" + val.SiteUrl + "/" + val.FeedItemId + "'>" + val.Title + "</a>";
            item += "<div class='ItemSiteTitle' style='background-image: url(http://www.google.com/s2/favicons?domain=" + val.SiteUrl + ");'>" + val.SiteTitle + "</div>";
            item += "<div class='ItemContent'>" + val.Description + "</div>";
            item += "</div>";
            items.push(item);
        });
        var btnMore = '<button type="button" style="width: 100%;position: relative;font-family: tahoma;font-size: 1em;" data-content="' + content + '" data-pageindex="' + (pageIndex + 1) + '" data-type="' + type + '" onclick="loadPopUpItems(this)" class="btn btn-primary btn-moreItems">مطالب بیشتر...</button>';
        $("#shareIndexModal .modal-body").html(items.join(""));
        $("#shareIndexModal #dMoreLoadItemsBtn").html(btnMore);

        $(window).trigger('resize');
    });

    addGAEvent("MoreData", type + ":" + content, pageIndex);
}