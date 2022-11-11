const filterbtn=document.querySelector(".filterButtonForProducts");
const closeiteminfilter=document.querySelector(".closeiteminfilter");
const col_1_of_4=document.querySelector(".custom_filtred_products .products-layout .col-1-of-4")


filterbtn.addEventListener('click',function(){
    col_1_of_4.classList.remove("d-none")
    col_1_of_4.classList.add("customactiveclassforsidefilter")
    closeiteminfilter.classList.remove("d-none");
    closeiteminfilter.classList.add("customclassfortimesinsidefilter");
})
closeiteminfilter.addEventListener('click',()=>{
    col_1_of_4.classList.remove("customactiveclassforsidefilter")
})