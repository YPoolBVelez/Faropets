using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
  
    public class RegistroModelInput : TReports_Clients
    {
        [Key]
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        public string Apellido {get; set;}
        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido, por favor ingrese un correo electrónico Valido.")]
        public string Email {get; set;}
        [Required(ErrorMessage = "El campo Direccion es obligatorio")]
        public string Direccion {get; set;}
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2}\)?[- , ]?([0-9]{5}$", ErrorMessage = "El formato del telefono no es válido.")]
        public string Telefono {get; set;}
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        [DataType(DataType.Date)]
        public DateTime Date {get; set;}
        public bool Credito {get; set;}
        public byte[] Image { get; set; }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public DateTime Time1 { get; set; }
        public DateTime Time2 { get; set; }   
        public int IdPayments { get; set; }
        public Decimal Payment { get; set; }
        public Decimal previousDebet { get; set; }
        public string IdUser { get; set; }
        public string User { get; set; }



    }
}
