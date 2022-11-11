const sideshopping = document.querySelector('.customsidecart')
const closeiconforsideshopping=document.querySelector('.customsidecart .closesidecartitem .close_side_cart_icon')
const shoppingiconforopenside=document.querySelector(".nav-links .sideshoppingbar a");


const containerinbody=document.querySelector("body .container");
const navinbody=document.querySelector("body nav");
const sliderinbody=document.querySelector("body .slider");
const footerinbody=document.querySelector("body footer");


const search_box=document.querySelector(".search-box");
const search_text=document.querySelector(".search-text");
const search_icon_for_products=document.querySelector(".search_icon_for_products");

// const filterbtn=document.querySelector(".filterButtonForProducts");
// const closeiteminfilter=document.querySelector(".closeiteminfilter");
// const col_1_of_4=document.querySelector(".custom_filtred_products .products-layout .col-1-of-4")

// console.log("Salam")

// closeiconforsideshopping.addEventListener('click',()=>{
//     sideshopping.classList.add("d-none");
//     navinbody.classList.remove("deactiveallbody");
//     sliderinbody.classList.remove("deactiveallbody");
//     containerinbody.classList.remove("deactiveallbody");
//     footerinbody.classList.remove("deactiveallbody");
//     document.body.classList.remove("deactivebodyscroll");
// })
// shoppingiconforopenside.addEventListener('click',()=>{
//     sideshopping.classList.remove("d-none");
//     navinbody.classList.add("deactiveallbody");
//     sliderinbody.classList.add("deactiveallbody");
//     containerinbody.classList.add("deactiveallbody");
//     footerinbody.classList.add("deactiveallbody");
//     document.body.classList.add("deactivebodyscroll")
// })

// closeiconforsideshopping.addEventListener("click",()=>{
// 	sideshopping.style.display="none"
// })

//for experience

// function myFunction(x) {
// 	if (x.matches) { // If media query matches
//         if(closeiconforsideshopping){
//             closeiconforsideshopping.addEventListener('click',()=>{
//                 sideshopping.classList.add("d-none");
//                 navinbody.classList.remove("deactiveallbody");
//                 sliderinbody.classList.remove("deactiveallbody");
//                 containerinbody.classList.remove("deactiveallbody");
//                 footerinbody.classList.remove("deactiveallbody");
//                 document.body.classList.remove("deactivebodyscroll");
//             })
//         }
// 		if(shoppingiconforopenside){
//             shoppingiconforopenside.addEventListener('click',()=>{
//                 sideshopping.classList.remove("d-none");
//                 navinbody.classList.add("deactiveallbody");
//                 sliderinbody.classList.add("deactiveallbody");
//                 containerinbody.classList.add("deactiveallbody");
//                 footerinbody.classList.add("deactiveallbody");
//                 document.body.classList.add("deactivebodyscroll")
//             })
//         }
		

// 		if(filterbtn){
//             filterbtn.addEventListener('click',function(){
//                 col_1_of_4.classList.remove("d-none")
//                 col_1_of_4.classList.add("customactiveclassforsidefilter")
//                 closeiteminfilter.classList.remove("d-none");
//                 closeiteminfilter.classList.add("customclassfortimesinsidefilter");
//             })
//         }

// 		if(closeiteminfilter){
//             closeiteminfilter.addEventListener('click',()=>{
//                 col_1_of_4.classList.remove("customactiveclassforsidefilter")
//             })
//         }
// 	} else {
// 		console.log("Smile")
// 	}
// }
// var x = window.matchMedia("(max-width: 1072px)")
// myFunction(x) // Call listener function at run time
// x.addListener(myFunction) // Attach listener function on state changes 


if(closeiconforsideshopping){
    closeiconforsideshopping.addEventListener('click',()=>{
        sideshopping.classList.add("d-none");
        navinbody.classList.remove("deactiveallbody");
        if(sliderinbody){
            sliderinbody.classList.remove("deactiveallbody");
        }
        containerinbody.classList.remove("deactiveallbody");
        footerinbody.classList.remove("deactiveallbody");
        document.body.classList.remove("deactivebodyscroll");
    })
}
if(shoppingiconforopenside){
    shoppingiconforopenside.addEventListener('click',()=>{
        sideshopping.classList.remove("d-none");
        navinbody.classList.add("deactiveallbody");
        if(sliderinbody){
            sliderinbody.classList.add("deactiveallbody");
        }
        containerinbody.classList.add("deactiveallbody");
        footerinbody.classList.add("deactiveallbody");
        document.body.classList.add("deactivebodyscroll")
    })
}


// if(filterbtn){
//     filterbtn.addEventListener('click',function(){
//         col_1_of_4.classList.remove("d-none")
//         col_1_of_4.classList.add("customactiveclassforsidefilter")
//         closeiteminfilter.classList.remove("d-none");
//         closeiteminfilter.classList.add("customclassfortimesinsidefilter");
//     })
// }

// if(closeiteminfilter){
//     closeiteminfilter.addEventListener('click',()=>{
//         col_1_of_4.classList.remove("customactiveclassforsidefilter")
//     })
// }



// function appendchildtosidecart(){
//     let child=document.createElement("div");
//     child.className="childbigbody";
//     console.log(child);

// }
// appendchildtosidecart();


let array=[];
function add(company_name,Color,price,key="item"){
    
    let obj={
        name:company_name,
        Color:Color,
        price:price
    };
    array.push(obj)
    let value=array;
    localStorage.setItem(key,JSON.stringify(value))
}
// setInterval(()=>add("Ad","Red",63),2000)


const btns_basket_products=document.querySelectorAll(".shoppyfy_btns")
console.log(btns_basket_products);
let count_for_search=0;
// search_icon_for_products.addEventListener("click",()=>{
//     if (count_for_search===0){
//         search_text.classList.add("active_class_for_search_input");
//         btns_basket_products.forEach((e)=>{
//             e.style.display="none";
//         });
//         count_for_search++;
//     }else{
//         search_text.classList.remove("active_class_for_search_input");
//         btns_basket_products.forEach((e)=>{
//             e.style.display="inline";
//         });
//         count_for_search=0;
//     }
// })

const btn_show_localstorage=document.querySelector(".btn_show_localstorage")
const btn_delete_localstorage=document.querySelector(".btn_delete_localstorage")
function mysearchpart(x) {
    if (x.matches) { // If media query matches
        search_icon_for_products.addEventListener("click",()=>{
            if (count_for_search==0){
                count_for_search++;
                search_text.style.width="180px";
                btn_show_localstorage.style.display="none";
                btn_delete_localstorage.style.display="none";
            }else{
                search_text.style.width="0px";
                count_for_search=0;
                btn_show_localstorage.style.display="";
                btn_delete_localstorage.style.display="";
            }

        })
    } else {
        console.log("Low then 610px query")
        // if (count_for_search==0){
        //     count_for_search++;
        //     search_text.style.width="180px";
        // }else{
        //     search_text.style.width="0px";
        //     count_for_search=0;
        // }
    }
}
var x = window.matchMedia("(max-width: 610px)")
mysearchpart(x) // Call listener function at run time
x.addListener(mysearchpart) // Attach listener function on state changes




document.querySelector('#contact-form').addEventListener('submit', (e) => {
    e.preventDefault();
    e.target.elements.name.value = '';
    e.target.elements.email.value = '';
    e.target.elements.message.value = '';
  });

