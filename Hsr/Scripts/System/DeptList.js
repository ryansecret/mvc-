

$('#Dept').click(function () {
    var orgPid = $('#detail_CompId').val();
    $.ajax({
        type: 'Get',
        url: "/OrganizeInfo/Judge",
        cache: false,
        data: { orgPid: orgPid },
        success: function (data) {
            if (data == null || data == '')
            {
            } else {
                $('#DeptpopupDialog').replaceWith("<div id='DeptpopupDialog'>"+data+"</div>");
                DeptCheck();
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

function DeptCheck() {
    var deptname = $('#detail_DeptName').val();
    
   var selects = document.getElementsByName("radio-choice-v-1");
   for (var i = 0; i < selects.length; i++) {
       if (selects[i].value == deptname) {
           selects[i].checked = "checked";
           break;
       }
   }
}

//����رջ�ȡѡ��ֵ
$('#DeptClose').click(function () {
    var deptid = $('input:radio:checked').attr("id");
    var deptname = $('input:radio:checked').val();
    $('#detail_DeptId').val(deptid);
    $('#detail_DeptName').val(deptname);
    $('#depttext').val(deptname);
});

//ҳ�����ʱ�����Ÿ�����
$(document).ready(function () {
    var compname = $('#detail_DeptName').val();
    $('#depttext').val(compname);
});