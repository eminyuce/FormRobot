
$(document).ready(function () {
    $("#GetAirDropForm").click(function () {
        $("#airDropHtmlPage").html("<h3>WAIT A SECOND PLEASE<h3>");
        var airDropLink = $("#airDropLinkTxt").val();
        var postData = JSON.stringify({ "airDropLink": airDropLink });
        ajaxMethodCall(postData, "/Home/GetAirDropHtmlPage", function (data) {
            $("#airDropHtmlPage").html(data);
        });
    });
});


function ajaxMethodCall(postData, ajaxUrl, successFunction) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: successFunction,
        error: function (jqXHR, exception) {
            if (jqXHR.status === 0) {
                console.error('Not connect.\n Verify Network.');
            } else if (jqXHR.status == 404) {
                console.error('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                console.error('Internal Server Error [500].');
            } else if (exception === 'parsererror') {
                console.error('Requested JSON parse failed.');
            } else if (exception === 'timeout') {
                console.error('Time out error.');
            } else if (exception === 'abort') {
                console.error('Ajax request aborted.');
            } else {
                console.error('Uncaught Error.\n' + jqXHR.responseText);
            }
        },
        contentType: "application/json",
        dataType: "json"
    });
}