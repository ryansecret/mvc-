
var HsrSelectMenuOption = {
    searchUri: "",
    fromElementId:"",
    destinyElmentId:""
};

function HsrSelectMenu(opt) {
    var option = $.extend({}, HsrSelectMenuOption, opt);
    $("#"+option.fromElementId).change(function () {
        $.ajax({
            type: 'Get',
            url: option.searchUri,
            data: {
                'id': $(this).val()
            },
            dataType: 'json',
            cache: false,
            success: function (dataResponse) {
                var data = { areas: [] };
                data.areas = dataResponse;
                var templateElement = document.getElementById("template"), template = templateElement.innerHTML,
                compiledTemplate = Handlebars.compile(template), options=compiledTemplate(data);
                
                $('#' + option.destinyElmentId).html(options).change();
               
            },
            error: function (myErrorData) {
                
                alertError(myErrorData.message);
            },
            beforeSend: function () {
                ShowLoding();
            },
            complete: function () {
                HideLoding();
            }
        });
    });
}

 
function HsrSelectedMenu(searchUri, destinyElmentId) {
    this.searchUri = searchUri,
    this.destinyElmentId = destinyElmentId;
};
HsrSelectedMenu.prototype.generate = function (pid) {
    var that = this;
    $.ajax({
        type: 'Get',
        url: this.searchUri,
        data: {
            'id': pid
        },
        dataType: 'json',
        cache: false,
        success: function(dataResponse) {
            var data = { areas: [] };
            data.areas = dataResponse;
            var templateElement = document.getElementById("template"), template = templateElement.innerHTML,
                compiledTemplate = Handlebars.compile(template), options = compiledTemplate(data);

            $('#' + that.destinyElmentId).html(options).change();

        },
        error: function(myErrorData) {
            alertError(myErrorData.message);
             
        },
        beforeSend: function () {
            ShowLoding();

        },
        complete: function () {
            HideLoding();
        }
    });
};

$.fn.selectMenu = function (opt) {
    
    var $this = $(this);
    var replica = new HsrSelectedMenu(opt.searchUri, opt.destinyElmentId);
    $this.change(function () {
        replica.generate($this.val());
    });
};
 


 
 

 