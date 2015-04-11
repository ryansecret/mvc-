

 
$(function(){
    var eq = "";
    var value = sessionStorage.getItem("class");
    if (value != null && value != "") {

        var newValue = value.split(",");
        for (var i = 0; i < newValue.length - 1; i++) {
            $(".ui-state-persist ul li a").eq(newValue[i]).attr("class", "ui-btn-active ui-link ui-btn ui-icon-grid ui-btn-icon-left");
        }
    }
    $(".ui-state-persist ul li a").click(function() {
        $(this).attr("class", "ui-btn-active ui-link ui-btn ui-icon-grid ui-btn-icon-left");
        if ($(this).attr("class") == "ui-btn-active ui-link ui-btn ui-icon-grid ui-btn-icon-left") {
            var index = $(this).parent("li").parent("ul").find("a").index($(this));
        }
        eq += index + ",";
        sessionStorage.setItem("class", eq);
    });
});


 


 
