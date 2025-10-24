using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TReports_Clients
    {
        [Key]
        public int IdReport { get; set; }
        public Decimal Debt { get; set; }
        public Decimal Monthly { get; set; }
        public Decimal Change { get; set; }
        public Decimal LastPayment { get; set; }
        public DateTime DatePayment { get; set; }
        public Decimal CurrentDebt { get; set; }
        public DateTime DateDebt { get; set; }
        public string Ticket { get; set; }
        public DateTime Deadline { get; set; }
        public int TClientsIdCliente { get; set; }
        public TClients TClients { get; set; }

        
    }
}
