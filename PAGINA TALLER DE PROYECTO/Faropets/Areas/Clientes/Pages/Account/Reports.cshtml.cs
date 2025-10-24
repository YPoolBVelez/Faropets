using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Faropets.Areas.Clientes.Models;
using Faropets.Data;
using Faropets.Library;
using Faropets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Faropets.Areas.Clientes.Pages.Account
{

    [Authorize]
    public class ReportsModel : PageModel
    {
        private LibClientes _clientes;
        private static int idCliente = 0;
        private string Money = "$";
        private static string _errorMessage;
        public static RegistroModelInput _dataClient;
        public static InputModelInterests _dataInterests;
        private LibCodes _codes;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userMnanager;

        public ReportsModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userMnanager = userManager;
            _codes = new LibCodes();
            _clientes = new LibClientes(context);
        }

        public IActionResult OnGet(int id, InputModel input)
        {
            if (idCliente == 0)
            {
                idCliente = id;
            }
            else
            {
                if (idCliente != id)
                {
                    idCliente = 0;
                    return Redirect("/Clientes//Clientes?area=Clientes");
                }
            }
            _dataClient = _clientes.getTClientReport(id);
            _dataClient.Time1 = Input.Time1;
            _dataClient.Time2 = Input.Time2;
            _dataInterests = _clientes.getTClientInterests(id);

            Input = new InputModel
            {
                DataClient = _dataClient,
                ErrorMessage = _errorMessage,
                TPayment = _clientes.GetPayments(id, 1, 10, _dataClient, Request),
                DataInterests = _dataInterests,
                TInterest = _clientes.GetInterests(id, 1, 10, _dataClient, Request)

            };
            _errorMessage = "";
            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Money { get; set; } = "$";
            [Required(ErrorMessage = "Seleccione una opción.")]
            public int RadioOptions { get; set; }
            [Required(ErrorMessage = "Seleccione una opción.")]
            [RegularExpression(@"^[0-9]+([,][0-9]+)$", ErrorMessage = "El pago no es correcto.")]
            public Decimal Payment { get; set; }
            public RegistroModelInput DataClient { get; set; }
            public InputModelInterests DataInterests { get; set; }
            [TempData]
            public String ErrorMessage { get; set; }
            public DataPaginador<TPayments_clients> TPayment { get; set; }
            public DateTime Time1 { get; set; } = DateTime.Now.Date;
            public DateTime Time2 { get; set; } = DateTime.Now.Date;

            public int AmountFees { get; set; }

            public LibPaginador<TPaymkents_Reports_Clients_Interests> TInterest { get; set; }

        }

        public async Task<IActionResult> OnPost()
        {
            var idUser = _userMnanager.GetUserId(User);
            var dateNow = DateTime.Now;
            var _nameCliente = $"{_dataClient.Nombre}{_dataClient.Apellido}";
            var user = _context.TUsers.Where(u => u.IdUser.Equals(_userMnanager.GetUserId(User))).ToList();
            var name = $"{user[0].Nombre}{user[0].Apellido}";
            var nowDate = $"{dateNow.Day}/{dateNow.Month}/{dateNow.Year}";
            var _ticket = _codes.codesTickets(_dataClient.Ticket);
            var previousDebt = String.Format($"{0:#,###,###,##0.00###}", _dataClient.previousDebet);
            switch (Input.RadioOptions)
            {
                case 1:
                    if (_dataClient.Debt.Equals(0.0m))
                    {
                        _errorMessage = "El cliente no posee ninguna deuda";
                    }
                    else
                    {
                        String _change = "";
                        Decimal _currentDebt = 0.0m, change;
                        if (Input.Payment >= _dataClient.Monthly)
                        {
                            if (Input.Payment.Equals(_dataClient.CurrentDebt) || Input.Payment > _dataClient.CurrentDebt)
                            {
                                change = Input.Payment - _dataClient.CurrentDebt;
                                _change = String.Format("{0:#,###,###,##0.00###}", change);
                                _errorMessage = $"Cambio para el cliente{Money}{_change}";
                                _currentDebt = 0.0m;
                            }
                            else
                            {
                                change = Input.Payment - _dataClient.Monthly;
                                _change = String.Format("{0:#,###,###,##0.00###}", change);
                                _errorMessage = $"Cambio para el cliente{Money}{_change}";
                                _currentDebt = _dataClient.CurrentDebt - _dataClient.Monthly;
                            }

                            var strategia = _context.Database.CreateExecutionStrategy();
                            await strategia.ExecuteAsync(async () =>
                            {
                                using (var transaction = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var _payment = String.Format("{0:#,###,###,##0.00###}", Input.Payment);
                                        var _debt = String.Format("{0:#,###,###,##0.00###}", _dataClient.Debt);
                                        var CurrentDebt = String.Format("{0:#,###,###,##0.00###}", _currentDebt);
                                        var cambio = String.Format("{0:#,###,###,##0.00###}", _change);
                                        var _currentDebtClient = String.Format("{0:#,###,###,##0.00###}", _dataClient.CurrentDebt);
                                        var _monthly = String.Format("{0:#,###,###,##0.00###}", _dataClient.Monthly);
                                        var date = DateTime.Now.AddMonths(1);
                                        var _deadLine = _dataClient.CurrentDebt.Equals(0.0m) ? "--/--/--" : $"{ date.Day}{ date.Month}{ date.Year}";
                                        var client = _context.TClients.Where(u => u.IdCliente.Equals(_dataClient.IdCliente)).ToList().ElementAt(0);

                                        if (_currentDebt.Equals(0.0))
                                        {
                                            var report = new TReports_Clients
                                            {
                                                IdReport = _dataClient.IdReport,
                                                Debt = 0.0m,
                                                DateDebt = dateNow,
                                                Change = 0.0m,
                                                Monthly = 0.0m,
                                                LastPayment = 0.0m,
                                                DatePayment = dateNow, 
                                                Ticket = "0000000000",
                                                Deadline = dateNow,
                                                TClients = client,
                                            };
                                            _context.Update(report);
                                            _context.SaveChanges();
                                        }
                                    
                                        else
                                        {
                                            var report = new TReports_Clients
                                            {
                                                IdReport = _dataClient.IdReport,
                                                Debt = _dataClient.Debt,
                                                DateDebt = _dataClient.DateDebt,
                                                Monthly = _dataClient.Monthly,
                                                Change = change,
                                                LastPayment = Input.Payment,
                                                DatePayment = dateNow,
                                                CurrentDebt = _currentDebt,
                                                Ticket = _ticket,
                                                Deadline = dateNow,
                                                TClients = client,
                                            };
                                            _context.Update(report);
                                            _context.SaveChanges();
                                        }



                                        var payments = new TPayments_clients
                                        {
                                            Debt = _dataClient.Debt,
                                            Change = change,
                                            Payment = Input.Payment,
                                            Date = dateNow,
                                            CurrentDebt = _currentDebt,
                                            Ticket = _ticket,
                                            Deadline = date,
                                            DateDebt = _dataClient.DateDebt,
                                            Monthly = _dataClient.Monthly,
                                            PreviousDebt = _dataClient.CurrentDebt,
                                            IdUser = idUser,
                                            User = name,
                                            IdCliente = _dataClient.IdCliente
                                        };
                                        _context.Add(payments);
                                        _context.SaveChanges();

                                        LibTicket Ticket1 = new LibTicket();
                                        Ticket1.AbrirCajon(); //Abre el cajon
                                        Ticket1.TextoCentro("Sistemas de ventas Faropets");
                                        Ticket1.TextoIzquierda("Direeccion Url");
                                        Ticket1.TextoIzquierda("www.FaroPets.Cl");
                                        Ticket1.LineasGuion();
                                        Ticket1.TextoCentro("FACTURA"); //imprime en el centro
                                        Ticket1.LineasGuion();
                                        Ticket1.TextoIzquierda($"Factura:{_ticket}");
                                        Ticket1.TextoIzquierda($"Cliente:{_nameCliente}");
                                        Ticket1.TextoIzquierda($"Fecha:{nowDate}");
                                        Ticket1.TextoIzquierda($"Usuario:{name}");
                                        Ticket1.LineasGuion();
                                        Ticket1.TextoCentro($"Su credito{Money}{_debt}");
                                        Ticket1.TextoExtremo("Cuotas por meses:", $"{Money}{Money}");
                                        Ticket1.TextoExtremo("Deuda anterior:", $"{Money}{previousDebt}");
                                        Ticket1.TextoExtremo("Pago;", $"{Money}{_payment}");
                                        Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
                                        Ticket1.TextoExtremo("Deuda actual:", $"{Money}{CurrentDebt}");
                                        Ticket1.TextoExtremo("Proximo Pago:", $"{_deadLine}");
                                        Ticket1.TextoCentro("FaroPets");
                                        Ticket1.CortarTicket(); //Finaliza el Ticket

                                        Ticket1.ImprimirTicker("Microsoft XPS Document Writer");
                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {

                                        _errorMessage = ex.Message;
                                        transaction.Rollback();
                                    }
                                }
                            });
                        }
                        else
                        {
                            var monthly = String.Format("{0:#,###,###,##0.00####}", _dataClient.Monthly);
                            _errorMessage = $"El pago debe ser {Money}{monthly}";

                        }

                    }
                    break;
                case 2:
                    var Estrategia = _context.Database.CreateExecutionStrategy();
                    await Estrategia.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                Decimal changes = 0;
                                List<TClientes_interests_reports> interests = null;
                                List<TClients_interests> listIntereses = null;
                                var feed = _clientes.AmountFees(Input.AmountFees, idCliente);
                                var amountFees = Convert.ToDecimal(feed);
                                using (var dbContext = new ApplicationDbContext())
                                {
                                    interests = dbContext.TClientes_Interests_reports.Where(
                                        c => c.IdCliente.Equals(_dataClient.IdCliente)).ToList();

                                    listIntereses = dbContext.TClientes_Interests.Where(
                                        c => c.IdCliente.Equals(_dataClient.IdCliente) && c.Canceled.Equals(false)).ToList();
                                }
                                var Interests = interests.Count > 0 ? interests.ElementAt(0) : new TClientes_interests_reports();
                                var ticket = _codes.codesTickets(Interests.TicketInterest);
                                if (Input.Payment >= amountFees)
                                {
                                    if (Input.Payment > amountFees)
                                    {
                                        changes = Input.Payment - amountFees;
                                    }


                                    var reports = new TPaymkents_Reports_Clients_Interests
                                    {
                                        Interests = amountFees,
                                        Payment = Input.Payment,
                                        Change = changes,
                                        Fee = Input.AmountFees,
                                        Date = dateNow,
                                        Ticket = ticket,
                                        IdUser = idUser,
                                        User = name,
                                        IdCliente = _dataClient.IdCliente,
                                    };
                                    _context.Add(reports);
                                    _context.SaveChanges();

                                    if (listIntereses.Count > 0)
                                    {
                                        using (var dbContext = new ApplicationDbContext())
                                        {
                                            for (int i = 0; i < Input.AmountFees; i++)
                                            {
                                                var intereses = listIntereses[i];
                                                intereses.Canceled = true;
                                                dbContext.Update(intereses);
                                                dbContext.SaveChanges();
                                            }
                                        }
                                    }
                                    listIntereses.Clear();
                                    listIntereses = _context.TClientes_Interests.Where(
                                        c => c.IdCliente.Equals(_dataClient.IdCliente)
                                        && c.Canceled.Equals(false)).ToList();

                                    if (listIntereses.Count > 0)
                                    {
                                        var report = new TClientes_interests_reports
                                        {
                                            IdinterestsReports = Interests.IdinterestsReports,
                                            Interest = amountFees,
                                            Payment = Input.Payment,
                                            Change = changes,
                                            fee = Input.AmountFees,
                                            InterestDate = dateNow,
                                            TicketInterest = ticket,
                                            IdCliente = _dataClient.IdCliente
                                        };
                                        _context.Update(report);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        var report = new TClientes_interests_reports
                                        {
                                            IdinterestsReports = Interests.IdinterestsReports,
                                            Interest = 0.0m,
                                            Payment = 0.0m,
                                            Change = 0.0m,
                                            fee = 0,
                                            InterestDate = dateNow,
                                            TicketInterest = "0000000000",
                                            IdCliente = _dataClient.IdCliente
                                        };
                                        _context.Update(report);
                                        _context.SaveChanges();
                                    }

                                    var _debt = String.Format("{0:#,###,###,##0.00###}", _dataClient.Debt);
                                    var _payment = String.Format("{0:#,###,###,##0.00###}", Input.Payment);
                                    var _interests = String.Format("{0:#,###,###,##0.00###}", amountFees);
                                    var _changes = String.Format("{0:#,###,###,##0.00###}", changes);
                                    LibTicket ticket1 = new LibTicket();
                                    ticket1.AbrirCajon(); //Abre el cajon
                                    ticket1.TextoCentro("Sistemas de ventas Faropets");
                                    ticket1.TextoIzquierda("Direeccion Url");
                                    ticket1.TextoIzquierda("www.FaroPets.Cl");
                                    ticket1.LineasGuion();
                                    ticket1.TextoCentro("FACTURA"); //imprime en el centro
                                    ticket1.LineasGuion();
                                    ticket1.TextoIzquierda($"Factura:{_ticket}");
                                    ticket1.TextoIzquierda($"Cliente:{_nameCliente}");
                                    ticket1.TextoIzquierda($"Fecha:{nowDate}");
                                    ticket1.TextoIzquierda($"Usuario:{name}");
                                    ticket1.LineasGuion();
                                    ticket1.TextoCentro($"Su credito{Money}{_debt}");
                                    ticket1.TextoExtremo("Cuotas por meses:", $"{Money}{Money}");
                                    ticket1.TextoExtremo("Deuda anterior:", $"{Money}{previousDebt}");
                                    ticket1.TextoExtremo("Pago;", $"{Money}{_payment}");
                                    ticket1.TextoExtremo("Cambio:", $"{Money}{_changes}");
                                    ticket1.TextoCentro("FaroPets");
                                    ticket1.CortarTicket(); //Finaliza el Ticket

                                    ticket1.ImprimirTicker("Microsoft XPS Document Writer");


                                    transaction.Commit();
                                }
                            }
                            catch (Exception ex)
                            {

                                _errorMessage = ex.Message;
                                transaction.Rollback();
                            }
                        }
                    });
                    break;
            }
            return Redirect("/Clientes/Reports?id=" + idCliente);
        }
    }
}

