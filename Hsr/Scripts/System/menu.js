
var RyanTreeOption = {
    searchUri: "",
    delUri: "",
    addUri: "",
    addTempalteUri:"",
    treeId: "#jstree",
  
};

function HsrTree(opt) {
    var option= $.extend({},RyanTreeOption,opt);
    var _treeId = option.treeId;
     
    this.searchUri = option.searchUri;
    this.delUri = option.delUri;
    this.addTempalteUri = option.addTempalteUri;
    this.addUri = option.addUri;
    this.setTreeId = function (treeId) {
        _treeId = "#"+treeId;
    };
    
    this.searchResult = function () {
        var pid = $(_treeId).data("pid");
        var js = this;
        $.ajax({
            type: 'post',
            url: js.searchUri,
            data: {'pid':pid},
            cache: false,
            success: function (dataResponse) {
               
                if (dataResponse == null || dataResponse == '') {
                }
                else {
                    $('#divList').html(dataResponse);
                }
            },
            error: function (myErrorData) {
                alertError("查询错误");
            },
            beforeSend: function () {
                ShowLoding();
            },

            complete: function () {
                HideLoding();
            }
        });
    };
    this.initial = function () {
        var js = this, $tree = $(_treeId);
        $tree.jstree();
        $tree.data("pid", 0);
        $tree.on("changed.jstree", function (e, data) {
            $tree.data("pid", data.selected);
            $.jstree.reference(_treeId).open_node(data.node);
            js.searchResult();
        });
        $('#Create').click(function (e) {
            e.preventDefault();
            var pid = $tree.data("pid");
            window.location.href = js.addTempalteUri + "?pid=" + pid;
        });
    };
   
    this.deleteItem = function (id) {
        var delHref = this.delUri;
        var js = this;
       
        
       var confirm= function confirm(callback) {
             
            var pid = $(_treeId).data("pid");
            hsrAjaxLoad({
                type: 'post',
                url: delHref,
                data: { 'id': id, 'pid': pid },
                success: function (dataResponse) {
                    if (dataResponse != '0') {
                        alertSuccess("删除成功!");
                        
                        $('#divTree').html(dataResponse);
                        js.initial();
                        js.searchResult();
                        if (callback) {
                            callback();
                        }
                    }
                }
            })
           
       }
        confirm.isAsync = true;
        var deloption = {
            bodyContent: "是否删除？",
            confirmFun: confirm,
            isBig: false
        };
        ModalDialog(deloption);
        
    };
}




 
  




 


 