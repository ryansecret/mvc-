
//(function (factory) {
//    "use strict";
//    if (typeof define === 'function' && define.amd) {
//        define(['jquery'], factory);
//    }
//    else if(typeof exports === 'object') {
//        factory(require('jquery'));
//    }
//    else {
//        factory(jQuery);
//    }
//}(function ($) {

//    function uploadProgressDefault(evt) {
//        if (evt.lengthComputable) {
//            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
//            document.getElementById('progress').innerHTML = percentComplete.toString() + '%';
//        } else {
//            document.getElementById('progress').innerHTML = '未知';
//        }
//    }

//    function uploadCompleteDefault(evt) {
//        alert(evt.target.responseText);
//    }

//    function uploadFailedDefault(evt) {
//        alert("上传失败！");
//    }

//    function uploadCanceledDefault(evt) {

//    }

//    $.fn.SingleUpLoad = function (setting) {

//        var fd = new FormData($('#' + setting.formId)[0]), uploadProgress = setting.uploadProgress || uploadProgressDefault,
//            uploadComplete = setting.uploadComplete || uploadCompleteDefault, uploadFailed = setting.uploadFailed || uploadFailedDefault, uploadCanceled = setting.uploadCanceled || uploadCanceledDefault;
//        var fileName = this[0].files[0].name;
//        fd.append("fileToUpload", fileName);
//        var xhr = new XMLHttpRequest();
//        xhr.upload.addEventListener("progress", uploadProgress, false);
//        xhr.addEventListener("load", uploadComplete, false);
//        xhr.addEventListener("error", uploadFailed, false);
//        xhr.addEventListener("abort", uploadCanceled, false);
//        // xhr.open("POST", "Upload/UpLoadSingle.ashx");
//        xhr.open("POST", "Common/UpLoad");
//        xhr.send(fd);
//    };

//    $.fn.singleSelected = function () {

//        $(this).change(function () {
//            var file = this.files[0];
//            if (file) {
//                var fileSize = 0;
//                if (file.size > 1024 * 1024)
//                    fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
//                else
//                    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
//                var showText = "文件名称:" + file.name + ";大小：" + fileSize;
//                $('<div></div>').attr('id', "info").text(showText).insertAfter(this);
//                $('<div></div>').attr('id', "progress").insertAfter($('#info'));
//            }
//        });

//    };
//}))

//(function($) {

//    function uploadProgressDefault(evt) {
//        if (evt.lengthComputable) {
//            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
//            //$(function () {
//            //    var pro = $("#progress");   //进度条div
//            //    var proLabel = $(".progress-label"); //进度条里面文字

//            //    pro.progressbar({
//            //        value: percentComplete,   //初始化的值为0
//            //        change: function () {
//            //            //当value值改变时，同时修改label的字
//            //            proLabel.text(pro.progressbar("value") + "%");
//            //        },
//            //        complete: function () {
//            //            //当进度条完成时，显示complate
//            //            proLabel.text("100%");
//            //        }
//            //    });

//            //    //延迟500毫秒调用修改value的函数
//            //    setTimeout(addValue, 500);

//            //    //动态修改value的函数
//            //    function addValue() {
//            //        var pro = $("#progress");
//            //        var newValue = pro.progressbar("value") + 1;

//            //        pro.progressbar("value", newValue); //设置新值
//            //        if (newValue >= 100) { return; }    //超过100时，返回

//            //        setTimeout(addValue, 500); //延迟500毫秒递归调用自己
//            //    }
//            //});
//            document.getElementById('progress').innerHTML = percentComplete.toString() + '%';
//        } else {
//            document.getElementById('progress').innerHTML = '未知';
//        }
//    }

//    function uploadCompleteDefault(evt) {
//        alert(evt.target.responseText);
//    }

//    function uploadFailedDefault(evt) {
//        alert("上传失败！");
//    }

//    function uploadCanceledDefault(evt) {

//    }

//    $.fn.SingleUpLoad = function(setting) {

//        var fd = new FormData($('#' + setting.formId)[0]), uploadProgress = setting.uploadProgress || uploadProgressDefault,
//            uploadComplete = setting.uploadComplete || uploadCompleteDefault, uploadFailed = setting.uploadFailed || uploadFailedDefault, uploadCanceled = setting.uploadCanceled || uploadCanceledDefault;
//        //var fileName = this[0].files[0].name;
//        //fd.append("fileToUpload", this[0].files[0]);
//        var xhr = new XMLHttpRequest();
//        xhr.upload.addEventListener("progress", uploadProgress, false);
//        xhr.addEventListener("load", uploadComplete, false);
//        xhr.addEventListener("error", uploadFailed, false);
//        xhr.addEventListener("abort", uploadCanceled, false);
//        // xhr.open("POST", "Upload/UpLoadSingle.ashx");
//        xhr.open("POST", "Common/UpLoad");
//        xhr.send(fd);
//    };

//    $.fn.singleSelected = function() {
//        var $this = $(this);
//        $this.change(function () {
//            var file = this.files[0];
//            if (file) {
//                var fileSize = 0;
//                if (file.size > 1024 * 1024)
//                    fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
//                else
//                    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
//                var showText = "文件名称:" + file.name + ";大小：" + fileSize;
//                $this.siblings('div').remove();
//                var info= $('<div></div>').text(showText).insertAfter(this);
//                $('<div><div class="progress-label"></div></div>').attr('id', "progress").insertAfter(info);
                
//            }
//        });

//    };
//})(jQuery);

(function ($) {
    function uploadCompleteDefault(evt) {
        alert(evt.target.responseText);
    }

    function uploadFailedDefault(evt) {
       
        alertError("上传失败");
    }

    function uploadCanceledDefault(evt) {

    }

    function uploadProgressDefault(evt, progressBar) {
       
        if (progressBar&&evt.lengthComputable) {
               var percentComplete = Math.round(evt.loaded * 100 / evt.total);
                progressBar.children('.progress-bar').css("width", percentComplete + "%").text(percentComplete.toString());

            }  
    }
    var defaultSetting = {
        filer: [".jpg", ".jpeg", ".png"],
        imageId: "#photo",
        };
    $.fn.SingleUpLoad = function (setting) {
       
        var fd = new FormData($(this).parents("form")[0]), uploadProgress = setting.uploadProgress || uploadProgressDefault,
            uploadComplete = setting.uploadComplete || uploadCompleteDefault, uploadFailed = setting.uploadFailed || uploadFailedDefault, uploadCanceled = setting.uploadCanceled || uploadCanceledDefault;
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        
        xhr.open("POST", "/Common/UpLoad");
        xhr.send(fd);
    };

    $.fn.SingleDownLoad = function (option) {
        if (!option) {
            return;
        }
        var fd = new FormData();
        fd.append("fileName", option.filename);
        var progressBar = $("#"+option.imageContainerId).siblings(".progress");
        progressBar.fadeIn();
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", function (evt) {
            
            uploadProgressDefault(evt, progressBar);
        }, false);
        xhr.open("post", option.url, true);
        xhr.responseType = "blob";
        xhr.cache = true;
        xhr.onerror = function () {
            progressBar.fadeOut();
        };
        xhr.onload = function () {
            progressBar.fadeOut();
            if (this.status == 200) {
                var blob = this.response;
                var img = document.getElementById(option.imageContainerId);
                window.URL.revokeObjectURL(img.src);

                img.src = window.URL.createObjectURL(blob);

            }
        }
        xhr.send(fd);
    };


    $.fn.imageOnselect = function (options) {
        var $this = $(this);
        var option = $.extend({}, defaultSetting, options);
        
        var progressBar = $(option.imageId).siblings(".progress");
        var progress = function uploadProgress(evt) {
            if (evt.lengthComputable) {
                var percentComplete = Math.round(evt.loaded * 100 / evt.total);
                progressBar.children('.progress-bar').css("width", percentComplete+"%").text(percentComplete.toString());

            } else {

            }
        };
        var that = this;
        var complete = function(evt) {
            progressBar.fadeOut(1500);
            if (evt.target.responseText != "0") {
                if (options && options.inputImage) {
                    var filter = "[id*=" + options.inputImage + "]";
                    $("input"+filter).val(evt.target.responseText + "\\" + that[0].files[0].name);
                } else {
                    $("input[id*='Photo']").val(evt.target.responseText + "\\" + that[0].files[0].name);
                }
                alertSuccess("上传成功！");
            } else {
                alertError("上传失败！");
            }
        };

        $this.change(function () {
            var file = this.files[0];
            var extension = /\.[^\.]+/.exec(file.name);
            if (option.filer) {
                if (!option.filer.contains(extension)) {
                    alertError("图片格式错误");
                    return;
                }
            }
            if (file) {
                var fileSize = 0;
                if (file.size > 1024 * 1024)
                    fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                else
                    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
                var showText = "文件名称:" + file.name + ";大小：" + fileSize;
              
                var reader = new FileReader()
                reader.onload = function (e) {
                    $(option.imageId).attr("src", e.target.result);
                }
                reader.readAsDataURL(file);
                progressBar.fadeIn();
                $this.SingleUpLoad({ uploadProgress: progress, uploadComplete: complete });
            }
        });

    };
})(jQuery);
