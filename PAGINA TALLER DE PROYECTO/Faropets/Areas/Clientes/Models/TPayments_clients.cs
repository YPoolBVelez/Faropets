using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TPayments_clients
    {
        [Key]
        public int IdPayments { get; set; }
        [Display(Name = "Deuda")]
        public decimal Debt { get; set; }
        public decimal Change { get; set; }
        [Display(Name = "Pago")]
        public decimal Payment { get; set; }
        [Display(Name = "Fecha de Pago")]
        public DateTime Date { get; set; }
        [Display(Name = "Deuda Pendiente")]
        public decimal CurrentDebt { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateDebt { get; set; }
        public decimal Monthly { get; set; }
        public decimal PreviousDebt { get; set; }
        public string Ticket { get; set; }
        public string IdUser { get; set; }
        public string User { get; set; }
        public int IdCliente { get; set; }
       



    }
}
