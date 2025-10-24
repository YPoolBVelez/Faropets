using Faropets.Areas.Clientes.Models;
using Faropets.Data;
using Faropets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Library
{
    public class LibClientes : ListOject
    {
        public LibClientes(

            ApplicationDbContext context)
        {
            _context = context;


        }
        public List<RegistroModelInput> getTClientsAsync(string valor, int id)
        {

            List<TClients> listClients;
            List<RegistroModelInput> clientList = new List<RegistroModelInput>();
            if (valor == null && id.Equals(0))
            {
                listClients = _context.TClients.ToList();

            }
            else
            {
                if (id.Equals(0))
                {
                    listClients = _context.TClients.Where(u => u.Nombre.StartsWith(valor) || u.Apellido.StartsWith(valor)
                    || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listClients = _context.TClients.Where(u => u.IdCliente.Equals(id)).ToList();
                }
            }
            if (!listClients.Count.Equals(0))
            {
                foreach (var item in listClients)
                {

                    clientList.Add(new RegistroModelInput
                    {

                        IdCliente = item.IdCliente,
                        Nombre = item.Nombre,
                        Apellido = item.Apellido,
                        Email = item.Email,
                        Direccion = item.Direccion,
                        Date = DateTime.Now,
                        Image = item.Image,
                        Credito = item.Credito

                    });
                }
            }
            return clientList;
        }
        public List<TClients> getTClient(string Email)
        {
            var listTClients = new List<TClients>();
            using (var dbContext = new ApplicationDbContext())
            {
                listTClients = dbContext.TClients.Where(u => Email.Equals(Email)).ToList();
            }

            return listTClients;
        }
        public RegistroModelInput getTClientReport(int id)
        {
            var dataClients = new RegistroModelInput();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TClients.Join(dbContext.TReports_Clients,
                    c => c.IdCliente, r => r.TClientsIdCliente, (c, r) => new
                    {
                        c.IdCliente,
                        c.Nombre,
                        c.Apellido,
                        c.Telefono,
                        c.Email,
                        c.Direccion,
                        c.Credito,
                        r.IdReport,
                        r.Debt,
                        r.Monthly,
                        r.Change,
                        r.CurrentDebt,
                        r.DatePayment,
                        r.LastPayment,
                        r.Ticket,
                        r.Deadline,
                    }).Where(c => c.IdCliente.Equals(id)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new RegistroModelInput
                    {
                        IdCliente = data.IdCliente,
                        Nombre = data.Nombre,
                        Apellido = data.Apellido,
                        Telefono = data.Telefono,
                        Email = data.Email,
                        Direccion = data.Direccion,
                        Credito = data.Credito,
                        IdReport = data.IdReport,
                        Debt = data.Debt,
                        Monthly = data.Monthly,
                        Change = data.Change,
                        CurrentDebt = data.CurrentDebt,
                        DatePayment = data.DatePayment,
                        LastPayment = data.LastPayment,
                        Ticket = data.Ticket,
                        Deadline = data.Deadline,
                    };
                }
            }
            return dataClients;
        }
        public DataPaginador<TPayments_clients> GetPayments(int id, int page, int num, RegistroModelInput input, HttpRequest request)
        {
            Object[] objects = new object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetPayments_Clients(input, id);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LibPaginador<TPayments_clients>().paginador(data, page, num,
                    "Clientes", "Clientes", "Clientes/Reports", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TPayments_clients>();
            }
            var models = new DataPaginador<TPayments_clients>
            {
                List = (List<TPayments_clients>)objects[2],
                pagi_info = (String)objects[0],
                pagi_navegacion = (String)objects[1],
                Input = new TPayments_clients()


            };
            return models;
        }

        public List<TPayments_clients> GetPayments_Clients(RegistroModelInput input, int id)
        {
            var listTPayments = new List<TPayments_clients>();
            var listTPayments2 = new List<TPayments_clients>();
            /* Menos de cero : si t1 es anterior que t2.
             Cero: si t1 es lo mismo que t2.
            Mayor que cero: si es mayor a t2.*/
            var t1 = input.Time1.ToString("dd/MMM/yyy");
            var t2 = input.Time2.ToString("dd/MMM/yyy");
            List<TPayments_clients> listTPayment;

            if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1)
                && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
            {
                listTPayments2 = _context.TPayments_clients.Where(c => c.IdCliente.Equals(id)).ToList();
            }
            else
            {
                foreach (var item in _context.TPayments_clients.Where(c => c.IdCliente.Equals(id)).ToList())
                {
                    int fecha1 = DateTime.Compare(
                        DateTime.Parse(item.Date.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                    if (fecha1.Equals(0) || fecha1 > 0)
                    {
                        listTPayments.Add(item);
                    }
                }
                foreach (var item in listTPayments)
                {
                    int fecha2 = DateTime.Compare(DateTime.Parse(item.Date.ToString("dd/MMM/yyy")),
                        DateTime.Parse(t2));
                    if (fecha2.Equals(0) || fecha2 > 0)
                    {
                        listTPayments2.Add(item);
                    }
                }
            }
            return listTPayments;

        }
        public RegistroModelInput getTClientPayment(int idDebt)
        {
            var dataClients = new RegistroModelInput();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TPayments_clients.Join(dbContext.TClients,
                    p => p.IdCliente, c => c.IdCliente, (p, c) => new
                    {
                        c.IdCliente,
                        c.Nombre,
                        c.Apellido,
                        c.Telefono,
                        c.Email,
                        c.Direccion,
                        c.Credito,
                        p.IdPayments,
                        p.Debt,
                        p.Payment,
                        p.Change,
                        p.CurrentDebt,
                        p.Date,
                        p.Deadline,
                        p.DateDebt,
                        p.Monthly,
                        p.PreviousDebt,
                        p.Ticket,
                        p.IdUser,
                        p.User

                    }).Where(c => c.IdPayments.Equals(idDebt)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new RegistroModelInput
                    {
                        IdCliente = data.IdCliente,
                        Nombre = data.Nombre,
                        Apellido = data.Apellido,
                        Telefono = data.Telefono,
                        Email = data.Email,
                        Direccion = data.Direccion,
                        Credito = data.Credito,
                        IdPayments = data.IdPayments,
                        Debt = data.Debt,
                        Payment = data.Payment,
                        Change = data.Change,
                        CurrentDebt = data.CurrentDebt,
                        Date = data.Date,
                        DateDebt = data.DateDebt,
                        Monthly = data.Monthly,
                        previousDebet = data.PreviousDebt,
                        Ticket = data.Ticket,
                        IdUser = data.IdUser,
                        User = data.User
                    };
                }

            }
            return dataClients;
        }
        public int _interestsCuotas = 0;
        private Decimal _interests;
        public InputModelInterests getTClientInterests(int id)
        {
            var dataInterests = new InputModelInterests();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TClientes_Interests_reports.Where(c => c.IdCliente.Equals(id)).ToList();
                var listIntereses = dbContext.TClientes_Interests.Where(c => c.IdCliente.Equals(id)
                && c.Canceled.Equals(false)).ToList();
                if (listIntereses.Count.Equals(0))
                {
                    _interestsCuotas = 0;
                    _interests = 0.00m;
                }
                else
                {
                    _interestsCuotas = 0;
                    _interests = 0;
                    foreach (var item in listIntereses)
                    {
                        _interests += item.Interests;
                        _interestsCuotas++;
                    }
                }
                var data = query.Count.Equals(0) ? new TClientes_interests_reports() : query.ToList().Last();
                dataInterests = new InputModelInterests
                {
                    IdCliente = data.IdCliente,
                    IdinterestsReports = data.IdinterestsReports,
                    Interest = _interests,
                    Payment = data.Payment,
                    Change = data.Change,
                    fee = _interestsCuotas,
                    InterestDate = data.InterestDate,
                    TicketInterest = data.TicketInterest
                };
            }
            return dataInterests;
        }
        public String AmountFees(int fees, int IdCliente)
        {
            Decimal interests = 0;
            var listIntereses = _context.TClientes_Interests.Where(c => c.IdCliente.Equals(IdCliente)
                && c.Canceled.Equals(false)).ToList();
            if (!listIntereses.Count.Equals(0))
            {
                if (listIntereses.Count <= fees && fees <= listIntereses.Count)
                {
                    for (int i = 0; i < fees; i++)
                    {
                        interests += listIntereses[i].Interests;

                    }
                    return String.Format("{0:#,###,###,##0.00####}", interests);
                }
                else
                {
                    return "Sobrepasaste las cuotas a pagar";

                }
            }
            else
            {
                return "El cliente no debe intereses ";
            }
        }
        public DataPaginador<TPaymkents_Reports_Clients_Interests> GetInterests(int id, int page, int num,
            RegistroModelInput input, HttpRequest request)
        {
            Object[] objects = new object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetInterest_Clients(input, id);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LibPaginador<TPaymkents_Reports_Clients_Interests>().paginador(data, page, num, "Clientes", "Clientes", "Clientes/Reports", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TPaymkents_Reports_Clients_Interests>();
            }
            var models = new DataPaginador<TPaymkents_Reports_Clients_Interests>
            {
                List = (List<TPaymkents_Reports_Clients_Interests>)objects[2],
                pagi_info = (String)objects[0],
                pagi_navegacion = (String)objects[1],
                Input = new TPaymkents_Reports_Clients_Interests()


            };
            return models;

        }
        public List<TPaymkents_Reports_Clients_Interests> GetInterest_Clients(RegistroModelInput input , int id)
        {
            var listTPayments = new List<TPaymkents_Reports_Clients_Interests>();
            var listTPayments2 = new List<TPaymkents_Reports_Clients_Interests>();
            /* Menos de cero : si t1 es anterior que t2.
             Cero: si t1 es lo mismo que t2.
            Mayor que cero: si es mayor a t2.*/
            var t1 = input.Time1.ToString("dd/MMM/yyy");
            var t2 = input.Time2.ToString("dd/MMM/yyy");
            List<TPaymkents_Reports_Clients_Interests> listTPayment;

            if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1)
                && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
            {
                listTPayments2 = _context.TPayments_Reports_Clientes_Interests.Where(c => c.IdCliente.Equals(id)).ToList();
            }
            else
            {
                foreach (var item in _context.TPayments_Reports_Clientes_Interests.
                    Where(c => c.IdCliente.Equals(id)).ToList())
                {
                    int fecha1 = DateTime.Compare(
                        DateTime.Parse(item.Date.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                    if (fecha1.Equals(0) || fecha1 > 0)
                    {
                        listTPayments.Add(item);
                    }
                }
                foreach (var item in listTPayments)
                {
                    int fecha2 = DateTime.Compare(DateTime.Parse(item.Date.ToString("dd/MMM/yyy")),
                        DateTime.Parse(t2));
                    if (fecha2.Equals(0) || fecha2 > 0)
                    {
                        listTPayments2.Add(item);
                    }
                }
            }
            return listTPayments;
        }
    }
}
