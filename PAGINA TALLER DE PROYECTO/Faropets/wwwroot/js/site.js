// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var principal = new Principal();

// CODIGO DE USUARIOS.

var user = new Usuario();
var imageUser = (evt) => {
    user.archivo(evt, "imageUser");
}

// CODIGO DE CLIENTE.
var clientes = new Clientes();
var imageCliente = (evt) => {
    clientes.archivo(evt, "imageCliente");
}



$().ready(() => {
    let URLactual = window.location.pathname;
    principal.userLink(URLactual);

    $("#Input_AmountFees").change((e) =>
    {
        let idCliente = window.location.search.replace("?id=", "");
        clientes.GetInterests(e, null, idCliente);
    });
});