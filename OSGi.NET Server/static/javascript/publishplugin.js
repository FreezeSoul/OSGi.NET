$(document).ready(function () {
	$('#version').mask('099.0999.0999');
	$("#plugininterface").uniform({fileButtonHtml: '选择接口Zip',fileDefaultHtml: '未选择文件'});
    $("#pluginpackage").uniform({fileButtonHtml: '选择插件Zip',fileDefaultHtml: '未选择文件'});
	var queryString = window.location.search;
	queryString = queryString.substring(1);
	if(parseQueryString(queryString)["result"] == "success"){
		$(".alert-info").show().alert();
	}
	formValidation.init();
	$('#deleteConfirm').click(function(e) {
	    e.preventDefault();
	    var $link = $(this);
	    bootbox.confirm("请确认是否删除!", function (confirmation) {
	        confirmation && document.location.assign($link.attr('href'));
	    });        
	});
});

var formValidation = function () {

    	var handleValidation = function() {
        	// for more info visit the official plugin documentation: 
            // http://docs.jquery.com/Plugins/Validation
            var form1 = $('#pluginform');
            var error1 = $('.validation-error');
            var success1 = $('.validation-success');

            form1.validate({
                errorElement: 'span', //default input error message container
                errorClass: 'help-inline', // default input error message class
                rules: {
                    version: {
                        required: true
                    },
                    traceinfo: {
                        required: true
                    },
                    pluginpackage: {
                        required: true
                    }
                },
                messages: {
		            version: "请输入版本信息",
		            traceinfo: "请输入更新说明",
		            pluginpackage: "选择插件Zip"
		        },

                invalidHandler: function (event, validator) { //display error alert on form submit              
                    $(".alert-info").hide();
                    success1.hide();
                    error1.show();
                    formValidation.scrollTo(error1, -200);
                },

                highlight: function (element) { // hightlight error inputs
                    $(element).closest('.help-inline').removeClass('ok'); // display OK icon
                    $(element).closest('.control-group').removeClass('success').addClass('error'); // set error class to the control group
                },

                unhighlight: function (element) { // revert the change done by hightlight
                    $(element).closest('.control-group').removeClass('error'); // set error class to the control group
                },

                success: function (label) {
                    label.addClass('valid').addClass('help-inline ok') // mark the current input as valid and display OK icon
                    .closest('.control-group').removeClass('error').addClass('success'); // set success class to the control group
                },

                submitHandler: function (form) {
                    form.submit();
                }
            });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleValidation();
        },

		// wrapper function to scroll to an element
        scrollTo: function (el, offeset) {
            pos = el ? el.offset().top : 0;
            jQuery('html,body').animate({
                    scrollTop: pos + (offeset ? offeset : 0)
                }, 'slow');
        }

    };
}();
