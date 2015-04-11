 
function GetData() {
    $('#divjstree').jstree({
        'core': {
            'data': {
                'url': function (node) {
                    return node.id === '#' ? '/OrganizeInfo/GetArea' : '/OrganizeInfo/GetArea?pid=' + node.id;
                },
                'data': function (node) {

                    return { 'id': node.id };
                },
                beforeSend: function () {
                    ShowLoding();
                },
                complete: function () {
                    HideLoding();
                }
            }
        }
    });

    $('#divjstree').on('loaded.jstree', function () {

        $.jstree.reference('#divjstree').open_node("0");

    });

}


function list() {

    $('#jstree').jstree();

    $('#divjstree').on("changed.jstree", function (e, data) {
        $('#areaid').val(data.node.id);
        var areaid = $('#areaid').val();

        $.ajax({
            type: 'Post',
            url: '/OrganizeInfo/index',
            data: {
                'areaid': areaid,
            },
            beforeSend: function () {
                ShowLoding();
            },
            complete: function () {
                HideLoding();
            },
            cache: false,
            success: function (dataResponse) {

                if (dataResponse == null || dataResponse == '') {
                    $('#divOrganizeList').html("");
                } else {

                   
                    $('#divOrganizeList').html(dataResponse);
                    eventBind(0, areaid);
                }
                
            },
            error: function (myErrorData) {
                alert(myErrorData);

            },

                       
        });
    });
}


 
 