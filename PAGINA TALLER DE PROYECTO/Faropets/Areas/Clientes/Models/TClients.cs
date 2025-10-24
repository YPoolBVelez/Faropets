using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TClients
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha {get; set; }
        public bool Credito { get; set; }
        public byte[] Image { get; set; }
        public List<TReports_Clients> TReports_Clients { get; set; }
    }
}
