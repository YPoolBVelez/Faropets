using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Clientes.Controllers
{
    [Authorize]
    [Area("Clientes")]
    public class ClientesController : Controller
    {
        public IActionResult Clientes()
        {
            return View();
        }
        public String Fees(int fees, int IdCliente)
        {
            return fees.Equals(0) ? "" : _clientes.AmountFees(fees, IdCliente);
        }
    }
}
