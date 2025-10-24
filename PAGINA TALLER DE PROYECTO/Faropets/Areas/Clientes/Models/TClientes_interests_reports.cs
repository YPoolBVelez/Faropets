using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TClientes_interests_reports
    {
        [Key]
        public int IdinterestsReports { get; set; }
        public Decimal Interest { get; set; }
        public Decimal Payment { get; set; }
        public Decimal Change { get; set; }
        public int fee { get; set; }
        public DateTime InterestDate { get; set; }
        public string TicketInterest { get; set; }
        public int IdCliente { get; set; }

    }
}
