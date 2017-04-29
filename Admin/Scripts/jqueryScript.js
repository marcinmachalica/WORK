$(document).ready(function () {

    $("table").addClass("table").addClass("table-hover");

    //$(".LoadMonitor").change(function () {
    //    alert("ok")
    //    //if ($(this).html().slice(0,3) > 85) { $(this).addClass("alert"); }
    //});

    $("body").click(function () {
        $(".LoadMonitor").addClass("alert");

    });

    //var timer = setInterval(ajax, 1000);

    //function ajax(){

    //    $.ajax({
    //        url: "@Url.Action('Details','Home')",
    //        type: 'POST',
    //        dataType: 'html',
    //        cache: false,
    //        success: function (html) {
    //            //show it on page
    //        }
    //    });
    //}
  
}); 