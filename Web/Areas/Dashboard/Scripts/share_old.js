
function AjaxSuccess(data) {
    ResetLoading();
    if (data.Status) {
        notification.show("اطلاعات با موفقیت ذخیره شد");
    }
    else {
        notification.error("عملیات با اشکال برخورد کرد." + data.Message);
    }
}
function AjaxFailure(data) {
    ResetLoading();
    notification.error("oh no!\nan unwanted error occurred " + data.statusText);
}
function AjaxBegin() {
    SetLoading();
}

function SetLoading(element) {
    $(".box").append("<div id='div-loading' class='overlay'><i class='fa fa-refresh fa-spin'></i></div>");
}

function ResetLoading() {
    $("#div-loading").remove();
}


$(document).ready(function () {
    $("#content").css("min-height", $(document).height() - 100);
    $(".content-wrapper").css("min-height", $(document).height() - 100);

    location.hrefBase = location.href.indexOf('?') > 0 ? location.href.slice(0, location.href.indexOf('?')) : location.href;
});

function formatter(value, length) {
    if (value == null || value == "" || value.length < length)
        return value;
    return value.substring(0, length) + "...";
}

function persianDateTime(date) {
    var dateStr = kendo.format("{0:yyyy/MM/dd HH:mm}", date);
    return "<div style='text-align: center;direction: ltr;'>" + dateStr + "</div>";
}
function persianDate(date) {
    var dateStr = kendo.format("{0:yyyy/MM/dd}", date);
    return "<div style='text-align: center;direction: ltr;'>" + dateStr + "</div>";
}

function renderMultiSelect(list) {
    var res = '';
    if (list == null)
        return res;

    for (i = 0; i < list.length; i++) {
        res += list[i].Title + '|';
    }
    return res;
}

function openModal(url) {
    $("#shareModal .modal-body").html("");
    $('#shareModal').modal();
    $.get(url, function (data) {
        $("#shareModal .modal-body").html(data);
    }).fail(function (ex) {
        alert(ex);
    });
}
function closeModal() {
    $('#shareModal').modal('toggle');
}

/*---default submit---*/

function keyPress(e, targetBtn) {

    if (e.keyCode == '13') {
        targetBtn = targetBtn[0] == "#" ? targetBtn : "#" + targetBtn;
        $(targetBtn).click();
    }

};