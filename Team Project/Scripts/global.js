$(document).ready(function() {
	$('#increaseFont').click(function() {
		$('body').css('zoom', parseFloat($('body').css('zoom'))+0.1);
	});
	
	$('#decreaseFont').click(function() {
		$('body').css('zoom', parseFloat($('body').css('zoom'))-0.1);
	});
});