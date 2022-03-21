var arrProducts = [];

function addProduct(){
    var name = document.getElementById("pName").value;
    var price = document.getElementById("pPrice").value;
    var category = document.getElementById("sproduct").value;
    var image = document.getElementById("pDocument").files[0].name;

    var objProduct = new Object();
    objProduct.name = name;
    objProduct.price = price;
    objProduct.category = category;
    objProduct.image = "images/" + image;

    arrProducts.push(objProduct);

    setProducts();
}

function setProducts(){
    var divProducts = document.getElementById("divProducts");
    divProducts.innerHTML = "";

    for(var i = 0; i < arrProducts.length; i ++){
        var img = document.createElement("img");
        var pArea = document.createElement("p");
        var pName = document.createElement("p");
        var pPrice = document.createElement("p");
        var div = document.createElement("div");

        img.setAttribute("src", arrProducts[i].image);

        pArea.setAttribute("class", "product-area");
        pArea.innerHTML = arrProducts[i].category;

        pName.setAttribute("class", "product-name");
        pName.innerHTML = arrProducts[i].name;

        pPrice.setAttribute("class", "product-price");
        pPrice.innerHTML = "$" + arrProducts[i].price + ".00";

        div.setAttribute("class", "product");
        div.appendChild(img);
        div.appendChild(pArea);
        div.appendChild(pName);
        div.appendChild(pPrice);

        divProducts.appendChild(div);
    }

    var modal = document.getElementById("myModal");
    modal.style.display = "none";

    const password = document.querySelector("#password");
    const warning = document.querySelector("#warning");

    password.addEventListener("keyup", function(e){
    if(e.getModifierState("CapsLock")){
        warning.style.display = "block";
    }
    else{
        warning.style.display = "none";
    }
    console.log("Password: "+ password.value);
    });


}