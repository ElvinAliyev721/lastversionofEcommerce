//openRegisterInfo

function openRegisterInfo() {
	$(document).ready(function(){
		$('.b-form').css("opacity","0.01");
		$('.box-form').css("left","-37%");
		$('.box-info').css("right","-37%");
	});
}
function closeRegisterInfo() {
	$(document).ready(function(){
		$('.b-form').css("opacity","1");
		$('.box-form').css("left","0px");
		$('.box-info').css("right","-5px");
	});
}

$(document).ready(function() {
	$("#do_login").click(function() {
		closeLoginInfo();
		$(this).parent().find('span').css("display","none");
		$(this).parent().find('span').removeClass("i-save");
		$(this).parent().find('span').removeClass("i-warning");
		$(this).parent().find('span').removeClass("i-close");

		var proceed = true;
		$("#login_form input").each(function(){

			if(!$.trim($(this).val())){
				$(this).parent().find('span').addClass("i-warning");
				$(this).parent().find('span').css("display","block");
				proceed = false;
			}
		});

		if(proceed) //everything looks good! proceed...
		{
			$(this).parent().find('span').addClass("i-save");
			$(this).parent().find('span').css("display","block");
		}
	});

	//reset previously results and hide all message on .keyup()
	$("#login_form input").keyup(function() {
		$(this).parent().find('span').css("display","none");
	});

	openRegisterInfo();
	setTimeout(closeRegisterInfo, 1000);
});





$(window).on('resize', function(){
	closeRegisterInfo();
});
