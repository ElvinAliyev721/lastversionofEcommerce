const sideshopping = document.querySelector('.customsidecart')
const closeiconforsideshopping=document.querySelector('.customsidecart .closesidecartitem .close_side_cart_icon')
const shoppingiconforopenside=document.querySelector(".nav-links .sideshoppingbar a");

const containerinbody=document.querySelector("body .container");
const navinbody=document.querySelector("body nav");
const sliderinbody=document.querySelector("body .slider");
const footerinbody=document.querySelector("body footer");

const filterbtn=document.querySelector(".filterButtonForProducts");
const closeiteminfilter=document.querySelector(".closeiteminfilter");
const col_1_of_4=document.querySelector(".custom_filtred_products .products-layout .col-1-of-4")


//slider


const myslide = document.querySelectorAll('.myslide');




function myFunction(x) {
	if (x.matches) { // If media query matches
		closeiconforsideshopping.addEventListener('click',()=>{
			sideshopping.classList.add("d-none");
			navinbody.classList.remove("deactiveallbody");
			sliderinbody.classList.remove("deactiveallbody");
			containerinbody.classList.remove("deactiveallbody");
			footerinbody.classList.remove("deactiveallbody");
			document.body.classList.remove("deactivebodyscroll");
		})
		shoppingiconforopenside.addEventListener('click',()=>{
			sideshopping.classList.remove("d-none");
			navinbody.classList.add("deactiveallbody");
			sliderinbody.classList.add("deactiveallbody");
			containerinbody.classList.add("deactiveallbody");
			footerinbody.classList.add("deactiveallbody");
			document.body.classList.add("deactivebodyscroll")
		})

		filterbtn.addEventListener('click',function(){
			col_1_of_4.classList.remove("d-none")
			col_1_of_4.classList.add("customactiveclassforsidefilter")
			closeiteminfilter.classList.remove("d-none");
			closeiteminfilter.classList.add("customclassfortimesinsidefilter");
		})
		closeiteminfilter.addEventListener('click',()=>{
			col_1_of_4.classList.remove("customactiveclassforsidefilter")
		})
	} else {
		col_1_of_4.classList.remove("customactiveclassforsidefilter")
	}
}
if(col_1_of_4 != null){
	var x = window.matchMedia("(max-width: 1072px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes
}

if (myslide.length){
	let count = 1;
	function slidefun(n) {

		let i;
		for(i = 0;i<myslide.length;i++){
			myslide[i].style.display = "none";
		}
		if(n > myslide.length){
			count = 1;
		}
		if(n < 1){
			count = myslide.length;
		}
		myslide[count - 1].style.display = "block";
		// console.log(myslide);
	}


	let timer = setInterval(autoSlide, 8000);
	function autoSlide() {
		count += 1;
		slidefun(count);
	}
	function plusSlides(n) {
		count += n;
		slidefun(count);
		resetTimer();
	}
	function currentSlide(n) {
		count = n;
		slidefun(count);
		resetTimer();
	}
	function resetTimer() {
		clearInterval(timer);
		timer = setInterval(autoSlide, 8000);
	}
	slidefun(count);
}






//second page

const imgs = document.querySelectorAll('.img-select a');
if(imgs){
	const imgBtns = [...imgs];
	let imgId = 1;
	function slideImage(){
		const img_ShowCase_Img=document.querySelector('.img-showcase img:first-child')
		let displayWidth;
		if (img_ShowCase_Img!=null){
			displayWidth= img_ShowCase_Img.clientWidth;
		}

		let main_img_show_case=document.querySelector('.img-showcase');
		if (main_img_show_case!=null){
			main_img_show_case.style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
		}

	}

	imgBtns.forEach((imgItem) => {
		imgItem.addEventListener('click', (event) => {
			event.preventDefault();
			imgId = imgItem.dataset.id;
			slideImage();
		});
	});



	window.addEventListener('resize', slideImage);
}


//for colors

$(document).ready(function(){


    $(".shoes-colors span").click(function(){
		$(".shoes-colors span").removeClass("active");
		$(this).addClass("active");
		$("body").css("background",$(this).attr("data-color"));
	});
});



//first page


//side close icon
const sidebarforshopping=document.querySelector(".customsidecart");
const closeiconforsideshop=document.querySelector(".customsidecart .closesidecartitem .close_side_cart_icon");

closeiconforsideshop.addEventListener("click",()=>{
	sidebarforshopping.style.display="none"
})




//second page

// const shoes_color_span = document.querySelector(".shoes-colors span")
// 	shoes_color_span.addEventListener("click",function(){
// 		shoes_color_span.style.classList.remove("active");
// 	})




//login
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

	openLoginInfo();
	setTimeout(closeLoginInfo, 1000);
});

function openLoginInfo() {
	$(document).ready(function(){
		$('.b-form').css("opacity","0.01");
		$('.box-form').css("left","-37%");
		$('.box-info').css("right","-37%");
	});
}

function closeLoginInfo() {
	$(document).ready(function(){
		$('.b-form').css("opacity","1");
		$('.box-form').css("left","0px");
		$('.box-info').css("right","-5px");
	});
}

$(window).on('resize', function(){
	closeLoginInfo();
});


