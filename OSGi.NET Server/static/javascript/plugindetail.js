$(document).ready(function () {
     $('.nav').affix();
     $('a.page-scroll').bind('click', function(event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
     });


	$('#deleteConfirm').click(function(e) {
	    e.preventDefault();
	    var $link = $(this);
	    bootbox.confirm("请确认是否删除!", function (confirmation) {
	        confirmation && document.location.assign($link.attr('href'));
	    });        
	});
});
