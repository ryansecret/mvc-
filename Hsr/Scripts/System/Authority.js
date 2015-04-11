

$(function () {
    $('#jstree').jstree().on("changed.jstree", function (e, data) {
        $("#jstree").data("roleId",data.node.id);
        if (data.node.id==0) {
            return;
        }
        $("div.panel-heading b").text(data.node.text);
        hsrAjaxLoad({
            type: 'post',
            url: "/Authority/GetMenuTree",
            data: { "id": data.node.id, "pid": data.node.parent },
            cache: false,
            success: function (d) {
                var tree = $("#menuTree").jstree(true);
                if (tree) {
                    tree.destroy();
                }
                if (d.id) {
                    var $menutree = $("#menuTree");
                    $menutree.on("loaded.jstree", function (ee, dd) {
                        dd.instance.settings.checkbox.cascade = "down";
                    }).jstree({
                        'core': {
                            'data': d,                           
                        },
                        "checkbox": {
                             
                            "keep_selected_style": true,
                            "tie_selection": false,
                            "ryan":true
                         },
                        "plugins": ["checkbox"],
                    });
                    $menutree.on("check_node.jstree", function (ee, dd) {
                        if (!dd.instance.ischecking) {
                            dd.instance.settings.checkbox.cascade = "";
                            dd.instance.ischecking = true;
                            dd.instance.check_node(dd.node.parents);
                            
                            dd.instance.settings.checkbox.cascade = "down";
                            dd.instance.ischecking = false;
                        }
                    })
                }
          
            }
        });

    });
     
    //保存  
    $('#saveAuthority').click(function (e) {
        e.preventDefault();
        var menus = $("#menuTree").jstree("get_checked", false);
        var roleId = $("#jstree").data("roleId");
        if (roleId != 0) {
            hsrAjaxLoad({
                type: "post",
                url:"/Authority/Create",
                data: { "menuids": menus, "roleId": roleId },
                success: function (data) {
                    if (data == 1) {
                        alertSuccess("保存成功！");
                    } else {
                        alertError("保存失败！");
                    }
                }
            });
        }
    });

    $("#clearAuthority").click(function (e) {
        e.preventDefault();
        $("#menuTree").jstree(true).uncheck_all();
        $('#saveAuthority').trigger("click");
    });
});
 