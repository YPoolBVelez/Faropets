var modal = document.getElementById("myModal")

// OBTIENE EL BOTON
var btn = document.getElementById("myBtn");

// OBTIENE BOTON PARA CERRAR MODAL
var span = document.getElementsByClassName("close")[0];

//CRA EVENTO CLICK PARA EL BOTON
btn.onclick = function(){
    modal.style.display = "block";
}

// CIERRO MODAL
span.onclick = function (){
    modal.style.display = "none";
}