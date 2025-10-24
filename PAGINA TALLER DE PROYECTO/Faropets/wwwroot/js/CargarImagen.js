class CargarImagen{
    archivo(evt, Id){
        let files = evt.target.files; //Carga toda imagen que tengamos en nuestro computador
        let f = files[0];
        if (f.type.match('image.*')){
            let reader = new FileReader();
            reader.onload = ((theFile) => {
                return (e) => {
                    document.getElementById(Id).innerHTML = ['<img class="imageUser src="',
                        e.target.result, '"title="', escape(theFile.name), '"/>'].join('');
                }
            })(f);
            reader.readAsDataURL(f);
        }
    }
}