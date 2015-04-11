

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

    hsrAjaxLoad({
        url: "/OrganizeInfo/GetOrgTree",
        success: function (data) {
            $("#orgTree").on("loaded.jstree", function (e, d) {
                d.instance.open_node("0");
            }).on("changed.jstree", function (e, d) {
                $(this).data("org", {id:d.node.id,name:d.node.text});
                GetDataList();
            }).jstree({
               "core": {
                   "data":data
               }
            });
        }
    });
    
    $('#divList').on("click", ".pager a", function (event) {
        event.preventDefault();
        var pageNum = SerachHref(this.href, 'pagenumber');
        $('#divList').data("currentPage", pageNum);
        GetDataList({
            "pageNum": pageNum
        });
    });

    $('#CreateOrg').click(function (e) {
        e.preventDefault();
        create(false);
    });

    $('#CreateDep').click(function (e) {
        e.preventDefault();
        create(true);
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
                callback: function () {
                    operationIntial({
                        isEdit: true
                    });
                    var filename = $("#Logo").val();
                    if (filename) {
                        // LoadImage({ url: "/Common/DownLoad", filename: filename, imageContainerId: "photo" });
                        $.fn.SingleDownLoad({
                            url: "/Common/DownLoad",
                            filename: filename,
                            imageContainerId: "photo",
                            imageId:""
                        });
                    }
                },
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
                operationIntial({ 
                    
                    isView: true,
                    
                });
                var filename = $("#Logo").val();
                if (filename) {
                    LoadImage({ url: "/Common/DownLoad", filename: filename, imageContainerId: "photo" });
                }
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

function create(isDep) {
    try {
        var data = $('#orgTree').data("org");
        if (!data) {
            alertError("未选择父级公司或部门,请选择组织机构！");
            return;
        } else {
            if (isDep&&data.id==0) {
                alertError("未选择父级公司,请选择组织机构！");
                return;
            }
        }
        //$(this).ModalDialog({ cache: true, isLoading: true });

        var that = $(this);
        var url = "/OrganizeInfo/Create?isDept=" + isDep;
        if (data) {
            url += "&orgPid=" + data.id + "&orgPname=" + data.name;
        }
        EditData({
            isEdit: false,
            url: url,
            confirmUrl: url,

            callback: operationIntial,
            button: that
        });
        
    } catch (e) {
        alert(e.message);
    }
}


function operationIntial(option) {
    
    if ((option && !option.isView) || !option) {
        $("#photo").click(
            function () {
                var $image = $("#imageInput");
                $image.click();
            }
        );
        $("#imageInput").imageOnselect({ inputImage: "Logo" });
    }
    console.log("operationIntial 调用");
    $("#ProvinceId").change(function () {
        var $this = $(this);
        var province = $this.val();
        hsrAjaxLoad({
            url: '/Common/GetCitys?id='+province,
            success: function (d) {
                var items = "";
                for (var i = 0; i < d.length; ++i) {
                    items += "<option value ='" + d[i].id + "'>" + d[i].text + "</option>";
                }
                $("#CityId")[0].innerHTML = items;
                $("#CityId").trigger("change");
            }
        });
    });
    $("#CityId").change(function () {
        var $this = $(this);
        hsrAjaxLoad({
            url: '/Common/GetAreas?id=' + $this.val(),
            success: function (d) {
                var items = "";
                for (var i = 0; i < d.length; ++i) {
                    items += "<option value ='" + d[i].id + "'>" + d[i].text + "</option>";
                }
                $("#AreaId")[0].innerHTML = items;
            }
        });
    });
  
}


function GetDataList(option) {
    var param = {};
    if (option&&option.isArea) {
        var areaId = $("#divAreatree").data("areaId");
        if (areaId) {
            param.areaid = areaId;
        }
    } else {
        var org = $('#orgTree').data("org");
        if (org) {
            param.orgid = org.id;
        }
    }
    if (option && option.pageNum) {
        param.pageNum= option.pageNum;
    } else {
        var page = $('#divList').data("currentPage");
        if (page) {
            param.pageNum = option.pageNum;
        }
    }
    
    hsrAjaxLoad({
        url: '/OrganizeInfo/GetOrgList',
        type:"post",
        data:param,
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
 
 function Delete(orgId) { 
     var r = confirm("是否删除！");
     var orgPid = $("#selectedId").val();
     if (r == true) {
         $.ajax({
             type: 'Post',
             url: delUri + "?orgId=" + orgId + "&orgPid=" + orgPid,
             cache: false,
             dataType:'json',
             success: function (dataResponse) {
                 if (dataResponse != 0) {
                      
                 }
             },
             error: function (myErrorData) {
                 alertError("系统报错，请联系运维或开发人员!");
             }
         });
     }
   
 }



 