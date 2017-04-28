$(document).ready(function () {

    $("table").addClass("table").addClass("table-hover");

    $("LoadMonitor").change(function () {
        alert();
        if ($(this).html() > 85) { $(this).addClass("alert"); }
    });

    
  
}); 