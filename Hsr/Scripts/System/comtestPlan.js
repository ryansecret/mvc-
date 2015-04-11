

$(function () {
    $('#divAreatree').on("loaded.jstree", function (e, d) {
        d.instance.open_node("0");
    }).on("changed.jstree", function (e, d) {
        $(this).data("areaId", d.selected);
        GetDataList({isArea:true});
    }).jstree({
        'core': {
            'data': {
                'url': function(node) {
                    return node.id === '#' ? '/OrganizeInfo/GetArea' : '/OrganizeInfo/GetArea?pid=' + node.id;
                },
                'data': function(node) {
                    return { 'id': node.id };
                }
            }
        }
    });

    $("#SearchContains").click(function (e) {
        e.preventDefault();
        GetDataList();
    });
    
    $('#divList').on("click", ".pager a", function (event) {
        event.preventDefault();
        var pageNum = SerachHref(this.href, 'pagenumber');
        $('#divList').data("currentPage", pageNum);
        GetDataList({
            "pageNum": pageNum
        });
    });

    $('#Create').click(function (e) {
        e.preventDefault();
        var that = $(this);
        EditData({
            isEdit: false,
            url: this.href,
            confirmUrl: this.href,
            callback: operationIntial,
            button: that
        });
         
    });
    $('#divList').on("click", "table a:contains('编辑')", function (e) {
        e.preventDefault();
        try {
            var that = $(this);
            var index = this.href.lastIndexOf("/");
            var commitUrl = this.href.substring(0, index);
            EditData({
                isEdit: false,
                url: this.href,
                confirmUrl: commitUrl,
                button: that
            });
        } catch (e) {
            alert(e.message);
            console.log(e.message);
        }

    });
    $('#divList').on("click", "table a:contains('查看')", function (e) {
        e.preventDefault();
        var that = $(this);
   
        that.ModalDialog({ title: "查看", cache: true, isLoading: true });
        EditData({

            isEdit: false,
            url:this.href,
            callback: function () {
                $("form[action*='Edit'] input").each(
                    function () {
                        $(this).attr("disabled", "disabled");
                    }
                );
                $("form[action*='Edit'] select").each(
                   function () {
                       $(this).attr("disabled", "disabled");
                   }
               );
                $("form[action*='Edit'] textarea").each(
                   function () {
                       $(this).attr("disabled", "disabled");
                   }
               );
            },
            button: that
        });
    });
});

 
function operationIntial(option) {
    
}


function GetDataList(option) {
    var param = $("#collapseTwo form").serialize();
    var conditions = "?" + param;
    
    var areaId = $("#divAreatree").data("areaId");
    if (areaId) {
        conditions +="&areaId="+areaId;
     }
    
    if (option && option.pageNum) {
        conditions += "&pagenumber=" + option.pageNum;
    } else {
        var page = $('#divList').data("currentPage");
        if (page) {
            conditions += "&pagenumber=" + page;
        }
    }
   
    hsrAjaxLoad({
        url: '/CommTestPlan/GetList'+conditions,
        type:"post",
        success: function (dataResponse) {
            if (dataResponse == null || dataResponse == '') {
                $('#divList').html("");
            } else {
                $('#divList').html(dataResponse);
                if (option&&option.callback && $.isFunction(option.callback)) {
                    option.callback();
                }
            }
        }
    });
}
 
 function Delete(id) { 
     var confirm = function (callback) {
         hsrAjaxLoad({
             type: 'post',
             url: '/CommTestPlan/Delete',
             data: { 'id': id },
             success: function (dataResponse) {
                 if (dataResponse != '0') {
                     alertSuccess("删除成功！");

                     GetDataList();
                     if (callback) {
                         callback();
                     }
                 }
             }
         });
     };
     confirm.isAsync = true;
     var option = {
         bodyContent: "是否删除？",
         confirmFun: confirm,
         isBig: false
     };
     ModalDialog(option);
   
 }



 