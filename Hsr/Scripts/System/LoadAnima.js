

//显示加载
function ShowLoding() {

    var random = Math.floor(Math.random() * 4);
    if (random == 0) {
        $.mobile.loading("show",
        {
            textVisible: true,
            html: "<div><div class='Loading1'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>客官别急，正在拼命加载..</div>"

        });
    }
    if (random == 1) {
        $.mobile.loading("show",
        {
            textVisible: true,
            html: "<div><div class='Loading2'><div class='dot1'></div><div class='dot2'></div></div></div>客官别急,正在拼命加载..</div>"

        });
    }
    if (random == 2) {
        $.mobile.loading("show",
       {
           textVisible: true,
           html: "<div><div class='Loading3'><div class='Loading3-container container1'><div class='circle1'></div><div class='circle2'></div><div class='circle3'></div><div class='circle4'></div></div><div class='Loading3-container container2'><div class='circle1'></div><div class='circle2'></div><div class='circle3'></div><div class='circle4'></div></div><div class='Loading3-container container3'><div class='circle1'></div><div class='circle2'></div><div class='circle3'></div><div class='circle4'></div></div></div>客官别急，正在拼命加载..</div>"
       });
    }
    if(random == 3) {
        $.mobile.loading("show", {
           textVisible: true,
           html: "<div><div class='Loading4'></div>客官别急,正在拼命加载..</div>"
        });
        
      }
    
}





//隐藏加载样式
function HideLoding() {
    $.mobile.loading("hide");
}