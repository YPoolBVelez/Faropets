class Clientes extends CargarImagen {

    SetSection(value) {
        switch (value) {
            case 1:
                document.getElementById('inlineRadio1').checked = true;
                document.getElementById('inlineRadio2').checked = false;
                document.getElementById('inlineRadio1').disabled = false;
                document.getElementById('inlineRadio2').disabled = true;
                localStorage.getItem("section", value);
                this.Restore();
                break;
            case 2:
                document.getElementById('inlineRadio2').checked = true;
                document.getElementById('inlineRadio1').checked = false;
                document.getElementById('inlineRadio2').disabled = false;
                document.getElementById('inlineRadio1').disabled = true;
                localStorage.getItem("section", value);
                this.Restore();
                break;

        }
    }
    GetInterests(event, input, IdCliente) {
        var fees = 0;
        var key = window.Event ? event.which : event.Keycode;
        var chark = String.fromCharcode(key);
        if (input == null) {
            fees = document.getElementById("Input_AmountFees").value;
        } else {
            fees = input.value + chark;
        }
        $, post(
            window.location.origin + "/Clientes/Fees?area=Clientes",
            { fees: fees, IdCliente: IdCliente },
            (response) => {
                document.getElementById("amountFees").innerHTML = response
                localStorage.SetItem("payment", response);
            }
        );
    }
    Payments(event, input) {
        var tempvalue;
        var key = window.Event ? event.which : event.Keycode;
        var chark = String.fromCharCode(key);
        if (input == null) {
            tempvalue = document.getElementById("Input_Pyment").value;
        } else {
            tempvalue = input.value + chark;
        }
        var payment1 = parseFloat(tempvalue);
        let section = parseInt(localStorage.getItem("section"));
        switch (section) {
            case 2:
                var payment2 = parseFloat(localStorage.getItem("payment"));
                if (payment1 >= payment2) {
                    if (payment1 > payment2) {
                        let change >= payment1 - payment2
                        let value = "El cambio del cliente es" + numberDecimales(change);
                        document.getElementById("paymentMnenssage").innerHTML = value;
                    }
                    $('payment').attr('disabled', false);
                } else {
                    $('payment').attr('disabled', true);
                    document.getElementById("paymentMessage").innerHTML = "";
                }
            default:
        }
    }

    Restore() {
        document.getElementById("Input_AmountFees").value = 0;
        document.getElementById("amountFees").innerHTML = "";
        document.getElementById("Input_Payment").value = "";
        document.getElementById("paymentMessage").innerHTML = "";
        $('#payment').attr("disabled", true);
        

    }
}

 

