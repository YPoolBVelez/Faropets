using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faropets.Areas.Clientes.Models;
using Faropets.Data;
using Faropets.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Faropets.Areas.Clientes.Pages.Account.RegisterModel;

namespace Faropets.Areas.Clientes.Pages.Account
{
    [Authorize]
    public class DetailsDebtModel : PageModel
    {
        private static int _idDebt = 0;
        private static int _idCliente = 0;
        public string Money = "$";
        private static string _errorMessage;
        public static RegistroModelInput _dataClient;
        private LibCodes _codes;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LibClientes _clientes;

        public DetailsDebtModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _codes = new LibCodes();
            _clientes = new LibClientes(context);
        }

        public void OnGet(int idDebt, int idCliente)
        {
            if (_idDebt.Equals(0) && _idCliente.Equals(0))
            {
                _idDebt = idDebt;
                _idCliente = idCliente;
            }
            else
            {
                if (_idDebt != idDebt || _idCliente != idCliente)
                {
                    _idDebt = 0;
                    //return Redirect("/Clientes/Reports?id=" + _idCliente + "&area=Clientes");
                }
            }
            _dataClient = _clientes.getTClientPayment(idDebt);
            Input = new ModelInput
            {
                DataClient = _dataClient,

            };
            //return Page();
        }
        [BindProperty]
        public ModelInput Input { get; set; }
        public class ModelInput
        {
            public string Money { get; set; } = "$";
            public RegistroModelInput DataClient { get; set; }
        }
        public async Task<IActionResult> OnPost()
        {
            var _nameCliente = $"{_dataClient.Nombre}{_dataClient.Apellido}";
            var _debt = String.Format("{0:#,###,###,##0,00####}", _dataClient.Debt);
            var _currentDebt = String.Format("{0:#,###,###,##0,00####}", _dataClient.CurrentDebt);
            var _payment = String.Format("{0:#,###,###,##0,00####}", _dataClient.Payment);
            var _change = String.Format("{0:#,###,###,##0,00####}", _dataClient.Change);
            var monthly = String.Format("{0:#,###,###,##0,00####}", _dataClient.Monthly);
            //var previousDebt = String.Format("{0:#,###,###,##0,00####}", _dataClient.PreviousDebt);

            LibTicket Ticket1 = new LibTicket();
            Ticket1.AbrirCajon(); //Abre el cajon
            Ticket1.TextoCentro("Sistemas de ventas Faropets");
            Ticket1.TextoIzquierda("Direeccion Url");
            Ticket1.TextoIzquierda("www.FaroPets.Cl");
            Ticket1.LineasGuion();
            Ticket1.TextoCentro("FACTURA"); //imprime en el centro
            Ticket1.LineasGuion();
            Ticket1.TextoIzquierda($"Factura:{_dataClient.Ticket}");
            Ticket1.TextoIzquierda($"Cliente:{_nameCliente}");
            Ticket1.TextoIzquierda($"Fecha:{_dataClient.Date.ToString("dd/MMM/yyy")}");
            Ticket1.TextoIzquierda($"Usuario:{_dataClient.User}");
            Ticket1.LineasGuion();
            Ticket1.TextoCentro($"Su credito{Money}{_debt}");
            Ticket1.TextoExtremo("Cuotas por meses:", $"{Money}{monthly}");
            //Ticket1.TextoExtremo("Deuda anterior:", $"{Money}{previousDebt}");
            Ticket1.TextoExtremo("Pago;", $"{Money}{_payment}");
            Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
            Ticket1.TextoExtremo("Deuda actual:", $"{Money}{_currentDebt}");
            Ticket1.TextoExtremo("Proximo Pago:", $"{_dataClient.Deadline.Date.ToString("dd/MMM/yyy")}");
            Ticket1.TextoCentro("FaroPets");
            Ticket1.CortarTicket(); //Finaliza el Ticket

            Ticket1.ImprimirTicker("Microsoft XPS Document Writer");
            return Redirect("Clientes/Reports?id=" + _idCliente + "&area=Clientes");
        }

    }
}
