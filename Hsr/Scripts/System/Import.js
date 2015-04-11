

//点击添加（表）
$('#Create').click(function () {
    window.location.href = "/Import/Create?id=" + 0;
});


//点击添加（列）
$('#Create').click(function () {
    window.location.href = "/Import/CreateColumn?id=" + 0;
});

//删除判断（删除表）
function Delete(id) {
    var r = confirm("是否删除！");
    if (r == true) {
        $.ajax({
            type: 'Post',
            url: '/Import/Delete',
            data: { 'id': id },
            cache: false,
            success: function () {
               
            },
            error: function () {
                Messenger.options = {
                    extraClasses: 'messenger-fixed messenger-on-top',
                    theme: 'flat'
                }
                Messenger().post({
                    message: "系统报错，请联系运维或开发人员!",
                    type: 'error',
                    showCloseButton: true
                });
            }
        });
    }
}


//删除判断（删除列）
function Delete(id) {
    var r = confirm("是否删除！");
    if (r == true) {
        $.ajax({
            type: 'Post',
            url: '/Import/DeleteColumn',
            data: { 'id': id },
            cache: false,
            success: function () {

            },
            error: function () {
                Messenger.options = {
                    extraClasses: 'messenger-fixed messenger-on-top',
                    theme: 'flat'
                }
                Messenger().post({
                    message: "系统报错，请联系运维或开发人员!",
                    type: 'error',
                    showCloseButton: true
                });
            }
        });
    }
}



//页面加载tree
//$(document).ready(function () {
//    eventBind();
//});

//通过ajax分页
//function eventBind() {
//    $('.pager a').click(function (event) {
//        event.preventDefault();
//        var pageNum = SerachHref(this.href, 'pagenumber');
//        $.ajax({
//            type: 'Post',
//            url: '/Import/Index',
//            data: { 'pagenumber': pageNum },
//            success: function (data) {
//                if (data == null || data == '') {
//                    $('#Datamapping').html("");

//                } else {

//                    $('#Datamapping').html(data);
//                    eventBind();
//                }
//            },
//            error: function (myErrorData) {
//                Messenger.options = {
//                    extraClasses: 'messenger-fixed messenger-on-top',
//                    theme: 'flat'
//                }
//                Messenger().post({
//                    message: myErrorData,
//                    type: 'error',
//                    showCloseButton: true
//                });
//            }, beforeSend: function () {
//                ShowLoding();

//            },
//            complete: function () {
//                HideLoding();
//            }
//        });
//    });
//}

//function SerachHref(url, name) {

//    var index = url.indexOf('?');
//    var str = url.substr(index, url.length);
//    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");

//    var r = str.substr(1).match(reg);

//    if (r != null)
//        return unescape(r[2]);
//    return null;

//}