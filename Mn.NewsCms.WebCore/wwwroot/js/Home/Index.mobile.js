var EntityCode = "Home";
$(document).ready(function () {

    setTimeout(function () {
        $("#uTabsHeader").show();
        var tabs = $("#tabs").tabs().scrollabletab();
    }, 1000);

    setTimeout(function () { try { $('a').tooltip(); } catch (e) { } }, 1000);

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

function loadMoreItems(cati, PageIndex, target) {
    $(this).attr("disabled", "disabled");
    var divId = "#DMoreItems_" + cati + PageIndex;
    if ($(divId).html().length > 450) {
        console.log($(divId).html().length);
        return;
    }
    setLoadingHorizontal(divId);
    $(divId).load(encodeURI(target), function (html) {
        $(divId).html(html);
        $(divId + " a").tooltip();
        occureMore = false;
        PageIndex++;
        // addGAEvent("MoreData", EntityCode + ":" + Content, PageIndex);
    });
}


