//点击选择公司  ajax获取公司tree
$('#Comp').click(function () {
        $.ajax({
            type: 'Get',
            url: "/OrganizeInfo/Judge",
            dataType: 'json',
            cache: false,
            success: function (data) {
                if (data == null || data == '') {
                } else {
                    CompInital(data);
                    ComptreeCheck();
                    $('#menuTree').on("check_node.jstree", function (e, data) {
                        var node = data.node.id;

                        var menus = $("#menuTree").jstree("get_checked", true);
                        $.each(menus, function (i, n) {
                            if (n.id != node) {
                                $("#menuTree").jstree(true).uncheck_node(n.id);
                            }
                        });

                    }); 
                }
            },
            error: function (myErrorData) {

                Messenger.options = {
                    extraClasses: 'messenger-fixed messenger-on-top',
                    theme: 'flat'
                }
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

//公司tree初始化
function CompInital(data) {
    $('#menuTree').jstree({
        "core": {
            'data': data,
            "themes": {
                "variant": "large"
            }
        },
        "checkbox": {
            "three_state": false,
            "keep_selected_style": false,
            "tie_selection": false
        },
        "plugins": ["wholerow", "checkbox"],
    });
    $('#roleDialog').dialog({
        defaults: true
    });
}


//公司tree默认被选中
function ComptreeCheck() {
    var compid =
        $('#detail_CompId').val();
    $('#menuTree').on('loaded.jstree', function () {
        $.jstree.reference('#menuTree').check_node(compid);
    });
}

//点击关闭获取选中值
$("#CompClose").click(function () {
    var menu = $("#menuTree").jstree("get_checked", true);
    $.each(menu, function (i, n) {
        $('#detail_CompName').val(n.text);
        $('#detail_CompId').val(n.id);
        $('#comptext').val(n.text);
    });
});


//页面加载时给公司附名称
$(document).ready(function () {
    var compname = $('#detail_CompName').val();
    $('#comptext').val(compname);
});
