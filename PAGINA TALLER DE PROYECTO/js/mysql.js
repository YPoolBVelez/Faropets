const mysql = require('mysql')

const conection = mysql.createConnection({
host: 'localhost',
user: 'root',
password: '',
database: 'faropets'

})

conection.connect((err)=>{
    if(err) throw err
    console.log('Conexion Exitosa')

})

conection.query('SELECT * FROM usuarios', (err , rows)   =>{

    if(err) throw err
    console.log('Los datos guardados de la tabla')
    console.log(rows)
    console.log('La canntidad de resultados son:')
    console.log(rows.length)
})


conection.end()