//variables
const myslide = document.querySelectorAll('.myslide');



//for slide
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

