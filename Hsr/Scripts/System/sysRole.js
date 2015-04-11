 
$(document).ready(function() {
    EventBind(0);
});


var deleteUrl = "";
function SeerchResult() {
    var rolePid = $("#selectedId").val();
    if ($('#selectedId').val() == null) {
        Messenger.options = {
            extraClasses: 'messenger-fixed messenger-on-top',
            theme: 'flat'
        },
        Messenger().post({
            message: '请选择组织机构',
            type: 'error',
            showCloseButton: true
        });
        return false;
    }
    $.ajax({
        type: 'Get',
        url: searchUri,
        data: {
            'rolePid': rolePid
        },
        success: function (dataResponse) {

            if (dataResponse == null || dataResponse == '') {
                $('#divRoleList').html("");
            }
            else {
                $('#divRoleList').html(dataResponse);
                EventBind(rolePid);
            }
        },
        error: function (myErrorData) {
           
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
        },
    });
}


function EventBind(rolePid) {
    $("#pager a").click(function(event) {
        event.preventDefault();
        var pageNum = SerachHref(this.href, 'pagenumber');
        $.ajax({
            type: 'Get',
            url: '/Sysrole/GetList?rolePid=' + rolePid + '&pagenumber=' + pageNum,
            success: function(data) {
                if (data == null || data == '') {
                    $('#divRoleList').html('');
                }
                else {
                    $('#divRoleList').html(data);
                    EventBind(rolePid);
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
            },
        });
    });
}

function SerachHref(url, name) {                     //  获取Url里的参数

    var index = url.indexOf('?');
    var str = url.substr(index, url.length);
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = str.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}



var searchUri = "", delUri, addUri;

function InitialUri(search, del, add) {
    searchUri = search;
    delUri = del;
    addUri = add;
}

function Inital() {
    
    $('#jstree').jstree();
    $('#jstree').on("changed.jstree", function (e, data) {
          
        $('#selectedId').val(data.selected);
        $.jstree.reference('#jstree').open_node(data.node);

        if (data.node.parent != 0 && data.node.parent != "#") {
            $('#Create').hide();
        } else {
            $('#Create').show();
        }
        SeerchResult();
    });
    
    $('#Create').click(function (e) {
        e.preventDefault();
        var rolePid = $("#selectedId").val();
        window.location.href = addUri + "?rolePid=" + rolePid;
    });

     
    function getTreeChecked() {
        var menus = $("#jstree").jstree("get_checked", true);
        $.each(nodes, function (i, n) {
            var node = $(n);
        });
    }
}

function Delete(roleId) {
    var r = confirm("是否删除！");
    var rolePid = $("#selectedId").val();
    if (r == true) {
        $.ajax({
            type: 'Post',
            url: delUri + "?roleId=" + roleId + '&rolePid=' + rolePid,
            cache: false,
            success: function (dataResponse) {
                if (dataResponse != '0') {
                    window.location.href = "/Sysrole/Index?rolePid=" + rolePid;
                }
            },
            error: function (myErrorData) {
                Messenger.options = {
                    extraClasses: 'messenger-fixed messenger-on-top',
                    theme: 'flat'
                },
                Messenger().post({
                    message: "系统报错，请联系运维或开发人员!",
                    type: 'error',
                    showCloseButton: true
                });
            },
            beforeSend: function () {
                ShowLoding();

            },

            complete: function () {
                HideLoding();
            },
        });
    }
}

 