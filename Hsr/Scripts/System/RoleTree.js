
//点击选择角色   获取角色tree
$('#rolemenu').click(function () {

    $.ajax({
        type: 'Get',
        url: "/Sysrole/GetTree",
        cache: false,
        success: function (data) {
            if (data == null || data == '') {
            } else {

                $('#roletree').html(data);
                Inital();
                treeCheck();
            }
        },
        error: function (myErrorData) {
            Messenger.options = {
                extraClasses: 'messenger-fixed messenger-on-top',
                theme: 'flat'
            },
            Messenger().post({
                message: myErrorData,
                type: 'error',
                showCloseButton: true
            });
        },
        beforeSend: function () {
            ShowLoding();

        },
        complete: function () {
            HideLoding();
        }
    }); 
});


//角色tree初始化
function Inital() {
        $('#jstree').bind("loaded.jstree", function (e, data) {
            data.instance.open_all(0);
        }).jstree({
        "core": {
            "themes": {
                "variant": "large"
            }
        },
        "checkbox": {
            //"three_state": false,
            "keep_selected_style": false,
            "tie_selection": false
        },
        "plugins": ["wholerow", "checkbox"],
    });

        $('#roletree').dialog({
        defaults: true
    });
}


//关闭菜单获取被选中的值
$("#roleClose").click(function () {
        var menus = $("#jstree").jstree("get_checked", true);
        var roleIds = [];
        var roleNames = [];
        $.each(menus, function (i, n) {
            roleIds.push(n.id);
            roleNames.push(n.text);
        });

        $('#RoleIds').val(roleIds);
        $('#RoleNames').val(roleNames);
        $('#roletext').val(roleNames);
    });
    

//页面加载获取被选中角色名称
$(document).ready(function () {
    var roleNames = $('#RoleNames').val();
    $('#roletext').val(roleNames);
});


//角色tree附默认值
function treeCheck() {
    var ids =
     $('#RoleIds').val().split(",");
    $("#jstree").jstree(true).check_node(ids);
}