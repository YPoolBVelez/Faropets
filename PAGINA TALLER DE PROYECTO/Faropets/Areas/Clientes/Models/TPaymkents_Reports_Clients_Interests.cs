using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TPaymkents_Reports_Clients_Interests
    {
        [Key]
        public int IdPaymentsInterests { set; get; }
        public Decimal Interests { set; get; }
        public decimal Payment { get; set; }
        public decimal Change { get; set; }
        public int Fee { get; set; }
        public DateTime Date { get; set; }
        public string Ticket { get; set; }
        public string IdUser { get; set; }
        public string User { get; set; }
        public int IdCliente { get; set; }
    }
}
