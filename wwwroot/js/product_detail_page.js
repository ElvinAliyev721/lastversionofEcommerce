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
}


	window.addEventListener('resize', slideImage);

    $(document).ready(function(){


        $(".shoes-colors span").click(function(){
            $(".shoes-colors span").removeClass("active");
            $(this).addClass("active");
            $("body").css("background",$(this).attr("data-color"));
        });
    });


	// let cartObj={
	// 	price:null,
	// 	name:null,
	// 	count:null
	// }

	// cartObj.price=12;
	// localStorage.setItem('item',JSON.stringify(cartObj))

	// let getitem=localStorage.getItem("item")

	// console.log(JSON.parse(getitem));

	
