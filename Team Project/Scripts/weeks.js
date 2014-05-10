			//functionality for checking the weeks
			$(document).ready(function () {
    			$("#allWeeks").click(function () {
      				$(".all_weeks").prop('checked', $(this).prop('checked'));
					$("#defWeeks").attr('checked', false);
					$("#evenWeeks").attr('checked', false);
					$("#oddWeeks").attr('checked', false);
					$('.weeks:first').trigger('change');
    			});
    			
				$("#defWeeks").click(function () {
					$(".all_weeks").attr('checked', false);
	      			$(".default_weeks").prop('checked', $(this).prop('checked'));
					$("#allWeeks").attr('checked', false);
					$("#evenWeeks").attr('checked', false);
					$("#oddWeeks").attr('checked', false);
					$('.weeks:first').trigger('change');
    			});
			
				$("#oddWeeks").click(function () {
					$(".all_weeks").attr('checked', false);
					$(".odd_weeks").prop('checked', $(this).prop('checked'));
					$("#allWeeks").attr('checked', false);
					$("#defWeeks").attr('checked', false);
					$("#evenWeeks").attr('checked', false);
					$('.weeks:first').trigger('change');
    			});

				$("#evenWeeks").click(function () {
					$(".all_weeks").attr('checked', false);
      				$(".even_weeks").prop('checked', $(this).prop('checked'));
					$("#allWeeks").attr('checked', false);
					$("#defWeeks").attr('checked', false);
					$("#oddWeeks").attr('checked', false);
					$('.weeks:first').trigger('change');
    			});
		
				$("#clearWeeks").click(function () {
					$(".all_weeks").attr('checked', false);
					$("#allWeeks").attr('checked', false);
					$("#defWeeks").attr('checked', false);
					$("#oddWeeks").attr('checked', false);
					$("#evenWeeks").attr('checked', false);
					$('.weeks:first').trigger('change');
				});
			});