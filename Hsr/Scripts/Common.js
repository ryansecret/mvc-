function hsrAjaxLoad(setting) {
    
    var defaultSetting = {
        type: 'Get'
    };
    setting = $.extend({},defaultSetting,setting);
    $.ajax({
        type: setting.type,
        url: setting.url,
        data: setting.data,
        cache: setting.cache|false,
        success: function (data) {
            if ($.isFunction(setting.success)) {
                setting.success(data);
            }
        },
        error: function (myErrorData) {
            HideLoding();
            alertError("获取数据失败");
        },
        beforeSend: function () {
            ShowLoding();

        },
        complete: function () {
            HideLoding();
        }
    });
}

function attachValidate(parameters) {
    $.validator.unobtrusive.parse(document);
    console.log("绑定验证");
}

function alertSuccess(message) {
     
    Messenger().post({
        message: message,
        type: 'success',
        showCloseButton: true
    });
}

function alertError(message) {
    Messenger().post({
        message: message,
        type: 'error',
        showCloseButton: true
    });
}

function EditData(option) {
     
    hsrAjaxLoad({
        url: option.url,
        success: function (response) {
            var confirm = function (callback) {
                if (option.confirmUrl) {
                    if ($("form.form-horizontal").valid()) {
                        if (option.preAction && $.isFunction(option.preAction)) {
                            option.preAction();
                        }
                        hsrAjaxLoad({
                            type: 'post',
                            url: option.confirmUrl,
                            data: $("form.form-horizontal").serialize(),
                            success: function (dataResponse) {
                                if (dataResponse != '0') {

                                    alertSuccess("操作成功！");
                                    GetDataList();
                                    if (callback) {
                                        callback();
                                    }
                                } else {
                                    alertError("操作失败！");
                                }
                            }
                        });

                    }
                } else {
                    if (callback) {
                        callback();
                    }
                }

            };
            confirm.isAsync = true;
            option.button.ModalDialog({
                cache: true,
                title: option.title,
                bodyContent: response,
                confirmFun: confirm,
                loadedIntial: function () {
                    if (option.callback && $.isFunction(option.callback)) {
                        option.callback();
                    }

                }
            });
            attachValidate();
          
        }
    }

     );
}

function SerachHref(url, name) {

    var index = url.indexOf('?');
    var str = url.substr(index, url.length);
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");

    var r = str.substr(1).match(reg);

    if (r != null)
        return unescape(r[2]);
    return null;

}

function LoadImage(option) {
    if (!option) {
        return;
    }
        var fd = new FormData();
        fd.append("fileName", option.filename);
        var xhr = new XMLHttpRequest();
        xhr.open("post", option.url, true);
        xhr.responseType = "blob";
        xhr.cache = true;
        xhr.onload = function () {
            if (this.status == 200) {
                var blob = this.response;
                var img = document.getElementById(option.imageContainerId);
                window.URL.revokeObjectURL(img.src);  
                 
                img.src = window.URL.createObjectURL(blob);
                
            }
        }
        xhr.send(fd);
//    hsrAjaxLoad({
//        type: "post",
//        url: option.url,
//        data: { "fileName": option.filename },
//        cache:true
//})
}


$.ajaxSetup({
    statusCode: {
        401: function () {
            window.location.href = window.location.href;
        }
    }
});
 