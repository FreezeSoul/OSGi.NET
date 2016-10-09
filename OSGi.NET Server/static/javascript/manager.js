$(document).ready(function() {
	$("span.status:contains('在线')").each(function(index, domEle) {
		$(domEle).addClass("label-success");
	});
});