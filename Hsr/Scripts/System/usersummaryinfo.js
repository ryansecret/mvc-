

function dropListInitial(option) {
    hsrAjaxLoad({
        url: "/Sysrole/GetTree",
        success: function (data) {
            var $roleTree = $('#roleTree');
            $roleTree.replaceWith($(data).attr("id", "roleTree")[0]);
            console.log("获取数据");
           
            $('#roleTree:last').bind("loaded.jstree", function (e, d) {
                d.instance.open_all(0);
                console.log("roleTree加载");
            }).jstree({
                "checkbox": {
                    "keep_selected_style": false,
                    "tie_selection": false,
                },
                "plugins": ["wholerow", "checkbox"],
            });
            function setResult() {
                var result = $("#roleTree").jstree("get_checked", true);
                var ids = [], texts = [];
                if (result.length > 0) {
                    for (var i = 0; i < result.length; ++i) {
                        ids.push(result[i].id);
                        texts.push(result[i].text);
                    }
                }
                $(this).find(".dropResult").eq(0).text(texts.join(","));

                $("[name='RoleIds']").val(ids.join(","));
                $("[name='RoleNames']").val(texts.join(","));
            };
            if (option && option.roleIds) {
                $("#roleTree:last").jstree(true).check_node(option.roleIds.split(","));

                $.proxy(setResult, $("#roleDrop:last")[0])();
            }

            $('#roleDrop:last').on('hidden.bs.dropdown', function () {
                $.proxy(setResult, this)();
                $("form.form-horizontal:last").valid();

            });

        },
    });


    function dropList(setting) {

        hsrAjaxLoad({
            url: "/OrganizeInfo/GetOrgnization",
            data: setting.data,
            success: function (data) {

                var treeParm = {
                    'core': {
                        'data': data,
                        "multiple": false
                    },
                    "checkbox": {
                        "keep_selected_style": false,

                        "three_state": false,
                    },
                    "plugins": ["wholerow", "checkbox"],
                };

                var setResult = function () {
                    var $innerTree = $(setting.treeId);
                    var result = $innerTree.jstree("get_checked", true);
                    var ids = [], texts = [];
                    if (result.length > 0) {

                        for (var i = 0; i < result.length; ++i) {
                            ids.push(result[i].id);
                            texts.push(result[i].text);
                        }
                        if ($innerTree.data("ids")) {
                            if ($innerTree.data("ids").toString() == ids) {
                                return;
                            }
                        }

                        if (setting.callback && $.isFunction(setting.callback)) {
                            setting.callback({ "ids": ids[0], "isEdit": setting.isEdit });
                        }
                    }
                    $innerTree.data("ids", ids);
                    $(setting.dropId).find(".dropResult").eq(0).text(texts.join(","));
                    var $input = $(setting.dropId);

                    $input.siblings("[id$='Id']").val(ids.join(","));
                    $input.siblings("[id$='Name']").val(texts.join(","));

                };
                var $tree = $(setting.treeId).jstree(true);
                if ($tree) {
                    $(setting.treeId).jstree(true).destroy();
                }
                $(setting.treeId).bind("loaded.jstree", function (e, d) {
                    d.instance.open_all(0);
                    if (setting.isEdit) {
                        if (setting.callback) {
                            $(setting.treeId).jstree(true).check_node(option.compIds);

                        } else {
                            $(setting.treeId).jstree(true).check_node(option.deptIds);

                        }
                        setResult()
                        setting.isEdit = false;
                    } else {
                        setResult()
                    }
                }).jstree(treeParm);

                $(setting.dropId).on('hidden.bs.dropdown', function () {
                    setResult();
                    $("form.form-horizontal").valid();
                });

            },
        });
    }

    var settings = $.extend({}, {
        treeId: "#compTree",
        dropId: "#CompDrop",
        callback: function (d) {
            dropList({
                treeId: "#deptTree",
                dropId: "#DeptDrop",
                data: { "orgPid": d.ids },
                isEdit: d.isEdit
            })
        }
    }, option);
    dropList(settings);
}

function operationIntial(option) {
    if ((option && !option.isView) || !option) {
        
        $("#photo").click(
            function () {
           var $image = $("#imageInput");
           $image.click();}
        );
        $("#imageInput").imageOnselect();
    }

    dropListInitial(option);
}
function GetDataList(option) {

    var param = $("#collapseTwo form").serialize();
    var conditions = "?" + param;
    var org = $('#jstree').data("org");

    if (org && option && !option.ignoreOrg) {
        conditions += "&pid=" + org.id;
    }
    if (option && option.pageNum) {
        conditions += "&pagenumber=" + option.pageNum;
    } else {
        var page = $('#divList').data("currentPage");
        if (page) {
            conditions += "&pagenumber=" +page ;
        }
    }
    hsrAjaxLoad({
        url: '/UserSummaryInfo/GetList' + conditions,
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


$(function () {
    $('#divList').on("click", ".pager a", function (event) {
        event.preventDefault();
        var pageNum = SerachHref(this.href, 'pagenumber');
        $('#divList').data("currentPage",pageNum);
        GetDataList({
            "pageNum": pageNum
        });
    });
    
    $('#divList').on("click", "table a:contains('编辑')", function (e) {
        e.preventDefault();
        try {
            
        var that = $(this);
        var index = this.href.lastIndexOf("/");
        var commitUrl = this.href.substring(0, index);
        var compIds = that.attr("compIds"), deptIds = that.attr("deptIds"), roleIds = that.attr("roleIds");
         
        //that.ModalDialog({ cache: true, isLoading: true });
        EditData({
            isEdit: false,
            url: this.href,
            confirmUrl: commitUrl,
            callback: function () {
                operationIntial({
                    isEdit: true,
                    compIds: compIds,
                    deptIds: deptIds,
                    roleIds: roleIds
                });
                var filename = $("#Detail_Photo").val();
                if (filename) {
                    
                    $.fn.SingleDownLoad({
                        url: "/Common/DownLoad",
                        filename: filename,
                        imageContainerId: "photo",
                         
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
        var compIds = that.attr("compIds"), deptIds = that.attr("deptIds"), roleIds = that.attr("roleIds");
        that.ModalDialog({ title: "查看", cache: true, isLoading: true });
        EditData({
           
            isEdit: false,
            url: this.href,
            callback: function () {
                operationIntial({
                    isEdit: true,
                    isView:true,
                    compIds: compIds,
                    deptIds: deptIds,
                    roleIds: roleIds
                });
                var filename = $("#Detail_Photo").val();
                if (filename) {
                    LoadImage({ url: "/Common/DownLoad",filename:filename, imageContainerId: "photo" });
                }
                $("form[action*='Edit'] input").each(
                    function () {
                        $(this).attr("disabled", "disabled");
                    }
                );
                $("form[action*='Edit'] button").each(
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
    hsrAjaxLoad({
        url: "/OrganizeInfo/GetOrgTree",
        success: function (data) {
            $('#jstree').jstree({
                'core': {
                    'data': data
                }
            });

            $('#jstree').on("changed.jstree", function (e, d) {
                $(this).data("org", { "id": d.selected, "name": d.node.text });
                GetDataList({});
            });
        }
    });
    $('#SearchContains').click(function () {
        GetDataList({ ignoreOrg: true });
    });
   
    
    $('#Create').click(function (e) {
        e.preventDefault();
        try {
            var data = $('#jstree').data("org");

            //$(this).ModalDialog({ cache: true, isLoading: true });
            var that = $(this);
            var url = "/UserSummaryInfo/Create";
            if (data) {
                url = "/UserSummaryInfo/Create?selectedid=" + data.id + "&selectname=" + data.name;
            }

            EditData({
                isEdit: false,
                url: url,
                confirmUrl: url,
                preAction: function () {
                    var pwd = $('#UserPw').val();
                    if (pwd) {
                        var convertData = $.md5(pwd);
                        $('#UserPw').val(convertData);
                        $("#UserPwCompare").val(convertData);
                    }
                },
                callback: operationIntial,
                button: that
            });
        } catch(e) {
            alert(e.message);
        } 
      
    });
 
});

function Delete(id) {

    var confirm = function (callback) {

        hsrAjaxLoad({
            type: 'post',
            url: '/UserSummaryInfo/Delete',
            data: { 'id': id },
            success: function(dataResponse) {
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
        isBig:false
    };
    ModalDialog(option);

}