using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Models
{
    public class TClients_interests
    {
        [Key]
        public int IdInterests { set; get;}
        public DateTime DeadLine { set; get; }
        public Decimal Debt { set; get; }
        public Decimal Monthly { set; get; }
        public Decimal Interests { set; get; }
        public DateTime Date { set; get; }
        public bool Canceled { set; get; }
        public int IdCliente { set; get; }

    }
}
