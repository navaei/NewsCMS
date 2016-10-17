
//$(document).ready(function () {
//    $(".flowTag").click(function () {

//    });
//    $(".flowSite").click(function () {

//    });
//});
var Toggle = 1;

function FlowMeX(sender, EntityCode, Content) {
    var res = FlowMe(sender, EntityCode.toLowerCase(), Content);
    if (res) {
        var item = $(sender).parent().detach();
        if (EntityCode.toLowerCase() == "cat") {
            $("#DMyCats").append(item);
        }
        else if (EntityCode.toLowerCase() == "tag") {
            $("#DMyTags").append(item);
        }
        else if (EntityCode.toLowerCase() == "site") {
            $("#DMySites").append(item);
        }
        item.show();
        addGAEvent("User", "FlowMeX", EntityCode + ":" + Content);
    }
}
function UnFlowX(sender, EntityCode, Content) {
    var res = UnFlow(sender, EntityCode.toLowerCase(), Content);
    if (res) {
        $(sender).prev().show();
        var item = $(sender).parent().detach();
        if (EntityCode.toLowerCase() == "cat") {
            $("#DRecommandCats").append(item);
        }
        else if (EntityCode.toLowerCase() == "tag") {
            $("#DRecommandTags").append(item);
        }
        else if (EntityCode.toLowerCase() == "site") {
            $("#DRecommandSites").append(item);
        }
        item.show();
        addGAEvent("User", "UnFlowX", EntityCode + ":" + Content);
    }
}


if ($.browser.msie && parseInt($.browser.version, 10) === 8) {
    $(".SiteIcon").attr("class", "SiteIconNull");
}
function loadCatItems(sender, CatCode) {
    setLoading("#DIndexMainContent");
    $('#DIndexMainContent').load('/Cat/Items/' + CatCode, function (html) {
        $('#DIndexMainContent').html(html);
        if (Toggle == 1) {
            if ($(".ItemToggle").length > 0) {
                $(".ItemToggle").toggle();
                var bg = $(".UpBtn").css("background-image");
                bg = bg.replace("up", "down");
                $(".UpBtn").css("background-image", bg);
            }
        }
        addGAEvent("User", "loadCatItems", CatCode);
    });
}
function loadTagItems(sender, TagContent) {
    setLoading("#DIndexMainContent");
    $('#DIndexMainContent').load('/Tag/Items/' + TagContent, function (html) {
        $('#DIndexMainContent').html(html);
        if (Toggle == 1) {
            if ($(".ItemToggle").length > 0) {
                $(".ItemToggle").toggle();
                var bg = $(".UpBtn").css("background-image");
                bg = bg.replace("up", "down");
                $(".UpBtn").css("background-image", bg);
            }
        }
        addGAEvent("User", "loadTagItems", TagContent);
    });
}
function loadSiteItems(sender, siteURL) {
    setLoading("#DIndexMainContent");
    $('#DIndexMainContent').load('/Site/Items/' + siteURL, function (html) {
        $('#DIndexMainContent').html(html);
        if (Toggle == 1) {
            if ($(".ItemToggle").length > 0) {
                $(".ItemToggle").toggle();
                var bg = $(".UpBtn").css("background-image");
                bg = bg.replace("up", "down");
                $(".UpBtn").css("background-image", bg);
            }
        }
        addGAEvent("User", "loadSiteItems", siteURL);
    });
}

function CatsUsersDeleting(sender, baseLocation, catcode) {
    $.ajax({
        url: baseLocation + '/User/CatsUsersDeleting?CatCode=' + catcode,
        type: 'GET',
        success: function (result) {
            if (result == true) {
                alert("دسته مورد نظر حذف شد");
                $(sender).parent().parent().hide("slow");
            }
        }
    });
}

$(document).ready(function () {

    //if ($(".DMenu").find("a").length > 0)
    //    $(".DMenu").find("a")[0].click();
});

function ToggleItem(sender) {

    $(sender).next().toggle('slow');
    var bg = $(sender).css("background-image");
    if (bg.indexOf("down") > 0)
        bg = bg.replace("down", "up");
    else
        bg = bg.replace("up", "down");
    $(sender).css("background-image", bg);

}