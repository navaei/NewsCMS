
location.hrefBase = location.href.indexOf('?') > 0 ? location.href.slice(0, location.href.indexOf('?')) : location.href;

function AjaxSuccess(data) {
    ResetLoading();
    if (data.Status) {
        notification.success("اطلاعات با موفقیت ذخیره شد");
        setTimeout(function () {
            closeModal();
            $("*[data-role='grid']").data("kendoGrid").dataSource.fetch()
        }, 1000);
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

    bindDates();
});

function formatter(value, length) {
    if (value == null || value == "" || value.length < length)
        return value;
    return value.substring(0, length) + "...";
}


var persianDateTime = function (date) {
    var jalali = "";
    if (date != null) {
        var dateStr = kendo.format("{0:yyyy/MM/dd}", date);
        dateStr = dateStr.replace("T", " ");
        var timeStr = kendo.format("{0:HH:mm}", date);
        jalali = JalaliDate.gregorianToJalali(dateStr.split('/')[0], dateStr.split('/')[1], dateStr.split('/')[2]).join('/') + " - " + timeStr;
    }
    return "<div class='k-persian-date'>" + jalali + "</div>";
}

var persianDate = function persianDate(date) {
    var jalali = "";
    if (date != null) {
        var dateStr = kendo.format("{0:yyyy/MM/dd}", date);
        dateStr = dateStr.replace("-", "/").replace("-", "/").replace("-", "/");
        jalali = JalaliDate.gregorianToJalali(dateStr.split('/')[0], dateStr.split('/')[1], dateStr.split('/')[2]).join('/');
    }
    return "<div class='k-persian-date'>" + jalali + "</div>";
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
        bindDates();
        $.validator.unobtrusive.parse($("form[data-ajax='true']"));
        $("#shareModal .modal-body").find("button[type='reset']").click(function () {
            closeModal();
        })
    }).fail(function (ex) {
        alert(ex);
    });
}

function openModalText(bodyHtml) {
    bodyHtml = "<div style='padding:10px;'>" + bodyHtml + "</div>";
    $("#shareModal .modal-body").html(bodyHtml);
    $('#shareModal').modal();
}

function closeModal() {

    if (!$('#shareModal').hasClass('in'))
        return;
    $('#shareModal').modal('toggle');
}

/*---default submit---*/

function keyPress(e, targetBtn) {

    if (e.keyCode == '13') {
        targetBtn = targetBtn[0] == "#" ? targetBtn : "#" + targetBtn;
        $(targetBtn).click();
    }

};

function bindDates() {
    if (typeof dateElements != "undefined" && dateElements != null && dateElements.length > 0)
        for (var i = 0; i < dateElements.length; i++) {
            bindDate(dateElements[i]);
        }
}

function bindDate(inputName) {
    $(document).mouseup(function (e) {
        var $container = $(".calendar-wrapper");
        var $container2 = $(".pdp-default");

        if (!$container.is(e.target) // if the target of the click isn't the container...
            && !$container2.is(e.target)
            && !$container2.is($(e.target).parent().parent())
            && !$container2.is($(e.target).parent().parent().parent())
            && $container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            $(".pdp-default").hide(300);
        }
    });


    var self = this;
    var dateFormat = "YYYY-MM-DD";
    console.log(inputName);
    if ($("#" + inputName).val() !== "") {
        var dates = $("#" + inputName).val().split(' ');
        var date = dates[0].split('/');
        var sdate = JalaliDate.gregorianToJalali(date[2], date[1], date[0]);
        $("#" + inputName + "-picker").val(sdate[0] + '-' + sdate[1] + '-' + sdate[2]);
    }
    var fromDateConfig = {
        formatDate: dateFormat,
        onSelect: function () {
            var date = $("#" + inputName + "-picker").val();
            var grgDate = JalaliDate.jalaliToGregorian(date.split('-')[0], date.split('-')[1], date.split('-')[2]);
            $("#" + inputName).val(grgDate.join('-'));
        }
    };

    var toDateConfig = {
        formatDate: dateFormat
    };
    $("#" + inputName + "-picker").persianDatepicker(fromDateConfig);
}


// Hashem:  check Text length for Grid and return value
function CheckLenText(value) {
    var text = "";
    if (value.length >= 250) {
        text = "<div>" + value.substring(0, 250) + " ...  <a href='' class='long-text'>نمایش ادامه</a></div>";

    }
    else {
        text = "<div>" + value + "</div>";
    }
    return text;
}
