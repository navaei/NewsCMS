
var tags = Alltags.split(":");
$(document).ready(function () {
    $("#ddrCats").change(function () {
        var cat = this.value;
        $("#ddrTags").empty();
        $("#ddrTags").append($('<option>', { value: "AllItems" })
                    .text("همه موارد"));
        for (var i = 0; i < tags.length; i++) {
            if (tags[i].indexOf(cat) > -1)
                $("#ddrTags").append($('<option>', { value: tags[i].split("|")[1].split("#")[1] })
                    .text(tags[i].split("|")[1].split("#")[0]));
        }
        changeScript();
    });

    $("#txtScriptBody").focus(function () {
        var $this = $(this);
        $this.select();
        $this.mouseup(function () {
            $this.unbind("mouseup");
            return false;
        });
    });

});

function changeScript() {
    var selectedTag = $("#ddrTags").find('option:selected').val().toLowerCase();
    var selectedCat = $("#ddrCats").find('option:selected').val().toLowerCase();
    var selectedTagTitle = $("#ddrTags").find('option:selected').text().toLowerCase();
    var selectedCatTitle = $("#ddrCats").find('option:selected').text().toLowerCase();
    var scriptBody;
    if (selectedTag == "AllItems" || selectedTag == "allitems") {
        $("#tazeyab_test").attr("src", baseLocation + "/cat/itemsremote/" + selectedCat);
        scriptBody = "<a href='" + baseLocation + "/cat/" + selectedCat + "'>" + selectedCatTitle + "</a><script type='text/javascript' src='" + baseLocation + "/toolbox/script/?src=cat/" + selectedCat + "'></script>";
    }
    else {
        $("#tazeyab_test").attr("src", baseLocation + "/tag/itemsremote/" + encodeURI(selectedTag));
        scriptBody = "<a href='" + baseLocation + "/tag/" + encodeURI(selectedTag) + "'>" + selectedTagTitle + "</a><script type='text/javascript' src='" + baseLocation + "/toolbox/script/?src=tag/" + encodeURI(selectedTag) + "'></script>";
    }
    $("#txtScriptBody").val(scriptBody);
}