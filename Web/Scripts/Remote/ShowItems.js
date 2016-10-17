$('#DTazeyab').load($("#ATazeyab").attr("title"), function (html) {

    $('#DTazeyab').html(html);

    setInterval(function () { tick() }, 5000);

});

function tick() {
    $('#ticker li:first').slideUp(function () { $(this).appendTo($('#ticker')).slideDown(); });
}

