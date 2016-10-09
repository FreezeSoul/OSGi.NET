$(document).ready(function () {
   $('.editManifest').editable({
        type: 'textarea',
        url: '/updatemanifest', 
        mode: 'inline',
        send: 'always',
        placeholder: '请填写插件清单信息',
        params: function(params) {
            params.projectid = $(this).attr("projectid");
            params.pluginid = $(this).attr("pluginid");
            return params;
        },
        title: '请填写插件清单信息',
        success: function(response, newValue) {
            bootbox.alert("更新成功!");
        },
        error: function(response, newValue) {
            bootbox.alert("更新失败!");
        }
    });


   $('.deleteConfirm').click(function(e) {
        e.preventDefault();
        var $link = $(this);
        bootbox.confirm("请确认是否删除!<br/><br/>项目私有插件将会删除插件信息，共享插件将会移除引用关系！", function (confirmation) {
            confirmation && document.location.assign($link.attr('href'));
        });        
    });

   //移除成功
    var queryString = window.location.search;
    queryString = queryString.substring(1);
    if(parseQueryString(queryString)["result"] == "success"){
        $(".alert-info").show().alert();
    }
    
});
