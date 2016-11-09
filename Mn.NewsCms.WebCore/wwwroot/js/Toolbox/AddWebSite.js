
var retSite;

function GetSite(siteUrl) {
    $.ajax({
        url: FullPath + '/toolbox/GetSite?url=' + siteUrl,
        type: 'GET',
        success: function (result) {
            console.log(result);
            if (result != null && result != "") {
                retSite = result;
                $("#lblAlert").html("این سایت قبلا ثبت شده است");
                $("#txtSiteId").val(retSite.id);
                $("#txtSiteUrl").val(retSite.url);
                $("#txtSiteTitle").val(retSite.title);
                $("#txtSiteDesc").val(retSite.desc);
                $("#dSiteTitle").show();
                if (retSite.desc != null && retSite.desc != "")
                    $("#dSiteDesc").show();

                $("#btnAddFeed").show();
                $("#btnCheckSite").hide();
            }
        }
    });
}

var retFeeds;
function GetFeeds(siteId) {
    $.ajax({
        url: FullPath + '/toolbox/GetFeeds?siteId=' + siteId,
        type: 'GET',
        success: function (result) {
            console.log(result);
            if (result != null && result != "") {
                retFeeds = result;
                if (retFeeds.length > 0) {
                    var feeds = "<h4>فیدهای موجود: </h4> <ul>";
                    for (var i = 0; i < retFeeds.length; i++) {
                        feeds += "<li>" + retFeeds[i].title + ":" + retFeeds[i].link + "</li>";
                    }
                    feeds += "</ul>";
                    $("#dFeeds").html(feeds);
                    $("#lblAlert").html("");
                } else {
                    $("#lblAlert").html(" هیچ فیدی برای این سایت ثبت نشده است ");
                }
            }           
        }
    });
}

var cats = null;
function GetCats() {
    if (cats == null) {
        $.ajax({
            url: FullPath + '/toolbox/GetCats?viewMode=12',
            type: 'GET',
            success: function (result) {
                cats = result;
                for (var i = 0; i < result.length; i++)
                    $("#ddrCats").append($("<option value='" + cats[i].code + "'>" + cats[i].title + "</option>"));
            }
        });
    }
    else {
        for (var i = 0; i < result.length; i++)
            $("#ddrCats").append($("<option value='" + cats[i].code + "'>" + cats[i].title + "</option>"));
    }
};